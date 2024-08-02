using Core.Dtos;
using Core.Entities;

namespace Core.Contracts
{
    public interface IBusinessRepository
    {
        void Add(Business business);

        void Delete(Business businessToDelete);
        Task<bool> ExistsAsync(string username, string password);
        Task<List<Business>> GetAllAsync();

        Task<Business?> GetById(int businessId);


        Task<Business?> GetByUsernameAndPasswordAsync(BusinessDto businessObj);
        Task<Business?> GetByUsernameAsync(string username);


        Task<bool> ExistsAsync(string userName);
        Task<int> GetIdPerUserNameAsync(string userName);
    }
}