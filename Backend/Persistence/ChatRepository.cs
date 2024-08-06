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

        public async Task DeleteCascadeAsync(int chatId)
        {
            var messagesFromChat = await _context.Messages.Where(m => m.Chat_Id == chatId).ToListAsync();

            _context.Messages.RemoveRange(messagesFromChat);

        }

        public async Task<List<Chat>> GetAllAsync()
        {
            return await _context.Chats
                .Include(c => c.Business)
                .Include(c => c.Customer)
                .OrderBy(c => c.Business_Id).ToListAsync();
        }

        public async Task<Chat?> GetById(int chatId)
        {
            return await _context.Chats.Where(c => c.Id == chatId).FirstOrDefaultAsync();
        }

        public async Task<List<Chat>> GetChatsByUserName(string userName)
        {
            return await _context.Chats
                .Include(c => c.Business)
                .Include(c => c.Customer)
                .Where(c => c.Business!.UserName == userName)
              .ToListAsync();
        }
    }
}