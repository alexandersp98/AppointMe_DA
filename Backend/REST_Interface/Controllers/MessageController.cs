using Core.Contracts;
using Core.Dtos;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace REST_Interface.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class MessageController : Controller
    {
        IUnitOfWork _uow;

        public MessageController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet("/GetAllMessages")]
        public async Task<IActionResult> Get()
        {
            List<Message> messages = await _uow.MessageRepository.GetAllAsync();

            List<MessageDto> messageDtos = new List<MessageDto>();

            foreach (Message message in messages)
            {
                messageDtos.Add(new MessageDto()
                {
                    Id = message.Id,
                    Chat_Id = message.Chat_Id,
                    Text = message.Text,
                });

            }

            return Ok(messageDtos);



        }

        [HttpPost]
        public async Task<IActionResult> Post(Message newMessage)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            if (!Validator.TryValidateObject(newMessage, new ValidationContext(newMessage), results))
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
                _uow.MessageRepository.Add(newMessage);

                await _uow.SaveChangesAsync();


            }
            catch (ValidationException ex)
            {

                return BadRequest(ex.Message);

            }

            return Ok();


        }


        [HttpDelete]
        public async Task<IActionResult> Delete(int messageId)
        {
            Message? messageToDelete = await _uow.MessageRepository.GetById(messageId);

            if (messageToDelete == null)
            {
                return BadRequest("invalid Id");

            }

            else
            {
                _uow.MessageRepository.Delete(messageToDelete);

                await _uow.SaveChangesAsync();

                return Ok();
            }


        }

    }
}
