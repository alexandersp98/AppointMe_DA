using Core.Contracts;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;

namespace Persistence
{
    public class UnitOfWork : IUnitOfWork
    {

        private ApplicationDbContext _dbContext;

        public IEntrepreneurRepository EntrepreneurRepository { get; }

        public ICustomerRepository CustomerRepository { get; }

        public IChatRepository ChatRepository { get; }

        public IMessageRepository MessageRepository { get; }

        public IAppointmentRepository AppointmentRepository {  get; }

        public UnitOfWork() : this(new ApplicationDbContext())
        { }

        public UnitOfWork(ApplicationDbContext context)
        {
            _dbContext = context;

            EntrepreneurRepository = new EntrepreneurRepository(context);
            CustomerRepository = new CustomerRepository(context);
            ChatRepository = new ChatRepository(context);
            MessageRepository = new MessageRepository(context);
            AppointmentRepository = new AppointmentRepository(context);
        }

        public UnitOfWork(IConfiguration configuration) : this(new ApplicationDbContext(configuration))
        {



        }


        public async Task<int> SaveChangesAsync()
        {
            var entities = _dbContext!.ChangeTracker.Entries()
                .Where(entity => entity.State == EntityState.Added
                                 || entity.State == EntityState.Modified)
                .Select(e => e.Entity)
                .ToArray();  // Geänderte Entities ermitteln

            // Allfällige Validierungen der geänderten Entities durchführen
            foreach (var entity in entities)
            {
                ValidateEntity(entity);
            }
            return await _dbContext.SaveChangesAsync();

       }

        private void ValidateEntity(object entity)
        {       
            if(entity is Entrepreneur entrepreneur)
            {
                if(_dbContext.Entrepreneurs.Any(e => e.UserName == entrepreneur.UserName && e.Id != entrepreneur.Id))
                {

                    throw new ValidationException(new ValidationResult($"The username {entrepreneur.UserName} already exists."
                        , new List<string> { nameof(Entrepreneur.UserName) }), null, null);


                }

                if (_dbContext.Entrepreneurs.Any(e => e.EMail_Address == entrepreneur.EMail_Address && e.Id != entrepreneur.Id))
                {

                    throw new ValidationException(new ValidationResult($"The E-Mail address {entrepreneur.EMail_Address} already exists."
                        , new List<string> { nameof(Entrepreneur.EMail_Address) }), null, null);


                }


            }


           
        }

        public async Task DeleteDatabaseAsync() => await _dbContext!.Database.EnsureDeletedAsync();
        public async Task MigrateDatabaseAsync() => await _dbContext!.Database.MigrateAsync();
        public async Task CreateDatabaseAsync() => await _dbContext!.Database.EnsureCreatedAsync();

        public async ValueTask DisposeAsync()
        {
            await DisposeAsync(true);
            GC.SuppressFinalize(this);
        }

        protected virtual async ValueTask DisposeAsync(bool disposing)
        {   
                if (disposing)
                {
                    await _dbContext.DisposeAsync();
                }
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public async Task FillDbAsync()
        {
            await DeleteDatabaseAsync();
            await MigrateDatabaseAsync();

            Entrepreneur entrepreneur = new Entrepreneur() {
            EMail_Address = "m.mustermann@gmail.com",
            Password = "123456Ab",
            UserName = "MusterMaxi"};
            
            _dbContext.Entrepreneurs.Add(entrepreneur);


            await SaveChangesAsync();
        }
    }


}
