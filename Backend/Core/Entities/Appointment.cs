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


        public DateTime StartDate { get; set; }


        public DateTime EndDate { get; set; }

        
        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;



    }

}
