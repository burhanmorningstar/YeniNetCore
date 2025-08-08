using App.Domain.Entities;

namespace App.Application.Contracts.Persistent
{
    public interface IProductRepository : IGenericRepository<Product, int>
    {
        public Task<List<Product>> GetTopPriceProductsAsync(int count);
    }
}
