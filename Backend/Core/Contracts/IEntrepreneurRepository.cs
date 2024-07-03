using Core.Entities;

namespace Core.Contracts
{
    public interface IEntrepreneurRepository
    {
        Task<List<Entrepreneur>> GetAllAsync();
    }
}