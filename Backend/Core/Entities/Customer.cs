using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Customer : EntityObject
    {

        [StringLength(30)]
        public string FirstName { get; set; } = string.Empty;


        [StringLength(30)]
        public string LastName { get; set; } = string.Empty;


        [Required]
        [RegularExpression("^[\\w-\\.]+@([\\w-]+\\.)[\\w-]{2,4}")]
        public string E_Mail_Address { get; set; } = string.Empty;


        [RegularExpression("^[\\+]?[(]?[0-9]{3}[)]?[-\\s\\.]?[0-9]{3}[-\\s\\.]?[0-9]{4,6}$")]
        public string PhoneNumber { get; set; } = string.Empty;


        public string CustomerDemand { get; set; } = string.Empty;


        [ForeignKey(nameof(Business_Id))]
        public Business? Business { get; set; }


        public int? Business_Id { get; set; }

    }
}
