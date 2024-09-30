using Comtec.AS400XML.Output.Model.AllowedData;
using System.ComponentModel.DataAnnotations;

namespace Comtec.AS400XML.Attributes {
    [AttributeUsage(AttributeTargets.Property)]
    public class AS400XmlAllowedValuesAttribute : ValidationAttribute {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
            string dicKey = $"{validationContext.ObjectType.Name}:{validationContext.MemberName}";

            var allowedValues = AllowedDataHandler.Instance.GetAllowedData(validationContext.ObjectType.Name, validationContext.MemberName);

            var stringValue = value as string;

            if (!string.IsNullOrEmpty(stringValue) && !allowedValues.Contains(stringValue)) {
                var errorMessage = $"The value '{stringValue}' not found for {dicKey}. Allowed values are: {string.Join(", ", allowedValues)}.";
                return new ValidationResult(errorMessage);
            }

            return ValidationResult.Success;
        }
    }
}