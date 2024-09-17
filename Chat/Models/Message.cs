using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Models
{
    public class Message
    {
        public string  Id { get; set; } 
        public string SendUser { get; set; }
        public string ReceivedUser { get; set; }
        public DateTime Date { get; set; }
        public string Text { get; set; }
        public string imageurl { get; set; }
    }
}
