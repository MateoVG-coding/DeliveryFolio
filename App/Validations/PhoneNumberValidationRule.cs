using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Courier_Data_Control_App.Validations
{
    public class PhoneNumberValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string phoneNumber = (value ?? "").ToString();
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                return new ValidationResult(false, "El número de teléfono es obligatorio.");
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(phoneNumber, @"^[\d-]+$"))
            {
                return new ValidationResult(false, "El número de teléfono solo debe contener dígitos y guiones.");
            }
            return ValidationResult.ValidResult;
        }
    }
}