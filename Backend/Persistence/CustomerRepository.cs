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

        public async Task Delete(int customerId)
        {
            Customer? customerToRemove = await _context.Customers.Where(c => c.Id == customerId).FirstOrDefaultAsync();

            if(customerToRemove != null)
            {
                _context.Customers.Remove(customerToRemove);
            }

        }

        public async Task<List<Customer>> GetAllAsync()
        {
            return await _context.Customers.OrderBy(c => c.LastName).ToListAsync();
        }
    }
}