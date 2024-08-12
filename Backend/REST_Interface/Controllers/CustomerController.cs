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
                    

                });

            }

            return Ok(customerDtos);



        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CustomerDto newCustomerDto, [FromQuery] string userName)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            int businessId = await _uow.BusinessRepository.GetIdPerUserNameAsync(userName);

            if (businessId == 0)
            {
                return BadRequest("this business does not exist");
            }


            Customer newCustomer = new Customer()
            {
                E_Mail_Address = newCustomerDto.E_Mail_Address,
                FirstName = newCustomerDto.FirstName,
                LastName = newCustomerDto.LastName,
                PhoneNumber = newCustomerDto.PhoneNumber,
                Business_Id = businessId
            };

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
        public async Task<IActionResult> Delete([FromQuery] int customerId, [FromQuery] bool deleteCascade)
        {
            Customer? customerToDelete = await _uow.CustomerRepository.GetById(customerId);

            if (customerToDelete == null)
            {
                return BadRequest("invalid Id");

            }

            else
            {
                if (deleteCascade)
                {
                    await _uow.CustomerRepository.DeleteCascade(customerId);

                }


                _uow.CustomerRepository.Delete(customerToDelete);

                await _uow.SaveChangesAsync();

                return Ok();
            }


        }


        [HttpGet("/GetCustomersByBusinessUserName")]
        public async Task<IActionResult> Get([FromQuery] string userName)
        {

            if (!(await _uow.BusinessRepository.ExistsAsync(userName)))
            {
                return BadRequest("this user does not exist");
            }


            var customers = await _uow.CustomerRepository.GetCustomersByUserName(userName);

            List<CustomerDto> customerDto = new List<CustomerDto>();

            foreach (var customer in customers)
            {
                customerDto.Add(new CustomerDto()
                {
                    Id = customer.Id,

                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    E_Mail_Address = customer.E_Mail_Address,
                    PhoneNumber = customer.PhoneNumber

                });

            }

            return Ok(customerDto);
        }


    }
}
