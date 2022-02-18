using System;
using System.Collections.Generic;
using System.Text;

namespace GetEventsFacebook.Models
{
    public class Event
    {
        public string Name { get; set; }

        public string Date { get; set; }

        public string Address { get; set; }

        public byte[] ImageBytes { get; set; } 
    }
}
