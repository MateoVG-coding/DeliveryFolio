using CommunityToolkit.Mvvm.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Courier_Data_Control_App.Models;
using Courier_Data_Control_App.Pages;
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
        private Driver currentDriver = new();

        public ObservableCollection<Driver> Drivers => _sharedDataService.Drivers;
        public ObservableCollection<Driver> ActiveDrivers { get; } = new();
        public ObservableCollection<Driver> InactiveDrivers { get; } = new();

        public DriversViewModel(DriverRepository driverRepository, ISharedDataService sharedDataService)
        {
            _driverRepository = driverRepository;
            _sharedDataService = sharedDataService;
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

    public class ImageSourceToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Check if the value is null or empty
            if (string.IsNullOrEmpty(value as string))
            {
                return Visibility.Visible; // Show PackIcon if image source is not available
            }
            return Visibility.Collapsed; // Hide PackIcon if image source is available
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
