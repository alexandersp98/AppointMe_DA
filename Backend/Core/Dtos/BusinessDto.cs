using Core.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dtos
{
    public class BusinessDto
    {

        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;

        public string EMail_Address { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;


    }
}
