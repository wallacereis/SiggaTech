using SiggaTechAPP.Services.Navigation;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using MvvmHelpersBaseViewModel = MvvmHelpers.BaseViewModel;

namespace SiggaTechAPP.ViewModels.Base
{
    abstract class BaseViewModel : MvvmHelpersBaseViewModel
    {
        #region Properties
        /*---------------------- NavigationService Properties ----------------------*/
        protected NavigationService NavigationService => NavigationService.Current;
        #endregion

        /*---------------------- Command OnException ----------------------*/
        protected void CommandOnException(Exception obj)
        {
            //throw new NotImplementedException();
        }

        /*---------------------- AsyncCommand GoBackCommand ----------------------*/
        private AsyncCommand _goBackCommand;
        public AsyncCommand GoBackCommand
            => _goBackCommand
            ??= new AsyncCommand(
                execute: () => NavigationService.GoBack(),
                //canExecute: GoBackCommandCanExecute,
                onException: CommandOnException);

        /*---------------------- Display Alert Single ----------------------*/
        public async Task DisplayAlert(string title, string message, string cancel)
        {
            await App.Current.MainPage.DisplayAlert(title, message, cancel);
        }
        /*---------------------- Display Alert Actions ----------------------*/
        public async Task<bool> DisplayAlert(string title, string message, string accept, string cancel)
        {
            return await App.Current.MainPage.DisplayAlert(title, message, accept, cancel);
        }

        /*---------------------- Xamarin Essentials Connectivity ----------------------*/
        public static bool InternetConnectionActive()
        {
            var profiles = Connectivity.ConnectionProfiles;
            if (profiles.Contains(ConnectionProfile.WiFi) || profiles.Contains(ConnectionProfile.Cellular))
                // Active Internet Connection.
                return true;
            else
                // No Internet Connection.
                return false;
        }
    }
}
