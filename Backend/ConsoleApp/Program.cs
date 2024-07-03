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


            Entrepreneur ent = new Entrepreneur()
            {
                EMail_Address = "m.mustermann@gmail.net",
                Password = "123456Ab",
                UserName = "MusterMaxi2"


            };

            List<ValidationResult> res = new List<ValidationResult>();

            if (Validator.TryValidateObject(ent, new ValidationContext(ent), res, true))
            {

                uow.EntrepreneurRepository.Add(ent);

                await uow.SaveChangesAsync();

            }

            Console.WriteLine();

        }
    }
}
