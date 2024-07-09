using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Appointment : EntityObject
    {
        public DateTime Appointment_Date { get; set; }

        [ForeignKey(nameof(Customer_Id))]
        public Customer? Customer { get; set; }

        
        [ForeignKey(nameof(Business_Id))]
        public Business? Business { get; set; }


        [StringLength(300)]
        public string Description { get; set; } = string.Empty;


        public int? Customer_Id { get; set; }

        public int? Business_Id { get; set; }
    }

}
