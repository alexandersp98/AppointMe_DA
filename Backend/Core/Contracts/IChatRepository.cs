using Core.Entities;

namespace Core.Contracts
{
    public interface IChatRepository
    {
        void Add(Chat chat);

        Task Delete(int chatId);

        Task<List<Chat>> GetAllAsync();


    }
}