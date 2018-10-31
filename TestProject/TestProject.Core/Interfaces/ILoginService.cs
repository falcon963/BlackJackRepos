using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TestProject.Core.Models;

namespace TestProject.Core.Interfaces
{
    public interface ILoginService
    {
        void SetLoginAndPassword(String login, String password);
    }
}
