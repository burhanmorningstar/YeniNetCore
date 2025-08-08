using App.Application.Contracts.Persistent;
using App.Application.Features.Categories.Create;
using App.Application.Features.Categories.Dto;
using App.Application.Features.Categories.Update;
using App.Domain.Entities;
using AutoMapper;
using System.Net;

namespace App.Application.Features.Categories
{
    public class CategoryService(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork, IMapper mapper)
        : ICategoryService
    {
        public async Task<ApplicationResult<CategoryWithProductDto>> GetCategoryWithProductsAsync(int categoryId)
        {
            var category = await categoryRepository.GetCategoryWithProductsAsync(categoryId);

            if (category is null)
            {
                return ApplicationResult<CategoryWithProductDto>.Fail("Category not found", HttpStatusCode.NotFound);
            }

            var categoryWithProductsAsDto = mapper.Map<CategoryWithProductDto>(category);

            return ApplicationResult<CategoryWithProductDto>.Success(categoryWithProductsAsDto);
        }

        public async Task<ApplicationResult<List<CategoryWithProductDto>>> GetCategoryWithProductsAsync()
        {
            var category = await categoryRepository.GetCategoryWithProductsAsync();

            var categoryWithProductsAsDto = mapper.Map<List<CategoryWithProductDto>>(category);

            return ApplicationResult<List<CategoryWithProductDto>>.Success(categoryWithProductsAsDto);
        }

        public async Task<ApplicationResult<List<CategoryDto>>> GetAllListAsync()
        {
            var categories = await categoryRepository.GetAllAsync();

            var categoriesAsDto = mapper.Map<List<CategoryDto>>(categories);

            return ApplicationResult<List<CategoryDto>>.Success(categoriesAsDto);
        }

        public async Task<ApplicationResult<CategoryDto>> GetByIdAsync(int id)
        {
            var category = await categoryRepository.GetByIdAsync(id);

            if (category is null)
            {
                return ApplicationResult<CategoryDto>.Fail("Category not found", HttpStatusCode.NotFound);
            }
            var categoryAsDto = mapper.Map<CategoryDto>(category);

            return ApplicationResult<CategoryDto>.Success(categoryAsDto);
        }
        public async Task<ApplicationResult<int>> CreateAsync(CreateCategoryRequest request)
        {
            var anyCategory = await categoryRepository.AnyAsync(x => x.Name == request.Name);

            if (anyCategory)
            {
                return ApplicationResult<int>.Fail("Category name must be unique",
                    HttpStatusCode.BadRequest);
            }

            var newCategory = mapper.Map<Category>(request);

            await categoryRepository.AddAsync(newCategory);
            await unitOfWork.SaveChangesAsync();

            return ApplicationResult<int>.SuccessAsCreated(newCategory.Id, $"api/categories/{newCategory.Id}");
        }

        public async Task<ApplicationResult> UpdateAsync(int id, UpdateCategoryRequest request)
        {
            var isCategoryExist = await categoryRepository.AnyAsync(x => x.Name == request.Name && x.Id != id);
            if (isCategoryExist)
            {
                return ApplicationResult.Fail("Category name must be unique",
                    HttpStatusCode.BadRequest);
            }

            var category = mapper.Map<Category>(request);
            category.Id = id;

            categoryRepository.Update(category);
            await unitOfWork.SaveChangesAsync();

            return ApplicationResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ApplicationResult> DeleteAsync(int id)
        {
            var category = await categoryRepository.GetByIdAsync(id);

            categoryRepository.Delete(category!);
            await unitOfWork.SaveChangesAsync();

            return ApplicationResult.Success(HttpStatusCode.NoContent);
        }
    }
}
