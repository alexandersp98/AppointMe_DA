using Core.Contracts;
using Core.Dtos;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Persistence;
using System.ComponentModel.DataAnnotations;

namespace REST_Interface.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BusinessController : Controller
    {

        IUnitOfWork _uow;

        public BusinessController(IUnitOfWork uow)
        {
            _uow = uow;
        }


        [HttpGet("/GetAllBusinesses")]
        public async Task<IActionResult> Get()
        {
            List<Business> businesses = await _uow.BusinessRepository.GetAllAsync();

            List<BusinessDto> businessDtos = new List<BusinessDto>();

            foreach (Business business in businesses)
            {
                businessDtos.Add(new BusinessDto()
                {
                    Id = business.Id,
                    UserName = business.UserName,
                    EMail_Address = business.EMail_Address,
                    Password = business.Password
                });

            }

            return Ok(businessDtos);



        }

        [HttpGet("/BusinessLoginCheck")]
        public async Task<IActionResult> Get(string username, string password)
        {
            if(!(await _uow.BusinessRepository.ExistsAsync(username, password)))
            {

                return BadRequest("username or password invalid");

            }



            return Ok();
        }


        [HttpPost]
        public async Task<IActionResult> Post(Business newBusiness)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            if (!Validator.TryValidateObject(newBusiness, new ValidationContext(newBusiness), results, true))
            {
                string errorMessages = "";

                foreach (ValidationResult validationResult in results)
                {
                    errorMessages = errorMessages + "\n" + validationResult.ErrorMessage;
                }

                return BadRequest(errorMessages);

            }

            try
            {
                _uow.BusinessRepository.Add(newBusiness);

                await _uow.SaveChangesAsync();


            }
            catch (ValidationException ex)
            {

                return BadRequest(ex.Message);

            }

            return Ok();


        }


        [HttpDelete]
        public async Task<IActionResult> Delete(int businessId)
        {
            Business? businessToDelete = await _uow.BusinessRepository.GetById(businessId);

            if (businessToDelete == null)
            {
                return BadRequest("invalid Id");

            }

            else
            {
                _uow.BusinessRepository.Delete(businessToDelete);

                await _uow.SaveChangesAsync();

                return Ok();
            }


        }



    }
}
