using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Courier_Data_Control_App.Classes
{
    public class Delivery
    {
        private int Id;
        private string Name;
        private int PhoneNumber;
        private string Address;
        private string Description;
        private Driver Driver;

        public Delivery(string name, int phoneNumber, string address, string description, Driver driver = null)
        {
            Name = name;
            PhoneNumber = phoneNumber;
            Address = address;
            Description = description;
            Driver = driver;
        }
    }
}
