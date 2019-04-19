using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProject.Core.Interfacies;
using TestProject.Core.Models;
using TestProject.Core.ViewModels;

namespace TestProject.Core.Servicies
{
    public class ValidationService<T>
        : IValidationService<T> where T: BaseViewModel
    {
        public List<ValidationResult> _result;
        public ValidationContext _context;

        public ValidationService(T view)
        {

        }

        public bool GetValidationPassword(PasswordValidationModel model)
        {
            _result = new List<ValidationResult>();
            _context = new ValidationContext(model);
            return Validator.TryValidateObject(model, _context, _result, true);
        }

        public bool GetViewModelValidation(T view)
        {
            _result = new List<ValidationResult>();
            _context = new ValidationContext(view);
            return Validator.TryValidateObject(view, _context, _result, true);
        }

        List<string> IValidationService<T>.GetValidationError()
        {
            List<string> error = new List<string>();
            foreach(var item in _result)
            {
                error.Add(item.ErrorMessage);
            }
            return error;
        }
    }
}
