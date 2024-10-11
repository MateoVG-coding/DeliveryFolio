using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Courier_Data_Control_App.Models
{
    public partial class Driver : ObservableValidator, IEditableObject
    {
        public int Id { get; set; }

        [ObservableProperty]
        [Required]
        private string fullName;

        [ObservableProperty]
        [Required]
        private string phoneNumber;

        [ObservableProperty]
        [Required]
        private string licensePlate;

        [ObservableProperty]
        private DateTime dateCreated = DateTime.Now;

        [ObservableProperty]
        private bool status;

        [ObservableProperty]
        private string? imagePath;

        [ObservableProperty]
        private bool isInCompany = true;

        private Driver _backupDriver;

        public void BeginEdit()
        {
            _backupDriver = (Driver)this.MemberwiseClone();
        }

        public void CancelEdit()
        {
            if (_backupDriver != null)
            {
                Id = _backupDriver.Id;
                FullName = _backupDriver.FullName;
                PhoneNumber = _backupDriver.PhoneNumber;
                LicensePlate = _backupDriver.LicensePlate;
                DateCreated = _backupDriver.DateCreated;
                Status = _backupDriver.Status;
                ImagePath = _backupDriver.ImagePath;
            }
        }

        public void EndEdit()
        {
            _backupDriver = null;
        }
    }
}
