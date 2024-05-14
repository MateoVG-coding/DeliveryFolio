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

        public Client(string name, int phoneNumber, string address)
        {
            Name = name;
            PhoneNumber = phoneNumber;
            Address = address;
        }
    }
}
