using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProject.Core.Models;
using TestProject.Core.ViewModels;

namespace TestProject.Core.Servicies.Interfaces
{
    public interface IValidationService
    {
        ModelState Validate(object validateModel);
    }
}
