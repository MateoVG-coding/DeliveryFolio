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
using Microsoft.Win32;


namespace Courier_Data_Control_App.ViewModels
{
    /// <summary>
    /// View model for drivers page
    /// </summary>
    public partial class DriversViewModel : ObservableObject
    {
        private readonly DriverRepository _driverRepository;
        private readonly ISharedDataService _sharedDataService;

        public ObservableCollection<Driver> Drivers => _sharedDataService.Drivers;
        public ICollectionView AvailableDriversView { get; private set; }
        public ICollectionView UnavailableDriversView { get; private set; }

        [ObservableProperty]
        private Driver newDriver = new Driver();

        //Properties to apply searching
        [ObservableProperty]
        private string searchDriverName = string.Empty;
        [ObservableProperty]
        private bool isFiltering;

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

            AvailableDriversView.Refresh();
            UnavailableDriversView.Refresh();
        }
        private void OnDriverPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CurrentDriver.Status))
            {
                UpdateDriverCommand.Execute(null);
                AvailableDriversView.Refresh();
                UnavailableDriversView.Refresh();
            }

            if (e.PropertyName == nameof(CurrentDriver.ImagePath))
            {
                UpdateDriverCommand.Execute(null);
            }
        }
        // END: Methods and properties to handle editing a driver item.

        public DriversViewModel(DriverRepository driverRepository, ISharedDataService sharedDataService)
        {
            _driverRepository = driverRepository;
            _sharedDataService = sharedDataService;

            AvailableDriversView = new ListCollectionView(Drivers);
            AvailableDriversView.Filter = driver => ((Driver)driver).Status;

            UnavailableDriversView = new ListCollectionView(Drivers);
            UnavailableDriversView.Filter = driver => !((Driver)driver).Status;

            foreach (var driver in Drivers)
            {
                driver.PropertyChanged += OnDriverPropertyChanged;
            }

            Drivers.CollectionChanged += OnDriversCollectionChanged;
        }

        /// <summary>
        /// Gets the current drivers for the collection of the view model
        /// </summary>
        [RelayCommand]
        async Task SearchDriversAsync()
        {
            await LoadDriversAsync();

            if (!string.IsNullOrEmpty(SearchDriverName))
            {
                IsFiltering = true;
            }
            else
            {
                IsFiltering = false;
            }
        }
        async Task LoadDriversAsync()
        {
            var drivers = await _driverRepository.GetAllDriversAsync(SearchDriverName);

            if (drivers.Count() == 0)
            {
                MessageBox.Show($"No se han encontrado mensajeros que coincidan con sus criterios de búsqueda. Vuelva a intentarlo con otro nombre.",
                    "", MessageBoxButton.OK, MessageBoxImage.Information);

                await ClearFilters();
                return;
            }

            Drivers.Clear();

            foreach (var driver in drivers)
            {
                Drivers.Add(driver);
            }
        }

        /// <summary>
        /// Clear search or filters applied
        /// </summary>
        [RelayCommand]
        async Task ClearFilters()
        {
            SearchDriverName = String.Empty;
            IsFiltering = false;
            await LoadDriversAsync();
        }

        /// <summary>
        /// Add a driver to the drivers repository and add it to the collection
        /// of the view model
        /// </summary>
        [RelayCommand]
        async Task AddDriverAsync()
        {
            await _driverRepository.AddDriverAsync(NewDriver);

            //Insert the new driver in the collection in the right index
            var index = Drivers
                .Select((driver, i) => new { driver.FullName, Index = i })
                .Where(d => string.Compare(NewDriver.FullName, d.FullName, StringComparison.Ordinal) < 0)
                .Select(d => d.Index)
                .FirstOrDefault();

            if (index == 0 && Drivers.Any(d => string.Compare(NewDriver.FullName, d.FullName, StringComparison.Ordinal) >= 0))
            {
                Drivers.Add(NewDriver);
            }
            else
            {
                Drivers.Insert(index, NewDriver);
            }

            NewDriver = new Driver();
        }

        /// <summary>
        /// Update the selected driver in the db with its new data
        /// </summary>
        [RelayCommand]
        async Task UpdateDriverAsync()
        {
            await _driverRepository.UpdateDriverAsync(CurrentDriver);
            CurrentDriver.EndEdit(); // Clear backup if successful
        }

        /// <summary>
        /// Update the driver image path
        /// </summary>
        [RelayCommand]
        private void LoadDriverImage()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.png",
                Title = "Select Driver Image"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                CurrentDriver.ImagePath = openFileDialog.FileName;
                OnPropertyChanged(nameof(CurrentDriver));
            }
        }

        /// <summary>
        /// Delete a driver from the drivers repository and the view model collection.
        /// </summary>
        [RelayCommand]
        async Task SoftDeleteDriverAsync()
        {
            var result = MessageBox.Show($"¿Estás seguro de que quieres eliminar a {CurrentDriver.FullName} de los registros?",
                "Eliminar Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                CurrentDriver.IsInCompany = false;
                await _driverRepository.UpdateDriverAsync(CurrentDriver);
                Drivers.Remove(CurrentDriver);
            }
        }
    }
}
