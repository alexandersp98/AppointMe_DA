using Core.Entities;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace Core.Contracts
{
    public interface IFormularObjectRepository
    {
        void Add(FormularObject formularObject);

        void Delete(FormularObject formularObject);

        Task<List<FormularObject>> GetAllAsync();

        Task<FormularObject?> GetById(int formularObjectId);

        Task<List<FormularObject>> GetByBusiness(string businessUserName);
    }
}