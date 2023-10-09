using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace la_mia_pizzeria_static.ValidationCustom
{
    public class AtLeastFiveWords : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is string)
            {
                string imputValue = (string)value;

                if (imputValue == null ||  imputValue.Split(' ').Length <= 4) 
                {
                    return new ValidationResult("Il campo deve contenere almeno 5 parole");
                }
                return ValidationResult.Success;
            }

            return new ValidationResult("Il campo inserito non è di tipo stringa");
        }
    }
}
