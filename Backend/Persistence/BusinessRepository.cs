using Core.Contracts;
using Core.Dtos;
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

        public async Task<bool> ExistsAsync(string username, string password)
        {

            return await _context.Businesses.AnyAsync(b => b.UserName == username && b.Password == password);

        }

        public async Task<bool> ExistsAsync(string userName)
        {
           return await _context.Businesses.AnyAsync
                (b => b.UserName == userName);
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

        public async Task<Business?> GetByUsernameAndPasswordAsync(BusinessDto businessObj)
        {
            return await _context.Businesses.Where(b => b.UserName == businessObj.UserName && b.Password == businessObj.Password).FirstOrDefaultAsync();
        }

        public async Task<Business?> GetByUsernameAsync(string username)
        {
            return await _context.Businesses.Where(b => b.UserName == username).FirstOrDefaultAsync();
        }


    }
}