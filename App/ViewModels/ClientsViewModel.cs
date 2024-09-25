using CommunityToolkit.Mvvm.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Courier_Data_Control_App.Models;
using Courier_Data_Control_App.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
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

        /// <summary>
        /// Gets the current clients in the database for the collection of
        /// the view model
        /// </summary>
        [RelayCommand]
        async Task LoadClientsAsync()
        {
            var clients = await _clientRepository.GetAllClientsAsync();

            Clients = new ObservableGroupedCollection<string, Client>(
                clients.GroupBy(c => c.Name)
                .OrderBy(g => g.Key));
        }

        /// <summary>
        /// Add a client to the clients repository and add it to the collection
        /// of the view model
        /// </summary>
        [RelayCommand]
        async Task AddClientAsync(Client newClient)
        {
            Clients.AddItem(newClient.Name, newClient);

            await _clientRepository.AddClientAsync(newClient);
        }

        /// <summary>
        /// Update the selected client by deleting and adding it to the clients repository
        /// and the view model collection with their new data
        /// </summary>
        [RelayCommand]
        async Task UpdateClientAsync(UpdateClientParameters updateClientParameters)
        {
            Clients.FirstGroupByKey(updateClientParameters.SelectedClient.Name).Remove(updateClientParameters.SelectedClient);

            Clients.AddItem(updateClientParameters.UpdatedClient.Name, updateClientParameters.UpdatedClient);

            await _clientRepository.UpdateClientAsync(updateClientParameters.UpdatedClient);
        }

        /// <summary>
        /// Delete a client from the clients repository and the view model collection.
        /// </summary>
        [RelayCommand]
        async Task DeleteClientAsync(Client selectedClient)
        {
            Clients.FirstGroupByKey(selectedClient.Name).Remove(selectedClient);

            await _clientRepository.DeleteClientAsync(selectedClient);
        }
    }
}
