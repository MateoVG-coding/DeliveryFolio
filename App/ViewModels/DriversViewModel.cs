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
    public class UpdateDriverParameters
    {
        public Driver SelectedDriver { get; set; }
        public Driver UpdatedDriver { get; set; }
    }

    /// <summary>
    /// A view model for list of drivers.
    /// </summary>
    internal class DriversViewModel : ObservableObject
    {
        private readonly DriverRepository _driverRepository;
        public ObservableGroupedCollection<string, Driver> Drivers { get; private set; } = new ObservableGroupedCollection<string, Driver>();

        public DriversViewModel(DriverRepository driverRepository)
        {
            _driverRepository = driverRepository;

            LoadDriversCommand = new AsyncRelayCommand(LoadDriversAsync);
            AddDriverCommand = new AsyncRelayCommand<Driver>(AddDriverAsync);
            UpdateDriverCommand = new AsyncRelayCommand<UpdateDriverParameters>(UpdateDriverAsync);
            DeleteDriverCommand = new AsyncRelayCommand<Driver>(DeleteDriverAsync);

            // Load initial data
            LoadDriversCommand.Execute(null);
        }

        /// <summary>
        /// Gets the current drivers in the database for the collection of
        /// the view model
        /// </summary>
        private async Task LoadDriversAsync()
        {
            var drivers = await _driverRepository.GetAllDriversAsync();

            Drivers = new ObservableGroupedCollection<string, Driver>(
                drivers.GroupBy(c => c.FullName)
                .OrderBy(g => g.Key));

            OnPropertyChanged(nameof(Drivers));
        }

        /// <summary>
        /// Add a driver to the drivers repository and add it to the collection
        /// of the view model
        /// </summary>
        private async Task AddDriverAsync(Driver newDriver)
        {
            Drivers.AddItem(newDriver.FullName, newDriver);

            await _driverRepository.AddDriverAsync(newDriver);
        }

        /// <summary>
        /// Update the selected driver by deleting and adding it to the drivers repository
        /// and the view model collection with their new data
        /// </summary>
        private async Task UpdateDriverAsync(UpdateDriverParameters updateDriverParameters)
        {
            Drivers.FirstGroupByKey(updateDriverParameters.SelectedDriver.FullName).Remove(updateDriverParameters.SelectedDriver);

            Drivers.AddItem(updateDriverParameters.UpdatedDriver.FullName, updateDriverParameters.UpdatedDriver);

            await _driverRepository.UpdateDriverAsync(updateDriverParameters.UpdatedDriver);
        }

        /// <summary>
        /// Delete a driver from the drivers repository and the view model collection.
        /// </summary>
        private async Task DeleteDriverAsync(Driver selectedDriver)
        {
            Drivers.FirstGroupByKey(selectedDriver.FullName).Remove(selectedDriver);

            await _driverRepository.DeleteDriverAsync(selectedDriver);
        }

        public IAsyncRelayCommand LoadDriversCommand { get; }
        public IAsyncRelayCommand AddDriverCommand { get; }
        public IAsyncRelayCommand UpdateDriverCommand { get; }
        public IAsyncRelayCommand DeleteDriverCommand { get; }
    }
}
