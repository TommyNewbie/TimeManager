using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TimeManager.UI.Infrastructure.ValidationAttributes
{
    public class EndDateValidationAttribute : ValidationAttribute, IClientValidatable
    {
        private readonly string _beginDate;
        private readonly string _beginName;
        private readonly string _errorMessage = "Property {0} does not exist";

        public EndDateValidationAttribute(string beginDateDisplayName, string beginDate)
        {
            _beginName = beginDateDisplayName;
            _beginDate = beginDate;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule();
            rule.ErrorMessage = string.Format(_errorMessage, metadata.DisplayName, _beginName);
            rule.ValidationType = "morethen";
            rule.ValidationParameters.Add("id", _beginDate);
            yield return rule;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var prop = validationContext.ObjectType.GetProperty(_beginDate);
            if (prop == null)
                throw new ArgumentException(string.Format("Property {0} does not exist", _beginDate));

            var beginDate = prop.GetValue(validationContext.ObjectInstance);
            if (!(beginDate is DateTime))
                throw new ArgumentException(string.Format("Property {0} has a wrong type", _beginDate));

            if (value is DateTime && prop != null)
            {
                if((DateTime)value < (DateTime)beginDate)
                {
                    return new ValidationResult(string.Format(_errorMessage, validationContext.DisplayName, _beginName));
                }
            }
            return ValidationResult.Success;
        }
    }
}