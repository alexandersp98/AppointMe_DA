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
                    Customer_Id = chat.Customer_Id,
                    Entrepreneur_Id = chat.Business_Id,
                });

            }

            return Ok(chatDto);



        }

        [HttpPost]
        public async Task<IActionResult> Post(Chat newChat)
        {
            List<ValidationResult> results = new List<ValidationResult>();

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
        public async Task<IActionResult> Delete(int chatId)
        {
            Chat? chatToDelete = await _uow.ChatRepository.GetById(chatId);

            if (chatToDelete == null)
            {
                return BadRequest("invalid Id");

            }

            else
            {
                _uow.ChatRepository.Delete(chatToDelete);

                await _uow.SaveChangesAsync();

                return Ok();
            }


        }
    }
}
