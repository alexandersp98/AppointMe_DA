using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class FormularObject: EntityObject
    {

        public int FirstNameField { get; set; }

        public int LastNameField { get; set; }

        public int Email_AdressField { get; set; }

        public int PhoneNumberField { get; set; }

        public int HouseNrField { get; set; }

        public int StreetField { get; set; }


        public int ResidenceField { get; set; }


        [ForeignKey(nameof(Business_Id))]
        public Business? Business { get; set; }

        public int Business_Id { get; set; }
    }
}
