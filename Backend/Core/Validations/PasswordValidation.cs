using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Validations
{
    public class PasswordValidation : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {

            if (value is string passwordToValidate)
            {
                if (passwordToValidate.Length >= 8)
                {
                    if (!passwordToValidate.Any(ch => !char.IsAsciiLetterOrDigit(ch)))
                    {



                        if (passwordToValidate.Any(c => c >= 'A' && c <= 'Z')
                            && passwordToValidate.Any(c => c >= 'a' && c <= 'z'))
                        {


                            return ValidationResult.Success;


                        }

                        return new ValidationResult("password does not contain one upper- or lower letter");

                    }

                    return new ValidationResult("password should only contain numbers or letters");



                }

                return new ValidationResult("password not long enough, 8 letters minimum");



            }

            return new ValidationResult("value is no string");


        }


    }
}
