using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProject.Core.Models;
using TestProject.Core.Services.Interfaces;
using TestProject.Core.ViewModels;

namespace TestProject.Core.Services
{
    public class ValidationService
        : IValidationService
    {

        public ModelState Validate(object validateModel)
        {
            var result = new List<ValidationResult>();
            var context = new ValidationContext(validateModel);

            bool isValid = Validator.TryValidateObject(validateModel, context, result, true);

            List<string> errorMessageList = result.Select(p => p.ErrorMessage).ToList();

            ModelState modelState = new ModelState
            {
                IsValid = isValid,
                Errors = errorMessageList
            };

            return modelState;
        }
    }
}
