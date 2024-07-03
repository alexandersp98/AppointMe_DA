using Core.Contracts;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;

namespace Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        const string FILENAME = "games.csv";

        private ApplicationDbContext _dbContext;

        public ICustomerRepository CustomerRepository { get;}

        public UnitOfWork() : this(new ApplicationDbContext())
        { }

        public UnitOfWork(ApplicationDbContext context)
        {
            _dbContext = context;
            CustomerRepository = new CustomerRepository(_dbContext);


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


            _dbContext.Customers.Add(new Customer()
            {
                FirstName = "Max",
                LastName = "Mustermann",
                Iban = "0934fajkl39420"
            });

            _dbContext.Customers.Add(new Customer()
            {
                FirstName = "Maria",
                LastName = "Musterfrau",
                Iban = "0934faj39420"
            });

            _dbContext.Customers.Add(new Customer()
            {
                FirstName = "Franz",
                LastName = "Amann",
                Iban = "asdf32gaf"
            });


            await SaveChangesAsync();
        }
    }


}
