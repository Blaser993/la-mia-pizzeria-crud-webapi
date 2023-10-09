using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace la_mia_pizzeria_static.ValidationCustom
{
    public class PositiveDecimal : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is float)
            {
                   
                float imputValue = (float)value;
                    if (imputValue == null || imputValue <= 0)
                    {
                    return new ValidationResult("Il campo deve contenere un valore positivo");
                    }
                    return ValidationResult.Success;


            }
            return new ValidationResult("Dato non valido");


        }
    }
}