using Courier_Data_Control_App.Classes;
using Courier_Data_Control_App.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Courier_Data_Control_App.Services
{
    public class SharedDataService : ISharedDataService
    {
        private readonly DeliveryRepository _deliveryRepository;
        private readonly DriverRepository _driverRepository;
        private readonly ClientRepository _clientRepository;

        public ObservableCollection<Driver> Drivers { get; } = new ObservableCollection<Driver>();
        public ObservableCollection<Client> Clients { get; } = new ObservableCollection<Client>();
        public ObservableCollection<Delivery> Deliveries { get; } = new ObservableCollection<Delivery>();

        public SharedDataService(DeliveryRepository deliveryRepository, DriverRepository driverRepository, ClientRepository clientRepository)
        {
            _deliveryRepository = deliveryRepository;
            _driverRepository = driverRepository;
            _clientRepository = clientRepository;

            InitializeData();
        }
        private async void InitializeData()
        {
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            //Just load drivers and clients data since does not require pagination
            var drivers = await _driverRepository.GetAllDriversAsync();
            var clients = await _clientRepository.GetAllClientsAsync();

            foreach (var driver in drivers)
            {
                Drivers.Add(driver);
            }

            foreach (var client in clients)
            {
                Clients.Add(client);
            }
        }
    }
}
