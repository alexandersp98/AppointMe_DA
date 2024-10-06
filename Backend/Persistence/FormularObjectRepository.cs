using Core.Contracts;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    internal class FormularObjectRepository : IFormularObjectRepository
    {
        private ApplicationDbContext _context;

        public FormularObjectRepository(ApplicationDbContext context)
        {
            this._context = context;

            
        }

        

        public void Add(FormularObject formularObject)
        {
            _context.FormularObjects.Add(formularObject);
        }

        public void Delete(FormularObject formularObject)
        {
            _context.FormularObjects.Remove(formularObject);
        }

        public async Task<List<FormularObject>> GetAllAsync()
        {
            return await _context.FormularObjects.OrderBy(fo => fo.Business_Id)
                .ToListAsync();
        }


        public async Task<List<FormularObject>> GetByBusiness(string businessUserName)
        {
            return await _context.FormularObjects.Where(fo => fo.Business!.UserName == businessUserName)
                .ToListAsync();
        }

        public async Task<FormularObject?> GetById(int formularObjectId)
        {
            return await _context.FormularObjects.Where(fo => fo.Id ==  formularObjectId)
                .FirstOrDefaultAsync();
        }

        
    }
}