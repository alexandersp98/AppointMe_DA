using Core.Entities;

namespace Core.Contracts
{
    public interface IChatRepository
    {
        void Add(Chat chat);
        void Delete(Chat chatToDelete);
        Task DeleteCascadeAsync(int chatId);
        Task<List<Chat>> GetAllAsync();

        Task<Chat?> GetById(int chatId);

        Task<List<Chat>> GetChatsByUserName(string userName);



    }
}