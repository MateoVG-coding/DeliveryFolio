using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Courier_Data_Control_App.Validations
{
    public class DriverValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return value == null
                ? new ValidationResult(false, "Es obligatorio seleccionar un mensajero.")
                : ValidationResult.ValidResult;
        }
    }
}
