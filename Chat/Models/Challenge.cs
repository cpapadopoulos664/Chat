using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Models
{
    public class Challenge
    {
        public string PhotoUrl { get; set; }
        public string InfoText { get; set; }
        public string UID { get; set; }
        public string Username { get; set; }
        public string GPS { get; set; } 
        public string Type { get; set; }
        public string Link { get; set; }    
    }
}
