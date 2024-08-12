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
                    AllDay = app.AllDay,
                    Start = app.Start,
                    End = app.End,
                    Title = app.Title
                });

            }
            return Ok(appointmentDtos);
        }


        [HttpGet("/GetAppointmentsByBusinessUserName")]
        public async Task<IActionResult> Get([FromQuery] string userName)
        {

            if (!(await _uow.BusinessRepository.ExistsAsync(userName)))
            {
                return BadRequest("this user does not exist");
            }


            var appointments = await _uow.AppointmentRepository.GetAppointmentsByUserName(userName);

            List<AppointmentDto> appointmentDto = new List<AppointmentDto>();

            foreach (var appointment in appointments)
            {
                appointmentDto.Add(new AppointmentDto()
                {

                    Id = appointment.Id,
                    Description= appointment.Description,
                    Start = appointment.Start,
                    End = appointment.End,
                    Title = appointment.Title,
                    /*Customer = new CustomerDto() 
                    { 
                        FirstName = appointment.Customer!.FirstName,
                        E_Mail_Address = appointment.Customer.E_Mail_Address,
                        LastName = appointment.Customer.LastName,
                        PhoneNumber = appointment.Customer.PhoneNumber,
                        Id = appointment.Id,
                        
                    },*/
                    

                });

            }

            return Ok(appointmentDto);


        }
        
        
        [HttpPost("/CreateAppointment")]
        public async Task<IActionResult> Post([FromQuery] string userName, [FromQuery] int customerId, AppointmentWithoutCustomerDto newAppointmentDto)
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

            if(!await _uow.CustomerRepository.BelongsToBusinessAsnc(customerId, businessId))
            {
                return BadRequest("customer does not belong to business");
            }


            Appointment newAppointment = new Appointment() {
                Description = newAppointmentDto.Description,
                Start = newAppointmentDto.Start,
                End = newAppointmentDto.End,
                Title = newAppointmentDto.Title,
                AllDay = newAppointmentDto.AllDay,
                Business_Id = businessId,
                Customer_Id = customerId,
            };


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

            return Ok("Success");


        }
        

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int appointmentId)
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
