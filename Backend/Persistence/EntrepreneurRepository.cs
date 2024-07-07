using Core.Contracts;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    internal class EntrepreneurRepository : IEntrepreneurRepository
    {
        private ApplicationDbContext _context;

        public EntrepreneurRepository(ApplicationDbContext context)
        {
            this._context = context;
        }

        public void Add(Entrepreneur entrepreneur)
        {
            _context.Entrepreneurs.Add(entrepreneur);
        }

        public async Task Delete(int entrepreunerId)
        {
            Entrepreneur? entrepreneurToRemove = await _context.Entrepreneurs.Where(e => e.Id == entrepreunerId).FirstOrDefaultAsync();

            if(entrepreneurToRemove != null)
            {
                _context.Entrepreneurs.Remove(entrepreneurToRemove);

            }

        }

        public async Task<List<Entrepreneur>> GetAllAsync()
        {
            return await _context.Entrepreneurs.OrderBy(e => e.UserName)
                .ToListAsync();
        }
    }
}