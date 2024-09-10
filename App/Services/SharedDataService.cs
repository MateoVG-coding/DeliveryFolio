using Courier_Data_Control_App.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Courier_Data_Control_App.Services
{
    public class SharedDataService
    {
        public ObservableCollection<Driver> Drivers { get; set; }
        public ObservableCollection<Delivery> Deliveries { get; set; }
        public ObservableCollection<Client> Clients { get; set; }

        public SharedDataService()
        {
            Drivers = new ObservableCollection<Driver>();
            Deliveries = new ObservableCollection<Delivery>();
            Clients = new ObservableCollection<Client>();
        }
    }
}
