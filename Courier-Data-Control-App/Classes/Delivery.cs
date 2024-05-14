using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.AccessControl;
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
        private bool Status;
        private Driver Driver;

        public Delivery(string name, int phoneNumber, string address, string description, bool status = false, Driver driver = null)
        {
            Name = name;
            PhoneNumber = phoneNumber;
            Address = address;
            Description = description;
            Status = status;
            Driver = driver;
        }
    }
}
