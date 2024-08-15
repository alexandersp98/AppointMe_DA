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
                    Title = app.Title,
                    AllDay = app.AllDay,
                    Start = app.Start,
                    End = app.End,
                    ExtendedProps = new ExtendedPropsDto
                    {
                        Description = app.Description,
                        CustomerId = (int)app.Customer_Id!
                    }
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
                    Title = appointment.Title,
                    AllDay = appointment.AllDay,
                    Start = appointment.Start,
                    End = appointment.End,
                    ExtendedProps = new ExtendedPropsDto
                    {
                        Description = appointment.Description,
                        CustomerId = (int)appointment.Customer_Id!
                    }
                });

            }

            return Ok(appointmentDto);


        }


        [HttpPost("/CreateAppointment")]
        public async Task<IActionResult> Post([FromQuery] string userName, [FromQuery] int customerId, [FromBody] AppointmentWithoutCustomerDto newAppointmentDto)
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


            Appointment newAppointment = new Appointment()
            {
                Description = newAppointmentDto.ExtendedProps!.Description,
                Start = newAppointmentDto.Start,
                End = newAppointmentDto.End,
                Title = newAppointmentDto.Title,
                AllDay = newAppointmentDto.AllDay,
                Business_Id = businessId,
                Customer_Id = customerId,
            };
            System.Diagnostics.Debug.WriteLine($"Received Description: {newAppointmentDto.ExtendedProps.Description}");

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

                var createdAppointmentDto = new AppointmentDto
                {
                    Id = newAppointment.Id,
                    Title = newAppointment.Title,
                    AllDay = newAppointment.AllDay,
                    Start = newAppointment.Start,
                    End = newAppointment.End,
                    ExtendedProps = new ExtendedPropsDto
                    {
                        Description = newAppointment.Description,
                        CustomerId = newAppointment.Customer_Id.HasValue ? newAppointment.Customer_Id.Value : 0
                    }
                };

                return Ok(createdAppointmentDto);

            }
            catch (ValidationException ex)
            {

                return BadRequest(ex.Message);

            }

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


        [HttpPut("/UpdateAppointment")]
        public async Task<IActionResult> UpdateAppointment([FromQuery] int appointmentId, [FromBody] AppointmentWithoutCustomerDto updatedAppointmentDto)

        {
            if (appointmentId <= 0)
            {
                return BadRequest("Invalid appointment ID");
            }

            var existingAppointment = await _uow.AppointmentRepository.GetById(appointmentId);
            if (existingAppointment == null)
            {
                return NotFound("Appointment not found");
            }

            existingAppointment.Title = updatedAppointmentDto.Title;
            existingAppointment.Start = updatedAppointmentDto.Start;
            existingAppointment.End = updatedAppointmentDto.End;
            existingAppointment.AllDay = updatedAppointmentDto.AllDay;
            existingAppointment.Description = updatedAppointmentDto.ExtendedProps?.Description!;
            existingAppointment.Customer_Id = updatedAppointmentDto.ExtendedProps?.CustomerId;

            // Validate the updated appointment
            var results = new List<ValidationResult>();
            if (!Validator.TryValidateObject(existingAppointment, new ValidationContext(existingAppointment), results, true))
            {
                string errorMessages = string.Join("\n", results.Select(r => r.ErrorMessage));
                return BadRequest(errorMessages);
            }

            try
            {
                _uow.AppointmentRepository.UpdateAppointment(existingAppointment);
                await _uow.SaveChangesAsync();

                var newAppointmentDto = new AppointmentDto
                {
                    Id = existingAppointment.Id,
                    Title = existingAppointment.Title,
                    Start = existingAppointment.Start,
                    End = existingAppointment.End,
                    AllDay = existingAppointment.AllDay,
                    ExtendedProps = new ExtendedPropsDto
                    {
                        Description = existingAppointment.Description,
                        CustomerId = existingAppointment.Customer_Id.HasValue ? existingAppointment.Customer_Id.Value : 0
                    }
                };

                return Ok(newAppointmentDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


    }
}
