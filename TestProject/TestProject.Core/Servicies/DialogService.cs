using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TestProject.Core.Constants;
using TestProject.Core.Servicies.Interfacies;
using TestProject.Resources;

namespace TestProject.Core.Servicies
{
    public class DialogsService
        : IDialogsService
    {
        public void ShowAlert(string message)
        {
            UserDialogs.Instance.Alert(
                            new AlertConfig
                            {
                                Message = message,
                                OkText = Strings.OkText,
                                Title = Strings.ErrorAlert
                            });
        }

        public async Task<bool> ShowConfirmDialogAsync(string message, string title)
        {
            var alert = await UserDialogs.Instance.ConfirmAsync(new ConfirmConfig
            {
                Message = message,
                OkText = Strings.OkText,
                Title = title,
                CancelText = Strings.NoText
            });
            return alert;
        }
    }
}
