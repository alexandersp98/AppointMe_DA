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
                    E_Mail_Address = business.E_Mail_Address,
                    Password = business.Password
                });

            }

            return Ok(businessDtos);



        }

        [HttpGet("/BusinessLoginCheck")]
        public async Task<IActionResult> Get([FromQuery] string username,[FromQuery] string password)
        {
            if(!(await _uow.BusinessRepository.ExistsAsync(username, password)))
            {

                return Ok(0);

            }

            Business? business = await _uow.BusinessRepository.GetByUsernameAsync(username);

            BusinessDto businessDto = new BusinessDto()
            {
                Id = business!.Id,
                UserName = business.UserName,
                E_Mail_Address = business.E_Mail_Address,
                Password = business.Password
            };

            return Ok(businessDto);
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BusinessDto newBusinessDto)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            Business newBusiness = new Business()
            {
                E_Mail_Address = newBusinessDto.E_Mail_Address,
                Password = newBusinessDto.Password,
                UserName = newBusinessDto.UserName,
            };

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
        public async Task<IActionResult> Delete([FromQuery] int businessId)
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
