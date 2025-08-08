using App.Repositories;
using App.Repositories.Products;
using App.Services.Products.Create;
using App.Services.Products.Update;
using App.Services.Products.UpdateStock;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace App.Services.Products
{
    public class ProductService(IProductRepository productRepository,
        IUnitOfWork unitOfWork, IMapper mapper) : IProductService
    {
        public async Task<ServiceResult<List<ProductDto>>> GetTopPriceProductsAsync(int count)
        {
            var products = await productRepository.GetTopPriceProductsAsync(count);

            var productsAsDto = mapper.Map<List<ProductDto>>(products);

            return ServiceResult<List<ProductDto>>.Success(productsAsDto);
        }

        public async Task<ServiceResult<List<ProductDto>>> GetAllListAsync()
        {
            var products = await productRepository.GetAll().ToListAsync();

            //var productsAsDto = products.Select(p => new ProductDto(p.Id, p.Name, p.Price, p.Stock)).ToList();

            var productsAsDto = mapper.Map<List<ProductDto>>(products);

            return ServiceResult<List<ProductDto>>.Success(productsAsDto);
        }

        public async Task<ServiceResult<List<ProductDto>>> GetPagedAllListAsync(int pageNumber, int pageSize)
        {
            var products = await productRepository.GetAll()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            //var productsAsDto = products.Select(p => new ProductDto(p.Id, p.Name, p.Price, p.Stock)).ToList();
            var productsAsDto = mapper.Map<List<ProductDto>>(products);
            return ServiceResult<List<ProductDto>>.Success(productsAsDto);

        }

        public async Task<ServiceResult<ProductDto?>> GetByIdAsync(int id)
        {
            var product = await productRepository.GetByIdAsync(id);
            if (product is null)
            {
                return ServiceResult<ProductDto?>.Fail("Product not found", HttpStatusCode.NotFound);
            }

            //var productsAsDto = new ProductDto(product.Id, product.Name, product.Price, product.Stock);

            var productAsDto = mapper.Map<ProductDto>(product);

            return ServiceResult<ProductDto?>.Success(productAsDto);
        }

        public async Task<ServiceResult<CreateProductResponse>> CreateAsync(CreateProductRequest request)
        {

            //2. yol async manuel service business check
            var anyProduct = await productRepository.Where(x => x.Name == request.Name).AnyAsync();

            if (anyProduct)
            {
                return ServiceResult<CreateProductResponse>.Fail("Product name must be unique",
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
            return ServiceResult<CreateProductResponse>.SuccessAsCreated(new CreateProductResponse(product.Id),
                $"api/products/{product.Id}");


        }

        public async Task<ServiceResult> UpdateAsync(int id, UpdateProductRequest request)
        {
            //fast fail

            //guard clauses


            var isProductNameExist = await productRepository.Where(x => x.Name == request.Name &&
                                                                        x.Id != id).AnyAsync();

            if (isProductNameExist)
            {
                return ServiceResult.Fail("Product name must be unique",
                    HttpStatusCode.BadRequest);
            }


            var product = mapper.Map<Product>(request);
            product.Id = id;

            productRepository.Update(product);

            await unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult> UpdateStockAsync(UpdateProductStockRequest request)
        {
            var product = await productRepository.GetByIdAsync(request.ProductId);


            product!.Stock = request.Quantity;

            productRepository.Update(product);
            await unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult> DeleteAsync(int id)
        {
            var product = await productRepository.GetByIdAsync(id);

            productRepository.Delete(product!);
            await unitOfWork.SaveChangesAsync();
            return ServiceResult.Success(HttpStatusCode.NoContent);
        }
    }
}