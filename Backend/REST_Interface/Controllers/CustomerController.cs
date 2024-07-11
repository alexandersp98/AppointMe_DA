using Core.Contracts;
using Core.Dtos;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace REST_Interface.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class CustomerController : Controller
    {
        IUnitOfWork _uow;

        public CustomerController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet("/GetAllCustomers")]
        public async Task<IActionResult> Get()
        {
            List<Customer> customers = await _uow.CustomerRepository.GetAllAsync();

            List<CustomerDto> customerDtos = new List<CustomerDto>();

            foreach (Customer cust in customers)
            {
                customerDtos.Add(new CustomerDto()
                {
                    Id = cust.Id,

                    E_Mail_Address = cust.E_Mail_Address,
                    FirstName = cust.FirstName,
                    LastName = cust.LastName,
                    PhoneNumber = cust.PhoneNumber,
                    Entrepreneur_Id = cust.Business_Id

                });

            }

            return Ok(customerDtos);



        }

        [HttpPost]
        public async Task<IActionResult> Post(Customer newCustomer)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            if (!Validator.TryValidateObject(newCustomer, new ValidationContext(newCustomer), results, true))
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
                _uow.CustomerRepository.Add(newCustomer);

                await _uow.SaveChangesAsync();


            }
            catch (ValidationException ex)
            {

                return BadRequest(ex.Message);

            }

            return Ok();


        }


        [HttpDelete]
        public async Task<IActionResult> Delete(int customerId)
        {
            Customer? customerToDelete = await _uow.CustomerRepository.GetById(customerId);

            if (customerToDelete == null)
            {
                return BadRequest("invalid Id");

            }

            else
            {
                _uow.CustomerRepository.Delete(customerToDelete);

                await _uow.SaveChangesAsync();

                return Ok();
            }


        }


    }
}
