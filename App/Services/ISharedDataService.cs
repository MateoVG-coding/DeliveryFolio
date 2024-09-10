using Courier_Data_Control_App.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Courier_Data_Control_App.Services
{
    public interface ISharedDataService
    {
        ObservableCollection<Driver> Drivers { get; }
        ObservableCollection<Client> Clients { get; }
        ObservableCollection<Delivery> Deliveries { get; }
    }
}
