using Core.Entities;

namespace Core.Contracts
{
    public interface IEntrepreneurRepository
    {
        void Add(Entrepreneur entrepreneur);

        Task Delete(int entrepreunerId);

        Task<List<Entrepreneur>> GetAllAsync();
    }
}