using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Courier_Data_Control_App.Models
{
    public partial class Delivery : ObservableValidator
    {
        [ObservableProperty]
        private int id;

        [ObservableProperty]
        private string customerName;

        [ObservableProperty]
        [Required]
        private string phoneNumber;

        [ObservableProperty]
        [Required]
        private string address;

        [ObservableProperty]
        private string description;

        [ObservableProperty]
        private bool status;

        [ObservableProperty]
        private DateTime dateCreated = DateTime.Now;

        [ObservableProperty]
        [Required]
        private int driverId;

        [ObservableProperty]
        [Required]
        private Driver driver;

        [NotMapped]
        private bool _isSelected;

        [NotMapped]
        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }
    }
}
