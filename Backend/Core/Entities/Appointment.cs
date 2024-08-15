using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public partial class Appointment : EntityObject
    {
        [MaxLength(50)]
        public string Title { get; set; } = string.Empty;

        [ForeignKey(nameof(Customer_Id))]
        public Customer? Customer { get; set; }


        [ForeignKey(nameof(Business_Id))]
        public Business? Business { get; set; }


        public int? Customer_Id { get; set; }

        public int? Business_Id { get; set; }


        public bool AllDay { get; set; }


        // Need to set the time to UTC
        private DateTime _start;
        public DateTime Start
        {
            get => _start;
            set => _start = DateTime.SpecifyKind(value, DateTimeKind.Utc);
        }

        // Need to set the time to UTC
        private DateTime _end;
        public DateTime End
        {
            get => _end;
            set => _end = DateTime.SpecifyKind(value, DateTimeKind.Utc);
        }


        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

    }
}
