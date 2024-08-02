using Core.Entities;

namespace Core.Contracts
{
    public interface IMessageRepository
    {

        void Add(Message message);

        void Delete(Message messageToDelete);

        Task<List<Message>> GetAllAsync();

        Task<Message?> GetById(int messageId);

        Task<Message?> GetByUserName(string userName);

    }
}