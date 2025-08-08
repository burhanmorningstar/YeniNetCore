using App.Domain.Entities;

namespace App.Application.Contracts.Persistent
{
    public interface ICategoryRepository : IGenericRepository<Category, int>
    {
        Task<Category?> GetCategoryWithProductsAsync(int id);

        Task<List<Category>> GetCategoryWithProductsAsync();
    }
}
