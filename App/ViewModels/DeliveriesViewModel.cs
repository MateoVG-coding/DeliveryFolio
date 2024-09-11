using CommunityToolkit.Mvvm.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Courier_Data_Control_App.Classes;
using Courier_Data_Control_App.Repositories;
using Courier_Data_Control_App.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Courier_Data_Control_App.ViewModels
{
    /// <summary>
    /// A view model for list of deliveries.
    /// </summary>
    public partial class DeliveriesViewModel : ObservableObject
    {
        private readonly DeliveryRepository _deliveryRepository;
        private readonly ISharedDataService _sharedDataService;

        public ObservableCollection<Delivery> Deliveries => _sharedDataService.Deliveries;
        public ObservableCollection<Driver> Drivers  => _sharedDataService.Drivers;
        public ObservableCollection<Client> Clients=> _sharedDataService.Clients;

        [ObservableProperty]
        private Delivery _newDelivery = new Delivery();

        public ObservableCollection<string> DeliveryTypes { get; set; } = new ObservableCollection<string>
        {
            "Entrega estándar",
            "Entrega urgente",
            "Entrega de compras",
            "Entrega bancaria",
            "Entrega programada"
        };

        //Properties to apply paging

        private const int PageSize = 10;
        [ObservableProperty]
        private int _totalPages;
        [ObservableProperty]
        private int _currentPage;

        [ObservableProperty]
        private bool _canNavigatePrevious;
        [ObservableProperty]
        private bool _canNavigateNext;

        public DeliveriesViewModel(DeliveryRepository deliveryRepository, ISharedDataService sharedDataService)
        {
            _deliveryRepository = deliveryRepository;
            _sharedDataService = sharedDataService;

            InitializePagination();
        }

        /// <summary>
        /// Gets the current deliveries in the database for the collection of
        /// the view model using paging
        /// </summary>
        [RelayCommand]
        async Task LoadDeliveriesAsync(int pageNumber)
        {
            if (pageNumber < 1 || pageNumber > TotalPages)
            {
                return;
            }

            CurrentPage = pageNumber;
            var deliveries = await _deliveryRepository.GetAllDeliveriesAsync(CurrentPage, PageSize);

            Deliveries.Clear();

            foreach (var delivery in deliveries)
            {
                Deliveries.Add(delivery);
            }

            UpdateNavigationButtons();
        }

        /// <summary>
        /// Add a delivery to the deliveries repository and add it to the collection
        /// of the view model
        /// </summary>
        [RelayCommand]
        async Task AddDeliveryAsync()
        {
            await _deliveryRepository.AddDeliveryAsync(NewDelivery);
            await LoadDeliveriesAsync(CurrentPage);

            // Reset the NewDelivery object for further use
            NewDelivery = new Delivery();
        }

        /// <summary>
        /// Update the selected delivery in the view model collection with their new data
        /// </summary>
        [RelayCommand]
        async Task UpdateDeliveryAsync(DataGridRowEditEndingEventArgs args)
        {
            if (args.Row.DataContext is Delivery updatedDelivery)
            {
                await _deliveryRepository.UpdateDeliveryAsync(updatedDelivery);
                await LoadDeliveriesAsync(CurrentPage);
            }
        }

        /// <summary>
        /// Delete a delivery from the deliveries repository and the view model collection.
        /// </summary>
        [RelayCommand]
        async Task DeleteDeliveryAsync(Delivery selectedDelivery)
        {
            await _deliveryRepository.DeleteDeliveryAsync(selectedDelivery);
            await LoadDeliveriesAsync(CurrentPage);
        }

        /// <summary>
        /// Command to handle the selection of the client when the user chooses from the suggestions
        /// </summary>
        [RelayCommand]
        private void SelectClient()
        {
            var client = Clients.FirstOrDefault(c => c.Name.Equals(NewDelivery.CustomerName, StringComparison.OrdinalIgnoreCase));

            if (client != null)
            {
                NewDelivery.Address = client.Address;
                NewDelivery.PhoneNumber = client.PhoneNumber;

                OnPropertyChanged(nameof(NewDelivery));
            }

        }

        /// <summary>
        /// Allows the view to show the existing deliveries using paging
        /// </summary>
        void UpdateNavigationButtons()
        {
            CanNavigatePrevious = CurrentPage > 1;
            CanNavigateNext = CurrentPage < TotalPages;
        }

        async void InitializePagination()
        {
            CurrentPage = 1;

            var totalDeliveries = await _deliveryRepository.GetTotalDeliveriesCountAsync();
            TotalPages = (int)Math.Ceiling(totalDeliveries / (double)PageSize);

            await LoadDeliveriesAsync(CurrentPage);
        }

        [RelayCommand]
        async Task NextPageAsync()
        {
            if (CanNavigateNext)
            {
                await LoadDeliveriesAsync(CurrentPage + 1);
            }
        }

        [RelayCommand]
        async Task PreviousPageAsync()
        {
            if (CanNavigatePrevious)
            {
                await LoadDeliveriesAsync(CurrentPage - 1);
            }
        }
    }
}
