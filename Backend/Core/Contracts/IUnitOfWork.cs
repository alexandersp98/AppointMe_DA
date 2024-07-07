using Core.Contracts;


namespace Core.Contracts
{
    public interface IUnitOfWork : IAsyncDisposable, IDisposable
    {

        IEntrepreneurRepository EntrepreneurRepository { get; }
        ICustomerRepository CustomerRepository { get; }

        IChatRepository ChatRepository { get; }

        IMessageRepository MessageRepository { get; }

        ICalendarRepository CalendarRepository { get; }

        Task<int> SaveChangesAsync();
        Task DeleteDatabaseAsync();
        Task MigrateDatabaseAsync();
        Task CreateDatabaseAsync();

        Task FillDbAsync();
    }
}
