using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Chat : EntityObject
    {

        [ForeignKey(nameof(Customer_Id))]
        public Customer? Customer { get; set; }

        [ForeignKey(nameof(Entrepreneur_Id))]
        public Entrepreneur? Entrepreneur { get; set; }

        public int Entrepreneur_Id { get; set; }

        public int Customer_Id { get; set; }
    }
}
