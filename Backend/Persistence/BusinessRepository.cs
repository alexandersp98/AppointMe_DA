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

        public async Task DeleteCascadeAsync(int businessId)
        {
            var customersFromBusiness = await _context.Customers.Where(c => c.Business_Id == businessId).ToListAsync();
            var appointmentsFromBusiness = await _context.Appointments.Where(a => a.Business_Id == businessId).ToListAsync();
            var chatsFromBusiness = await _context.Chats.Where(c => c.Business_Id == businessId).ToListAsync();
            var formularsFromBusiness = await _context.FormularObjects.Where(f => f.Business_Id == businessId).ToListAsync();
            var messagesFromBusiness = await _context.Messages.Where(m => m.Chat!.Business_Id == businessId).ToListAsync();


            _context.Messages.RemoveRange(messagesFromBusiness);
            _context.Chats.RemoveRange(chatsFromBusiness);
            _context.FormularObjects.RemoveRange(formularsFromBusiness);
            _context.Appointments.RemoveRange(appointmentsFromBusiness);
            _context.Customers.RemoveRange(customersFromBusiness);


        
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

        public async Task<int> GetIdPerUserNameAsync(string userName)
        {
            return await _context.Businesses.Where(b => b.UserName == userName)
                .Select(e => e.Id).FirstOrDefaultAsync();

           
        }
    }
}