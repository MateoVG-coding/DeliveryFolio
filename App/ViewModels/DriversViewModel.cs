using CommunityToolkit.Mvvm.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Courier_Data_Control_App.Classes;
using Courier_Data_Control_App.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;


namespace Courier_Data_Control_App.ViewModels
{
    /// <summary>
    /// A view model for list of drivers.
    /// </summary>
    public partial class DriversViewModel : ObservableObject
    {
        private readonly DriverRepository _driverRepository;
        public ObservableCollection<Driver> Drivers { get; private set; } = new ObservableCollection<Driver>();

        /// <summary>
        /// Gets the current drivers in the database for the collection of
        /// the view model
        /// </summary>
        private async Task LoadDriversAsync()
        {
            var drivers = await _driverRepository.GetAllDriversAsync();

        }

        /// <summary>
        /// Add a driver to the drivers repository and add it to the collection
        /// of the view model
        /// </summary>
        [RelayCommand]
        async Task AddDriverAsync(Driver newDriver)
        {
            await _driverRepository.AddDriverAsync(newDriver);
            await LoadDriversAsync();
        }

        /// <summary>
        /// Update the selected driver in the view model collection with their new data
        /// </summary>
        [RelayCommand]
        async Task UpdateDriverAsync(Driver updatedDriver)
        {
            await _driverRepository.UpdateDriverAsync(updatedDriver);
            await LoadDriversAsync();
        }

        /// <summary>
        /// Delete a driver from the drivers repository and the view model collection.
        /// </summary>
        [RelayCommand]
        async Task DeleteDriverAsync(Driver selectedDriver)
        {
            await _driverRepository.DeleteDriverAsync(selectedDriver);
            await LoadDriversAsync();
        }
    }
}
