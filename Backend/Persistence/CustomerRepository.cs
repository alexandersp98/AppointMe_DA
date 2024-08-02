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

        public void Delete(Customer customerToDelete)
        {
            _context.Customers.Remove(customerToDelete);
        }

        public async Task<List<Customer>> GetAllAsync()
        {
            return await _context.Customers.OrderBy(c => c.LastName).ToListAsync();
        }

        public async Task<Customer?> GetById(int customerId)
        {
            return await  _context.Customers.Where(c => c.Id == customerId).FirstOrDefaultAsync();
        }

        public async Task<Customer?> GetByUserName(string userName)
        {
            return await _context.Customers.Where(c => c.Business!.UserName == userName)
                .FirstOrDefaultAsync();
        }
    }
}