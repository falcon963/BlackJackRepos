using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.Core.Models
{
    public class PasswordValidationModel
    {
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare("PasswordValidationModel.Password", ErrorMessage = "The fields Password and PasswordConfirmation should be equals")]
        public string PasswordConfirm { get; set; }
    }
}
