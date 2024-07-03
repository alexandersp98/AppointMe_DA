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

        public async Task<List<Entrepreneur>> GetAllAsync()
        {
            return await _context.Entrepreneurs.OrderBy(e => e.UserName)
                .ToListAsync();
        }
    }
}