using CommunityToolkit.Mvvm.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Courier_Data_Control_App.Classes;
using Courier_Data_Control_App.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Courier_Data_Control_App.ViewModels
{
    /// <summary>
    /// Wrapper Class to encapsulate multiple parameters
    /// </summary>
    public class UpdateDeliveryParameters
    {
        public Delivery SelectedDelivery { get; set; }
        public Delivery UpdatedDelivery { get; set; }
    }

    /// <summary>
    /// A view model for list of deliveries.
    /// </summary>
    public partial class DeliveriesViewModel : ObservableObject
    {
        private readonly DeliveryRepository _deliveryRepository;
        public ObservableGroupedCollection<DateTime, Delivery> Deliveries { get; private set; } = new ObservableGroupedCollection<DateTime, Delivery>();
        private int _currentPage = 1;
        private const int PageSize = 20;

        public DeliveriesViewModel(DeliveryRepository deliveryRepository)
        {
            _deliveryRepository = deliveryRepository;

            LoadDeliveriesCommand = new AsyncRelayCommand<int>(LoadDeliveriesAsync);
            AddDeliveryCommand = new AsyncRelayCommand<Delivery>(AddDeliveryAsync);
            UpdateDeliveryCommand = new AsyncRelayCommand<UpdateDeliveryParameters>(UpdateDeliveryAsync);
            DeleteDeliveryCommand = new AsyncRelayCommand<Delivery>(DeleteDeliveryAsync);

            // Load initial data
            LoadDeliveriesCommand.Execute(_currentPage);
        }

        /// <summary>
        /// Gets the current deliveries in the database for the collection of
        /// the view model using paging
        /// </summary>
        private async Task LoadDeliveriesAsync(int pageNumber)
        {
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            _currentPage = pageNumber;
            var deliveries = await _deliveryRepository.GetAllDeliveriesAsync(_currentPage, PageSize);

            Deliveries.Clear();
            Deliveries = new ObservableGroupedCollection<DateTime, Delivery>(
               deliveries.GroupBy(c => c.DateCreated.Date)
               .OrderBy(g => g.Key));

            OnPropertyChanged(nameof(Deliveries));
        }

        /// <summary>
        /// Add a delivery to the deliveries repository and add it to the collection
        /// of the view model
        /// </summary>
        private async Task AddDeliveryAsync(Delivery newDelivery)
        {
            Deliveries.AddItem(newDelivery.DateCreated.Date, newDelivery);

            await _deliveryRepository.AddDeliveryAsync(newDelivery);
        }

        /// <summary>
        /// Update the selected delivery by deleting and adding it to the deliveries repository
        /// and the view model collection with their new data
        /// </summary>
        private async Task UpdateDeliveryAsync(UpdateDeliveryParameters updateDeliveryParameters)
        {
            Deliveries.FirstGroupByKey(updateDeliveryParameters.SelectedDelivery.DateCreated.Date).Remove(updateDeliveryParameters.SelectedDelivery);

            Deliveries.AddItem(updateDeliveryParameters.UpdatedDelivery.DateCreated.Date, updateDeliveryParameters.UpdatedDelivery);

            await _deliveryRepository.UpdateDeliveryAsync(updateDeliveryParameters.UpdatedDelivery);
        }

        /// <summary>
        /// Delete a delivery from the deliveries repository and the view model collection.
        /// </summary>
        private async Task DeleteDeliveryAsync(Delivery selectedDelivery)
        {
            Deliveries.FirstGroupByKey(selectedDelivery.DateCreated.Date).Remove(selectedDelivery);

            await _deliveryRepository.DeleteDeliveryAsync(selectedDelivery);
        }

        public IAsyncRelayCommand LoadDeliveriesCommand { get; }
        public IAsyncRelayCommand AddDeliveryCommand { get; }
        public IAsyncRelayCommand UpdateDeliveryCommand { get; }
        public IAsyncRelayCommand DeleteDeliveryCommand { get; }

    }
}
