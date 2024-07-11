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

            //Business business = new Business() 
            //{ 
            //    EMail_Address = "m.test@gmail.com",
            //    Password = "passw324324A",
            
            //    UserName = "test",
            //};
            
            //List<ValidationResult> res = new List<ValidationResult>();

            //Validator.TryValidateObject(business, new ValidationContext(business), res, true);


            Console.WriteLine();

        }
    }
}
