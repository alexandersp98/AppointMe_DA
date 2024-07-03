using Core.Contracts;

namespace Core.Contracts
{
    public interface IUnitOfWork : IAsyncDisposable, IDisposable
    {

        Task<int> SaveChangesAsync();
        Task DeleteDatabaseAsync();
        Task MigrateDatabaseAsync();
        Task CreateDatabaseAsync();

        Task FillDbAsync();
    }
}
