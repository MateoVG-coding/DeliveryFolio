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
        private int Id;
        private string Name;
        private int PhoneNumber;
        private string LicensePlate;
        private bool Status;

        public Driver(int id, string name, int phoneNumber, string licensePlate, bool status = false)
        {
            Name = name;
            PhoneNumber = phoneNumber;
            LicensePlate = licensePlate;
            Status = status;
        }
    }
}
