using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public partial class Appointment : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(Start > End)
            {
                yield return new ValidationResult("The start date is later than the end date",
                    new List<string> { nameof(Start), nameof(End) });

            }

        }
    }
}
