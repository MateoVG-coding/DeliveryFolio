using CommunityToolkit.Mvvm.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Courier_Data_Control_App.Classes;
using Courier_Data_Control_App.Pages;
using Courier_Data_Control_App.Repositories;
using Courier_Data_Control_App.Services;
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
        private readonly ISharedDataService _sharedDataService;

        [ObservableProperty]
        private Driver currentDriver = new Driver();

        public ObservableCollection<Driver> Drivers => _sharedDataService.Drivers;

        public DriversViewModel(DriverRepository driverRepository, ISharedDataService sharedDataService)
        {
            _driverRepository = driverRepository;
            _sharedDataService = sharedDataService;
        }

        /// <summary>
        /// Gets the current drivers in the database for the collection of
        /// the view model
        /// </summary>
        [RelayCommand]
        async Task LoadDriversAsync()
        {
            var drivers = await _driverRepository.GetAllDriversAsync();

            foreach (var driver in drivers)
            {
                Drivers.Add(driver);
            }
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
