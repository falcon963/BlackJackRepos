using BooksLibrary.Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BooksLibrary.Identity.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Email", ResourceType = typeof(StringsDictionary))]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(StringsDictionary))]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password), ErrorMessageResourceName = "PasswordDontCompare", ErrorMessageResourceType = typeof(StringsDictionary))]
        [DataType(DataType.Password)]
        [Display(Name = "ConfirmedPassword", ResourceType = typeof(StringsDictionary))]
        public string PasswordConfirm { get; set; }
    }
}
