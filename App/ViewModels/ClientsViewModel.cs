using CommunityToolkit.Mvvm.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Courier_Data_Control_App.Models;
using Courier_Data_Control_App.Repositories;
using Courier_Data_Control_App.Services;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Courier_Data_Control_App.ViewModels
{
    /// <summary>
    /// View model for clients page
    /// </summary>
    public partial class ClientsViewModel : ObservableObject
    {
        private readonly ClientRepository _clientRepository;
        private readonly ISharedDataService _sharedDataService;

        public ObservableCollection<Client> Clients => _sharedDataService.Clients;

        [ObservableProperty]
        private Client newClient = new Client();

        //Properties to apply searching
        [ObservableProperty]
        private string searchClientName = string.Empty;
        [ObservableProperty]
        private bool isFiltering;

        // START: Methods and properties to handle editing a client item.
        private Client _currentClient;
        public Client CurrentClient
        {
            get => _currentClient;
            set
            {
                if (SetProperty(ref _currentClient, value))
                {
                    _currentClient?.BeginEdit();
                }
            }
        }
        public void CancelEdit()
        {
            CurrentClient?.CancelEdit();
        }
        private void OnClientsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (INotifyPropertyChanged item in e.OldItems)
                    item.PropertyChanged -= OnClientPropertyChanged;
            }
            if (e.NewItems != null)
            {
                foreach (INotifyPropertyChanged item in e.NewItems)
                    item.PropertyChanged += OnClientPropertyChanged;
            }
        }
        private void OnClientPropertyChanged(object sender, PropertyChangedEventArgs e)
        {

        }
        // END: Methods and properties to handle editing a client item.

        public ClientsViewModel(ClientRepository clientRepository, ISharedDataService sharedDataService)
        {
            _clientRepository = clientRepository;
            _sharedDataService = sharedDataService;

            foreach (var client in Clients)
            {
                client.PropertyChanged += OnClientPropertyChanged;
            }

            Clients.CollectionChanged += OnClientsCollectionChanged;
        }   

        /// <summary>
        /// Gets the current clients for the collection of the view model
        /// </summary>
        [RelayCommand]
        async Task SearchClientsAsync()
        {
            await LoadClientsAsync();

            if (!string.IsNullOrEmpty(SearchClientName))
            {
                IsFiltering = true;
            }
            else
            {
                IsFiltering = false;
            }
        }
        async Task LoadClientsAsync()
        {
            var clients = await _clientRepository.GetAllClientsAsync(SearchClientName);

            if (clients.Count() == 0)
            {
                MessageBox.Show($"No se han encontrado clientes que coincidan con sus criterios de búsqueda. Vuelva a intentarlo con otro nombre.",
                    "", MessageBoxButton.OK, MessageBoxImage.Information);

                await ClearFilters();
                return;
            }

            Clients.Clear();

            foreach (var client in clients)
            {
                Clients.Add(client);
            }
        }

        /// <summary>
        /// Clear search or filters applied
        /// </summary>
        [RelayCommand]
        async Task ClearFilters()
        {
            SearchClientName = String.Empty;
            IsFiltering = false;
            await LoadClientsAsync();
        }

        /// <summary>
        /// Add a clients to the clients repository and add it to the collection
        /// of the view model
        /// </summary>
        [RelayCommand]
        async Task AddClientAsync()
        {
            await _clientRepository.AddClientAsync(NewClient);

            //Insert the new client in the collection in the right index
            var index = Clients
                .Select((client, i) => new { client.Name, Index = i })
                .Where(d => string.Compare(NewClient.Name, d.Name, StringComparison.Ordinal) < 0)
                .Select(d => d.Index)
                .FirstOrDefault();

            if (index == 0 && Clients.Any(d => string.Compare(NewClient.Name, d.Name, StringComparison.Ordinal) >= 0))
            {
                Clients.Add(NewClient);
            }
            else
            {
                Clients.Insert(index, NewClient);
            }

            NewClient = new Client();
        }

        /// <summary>
        /// Update the selected client in the db with its new data
        /// </summary>
        [RelayCommand]
        async Task UpdateClientAsync()
        {
            await _clientRepository.UpdateClientAsync(CurrentClient);
            CurrentClient.EndEdit(); // Clear backup if successful
        }

        /// <summary>
        /// Delete a client from the clients repository and the view model collection.
        /// </summary>
        [RelayCommand]
        async Task DeleteClientAsync()
        {
            var result = MessageBox.Show($"¿Estás seguro de que quieres eliminar a {CurrentClient.Name} de los registros?",
                "Eliminar Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                await _clientRepository.DeleteClientAsync(CurrentClient);
                Clients.Remove(CurrentClient);
            }
        }
    }
}
