using MvvmHelpers.Commands;
using MvvmHelpers.Interfaces;
using SiggaTechAPP.Models;
using SiggaTechAPP.Interfaces;
using SiggaTechAPP.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Linq;

namespace SiggaTechAPP.ViewModels
{
    sealed class HomePageViewModel : BasePageViewModel
    {
        #region Properties
        /*---------------------- ObservableCollection Properties ----------------------*/
        private ObservableCollection<User> _user;
        public ObservableCollection<User> Users
        {
            get { return _user; }
            set { SetProperty(ref _user, value); }
        }
        #endregion

        #region Privates
        private readonly ISQLiteRetositoryBase<User> _repositoryBase;
        private readonly IFlurlAPI<User> _flurlAPI;
        private readonly IDialogService _dialogService;
        #endregion

        #region Commands
        public IAsyncCommand<User> SelectionChangedCommand => new AsyncCommand<User>((User obj) => SelectionChangedCommandExecuteAsync(obj));
        public IAsyncCommand RefreshingCommand => new AsyncCommand(() => RefreshingCommandExecuteAsync());
        public IAsyncCommand LogoutCommand => new AsyncCommand(() => LogoutCommandExecuteAsync());
        #endregion

        #region Constructor
        public HomePageViewModel()
        {
            _repositoryBase = DependencyService.Get<ISQLiteRetositoryBase<User>>();
            _flurlAPI = DependencyService.Get<IFlurlAPI<User>>();
            _dialogService = DependencyService.Get<IDialogService>();
        }
        #endregion

        #region Overrides
        public override async Task Initialize(params object[] args)
        {
            await base.Initialize(args);
            await LoadUsersAsync();
        }
        #endregion

        #region Private Methods
        private async Task LoadUsersAsync()
        {
            try
            {
                IsBusy = true;

                List<User> listUsers = null;

                if (InternetConnectionActive())
                {
                    Title = "Users On-Line";
                    listUsers = await _flurlAPI.GetAllItemsAsync("users");
                }
                else
                {
                    Title = "Users Off-Line";
                    listUsers = await _repositoryBase.GetAllItemsAsync<User>();
                }
                if (listUsers.Count > 0)
                {
                    Users = new ObservableCollection<User>(listUsers);
                    Users.Select(u => {u.IsOnline = InternetConnectionActive(); return u; }).ToList();
                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                _dialogService.ShowToast(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task RefreshingCommandExecuteAsync()
        {
            await LoadUsersAsync();
        }

        private async Task SelectionChangedCommandExecuteAsync(User user)
        {
            await NavigationService.Navigate<UserProfileViewModel>(user);
        }

        private async Task LogoutCommandExecuteAsync()
        {
            await NavigationService.Navigate<LoginViewModel>();
        }
        #endregion
    }
}
