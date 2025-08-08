using App.Application.Features.Categories.Create;
using App.Application.Features.Categories.Dto;
using App.Application.Features.Categories.Update;

namespace App.Application.Features.Categories
{
    public interface ICategoryService
    {
        Task<ApplicationResult<CategoryWithProductDto>> GetCategoryWithProductsAsync(int categoryId);
        Task<ApplicationResult<List<CategoryWithProductDto>>> GetCategoryWithProductsAsync();
        Task<ApplicationResult<List<CategoryDto>>> GetAllListAsync();
        Task<ApplicationResult<CategoryDto>> GetByIdAsync(int id);
        Task<ApplicationResult<int>> CreateAsync(CreateCategoryRequest request);
        Task<ApplicationResult> UpdateAsync(int id, UpdateCategoryRequest request);
        Task<ApplicationResult> DeleteAsync(int id);
    }
}
