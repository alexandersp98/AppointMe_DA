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

        public async Task Delete(int messageId)
        {
            Message? messageToRemove = await _context.Messages.Where(m => m.Id == messageId).FirstOrDefaultAsync();

            if(messageToRemove != null)
            {
                _context.Messages.Remove(messageToRemove);
            }

        }

        public async Task<List<Message>> GetAllAsync()
        {
            return await _context.Messages.OrderBy(m => m.Chat_Id).ToListAsync();
        }
    }
}