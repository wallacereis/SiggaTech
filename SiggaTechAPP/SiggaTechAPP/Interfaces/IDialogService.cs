using System.Threading.Tasks;

namespace SiggaTechAPP.Interfaces
{
    public interface IDialogService
    {
        Task ShowAlertAsync(string message, string title, string buttonLabel);
        void ShowToast(string message);
    }
}
