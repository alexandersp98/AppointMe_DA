using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Message : EntityObject
    {
        [ForeignKey(nameof(Chat_Id))]
        public Chat? Chat { get; set; }

        public int Chat_Id { get; set; }



        public string Text { get; set; } = string.Empty;


    }
}
