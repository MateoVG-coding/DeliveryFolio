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
    public class NameValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var Name = value as string;

            if (string.IsNullOrWhiteSpace(Name))
            {
                return new ValidationResult(false, "El nombre es obligatorio.");
            }

            if (!Regex.IsMatch(Name, @"^[a-zA-Z\s]+$"))
            {
                return new ValidationResult(false, "El nombre solo puede contener letras.");
            }

            if (Name.Length < 3)
            {
                return new ValidationResult(false, "El nombre debe tener al menos 3 caracteres.");
            }

            return ValidationResult.ValidResult;
        }
    }
}
