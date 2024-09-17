﻿using CommunityToolkit.Mvvm.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Courier_Data_Control_App.Classes;
using Courier_Data_Control_App.Pages;
using Courier_Data_Control_App.Repositories;
using Courier_Data_Control_App.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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
        private Delivery newDelivery = new Delivery();
        public ObservableCollection<string> DeliveryTypes { get; set; }

        //Properties to apply paging
        private const int PageSize = 10;
        [ObservableProperty]
        private int totalPages;
        [ObservableProperty]
        private int currentPage;

        [ObservableProperty]
        private bool canNavigatePrevious;
        [ObservableProperty]
        private bool canNavigateNext;

        // Property and method for the header CheckBox
        [ObservableProperty]
        private bool isAllItemsSelected;
        partial void OnIsAllItemsSelectedChanged(bool value)
        {
            foreach (var delivery in Deliveries)
            {
                delivery.IsSelected = value;
            }

            LoadDeliveriesAsync(CurrentPage);
        }

        public DeliveriesViewModel(DeliveryRepository deliveryRepository, ISharedDataService sharedDataService)
        {
            _deliveryRepository = deliveryRepository;
            _sharedDataService = sharedDataService;

            DeliveryTypes = new ObservableCollection<string>
            {
                "Entrega estándar", "Entrega urgente", "Entrega de compras", "Entrega bancaria", "Entrega programada"
            };

            int pageNumber = 1;

            CalculatePagination();
            _ = LoadDeliveriesAsync(pageNumber);
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

            CalculatePagination();
            await LoadDeliveriesAsync(CurrentPage);

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
            var deliveriesToDelete = Deliveries.Where(d => d.IsSelected).ToList();

            foreach (var delivery in deliveriesToDelete)
            {
                await _deliveryRepository.DeleteDeliveryAsync(delivery);
                Deliveries.Remove(delivery);
            }

            //Ensure the user stays within valid page range
            CalculatePagination();
            if (CurrentPage > TotalPages)
            {
                CurrentPage = CurrentPage = TotalPages;
            }

            await LoadDeliveriesAsync(CurrentPage);
        }

        /// <summary>
        /// Handles the selection of the client when the user chooses from the suggestions
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

        async void CalculatePagination()
        {
            var totalDeliveries = await _deliveryRepository.GetTotalDeliveriesCountAsync();
            TotalPages = (int)Math.Ceiling(totalDeliveries / (double)PageSize);
        }

        [RelayCommand]
        async Task NextPageAsync()
        {
            if (CanNavigateNext)
            {
                await LoadDeliveriesAsync(CurrentPage + 1);
            }

            //Reset the checkbox header of Deliveries data grid
            IsAllItemsSelected = false;

        }

        [RelayCommand]
        async Task PreviousPageAsync()
        {
            if (CanNavigatePrevious)
            {
                await LoadDeliveriesAsync(CurrentPage - 1);
            }

            //Reset the checkbox header of Deliveries data grid
            IsAllItemsSelected = false;
        }
    }
}
