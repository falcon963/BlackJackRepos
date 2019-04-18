using System;
using System.Collections.Generic;
using System.Text;

namespace TestProject.Core.Helpers.Interfaces
{
    public interface ICheckNullOrWhiteSpaceHelper
    {
        bool Check2Strings(string field1, string field2);
        bool Check3Strings(string field1, string field2, string field3);
        bool Check2FieldsConfirm(string field1, string field2);
        bool Check3FieldsConfirm(string field1, string field2, string field3);
    }
}
