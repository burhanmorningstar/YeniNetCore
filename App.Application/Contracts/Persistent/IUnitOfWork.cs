namespace App.Application.Contracts.Persistent
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();
    }
}
