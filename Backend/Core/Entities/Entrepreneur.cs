using Core.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Entrepreneur : EntityObject
    {

        [Required]
        public string UserName { get; set; } = string.Empty;


        [Required]
        [RegularExpression("^[\\w-\\.]+@([\\w-]+\\.)[\\w-]{2,4}")]

        public string EMail_Address { get; set; } = string.Empty;


        [PasswordValidation]
        public string Password { get; set; } = string.Empty;


    }
}
