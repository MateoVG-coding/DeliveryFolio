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
        private int Id { get; set; }
        private string Name { get; set; }
        private int PhoneNumber { get; set; }
        private string LicensePlate { get; set; }
        private DateTime DateCreated { get; set; }
        private bool Status { get; set; }
    }
}
