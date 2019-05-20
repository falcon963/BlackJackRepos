using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using TestProject.LanguageResources;

namespace TestProject.Core.Models
{
    public class PasswordValidationModel
    {
        [Required]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password), 
            ErrorMessageResourceName = nameof(Strings.ConfirmPasswordMustBeComparePassword), 
            ErrorMessageResourceType = typeof(Strings))]
        public string PasswordConfirmation { get; set; }
    }
}
