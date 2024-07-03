using Core.Contracts;
using Persistence;

namespace ConsoleApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {

            using IUnitOfWork uow = new UnitOfWork();

            await uow.FillDbAsync();


        }
    }
}
