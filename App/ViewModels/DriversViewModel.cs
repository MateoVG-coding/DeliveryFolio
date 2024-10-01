using CommunityToolkit.Mvvm.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Courier_Data_Control_App.Models;
using Courier_Data_Control_App.Repositories;
using Courier_Data_Control_App.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;
using System.Windows.Input;
using System.ComponentModel;
using Courier_Data_Control_App.Validations;
using System.Collections.Specialized;


namespace Courier_Data_Control_App.ViewModels
{
    /// <summary>
    /// A view model for list of drivers.
    /// </summary>
    public partial class DriversViewModel : ObservableObject
    {
        private readonly DriverRepository _driverRepository;
        private readonly ISharedDataService _sharedDataService;

        public ObservableCollection<Driver> Drivers => _sharedDataService.Drivers;
        public ObservableCollection<Driver> ActiveDrivers { get; } = new();
        public ObservableCollection<Driver> InactiveDrivers { get; } = new();

        // START: Methods and properties to handle editing a driver item.
        private Driver _currentDriver;
        public Driver CurrentDriver
        {
            get => _currentDriver;
            set
            {
                if (SetProperty(ref _currentDriver, value))
                {
                    _currentDriver?.BeginEdit();
                }
            }
        }
        public void CancelEdit()
        {
            CurrentDriver?.CancelEdit();
        }
        private void OnDriversCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (INotifyPropertyChanged item in e.OldItems)
                    item.PropertyChanged -= OnDriverPropertyChanged;
            }
            if (e.NewItems != null)
            {
                foreach (INotifyPropertyChanged item in e.NewItems)
                    item.PropertyChanged += OnDriverPropertyChanged;
            }
        }
        private void OnDriverPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        }
        // END: Methods and properties to handle editing a driver item.

        public DriversViewModel(DriverRepository driverRepository, ISharedDataService sharedDataService)
        {
            _driverRepository = driverRepository;
            _sharedDataService = sharedDataService;

            foreach (var driver in Drivers)
            {
                driver.PropertyChanged += OnDriverPropertyChanged;
            }

            Drivers.CollectionChanged += OnDriversCollectionChanged;
        }

        /// <summary>
        /// Add a driver to the drivers repository and add it to the collection
        /// of the view model
        /// </summary>
        [RelayCommand]
        async Task AddDriverAsync(Driver newDriver)
        {
            await _driverRepository.AddDriverAsync(newDriver);
        }

        /// <summary>
        /// Update the selected driver in the db with its new data
        /// </summary>
        [RelayCommand]
        async Task UpdateDriverAsync()
        {
            await _driverRepository.UpdateDriverAsync(CurrentDriver);
            CurrentDriver.EndEdit(); // Clear backup if successful
            MessageBox.Show("Changes saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Delete a driver from the drivers repository and the view model collection.
        /// </summary>
        [RelayCommand]
        async Task DeleteDriverAsync(Driver selectedDriver)
        {
            await _driverRepository.DeleteDriverAsync(selectedDriver);
        }
    }
}
