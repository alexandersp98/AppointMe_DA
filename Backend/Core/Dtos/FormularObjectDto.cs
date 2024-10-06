using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dtos
{
    public class FormularObjectDto
    {

        public int Id { get; set; }
        public int FirstNameField { get; set; }

        public int LastNameField { get; set; }

        public int Email_AdressField { get; set; }

        public int PhoneNumberField { get; set; }

        public int HouseNrField { get; set; }

        public int StreetField { get; set; }


        public int ResidenceField { get; set; }


    }
}
