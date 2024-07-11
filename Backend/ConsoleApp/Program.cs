using Core.Contracts;
using Core.Entities;
using Microsoft.Extensions.Options;
using Persistence;
using System.ComponentModel.DataAnnotations;

namespace ConsoleApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {

            using IUnitOfWork uow = new UnitOfWork();

            await uow.FillDbAsync();
            


            Console.WriteLine();

        }
    }
}
