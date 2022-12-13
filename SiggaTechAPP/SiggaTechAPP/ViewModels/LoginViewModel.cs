﻿using PCLExt.FileStorage.Folders;
using PCLExt.FileStorage;
using SiggaTechAPP.Models;
using SiggaTechAPP.Interfaces;
using SiggaTechAPP.ViewModels.Base;
using SQLite;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Text.RegularExpressions;
using System;
using System.Linq;
using SiggaTechAPP.Services.Navigation;
using MvvmHelpers.Commands;
using Acr.UserDialogs;
using MvvmHelpers.Interfaces;
using System.Collections.Generic;

namespace SiggaTechAPP.ViewModels
{
    sealed class LoginViewModel : BasePageViewModel
    {
        #region Properties
        /*---------------------- Email Properties ----------------------*/
        private string _email;
        public string Email
        {
            get { return _email; }
            set { SetProperty(ref _email, value); }
        }
        /*---------------------- Password Properties ----------------------*/
        private string _password;
        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }
        #endregion

        #region Privates
        private readonly IFlurlAPI<User> _flurlAPI;
        private readonly IFlurlAPI<Post> _flurlPostAPI;

        private readonly ISQLiteRetositoryBase<User> _repositoryUser;
        private readonly ISQLiteRetositoryBase<Address> _repositoryAddress;
        private readonly ISQLiteRetositoryBase<GeoLocation> _repositoryGeoLocation;
        private readonly ISQLiteRetositoryBase<Company> _repositoryCompany;
        private readonly ISQLiteRetositoryBase<Post> _repositoryPost;
        private readonly IDialogService _dialogService;
        Company lastSelection;
        #endregion

        #region Commands
        public IAsyncCommand LoginCommand => new AsyncCommand(() => LoginCommandExecuteAsync());
        #endregion

        #region Constructor
        public LoginViewModel()
        {
            _flurlAPI = DependencyService.Get<IFlurlAPI<User>>();
            _flurlPostAPI = DependencyService.Get<IFlurlAPI<Post>>();

            _repositoryUser = DependencyService.Get<ISQLiteRetositoryBase<User>>();
            _repositoryAddress = DependencyService.Get<ISQLiteRetositoryBase<Address>>();
            _repositoryGeoLocation = DependencyService.Get<ISQLiteRetositoryBase<GeoLocation>>();
            _repositoryCompany = DependencyService.Get<ISQLiteRetositoryBase<Company>>();
            _repositoryPost = DependencyService.Get<ISQLiteRetositoryBase<Post>>();
            _dialogService = DependencyService.Get<IDialogService>();
        }
        #endregion

        #region Overrides
        public override async Task Initialize(params object[] args)
        {
            await base.Initialize(args);
            await CreateTablesSQLiteAsync();
        }
        #endregion

        #region Private Methods
        private async Task LoginCommandExecuteAsync()
        {
            try
            {
                UserDialogs.Instance.ShowLoading("Processing, wait...", MaskType.Black);
                if (!UserLoginIsValid())
                {
                    _dialogService.ShowToast("Por favor, informe o E-mail e a Senha!");
                    return;
                }

                if (!UserEmailIsValid())
                {
                    _dialogService.ShowToast("Por favor, informe um E-mail válido!");
                    return;
                }

                if (LoginSiggaIsValid())
                {
                    await NavigationService.Navigate<HomePageViewModel>();
                    return;
                }

                List<User> user = null;
                if (InternetConnectionActive()) 
                    user = await _flurlAPI.GetItemByLoginAsync("users", Email, Password);
                else
                    user = await _repositoryUser.GetAllItemsAsync<User>(u => u.Email == Email && u.UserName == Password);
                
                if (user.Count == 0)
                {
                    _dialogService.ShowToast("Usuário não encontrado! Por favor, verifique suas credenciais!");
                    return;
                }

                await NavigationService.Navigate<UserProfileViewModel>(user.FirstOrDefault());
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                _dialogService.ShowToast(ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        private bool UserLoginIsValid()
        {
            return !string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(Password);
        }

        private bool UserEmailIsValid()
        {
            return Regex.IsMatch(Email.Trim(), @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
        }

        private bool LoginSiggaIsValid()
        {
            return Email == "contato@sigga.com" && Password == "sigga@123";
        }

        private async Task CreateTablesSQLiteAsync()
        {
            try
            {
                if (!InternetConnectionActive())
                {
                    await DisplayAlert("SiggaTech", "Could not load local database. Check your connection and try again.", "OK!");
                    return;
                }

                UserDialogs.Instance.ShowLoading("Loading Local Database, wait...", MaskType.Black);
                IFolder folder = new LocalRootFolder();
                ExistenceCheckResult fileExists = await folder.CheckExistsAsync("SiggaTech.db3");
                if (fileExists == ExistenceCheckResult.NotFound)
                {
                    IFile _file = folder.CreateFile("SiggaTech.db3", CreationCollisionOption.OpenIfExists);

                    using (var db = new SQLiteConnection(_file.Path))
                    {
                        //------------ Create Tables in SQLite ------------
                        db.CreateTable<User>();
                        db.CreateTable<Address>();
                        db.CreateTable<GeoLocation>();
                        db.CreateTable<Company>();
                        db.CreateTable<Post>();
                    }

                    //------------ First Offline Data Load ------------
                    await InsertItemsInSQLiteAsync();
                }
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                _dialogService.ShowToast(ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        private async Task InsertItemsInSQLiteAsync()
        {
            try
            {
                var result = await _flurlAPI.GetAllItemsAsync("users");

                foreach(var item in result)
                {
                    //------------ Insert User ------------
                    await _repositoryUser.AddItemAsync(item);

                    //------------ Insert Address ------------
                    await InsertAddressInSQLiteAsync(item);

                    //------------ Insert Location ------------
                    await InsertGeoLocationSQLiteAsycnc(item);

                    //------------ Insert Company ------------
                    await InsertCompanyInSQLiteAsync(item);

                    //------------ Insert Posts ------------
                    await InsertPostsInSQLiteAsync(item.Id);
                }
            }
            catch (Exception ex)
            {
                _dialogService.ShowToast(ex.Message);
            }
        }

        private async Task InsertAddressInSQLiteAsync(User user)
        {
            try
            {
                var address = new Address
                {
                    UserId = user.Id,
                    Street = user.Address.Street,
                    Suite = user.Address.Suite,
                    City = user.Address.City,
                    ZipCode = user.Address.ZipCode
                };
                await _repositoryAddress.AddItemAsync(address);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task InsertGeoLocationSQLiteAsycnc(User user)
        {
            try
            {
                var geoLocation = new GeoLocation
                {
                    AddressId = user.Id,
                    Latitude = user.Address.GeoLocation.Latitude,
                    Longitude = user.Address.GeoLocation.Longitude,
                };
                await _repositoryGeoLocation.AddItemAsync(geoLocation);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task InsertCompanyInSQLiteAsync(User item)
        {
            try
            {
                var company = new Company
                {
                    UserId = item.Id,
                    Name = item.Company.Name,
                    CatchPhrase = item.Company.CatchPhrase,
                    Bs = item.Company.Bs
                };
                await _repositoryCompany.AddItemAsync(company);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task InsertPostsInSQLiteAsync(int id)
        {
            try
            {
                var result = await _flurlPostAPI.GetAllItemsAsync("posts", "userId", id.ToString());

                foreach (var item in result)
                {
                    var post = new Post
                    {
                        UserId = id,
                        Title = item.Title,
                        Body = item.Body
                    };
                    await _repositoryPost.AddItemAsync(post);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        #endregion
    }
}