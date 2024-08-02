using Core.Contracts;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    internal class CustomerRepository : ICustomerRepository
    {
        private ApplicationDbContext _context;

        public CustomerRepository(ApplicationDbContext context)
        {
            this._context = context;
        }

        public void Add(Customer customer)
        {
            _context.Customers.Add(customer);
        }

        public async Task<bool> BelongsToBusinessAsnc(int customerId, int businessId)
        {
            return await _context.Customers.Where(c => c.Id == customerId)
                .AnyAsync(c => c.Business_Id == businessId);

        }

        public void Delete(Customer customerToDelete)
        {
            _context.Customers.Remove(customerToDelete);
        }

        public async Task<bool> ExistAsync(int customerId)
        {
            return await _context.Customers.AnyAsync(x => x.Id == customerId);
        }

        public async Task<List<Customer>> GetAllAsync()
        {
            return await _context.Customers.OrderBy(c => c.LastName).ToListAsync();
        }

        public async Task<Customer?> GetById(int customerId)
        {
            return await  _context.Customers.Where(c => c.Id == customerId).FirstOrDefaultAsync();
        }

      

        public async Task<List<Customer>> GetCustomersByUserName(string userName)
        {
            return await _context.Customers.Where(c => c.Business!.UserName == userName)
                .ToListAsync();
        }

       
    }
}