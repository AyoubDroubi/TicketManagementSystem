using System.ComponentModel.DataAnnotations;

namespace TicketManagementUI.Extensions.Helpers
{
    public class ValidationHelpers
    {
        public List<ValidationResult> validationResults = new List<ValidationResult>();
        ValidationContext validationContext;
        object instance;

        public ValidationHelpers(object instance)
        {
            this.instance = instance;
            validationContext = new ValidationContext(instance);
        }

        public bool Validate()
        {
            return Validator.TryValidateObject(instance, validationContext, validationResults, true);
        }
    }
}
