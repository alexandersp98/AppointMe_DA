using Core.Entities;

namespace Core.Contracts
{
    public interface IEntrepreneurRepository
    {
        void Add(Entrepreneur entrepreneur);
        Task<List<Entrepreneur>> GetAllAsync();
    }
}