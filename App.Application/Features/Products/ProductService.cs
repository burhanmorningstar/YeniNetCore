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
        IUnitOfWork unitOfWork, IMapper mapper, IServiceBus busService) : IProductService
    {

        public async Task<ApplicationResult<List<ProductDto>>> GetTopPriceProductsAsync(int count)
        {
            var products = await productRepository.GetTopPriceProductsAsync(count);

            var productsAsDto = mapper.Map<List<ProductDto>>(products);

            return ApplicationResult<List<ProductDto>>.Success(productsAsDto);
        }

        public async Task<ApplicationResult<List<ProductDto>>> GetAllListAsync()
        {

            var products = await productRepository.GetAllAsync();

            var productsAsDto = mapper.Map<List<ProductDto>>(products);

            return ApplicationResult<List<ProductDto>>.Success(productsAsDto);
        }

        public async Task<ApplicationResult<List<ProductDto>>> GetPagedAllListAsync(int pageNumber, int pageSize)
        {
            var products = await productRepository.GetAllPagedAsync(pageNumber, pageSize);
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

            var productAsDto = mapper.Map<ProductDto>(product);

            return ApplicationResult<ProductDto?>.Success(productAsDto);
        }

        public async Task<ApplicationResult<CreateProductResponse>> CreateAsync(CreateProductRequest request)
        {

            var anyProduct = await productRepository.AnyAsync(x => x.Name == request.Name);

            if (anyProduct)
            {
                return ApplicationResult<CreateProductResponse>.Fail("Product name must be unique",
                    HttpStatusCode.BadRequest);
            }

            var product = mapper.Map<Product>(request);

            await productRepository.AddAsync(product);
            await unitOfWork.SaveChangesAsync();

            var productUrl = $"api/products/{product.Id}";

            await busService.PublishAsync(new ProductAddedEvent(product.Id, product.Name, product.Price, productUrl));

            return ApplicationResult<CreateProductResponse>.SuccessAsCreated(new CreateProductResponse(product.Id),
                productUrl);

        }

        public async Task<ApplicationResult> UpdateAsync(int id, UpdateProductRequest request)
        {
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