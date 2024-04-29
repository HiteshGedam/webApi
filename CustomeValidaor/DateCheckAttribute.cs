using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace WebApiApp.CustomeValidaor
{
    public class DateCheckAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var date = (DateTime?)value;
            if (date < DateTime.Now)
            {
                return new ValidationResult("the date muxt be geater than current date.");
            }
            return ValidationResult.Success;
        }
    }
}
