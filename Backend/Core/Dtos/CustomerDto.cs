using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dtos
{
    public class CustomerDto
    {

        public string FirstName { get; set; } = string.Empty;


        public string LastName { get; set; } = string.Empty;


        public string E_Mail_Address { get; set; } = string.Empty;


        public string PhoneNumber { get; set; } = string.Empty;


        public string CustomerDemand { get; set; } = string.Empty;


        public Entrepreneur? Entrepreneur { get; set; }


        public int Entrepreneur_Id { get; set; }
    }
}
