using Comtec.BE.LogEx;
using Comtec.DL.Model;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Comtec.AS400XML.Output.Model.Base {
    public abstract class BaseAllowedValuesValidationModel : BaseEntityIdModel {
        public bool IsValidXmlElement<T>(List<T>? list) where T : BaseAllowedValuesValidationModel {
            bool result = true;

            if (list != null) {
                foreach (var item in list) {
                    if (!item.IsValidXmlElement()) {
                        result = false;
                    }
                }
            }

            return result;
        }

        public bool IsValidXmlElement() {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(this);

            bool isValid = Validator.TryValidateObject(this, context, results, true);

            if (isValid) {
                return true;
            } else {
                foreach (var validationResult in results) {
                    LogHelper.WriteError(MethodBase.GetCurrentMethod(), validationResult.ErrorMessage);
                }

                return false;
            }
        }
    }
}