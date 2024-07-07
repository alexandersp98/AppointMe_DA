using Core.Contracts;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    internal class ChatRepository : IChatRepository
    {
        private ApplicationDbContext _context;

        public ChatRepository(ApplicationDbContext context)
        {
            this._context = context;
        }

        public void Add(Chat chat)
        {
            _context.Chats.Add(chat);
        }

        public async Task Delete(int chatId)
        {
            Chat? chatToRemove = await _context.Chats.Where(c => c.Id == chatId).FirstOrDefaultAsync();
        
        
            if (chatToRemove != null)
            {
                _context.Chats.Remove(chatToRemove);
            }
        
        }

        public async Task<List<Chat>> GetAllAsync()
        {
            return await _context.Chats.OrderBy(c => c.Entrepreneur_Id).ToListAsync();
        }
    }
}