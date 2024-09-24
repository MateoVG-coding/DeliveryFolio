using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Courier_Data_Control_App.Validations
{
    public class AddressValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string address = (value ?? "").ToString();
            if (string.IsNullOrWhiteSpace(address))
            {
                return new ValidationResult(false, "La dirección de entrega es obligatoria.");
            }
            if (address.Length < 5)
            {
                return new ValidationResult(false, "La dirección debe tener al menos 5 caracteres.");
            }
            return ValidationResult.ValidResult;
        }
    }
}
