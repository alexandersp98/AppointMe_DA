using Core.Contracts;
using Core.Entities;

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
            throw new NotImplementedException();
        }

        public void Delete(FormularObject formularObject)
        {
            throw new NotImplementedException();
        }

        public Task<List<FormularObject>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<FormularObject?> GetById(int formularObjectId)
        {
            throw new NotImplementedException();
        }
    }
}