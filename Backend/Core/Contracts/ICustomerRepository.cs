using Core.Entities;

namespace Core.Contracts
{
    public interface ICustomerRepository
    {
        void Add(Customer customer);

        void Delete(Customer customerToDelete);

        Task<List<Customer>> GetAllAsync();

        Task<Customer?> GetById(int customerId);

        Task<Customer?> GetByUserName(string userName);

    }
}