using App.Application.Contracts.Persistent;
using App.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace App.Persistence.Repositories
{
    public class CategoryRepository(AppDbContext context)
        : GenericRepository<Category, int>(context), ICategoryRepository
    {
        public async Task<Category?> GetCategoryWithProductsAsync(int id)
        {
            return await Context.Categories
                .Include(x => x.Products)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<List<Category>> GetCategoryWithProductsAsync()
        {
            return Context.Categories.Include(x => x.Products).ToListAsync();
        }

        public IQueryable<Category> GetCategoryWithProducts()
        {
            return Context.Categories
                .Include(x => x.Products).AsQueryable();
        }


    }
}
