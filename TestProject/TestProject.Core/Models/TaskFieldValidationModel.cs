using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TestProject.Resources;

namespace TestProject.Core.Models
{
    public class TaskFieldValidationModel
    {
        [Required(ErrorMessageResourceName = "TitleMustBeRequired", ErrorMessageResourceType = typeof(Strings))]
        public string Title { get; set; }

        [Required(ErrorMessageResourceName = "NoteMustBeRequired", ErrorMessageResourceType = typeof(Strings))]
        public string Note { get; set; }
    }
}
