using CommunityToolkit.Mvvm.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Courier_Data_Control_App.Classes;
using Courier_Data_Control_App.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Courier_Data_Control_App.ViewModels
{
    /// <summary>
    /// Wrapper Class to encapsulate multiple parameters
    /// </summary>
    public class UpdateClientParameters
    {
        public Client SelectedClient { get; set; }
        public Client UpdatedClient { get; set; }
    }

    /// <summary>
    /// A view model for list of clients.
    /// </summary>
    public partial class ClientsViewModel : ObservableObject
    {
        private readonly ClientRepository _clientRepository;
        public ObservableGroupedCollection<string, Client> Clients { get; private set; } = new ObservableGroupedCollection<string, Client>();

        public ClientsViewModel(ClientRepository clientRepository)
        {
            _clientRepository = clientRepository;

            LoadClientsCommand = new AsyncRelayCommand(LoadClientsAsync);
            AddClientCommand = new AsyncRelayCommand<Client>(AddClientAsync);
            UpdateClientCommand = new AsyncRelayCommand<UpdateClientParameters>(UpdateClientAsync);
            DeleteClientCommand = new AsyncRelayCommand<Client>(DeleteClientAsync);

            // Load initial data
            LoadClientsCommand.Execute(null);
        }

        /// <summary>
        /// Gets the current clients in the database for the collection of
        /// the view model
        /// </summary>
        private async Task LoadClientsAsync()
        {
            var clients = await _clientRepository.GetAllClientsAsync();

            Clients = new ObservableGroupedCollection<string, Client>(
                clients.GroupBy(c => c.Name)
                .OrderBy(g => g.Key));

            OnPropertyChanged(nameof(Clients));
        }
        
        /// <summary>
        /// Add a client to the clients repository and add it to the collection
        /// of the view model
        /// </summary>
        private async Task AddClientAsync(Client newClient)
        {
            Clients.AddItem(newClient.Name, newClient);

            await _clientRepository.AddClientAsync(newClient);
        }

        /// <summary>
        /// Update the selected client by deleting and adding it to the clients repository
        /// and the view model collection with their new data
        /// </summary>
        private async Task UpdateClientAsync(UpdateClientParameters updateClientParameters)
        {
            Clients.FirstGroupByKey(updateClientParameters.SelectedClient.Name).Remove(updateClientParameters.SelectedClient);

            Clients.AddItem(updateClientParameters.UpdatedClient.Name, updateClientParameters.UpdatedClient);

            await _clientRepository.UpdateClientAsync(updateClientParameters.UpdatedClient);
        }

        /// <summary>
        /// Delete a client from the clients repository and the view model collection.
        /// </summary>
        private async Task DeleteClientAsync(Client selectedClient)
        {
            Clients.FirstGroupByKey(selectedClient.Name).Remove(selectedClient);

            await _clientRepository.DeleteClientAsync(selectedClient);
        }

        public IAsyncRelayCommand LoadClientsCommand { get; }
        public IAsyncRelayCommand AddClientCommand { get; }
        public IAsyncRelayCommand UpdateClientCommand { get; }
        public IAsyncRelayCommand DeleteClientCommand { get; }
    }
}
