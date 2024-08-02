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
    public class Business : EntityObject
    {

        [Required]
        public string UserName { get; set; } = string.Empty;


        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;


        [Required]
        [RegularExpression("^[\\w-\\.]+@([\\w-]+\\.)[\\w-]{2,4}")]

        public string E_Mail_Address { get; set; } = string.Empty;


        [PasswordValidation]
        public string Password { get; set; } = string.Empty;

        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }


    }
}
