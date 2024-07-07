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
    public class EntrepreneurController : Controller
    {

        IUnitOfWork _uow;

        public EntrepreneurController(IUnitOfWork uow)
        {
            _uow = uow;
        }


        [HttpGet("/GetAll")]
        public async Task<IActionResult> Get()
        {
            List<Entrepreneur> entrepreneurs = await _uow.EntrepreneurRepository.GetAllAsync();

            List<EntrepreneurDto> entrepreneurDtos = new List<EntrepreneurDto>();

            foreach(Entrepreneur ent in entrepreneurs)
            {
                entrepreneurDtos.Add(new EntrepreneurDto()
                {
                    Id = ent.Id,
                    UserName = ent.UserName,
                    EMail_Address = ent.EMail_Address,
                    Password = ent.Password
                });

            }

            return Ok(entrepreneurDtos);



        }

        [HttpPost]
        public async Task<IActionResult> Post(Entrepreneur newEntrepreneur)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            if(!Validator.TryValidateObject(newEntrepreneur, new ValidationContext(newEntrepreneur), results))
            {
                string errorMessages = "";

                foreach(ValidationResult validationResult in results)
                {
                    errorMessages = errorMessages + "\n" +  validationResult.ErrorMessage;
                }

                return BadRequest(errorMessages);

            }

            try
            {
                _uow.EntrepreneurRepository.Add(newEntrepreneur);

                await _uow.SaveChangesAsync();


            }
            catch(ValidationException ex)
            {

                return BadRequest(ex.Message);

            }

            return Ok();


        }


        [HttpDelete]
        public async Task<IActionResult> Delete(int entrepreneurId)
        {
            Entrepreneur? entrepreneurToDelete = await _uow.EntrepreneurRepository.GetById(entrepreneurId);

            if(entrepreneurToDelete == null)
            {
                return BadRequest("invalid Id");

            }

            else
            {
                _uow.EntrepreneurRepository.Delete(entrepreneurToDelete);

                await _uow.SaveChangesAsync();

                return Ok();
            }


        }



    }
}
