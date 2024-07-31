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

        public IFormularObjectRepository FormularObjectRepository { get; }

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
            FormularObjectRepository = new FormularObjectRepository(context);
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

                if (_dbContext.Businesses.Any(e => e.E_Mail_Address == entrepreneur.E_Mail_Address && e.Id != entrepreneur.Id))
                {

                    throw new ValidationException(new ValidationResult($"The E-Mail address {entrepreneur.E_Mail_Address} already exists."
                        , new List<string> { nameof(Business.E_Mail_Address) }), null, null);


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

            if (entity is Chat chatToValidate)
            {
                if(_dbContext.Chats.Any(c => c.Business_Id ==  chatToValidate.Business_Id && 
                c.Customer_Id == chatToValidate.Customer_Id && c.Id != chatToValidate.Id))
                {
                    throw new ValidationException(new ValidationResult($"This Chat-Group already exists."
                                           , new List<string> { nameof(Chat.Business_Id), nameof(Chat.Customer_Id) }), null, null);


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

           

            Business ent = new Business()
            {
                E_Mail_Address = "m.mustermann@gmail.net",
                Password = "123456Ab",
                UserName = "MusterMaxi"


            };

            Business ent2 = new Business()
            {
                E_Mail_Address = "m.mustermann2@gmail.net",
                Password = "123456aB",
                UserName = "MusterMaxi2"


            };

            List<ValidationResult> res = new List<ValidationResult>();

            if (Validator.TryValidateObject(ent, new ValidationContext(ent), res, true))
            {

                BusinessRepository.Add(ent);


            }
            else
            {
                throw new Exception();
            }

            if (Validator.TryValidateObject(ent2, new ValidationContext(ent2), res, true))
            {

                BusinessRepository.Add(ent2);
                await SaveChangesAsync();

            }
            else
            {
                throw new Exception();
            }


            Customer customer = new Customer()
            {
                FirstName = "Maria",
                LastName = "Musterfrau",
                E_Mail_Address = "m.musterfrau@gmail.com",
                PhoneNumber = "0650 4554444",
                Business_Id = ent.Id
            };

            if (Validator.TryValidateObject(customer, new ValidationContext(customer), res, true))
            {

                CustomerRepository.Add(customer);
                await SaveChangesAsync();



            }
            else
            {
                throw new Exception();
            }


            Appointment appointment = new Appointment()
            {
                Appointment_Date = DateTime.Now,
                Description = "Termin von Musterfrau",
                Business_Id = ent.Id,
                Customer_Id = customer.Id,

            };

            if (Validator.TryValidateObject(appointment, new ValidationContext(appointment), res, true))
            {

                AppointmentRepository.Add(appointment);
                await SaveChangesAsync();


            }
            else
            {
                throw new Exception();
            }


            Chat chat = new Chat()
            {
                Business_Id = ent.Id,
                Customer_Id = customer.Id,
            };

            if (Validator.TryValidateObject(chat, new ValidationContext(chat), res, true))
            {

                ChatRepository.Add(chat);
                await SaveChangesAsync();


            }
            else
            {
                throw new Exception();
            }

            Message message = new Message()
            {
                Chat_Id = chat.Id,
                Text = "letzter Preis",
                BusinessIsWriter = false,
                SendTime = DateTime.Now,
            
                
                
            };

            if (Validator.TryValidateObject(message, new ValidationContext(message), res, true))
            {

                MessageRepository.Add(message);

                await SaveChangesAsync();

            }
            else
            {
                throw new Exception();
            }


            
        }
    }


}
