using App.Repositories;
using App.Repositories.Categories;
using App.Services.Categories.Create;
using App.Services.Categories.Dto;
using App.Services.Categories.Update;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace App.Services.Categories
{
    public class CategoryService(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork, IMapper mapper)
        : ICategoryService
    {
        public async Task<ServiceResult<CategoryWithProductDto>> GetCategoryWithProductsAsync(int categoryId)
        {
            var category = await categoryRepository.GetCategoryWithProductsAsync(categoryId);

            if (category is null)
            {
                return ServiceResult<CategoryWithProductDto>.Fail("Category not found", HttpStatusCode.NotFound);
            }

            var categoryWithProductsAsDto = mapper.Map<CategoryWithProductDto>(category);

            return ServiceResult<CategoryWithProductDto>.Success(categoryWithProductsAsDto);
        }

        public async Task<ServiceResult<List<CategoryWithProductDto>>> GetCategoryWithProductsAsync()
        {
            var category = await categoryRepository.GetCategoryWithProducts().ToListAsync();

            var categoryWithProductsAsDto = mapper.Map<List<CategoryWithProductDto>>(category);

            return ServiceResult<List<CategoryWithProductDto>>.Success(categoryWithProductsAsDto);
        }

        public async Task<ServiceResult<List<CategoryDto>>> GetAllListAsync()
        {
            var categories = await categoryRepository.GetAll().ToListAsync();

            var categoriesAsDto = mapper.Map<List<CategoryDto>>(categories);

            return ServiceResult<List<CategoryDto>>.Success(categoriesAsDto);
        }

        public async Task<ServiceResult<CategoryDto>> GetByIdAsync(int id)
        {
            var category = await categoryRepository.GetByIdAsync(id);

            if (category is null)
            {
                return ServiceResult<CategoryDto>.Fail("Category not found", HttpStatusCode.NotFound);
            }
            var categoryAsDto = mapper.Map<CategoryDto>(category);

            return ServiceResult<CategoryDto>.Success(categoryAsDto);
        }
        public async Task<ServiceResult<int>> CreateAsync(CreateCategoryRequest request)
        {
            var anyCategory = await categoryRepository.Where(x => x.Name == request.Name).AnyAsync();

            if (anyCategory)
            {
                return ServiceResult<int>.Fail("Category name must be unique",
                    HttpStatusCode.BadRequest);
            }

            var newCategory = mapper.Map<Category>(request);

            await categoryRepository.AddAsync(newCategory);
            await unitOfWork.SaveChangesAsync();

            return ServiceResult<int>.SuccessAsCreated(newCategory.Id, $"api/categories/{newCategory.Id}");
        }

        public async Task<ServiceResult> UpdateAsync(int id, UpdateCategoryRequest request)
        {
            var isCategoryExist = await categoryRepository.Where(x => x.Name == request.Name && x.Id != id).AnyAsync();
            if (isCategoryExist)
            {
                return ServiceResult.Fail("Category name must be unique",
                    HttpStatusCode.BadRequest);
            }

            var category = mapper.Map<Category>(request);
            category.Id = id;

            categoryRepository.Update(category);
            await unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult> DeleteAsync(int id)
        {
            var category = await categoryRepository.GetByIdAsync(id);

            categoryRepository.Delete(category!);
            await unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }
    }
}
