using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dtos
{
    public class ChatDto
    {
        public int Id { get; set; }
        public Customer? Customer { get; set; }

        public Business? Entrepreneur { get; set; }

        public int? Entrepreneur_Id { get; set; }

        public int? Customer_Id { get; set; }
    }
}
