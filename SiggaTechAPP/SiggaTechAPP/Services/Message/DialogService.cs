using Acr.UserDialogs;
using SiggaTechAPP.Interfaces;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SiggaTechAPP.Services.Message
{
    public class DialogService : IDialogService
    {
        public Task ShowAlertAsync(string message, string title, string buttonLabel)
        {
            return UserDialogs.Instance.AlertAsync(message, title, buttonLabel);
        }

        public void ShowToast(string message)
        {
            var toastConfig = new ToastConfig(message)
            .SetDuration(TimeSpan.FromSeconds(1))
            .SetPosition(ToastPosition.Bottom)
            .SetMessageTextColor(Color.White)
            .SetBackgroundColor(Color.FromHex("#064169"));
            UserDialogs.Instance.Toast(toastConfig);
        }
    }
}
