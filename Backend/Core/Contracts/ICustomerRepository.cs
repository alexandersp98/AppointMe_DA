using Core.Entities;

namespace Core.Contracts
{
    public interface ICustomerRepository
    {
        void Add(Customer customer);

        Task Delete(int customerId);

        Task<List<Customer>> GetAllAsync();


    }
}