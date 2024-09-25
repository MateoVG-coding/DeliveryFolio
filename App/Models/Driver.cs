using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Courier_Data_Control_App.Models
{
    public class Driver : ObservableValidator
    {
        public int Id { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string PhoneNumber { get;set; }
        [Required]
        public string LicensePlate { get; set; }
        public DateTime DateCreated { get; set; }
        public bool Status { get; set; }
        public string ImagePath { get; set; }
    }
}
