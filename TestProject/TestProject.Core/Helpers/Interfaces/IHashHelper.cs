﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TestProject.Core.Helpers.Interfaces
{
    interface IHashHelper
    {
        string GetHash(string encryptedString);
    }
}
