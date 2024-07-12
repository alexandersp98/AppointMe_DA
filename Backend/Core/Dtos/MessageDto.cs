using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dtos
{
    public class MessageDto
    {

        public int Id { get; set; }

        public int? Chat_Id { get; set; }

        public string Text { get; set; } = string.Empty;

        public bool BusinessIsWriter { get; set; }

        public DateTime SendTime { get; set; }


    }
}
