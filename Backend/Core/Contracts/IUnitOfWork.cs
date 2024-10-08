﻿using Core.Contracts;


namespace Core.Contracts
{
    public interface IUnitOfWork : IAsyncDisposable, IDisposable
    {

        IBusinessRepository BusinessRepository { get; }
        ICustomerRepository CustomerRepository { get; }

        IChatRepository ChatRepository { get; }

        IMessageRepository MessageRepository { get; }

        IAppointmentRepository AppointmentRepository { get; }

        IFormularObjectRepository FormularObjectRepository { get; }

        Task<int> SaveChangesAsync();
        Task DeleteDatabaseAsync();
        Task MigrateDatabaseAsync();
        Task CreateDatabaseAsync();

        Task FillDbAsync();
    }
}
