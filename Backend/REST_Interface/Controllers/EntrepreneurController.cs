using Core.Contracts;
using Core.Dtos;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Persistence;

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
                    UserName = ent.UserName,
                    EMail_Address = ent.EMail_Address,
                    Password = ent.Password
                });

            }

            return Ok(entrepreneurDtos);



        }



    }
}
