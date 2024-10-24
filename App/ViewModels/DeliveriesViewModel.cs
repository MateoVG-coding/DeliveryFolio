using CommunityToolkit.Mvvm.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Courier_Data_Control_App.Models;
using Courier_Data_Control_App.Repositories;
using Courier_Data_Control_App.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Xml.Linq;


namespace Courier_Data_Control_App.ViewModels
{
    /// <summary>
    /// View model for deliveries page
    /// </summary>
    public partial class DeliveriesViewModel : ObservableValidator
    {
        private readonly DeliveryRepository _deliveryRepository;
        private readonly ISharedDataService _sharedDataService;

        public ObservableCollection<Delivery> Deliveries => _sharedDataService.Deliveries;
        public ObservableCollection<Driver> Drivers => _sharedDataService.Drivers;
        public ObservableCollection<Client> Clients=> _sharedDataService.Clients;

        [ObservableProperty]
        private Delivery currentDelivery = new Delivery();

        public IReadOnlyList<string> DeliveryTypes { get; } = new[]
        {
            "Entrega estándar", "Entrega urgente",
            "Entrega de compras", "Entrega bancaria",
            "Entrega programada"
        };

        // START: Properties and methods to apply filtering
        [ObservableProperty]
        private string searchFilter = string.Empty;
        [ObservableProperty]
        private bool isFiltering;
        [ObservableProperty]
        private string selectedTimeSpan = "Todos";

        partial void OnSelectedTimeSpanChanged(string value)
        {
            _ = SearchDeliveriesAsync();
        }
        [RelayCommand]
        private void SelectTimeSpan(string timespan)
        {
            SelectedTimeSpan = timespan;
        }
        public DateTime? CalculateTimeSpan()
        {
            DateTime today = DateTime.Today;

            switch (SelectedTimeSpan)
            {
                case "Hoy":
                    return (today);

                case "Hace una semana":
                    return (today.AddDays(-7));

                case "Hace un mes":
                    return (today.AddMonths(-1));

                case "Hace un año":
                    return (today.AddYears(-1));
            }

            return null;
        }
        // END: Properties and methods to apply filtering

        //Properties to apply paging
        private const int _pageSize = 10;
        [ObservableProperty]
        private int totalPages;
        [ObservableProperty]
        private int currentPage;

        [ObservableProperty]
        private bool canNavigatePrevious;
        [ObservableProperty]
        private bool canNavigateNext;

        // START: Properties and methodS for the header CheckBox
        [ObservableProperty]
        private bool isAllDeliveriesSelected;
        [ObservableProperty]
        private bool isAnyDeliverySelected;

        partial void OnIsAllDeliveriesSelectedChanged(bool value)
        {
            if(!value)
            {
                DeselectAllDeliveries();
                return;
            }

            SelectAllDeliveries();
        }
        private void Delivery_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Delivery.IsSelected))
            {
                IsAnyDeliverySelected = Deliveries.Any(d => d.IsSelected);
            }
        }

        public void SelectAllDeliveries()
        {
            foreach (var delivery in Deliveries)
            {
                delivery.IsSelected = true;
            }

            IsAllDeliveriesSelected = IsAnyDeliverySelected = true;
        }
        public void DeselectAllDeliveries()
        {
            foreach (var delivery in Deliveries)
            {
                delivery.IsSelected = false; 
            }

            IsAllDeliveriesSelected = IsAnyDeliverySelected = false;
        }

        // END: Properties and method for the header CheckBox

        public DeliveriesViewModel(DeliveryRepository deliveryRepository, ISharedDataService sharedDataService)
        {
            _deliveryRepository = deliveryRepository;
            _sharedDataService = sharedDataService;

            int pageNumber = 1;
            CalculatePagination();
            _ = LoadDeliveriesAsync(pageNumber);
        }

        /// <summary>
        /// Gets the current deliveries for the collection of
        /// the view model using paging
        /// </summary>
        [RelayCommand]
        async Task SearchDeliveriesAsync()
        {
            if (!string.IsNullOrEmpty(SearchFilter))
            {
                IsFiltering = true;
            }
            else 
            {
                IsFiltering = false; 
            }

            CalculatePagination();
            await LoadDeliveriesAsync(1);
        }
        async Task LoadDeliveriesAsync(int pageNumber)
        {
            if (pageNumber < 1 || pageNumber > TotalPages)
            {
                //Ensure the user stays within valid page range
                if (!IsFiltering)
                {
                    TotalPages = 1;
                    CanNavigateNext = false;
                    Deliveries.Clear();
                    return;
                }

                MessageBox.Show($"No se han encontrado entregas que coincidan con sus criterios de búsqueda. Vuelva a intentarlo con otro nombre de cliente, " +
                $"número de teléfono, o mensajero.", "", MessageBoxButton.OK, MessageBoxImage.Information);

                await ClearFilters();
                return;
            }

            CurrentPage = pageNumber;
            var deliveries = await _deliveryRepository.GetAllDeliveriesAsync(CurrentPage, _pageSize, SearchFilter, CalculateTimeSpan());

            Deliveries.Clear();
            foreach (var delivery in deliveries)
            {
                delivery.PropertyChanged += Delivery_PropertyChanged;
                Deliveries.Add(delivery);
            }

            UpdateNavigationButtons();
        }

        /// <summary>
        /// Clear search or filters applied
        /// </summary>
        [RelayCommand]
        async Task ClearFilters()
        {
            SearchFilter = String.Empty;
            await SearchDeliveriesAsync();
        }

        /// <summary>
        /// Add a delivery to the deliveries repository and add it to the collection
        /// of the view model
        /// </summary>
        [RelayCommand]
        async Task AddDeliveryAsync()
        {
            if (CurrentDelivery == null)
            {
                CurrentDelivery = new Delivery();
            }

            await _deliveryRepository.AddDeliveryAsync(CurrentDelivery);
            CurrentDelivery = new Delivery();
            await SearchDeliveriesAsync();
        }

        /// <summary>
        /// Update the selected delivery in the view model collection and db with their new data
        /// </summary>
        [RelayCommand]
        async Task UpdateDeliveryAsync(DataGridRowEditEndingEventArgs args)
        {
            if (args.Row.DataContext is Delivery updatedDelivery)
            {
                await _deliveryRepository.UpdateDeliveryAsync(updatedDelivery);

                var deliveryInCollection = Deliveries.FirstOrDefault(d => d.Id == updatedDelivery.Id);
                if (deliveryInCollection != null)
                {
                    deliveryInCollection = updatedDelivery;
                }
            }
        }

        /// <summary>
        /// Delete the selected deliveries from the deliveries repository and the view model collection.
        /// </summary>
        [RelayCommand]
        async Task DeleteSelectedDeliveriesAsync(Delivery selectedDelivery)
        {
            var result = MessageBox.Show($"¿Estás seguro de que quieres eliminar los registros seleccionados?",
                "Eliminar Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                var deliveriesToDelete = Deliveries.Where(d => d.IsSelected).ToList();

                foreach (var delivery in deliveriesToDelete)
                {
                    await _deliveryRepository.DeleteDeliveryAsync(delivery);
                    Deliveries.Remove(delivery);
                }

                //Reset the checkbox header of Deliveries data grid
                IsAllDeliveriesSelected = false;

                //Ensure the user stays within valid page range
                CalculatePagination();
                if (CurrentPage > TotalPages)
                {
                    CurrentPage = CurrentPage = TotalPages;
                }

                await LoadDeliveriesAsync(CurrentPage);
            }
        }

        /// <summary>
        /// Handles the selection of the client when the user chooses from the suggestions
        /// </summary>
        [RelayCommand]
        private void SelectClient()
        {
            if (CurrentDelivery == null)
            {
                CurrentDelivery = new Delivery();
            }

            var client = Clients.FirstOrDefault(c => c.Name.Equals(CurrentDelivery.CustomerName, StringComparison.OrdinalIgnoreCase));

            if (client != null)
            {
                CurrentDelivery.Address = client.Address;
                CurrentDelivery.PhoneNumber = client.PhoneNumber;

                OnPropertyChanged(nameof(CurrentDelivery));
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
        async void CalculatePagination()
        {
            var totalDeliveries = 0;

            totalDeliveries = await _deliveryRepository.GetTotalDeliveriesCountAsync(SearchFilter, CalculateTimeSpan());

            TotalPages = (int)Math.Ceiling(totalDeliveries / (double)_pageSize);
        }
        [RelayCommand]
        async Task NextPageAsync()
        {
            if (CanNavigateNext)
            {
                await LoadDeliveriesAsync(CurrentPage + 1);  
            }

            //Reset the checkbox header of Deliveries data grid
            DeselectAllDeliveries();
        }
        [RelayCommand]
        async Task PreviousPageAsync()
        {
            if (CanNavigatePrevious)
            {
                await LoadDeliveriesAsync(CurrentPage - 1);
            }

            //Reset the checkbox header of Deliveries data grid
            DeselectAllDeliveries();
        }
    }
}
