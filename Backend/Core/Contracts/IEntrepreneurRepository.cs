using Core.Entities;

namespace Core.Contracts
{
    public interface IEntrepreneurRepository
    {
        void Add(Entrepreneur entrepreneur);

        void Delete(Entrepreneur entrepreneurToDelete);

        Task<List<Entrepreneur>> GetAllAsync();

        Task<Entrepreneur?> GetById(int entrepreneurId);

    }
}