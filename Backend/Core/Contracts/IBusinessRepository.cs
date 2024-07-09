using Core.Entities;

namespace Core.Contracts
{
    public interface IBusinessRepository
    {
        void Add(Business business);

        void Delete(Business businessToDelete);

        Task<List<Business>> GetAllAsync();

        Task<Business?> GetById(int businessId);

    }
}