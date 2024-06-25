using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Courier_Data_Control_App.Classes
{
    public class Driver
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int PhoneNumber { get; set; }
        public string LicensePlate { get; set; }
        public DateTime DateCreated { get; set; }
        public bool Status { get; set; }
    }
}
