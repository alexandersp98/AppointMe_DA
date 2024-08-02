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

        public BusinessWithoutTokenDto? Business { get; set; }

        public CustomerDto? Customer { get; set; }
    }
}
