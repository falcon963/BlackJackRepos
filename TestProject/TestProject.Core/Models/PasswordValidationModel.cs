using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using TestProject.Resources;

namespace TestProject.Core.Models
{
    public class PasswordValidationModel
    {
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare(nameof(Password), ErrorMessageResourceName = "Strings.PasswordNotEqualsPasswordConfirmation", ErrorMessageResourceType = typeof(ResourceManager))]
        public string PasswordConfirm { get; set; }
    }
}
