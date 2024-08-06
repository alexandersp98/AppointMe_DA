using Core.Entities;

namespace Core.Contracts
{
    public interface ICustomerRepository
    {
        void Add(Customer customer);
        Task<bool> BelongsToBusinessAsnc(int customerId, int businessId);
        void Delete(Customer customerToDelete);
        Task DeleteCascade(int customerId);
        Task<bool> ExistAsync(int customerId);
        Task<List<Customer>> GetAllAsync();

        Task<Customer?> GetById(int customerId);

        Task<List<Customer>> GetCustomersByUserName(string userName);
    }
}