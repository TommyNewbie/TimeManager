using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Text.RegularExpressions;

namespace TimeManager.UI.Infrastructure.ValidationAttributes
{
    public class EndDateValidationAttribute : ValidationAttribute, IClientValidatable
    {
        private readonly string _beginDisplayName;
        private readonly string _elemName;

        public EndDateValidationAttribute(string beginDateDisplayName, string beginDate):base("The {0} is invalid.")
        {
            _beginDisplayName = beginDateDisplayName;
            _elemName = beginDate;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule();
            rule.ErrorMessage = FormatErrorMessage(metadata.DisplayName);
            rule.ValidationType = "morethen";
            rule.ValidationParameters.Add("id", _elemName);
            yield return rule;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var prop = validationContext.ObjectType.GetProperty(_elemName);
            if (prop == null)
                throw new ArgumentException(string.Format("Property {0} does not exist", _elemName));

            var beginDate = prop.GetValue(validationContext.ObjectInstance);
            if (!(beginDate is DateTime))
                throw new ArgumentException(string.Format("Property {0} has a wrong type", _elemName));

            if (value is DateTime && prop != null)
            {
                if ((DateTime)value <= (DateTime)beginDate)
                {
                    return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                }
            }
            return ValidationResult.Success;
        }

        private new string FormatErrorMessage(string displayName)
        {
            if (Regex.IsMatch(ErrorMessage, @"(\{0\}.*\{1\})|(\{1\}.*\{0\})"))
            {
                var message = Regex.Replace(ErrorMessage, @"\{0\}", displayName);
                return Regex.Replace(message, @"\{1\}", _beginDisplayName);
            }
            return base.FormatErrorMessage(displayName);
        }
    }
}