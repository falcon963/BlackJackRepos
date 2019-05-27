using BooksLibrary.Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BooksLibrary.Identity.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = nameof(StringsDictionary.Email), ResourceType = typeof(StringsDictionary))]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = nameof(StringsDictionary.Password), ResourceType = typeof(StringsDictionary))]
        public string Password { get; set; }

        [Display(Name = nameof(StringsDictionary.RememberMe), ResourceType = typeof(StringsDictionary))]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
    }
}
