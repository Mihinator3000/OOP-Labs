using System.Globalization;
using System.Windows.Controls;
using Banks.Entities.Clients.Passport;
using Banks.Tools;

namespace Banks.Client.Views.ValidationRules
{
    public class PassportValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string passport = (string)value;

            if (string.IsNullOrEmpty(passport))
                return ValidationResult.ValidResult;

            try
            {
                _ = new PassportInfo(passport);
                return ValidationResult.ValidResult;
            }
            catch (BanksException)
            {
                return new ValidationResult(false, "Invalid passport info");
            }
        }
    }
}