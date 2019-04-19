using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProject.Core.Models;
using TestProject.Core.ViewModels;

namespace TestProject.Core.Interfacies
{
    public interface IValidationService<T> where T: BaseViewModel
    {
        bool GetValidationPassword(PasswordValidationModel model);
        bool GetViewModelValidation(T view);
        List<string> GetValidationError();
    }
}
