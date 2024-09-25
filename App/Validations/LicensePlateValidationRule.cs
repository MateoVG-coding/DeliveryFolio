using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Courier_Data_Control_App.Validations
{
    public class LicensePlateValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string input = value as string;

            if (string.IsNullOrWhiteSpace(input))
            {
                return new ValidationResult(false, "La placa de matrícula es obligatoria.");
            }

            var hasLetter = new Regex(@"[a-zA-Z]");
            var hasDigit = new Regex(@"[0-9]");
            var hasValidCharacters = new Regex(@"^[a-zA-Z0-9-]+$"); 

            if (!hasLetter.IsMatch(input) || !hasDigit.IsMatch(input) || !hasValidCharacters.IsMatch(input))
            {
                return new ValidationResult(false, "La placa de matrícula debe contener letras y números.");
            }

            return ValidationResult.ValidResult;
        }
    }
}
