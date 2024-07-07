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

        public void Delete(Chat chatToDelete)
        {
            _context.Chats.Remove(chatToDelete);
        }

        public async Task<List<Chat>> GetAllAsync()
        {
            return await _context.Chats.OrderBy(c => c.Entrepreneur_Id).ToListAsync();
        }

        public async Task<Chat?> GetById(int chatId)
        {
            return await _context.Chats.Where(c => c.Id == chatId).FirstOrDefaultAsync();
        }
    }
}