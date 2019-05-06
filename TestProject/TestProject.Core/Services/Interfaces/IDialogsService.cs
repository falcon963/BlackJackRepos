using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.Core.Services.Interfaces
{
    public interface IDialogsService
    {
        void ShowAlert(string message);

        void ShowSuccessMessage(string message);

        Task<bool> ShowConfirmDialogAsync(string message, string title);
    }
}
