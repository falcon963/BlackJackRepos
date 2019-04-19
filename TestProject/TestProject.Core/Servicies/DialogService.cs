using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Text;
using TestProject.Core.Constants;
using TestProject.Core.Servicies.Interfacies;
using TestProject.Resources;

namespace TestProject.Core.Servicies
{
    public class DialogsService
        : IDialogsService
    {
        public void UserDialogAlert(string message)
        {
            var alert = UserDialogs.Instance.Alert(
                            new AlertConfig
                            {
                                Message = message,
                                OkText = Strings.OkText,
                                Title = Strings.AlertMessege
                            });
        }
    }
}
