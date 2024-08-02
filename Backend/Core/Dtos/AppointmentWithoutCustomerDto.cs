using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dtos
{
    public class AppointmentWithoutCustomerDto
    {

        public DateTime Appointment_Date { get; set; }


        public string Description { get; set; } = string.Empty;

    }
}
