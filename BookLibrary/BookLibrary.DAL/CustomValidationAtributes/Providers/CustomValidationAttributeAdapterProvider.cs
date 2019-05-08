using BooksLibrary.DAL.CustomValidationAtributes.Adapters;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BooksLibrary.DAL.CustomValidationAtributes.Providers
{
    public class CustomValidationAttributeAdapterProvider
        : IValidationAttributeAdapterProvider
    {
        IValidationAttributeAdapterProvider baseProvider =
            new ValidationAttributeAdapterProvider();

        public IAttributeAdapter GetAttributeAdapter(ValidationAttribute attribute,
            IStringLocalizer stringLocalizer)
        {
            if (attribute is ADYearAttribute)
            {
                return new ADAttributeAdapter(
                    attribute as ADYearAttribute, stringLocalizer);
            }

            return baseProvider.GetAttributeAdapter(attribute, stringLocalizer);
        }
    }
}
