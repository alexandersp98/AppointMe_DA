using Core.Contracts;
using Core.Dtos;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace REST_Interface.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FormularObjectController : Controller
    {
        


        readonly IUnitOfWork _uow;

        public FormularObjectController(IUnitOfWork uow)
        {
            _uow = uow;
        }


        [HttpGet("/GetAllFormulars")]
        public async Task<IActionResult> Get()
        {
            var allFormulars = await _uow.FormularObjectRepository.GetAllAsync();

            List<FormularObjectDto> formularsDto = new();

            foreach (var formular in allFormulars)
            {
                formularsDto.Add(new FormularObjectDto
                {
                    Id = formular.Id,
                    Email_AdressField = formular.Email_AdressField,
                    FirstNameField = formular.FirstNameField,
                    HouseNrField = formular.HouseNrField,
                    LastNameField = formular.LastNameField,
                    PhoneNumberField = formular.PhoneNumberField,
                    ResidenceField = formular.ResidenceField,
                    StreetField = formular.StreetField,

                });

            }


            return Ok(formularsDto);
        }

        [HttpGet("/GetFormularsByBusiness")]
        public async Task<IActionResult> Get([FromQuery] string businessUserName)
        {

            var formulars = await _uow.FormularObjectRepository.GetByBusiness(businessUserName);


            List<FormularObjectDto> formularsDto = new();

            foreach(var formular in formulars)
            {
                formularsDto.Add(new FormularObjectDto
                {
                    Id = formular.Id,
                    Email_AdressField = formular.Email_AdressField,
                    FirstNameField = formular.FirstNameField,
                    HouseNrField = formular.HouseNrField,
                    LastNameField = formular.LastNameField,
                    PhoneNumberField = formular.PhoneNumberField,
                    ResidenceField = formular.ResidenceField,
                    StreetField = formular.StreetField,

                });

            }



            return Ok(formularsDto);

        }

        [HttpDelete()]
        public async Task<IActionResult> Delete([FromQuery] int formularId)
        {
            var formularToDelete = await _uow.FormularObjectRepository.GetById(formularId);

            if (formularToDelete == null)
            {
                return BadRequest("this formular does not exist");
                    
            }

            else
            {
                _uow.FormularObjectRepository.Delete(formularToDelete);

                await _uow.SaveChangesAsync();

                return Ok();

            }


        }

        [HttpPost("/CreateFormular")]
        public async Task<IActionResult> Post([FromQuery] string userName, [FromBody] FormularObjectDto newFormularDto)
        {
            int businessId = await _uow.BusinessRepository.GetIdPerUserNameAsync(userName);

            if (businessId == 0)
            {
                return BadRequest("invalid username");
            }

            FormularObject newFormularObject = new FormularObject
            {
                Business_Id = businessId,
                Email_AdressField = newFormularDto.Email_AdressField,
                FirstNameField = newFormularDto.FirstNameField,
                HouseNrField = newFormularDto.HouseNrField,
                LastNameField = newFormularDto.LastNameField,
                PhoneNumberField = newFormularDto.PhoneNumberField,
                ResidenceField = newFormularDto.ResidenceField,
                StreetField = newFormularDto.StreetField,
            };

            _uow.FormularObjectRepository.Add(newFormularObject);

            await _uow.SaveChangesAsync();

            return Ok();
        }

       
    }
}
