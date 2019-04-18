using System;
using System.Collections.Generic;
using System.Text;
using TestProject.Core.Helpers.Interfaces;

namespace TestProject.Core.Helpers
{
    public class CheckNullOrWhiteSpaceHelper
        : ICheckNullOrWhiteSpaceHelper
    {
        public CheckNullOrWhiteSpaceHelper()
        {

        }

        public bool Check2Strings(string field1, string field2)
        {
            if (String.IsNullOrEmpty(field1)
                || (String.IsNullOrWhiteSpace(field1))
                || (String.IsNullOrEmpty(field2))
                || (String.IsNullOrWhiteSpace(field2)))
            {
                return false;
            }
            return true;
        }

        public bool Check3Strings(string field1, string field2, string field3)
        {
            if (String.IsNullOrEmpty(field1)
               || (String.IsNullOrWhiteSpace(field1))
               || (String.IsNullOrEmpty(field2))
               || (String.IsNullOrWhiteSpace(field2))
               || (String.IsNullOrEmpty(field3))
               || (String.IsNullOrWhiteSpace(field3)))
            {
                return false;
            }
            return true;
        }

        public bool Check2FieldsConfirm(string field1, string field2)
        {
            if (String.IsNullOrEmpty(field1)
               && (String.IsNullOrWhiteSpace(field1))
               && (String.IsNullOrEmpty(field2))
               && (String.IsNullOrWhiteSpace(field2)))
            {
                return false;
            }
            return true;
        }

        public bool Check3FieldsConfirm(string field1, string field2, string field3)
        {
            if (String.IsNullOrEmpty(field1)
               && (String.IsNullOrWhiteSpace(field1))
               && (String.IsNullOrEmpty(field2))
               && (String.IsNullOrWhiteSpace(field2))
               && (String.IsNullOrEmpty(field3))
               && (String.IsNullOrWhiteSpace(field3)))
            {
                return false;
            }
            return true;
        }
    }
}
