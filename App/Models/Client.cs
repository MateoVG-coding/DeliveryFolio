using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Courier_Data_Control_App.Classes
{
    public class Client
    {
        private int Id;
        private string Name { get; set; }
        private int PhoneNumber { get; set; }
        private string Address { get; set; }
        private DateTime DateCreated { get; set; }
    }
}
