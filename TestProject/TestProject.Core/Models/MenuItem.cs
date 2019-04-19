using System;
using System.Collections.Generic;
using System.Text;
using TestProject.Core.Enums;

namespace TestProject.Core.Models
{
    public class MenuItem
    {
        public string ItemTitle { get; set; }
        public Action ItemAction {get;set;}
    }
}
