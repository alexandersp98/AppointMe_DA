using Core.Contracts;
using Core.Dtos;
using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Persistence;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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

        [Authorize]
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

        [HttpPost("/Authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] BusinessDto businessObj)
        {
            
            
            if (businessObj == null)
                return BadRequest();

            Business? business = await _uow.BusinessRepository.GetByUsernameAndPasswordAsync(businessObj);
            if (business == null)
            {
                return NotFound(new { Message = "User Not Found!" });
            }

            business.Token = CreateJwt(business);

            return Ok(new
            {
                Token = business.Token,
                Message = "Login Success!"
            });
        }

        private string CreateJwt(Business business)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("LeberkaaaaaaaaasssPepi123456789LeberkasIstSoGeil123456");

            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, business.UserName)
            });

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }


    }
}
