using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class FormularObject: EntityObject
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string ZipCode { get; set; } = string.Empty;

        public string CustomerDemands { get; set; } = string.Empty;

        public string Residence { get; set; } = string.Empty;

        public string Street { get; set; } = string.Empty;

        public string HouseNr { get; set; } = string.Empty;



        [ForeignKey(nameof(Business_Id))]
        public Business? Business { get; set; }

        public int Business_Id { get; set; }
    }
}
