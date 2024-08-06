using Core.Contracts;
using Core.Dtos;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace REST_Interface.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ChatController : Controller
    {
        IUnitOfWork _uow;

        public ChatController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet("/GetAllChats")]
        public async Task<IActionResult> Get()
        {
            List<Chat> chats = await _uow.ChatRepository.GetAllAsync();

            List<ChatDto> chatDto = new List<ChatDto>();

            foreach (Chat chat in chats)
            {
                chatDto.Add(new ChatDto()
                {
                    Id = chat.Id,
                    Business = new BusinessWithoutTokenDto()
                    {
                        FirstName = chat.Business!.FirstName,
                        LastName = chat.Business.LastName,
                        UserName = chat.Business.UserName,
                    },

                    Customer = new CustomerDto()
                    {
                        E_Mail_Address = chat.Customer!.E_Mail_Address,
                        FirstName = chat.Customer.FirstName,
                        LastName= chat.Customer.LastName,
                        PhoneNumber = chat.Customer.PhoneNumber,
                    }

                    
                    
                });

            }

            return Ok(chatDto);
        }

        [HttpGet("/GetChatsByBusinessUserName")]
        public async Task<IActionResult> Get([FromQuery] string userName)
        {

            if (!(await _uow.BusinessRepository.ExistsAsync(userName)))
            {
                return BadRequest("this user does not exist");
            }


            var chats = await _uow.ChatRepository.GetChatsByUserName(userName);

            List<ChatDto> chatDto = new List<ChatDto>();

            foreach (var chat in chats)
            {
                chatDto.Add(new ChatDto()
                {
                    Id = chat.Id,
                    Business = new BusinessWithoutTokenDto()
                    {
                        FirstName = chat.Business!.FirstName,
                        LastName = chat.Business.LastName,
                        UserName = chat.Business.UserName,
                    },

                    Customer = new CustomerDto()
                    {
                        E_Mail_Address = chat.Customer!.E_Mail_Address,
                        FirstName = chat.Customer.FirstName,
                        LastName = chat.Customer.LastName,
                        PhoneNumber = chat.Customer.PhoneNumber,
                    }


                });

            }

            return Ok(chatDto);


        }

        [HttpPost]
        public async Task<IActionResult> Post([FromQuery] string userName, [FromQuery] int customerId)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            int businessId = await _uow.BusinessRepository.GetIdPerUserNameAsync(userName);

            if (businessId == 0)
            {
                return BadRequest("this business does not exist");
            }


            if (!await _uow.CustomerRepository.ExistAsync(customerId))
            {
                return BadRequest("this customer does not exist");
            }

            if (!await _uow.CustomerRepository.BelongsToBusinessAsnc(customerId, businessId))
            {
                return BadRequest("customer does not belong to business");
            }

            Chat newChat = new Chat()
            {
               Customer_Id = customerId,
               Business_Id = businessId,

            };

            if (!Validator.TryValidateObject(newChat, new ValidationContext(newChat), results, true))
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
                _uow.ChatRepository.Add(newChat);

                await _uow.SaveChangesAsync();


            }
            catch (ValidationException ex)
            {

                return BadRequest(ex.Message);

            }

            return Ok();


        }


        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int chatId, [FromQuery] bool deleteCascade)
        {
            Chat? chatToDelete = await _uow.ChatRepository.GetById(chatId);

            if (chatToDelete == null)
            {
                return BadRequest("invalid Id");

            }

            else
            {
                if (deleteCascade)
                {
                    await _uow.ChatRepository.DeleteCascadeAsync(chatId);

                }


                _uow.ChatRepository.Delete(chatToDelete);

                await _uow.SaveChangesAsync();

                return Ok();
            }


        }
    }
}
