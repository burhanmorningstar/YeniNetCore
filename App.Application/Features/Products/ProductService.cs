using App.Application.Contracts.Caching;
using App.Application.Contracts.Persistent;
using App.Application.Contracts.ServiceBus;
using App.Application.Features.Products.Create;
using App.Application.Features.Products.Dto;
using App.Application.Features.Products.Update;
using App.Application.Features.Products.UpdateStock;
using App.Domain.Entities;
using App.Domain.Events;
using AutoMapper;
using System.Net;

namespace App.Application.Features.Products
{
    public class ProductService(IProductRepository productRepository,
        IUnitOfWork unitOfWork, IMapper mapper, ICacheService cacheService, IServiceBus busService) : IProductService
    {
        private const string ProductListCacheKey = "ProductListCache";

        public async Task<ApplicationResult<List<ProductDto>>> GetTopPriceProductsAsync(int count)
        {
            var products = await productRepository.GetTopPriceProductsAsync(count);

            var productsAsDto = mapper.Map<List<ProductDto>>(products);

            return ApplicationResult<List<ProductDto>>.Success(productsAsDto);
        }

        public async Task<ApplicationResult<List<ProductDto>>> GetAllListAsync()
        {
            //cache aside design pattern
            //1. any cache
            //2 if not from db
            //3 cache it

            var productListAsCached = await cacheService.GetAsync<List<ProductDto>>(ProductListCacheKey);

            if (productListAsCached is not null)
            {
                return ApplicationResult<List<ProductDto>>.Success(productListAsCached);
            }


            var products = await productRepository.GetAllAsync();

            //var productsAsDto = products.Select(p => new ProductDto(p.Id, p.Name, p.Price, p.Stock)).ToList();

            var productsAsDto = mapper.Map<List<ProductDto>>(products);

            await cacheService.AddAsync(ProductListCacheKey, productsAsDto, TimeSpan.FromMinutes(1));

            return ApplicationResult<List<ProductDto>>.Success(productsAsDto);
        }

        public async Task<ApplicationResult<List<ProductDto>>> GetPagedAllListAsync(int pageNumber, int pageSize)
        {
            var products = await productRepository.GetAllPagedAsync(pageNumber, pageSize);
            //var productsAsDto = products.Select(p => new ProductDto(p.Id, p.Name, p.Price, p.Stock)).ToList();
            var productsAsDto = mapper.Map<List<ProductDto>>(products);
            return ApplicationResult<List<ProductDto>>.Success(productsAsDto);

        }

        public async Task<ApplicationResult<ProductDto?>> GetByIdAsync(int id)
        {
            var product = await productRepository.GetByIdAsync(id);
            if (product is null)
            {
                return ApplicationResult<ProductDto?>.Fail("Product not found", HttpStatusCode.NotFound);
            }

            //var productsAsDto = new ProductDto(product.Id, product.Name, product.Price, product.Stock);

            var productAsDto = mapper.Map<ProductDto>(product);

            return ApplicationResult<ProductDto?>.Success(productAsDto);
        }

        public async Task<ApplicationResult<CreateProductResponse>> CreateAsync(CreateProductRequest request)
        {

            //2. yol async manuel service business check
            var anyProduct = await productRepository.AnyAsync(x => x.Name == request.Name);

            if (anyProduct)
            {
                return ApplicationResult<CreateProductResponse>.Fail("Product name must be unique",
                    HttpStatusCode.BadRequest);
            }


            //3. yol async manuel fluent validation business check
            //var valResult = await createProductRequestValidator.ValidateAsync(request);

            //if (!valResult.IsValid)
            //{
            //    return ServiceResult<CreateProductResponse>.Fail(valResult.Errors.Select(x => x.ErrorMessage).ToList());
            //}

            var product = mapper.Map<Product>(request);

            await productRepository.AddAsync(product);
            await unitOfWork.SaveChangesAsync();

            await busService.PublishAsync(new ProductAddedEvent(product.Id, product.Name, product.Price));

            return ApplicationResult<CreateProductResponse>.SuccessAsCreated(new CreateProductResponse(product.Id),
                $"api/products/{product.Id}");

        }

        public async Task<ApplicationResult> UpdateAsync(int id, UpdateProductRequest request)
        {
            //fast fail

            //guard clauses


            var isProductNameExist = await productRepository.AnyAsync(x => x.Name == request.Name &&
                                                                        x.Id != id);

            if (isProductNameExist)
            {
                return ApplicationResult.Fail("Product name must be unique",
                    HttpStatusCode.BadRequest);
            }


            var product = mapper.Map<Product>(request);
            product.Id = id;

            productRepository.Update(product);

            await unitOfWork.SaveChangesAsync();

            return ApplicationResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ApplicationResult> UpdateStockAsync(UpdateProductStockRequest request)
        {
            var product = await productRepository.GetByIdAsync(request.ProductId);


            product!.Stock = request.Quantity;

            productRepository.Update(product);
            await unitOfWork.SaveChangesAsync();

            return ApplicationResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ApplicationResult> DeleteAsync(int id)
        {
            var product = await productRepository.GetByIdAsync(id);

            productRepository.Delete(product!);
            await unitOfWork.SaveChangesAsync();
            return ApplicationResult.Success(HttpStatusCode.NoContent);
        }
    }
}