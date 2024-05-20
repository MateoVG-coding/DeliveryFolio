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
        private int Id { get; set; }
        private string Name { get; set; }
        private int PhoneNumber { get; set; }
        private string Address { get; set; }
        private string Description { get; set; }
        private bool Status { get; set; }
        private DateTime DateCreated { get; set; }
        private Driver Driver { get; set; }

        public Delivery(string name, int phoneNumber, string address, string description, DateTime dateCreated, bool status = false, Driver driver = null)
        {
            Name = name;
            PhoneNumber = phoneNumber;
            Address = address;
            Description = description;
            Status = status;
            Driver = driver;
            DateCreated = dateCreated;
        }
    }
}
