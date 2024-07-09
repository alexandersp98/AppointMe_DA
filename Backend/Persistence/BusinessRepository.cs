using Core.Contracts;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    internal class BusinessRepository : IBusinessRepository
    {
        private ApplicationDbContext _context;

        public BusinessRepository(ApplicationDbContext context)
        {
            this._context = context;
        }

        public void Add(Business business)
        {
            _context.Businesses.Add(business);
        }

        public void Delete(Business businessToDelete)
        {
            _context.Businesses.Remove(businessToDelete);
        }

        public async Task<List<Business>> GetAllAsync()
        {
            return await _context.Businesses.OrderBy(e => e.UserName)
                .ToListAsync();
        }

        public async Task<Business?> GetById(int businessId)
        {
            return await _context.Businesses.Where(e => e.Id == businessId).FirstOrDefaultAsync();
        }
    }
}