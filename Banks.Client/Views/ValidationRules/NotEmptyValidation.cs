using System.Globalization;
using System.Windows.Controls;

namespace Banks.Client.Views.ValidationRules
{
    public class NotEmptyValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return string.IsNullOrWhiteSpace((string)value)
                ? new ValidationResult(false, "Field cannot be empty")
                : ValidationResult.ValidResult;
        }
    }
}