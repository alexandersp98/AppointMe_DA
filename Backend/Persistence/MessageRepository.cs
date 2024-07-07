using Core.Contracts;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    internal class MessageRepository : IMessageRepository
    {
        private ApplicationDbContext _context;

        public MessageRepository(ApplicationDbContext context)
        {
            this._context = context;
        }

        public void Add(Message message)
        {
            _context.Add(message);
        }

        public void Delete(Message messageToDelete)
        {
            _context.Messages.Remove(messageToDelete);
        }

        public async Task<List<Message>> GetAllAsync()
        {
            return await _context.Messages.OrderBy(m => m.Chat_Id).ToListAsync();
        }

        public async Task<Message?> GetById(int messageId)
        {
            return await _context.Messages.Where(m => m.Id == messageId).FirstOrDefaultAsync();
        }
    }
}