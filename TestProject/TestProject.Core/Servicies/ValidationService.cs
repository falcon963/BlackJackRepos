using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProject.Core.Models;
using TestProject.Core.Servicies.Interfacies;
using TestProject.Core.ViewModels;

namespace TestProject.Core.Servicies
{
    public class ValidationService
        : IValidationService
    {

        public ValidationService()
        {

        }

        public ModelState GetViewModelValidation(object view)
        {
            var result = new List<ValidationResult>();
            var context = new ValidationContext(view);
            bool isValid = Validator.TryValidateObject(view, context, result, true);
            List<string> error = new List<string>();

            foreach (var item in result)
            {
                error.Add(item.ErrorMessage);
            }

            ModelState modelState = new ModelState
            {
                IsValid = isValid,
                Errors = error
            };
            return modelState;
        }
    }
}
