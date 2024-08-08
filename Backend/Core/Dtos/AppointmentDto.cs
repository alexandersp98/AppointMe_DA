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

        public string Title { get; set; } = string.Empty;


        public DateTime StartDate { get; set; }


        public DateTime EndDate { get; set; }

        public string Description { get; set; } = string.Empty;

        public CustomerDto? Customer { get; set; }

    }
}
