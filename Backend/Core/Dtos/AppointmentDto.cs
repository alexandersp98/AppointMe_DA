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
    public class AppointmentDto
    {

        public int Id { get; set; }
        public DateTime Appointment_Date { get; set; }

        public Customer? Customer { get; set; }

        public Business? Business { get; set; }


        public string Description { get; set; } = string.Empty;


        public int? Customer_Id { get; set; }

        public int? Business_Id { get; set; }


    }
}
