using Core.Contracts;
using Core.Dtos;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace REST_Interface.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class AppointmentController : Controller
    {
        IUnitOfWork _uow;
        public AppointmentController(IUnitOfWork uow)
        {
            _uow = uow;
        }


        [HttpGet("/GetAllAppointments")]
        public async Task<IActionResult> Get()
        {
            List<Appointment> appointments = await _uow.AppointmentRepository.GetAllAsync();

            List<AppointmentDto> appointmentDtos = new List<AppointmentDto>();

            foreach (Appointment app in appointments)
            {
                appointmentDtos.Add(new AppointmentDto()
                {
                    Id = app.Id,

                    Description = app.Description,
                    Appointment_Date = app.Appointment_Date,


                });

            }

            return Ok(appointmentDtos);



        }

        [HttpPost]
        public async Task<IActionResult> Post(Appointment newAppointment)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            if (!Validator.TryValidateObject(newAppointment, new ValidationContext(newAppointment), results, true))
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
                _uow.AppointmentRepository.Add(newAppointment);

                await _uow.SaveChangesAsync();


            }
            catch (ValidationException ex)
            {

                return BadRequest(ex.Message);

            }

            return Ok();


        }


        [HttpDelete]
        public async Task<IActionResult> Delete(int appointmentId)
        {
            Appointment? appointmentToDelete = await _uow.AppointmentRepository.GetById(appointmentId);

            if (appointmentToDelete == null)
            {
                return BadRequest("invalid Id");

            }

            else
            {
                _uow.AppointmentRepository.Delete(appointmentToDelete);

                await _uow.SaveChangesAsync();

                return Ok();
            }


        }
    }
}
