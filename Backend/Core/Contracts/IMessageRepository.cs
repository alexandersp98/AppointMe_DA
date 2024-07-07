using Core.Entities;

namespace Core.Contracts
{
    public interface IMessageRepository
    {

        void Add(Message message);

        Task Delete(int messageId);

        Task<List<Message>> GetAllAsync();


    }
}