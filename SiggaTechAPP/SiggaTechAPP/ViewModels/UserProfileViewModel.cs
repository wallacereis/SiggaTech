using SiggaTechAPP.Models;
using SiggaTechAPP.Interfaces;
using SiggaTechAPP.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using MvvmHelpers.Interfaces;
using MvvmHelpers.Commands;

namespace SiggaTechAPP.ViewModels
{
    sealed class UserProfileViewModel : BasePageViewModel
    {
        #region "Properties"
        /*---------------------- ObservableCollection Properties ----------------------*/
        private ObservableCollection<Post> _post;
        public ObservableCollection<Post> Posts
        {
            get { return _post; }
            set { SetProperty(ref _post, value); }
        }
        /*---------------------- UserProfile Properties ----------------------*/
        private User _user;
        public User UserProfile
        {
            get { return _user; }
            set { SetProperty(ref _user, value); }
        }
        #endregion

        #region Privates
        private readonly ISQLiteRetositoryBase<Post> _repositoryBase;
        private readonly IFlurlAPI<Post> _flurlAPI;
        private readonly IDialogService _dialogService;
        #endregion

        #region Commands
        public IAsyncCommand ReturnCommand => new AsyncCommand(() => ReturnCommandExecuteAsync());
        public IAsyncCommand RefreshingCommand => new AsyncCommand(() => RefreshingCommandExecuteAsync());
        #endregion

        #region Constructor        
        public UserProfileViewModel()
        {
            _repositoryBase = DependencyService.Get<ISQLiteRetositoryBase<Post>>();
            _flurlAPI = DependencyService.Get<IFlurlAPI<Post>>();
            _dialogService = DependencyService.Get<IDialogService>();
        }
        #endregion

        #region Overrides
        public override async Task Initialize(params object[] args)
        {
            await base.Initialize(args);
            //----------- Load Parameters -----------
            UserProfile = (User)args[0];
            await LoadPostsUserAsync(UserProfile);
        }
        #endregion

        #region Private Methods
        private async Task LoadPostsUserAsync(User user)
        {
            try
            {
                IsBusy = true;

                List<Post> listPosts = null;

                if (InternetConnectionActive())
                    listPosts = await _flurlAPI.GetAllItemsAsync("posts", "userId", user.Id.ToString());
                else
                    listPosts = await _repositoryBase.GetAllItemsAsync<Post>(u => u.UserId == user.Id);

                if (listPosts.Count > 0)
                {
                    Posts = new ObservableCollection<Post>(listPosts);
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
            await LoadPostsUserAsync(UserProfile);
        }

        private async Task ReturnCommandExecuteAsync()
        {
            await NavigationService.Navigate<HomePageViewModel>();
        }
        #endregion
    }
}
