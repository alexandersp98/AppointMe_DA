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

        public IBusinessRepository BusinessRepository { get; }

        public ICustomerRepository CustomerRepository { get; }

        public IChatRepository ChatRepository { get; }

        public IMessageRepository MessageRepository { get; }

        public IAppointmentRepository AppointmentRepository {  get; }

        public UnitOfWork() : this(new ApplicationDbContext())
        { }

        public UnitOfWork(ApplicationDbContext context)
        {
            _dbContext = context;

            BusinessRepository = new BusinessRepository(context);
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
            if(entity is Business entrepreneur)
            {
                if(_dbContext.Businesses.Any(e => e.UserName == entrepreneur.UserName && e.Id != entrepreneur.Id))
                {

                    throw new ValidationException(new ValidationResult($"The username {entrepreneur.UserName} already exists."
                        , new List<string> { nameof(Business.UserName) }), null, null);


                }

                if (_dbContext.Businesses.Any(e => e.EMail_Address == entrepreneur.EMail_Address && e.Id != entrepreneur.Id))
                {

                    throw new ValidationException(new ValidationResult($"The E-Mail address {entrepreneur.EMail_Address} already exists."
                        , new List<string> { nameof(Business.EMail_Address) }), null, null);


                }


            }

            if(entity is Customer customerToValidate)
            {
                if(_dbContext.Customers.Any(c => c.E_Mail_Address == customerToValidate.E_Mail_Address && customerToValidate.Id != c.Id))
                {
                    throw new ValidationException(new ValidationResult($"The E-Mail address {customerToValidate.E_Mail_Address} already exists."
                        , new List<string> { nameof(Customer.E_Mail_Address) }), null, null);

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

            Business entrepreneur = new Business() {
            EMail_Address = "m.mustermann@gmail.com",
            Password = "123456Ab",
            UserName = "MusterMaxi"};
            
            _dbContext.Businesses.Add(entrepreneur);


            await SaveChangesAsync();
        }
    }


}
