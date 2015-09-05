using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;

namespace TimeManager.UI.Infrastructure.ValidationAttributes
{
    public class StartsAfterTodayAttribute : ValidationAttribute, IClientValidatable
    {
        public StartsAfterTodayAttribute() : base("The {0} can't starts before today.")
        {

        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule();
            rule.ErrorMessage = FormatErrorMessage(metadata.DisplayName);
            rule.ValidationType = "startsafter";
            rule.ValidationParameters.Add("date", DateTime.Today.ToString(CultureInfo.InvariantCulture));
            yield return rule;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(value is DateTime)
            {
                var beginDate = (DateTime)value;
                if (beginDate < DateTime.Today)
                {
                    return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                }
            }
            return ValidationResult.Success;
        }
    }
}