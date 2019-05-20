using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TestProject.LanguageResources;

namespace TestProject.Core.Models
{
    public class TaskFieldValidationModel
    {
        [Required(ErrorMessageResourceName = nameof(Strings.TitleMustBeRequired),
            ErrorMessageResourceType = typeof(Strings))]
        public string Title { get; set; }

        [Required(ErrorMessageResourceName = nameof(Strings.NoteMustBeRequired),
            ErrorMessageResourceType = typeof(Strings))]
        public string Note { get; set; }
    }
}
