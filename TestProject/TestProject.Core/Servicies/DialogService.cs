using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Text;
using TestProject.Core.Constants;
using TestProject.Core.Interfacies;
using TestProject.Resources;

namespace TestProject.Core.Servicies
{
    public class DialogsService
        : IDialogsService
    {
        public void UserDialogAlert(string messenge)
        {
            var alert = UserDialogs.Instance.Alert(
                            new AlertConfig
                            {
                                Message = messenge,
                                OkText = Strings.OkText,
                                Title = Strings.AlertMessege
                            });
        }
    }
}
