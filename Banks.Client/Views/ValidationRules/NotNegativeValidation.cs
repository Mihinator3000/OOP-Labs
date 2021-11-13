using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace Banks.Client.Views.ValidationRules
{
    public class NotNegativeValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return
                value is null ||
                !Regex.Match((string)value, @"^(?=.+)(?:[1-9]\d*|0)?(?:\.\d+)?$").Success
                    ? new ValidationResult(false, "Number cannot be less than zero")
                    : ValidationResult.ValidResult;
        }
    }
}