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

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public bool AllDay { get; set; }

        public ExtendedPropsDto? ExtendedProps { get; set; }

    }
}
