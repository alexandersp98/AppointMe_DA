using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dtos
{
    public class AppointmentWithoutCustomerDto
    {

        public string Title { get; set; } = string.Empty;


        public DateTime StartDate { get; set; }


        public DateTime EndDate { get; set; }

        public string Description { get; set; } = string.Empty;

        public bool AllDay { get; set; }

    }
}
