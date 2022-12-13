using SiggaTechAPP.Models;
using SiggaTechAPP.Interfaces;
using SiggaTechAPP.Repositories;
using SiggaTechAPP.Services.Http;
using SiggaTechAPP.Services.Message;
using SiggaTechAPP.Services.Navigation;
using System.Globalization;
using Xamarin.Forms;

namespace SiggaTechAPP
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            /*------------- Start Navigation Service -------------*/
            NavigationService.Current.Initialize();

            /*------------- Culture Info -------------*/
            var cultureInfo = new CultureInfo("pt-BR");
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;

            /*------------- Register Dependency Service -------------*/
            RegisterDependencyService();
        }

        /*------------- Register Dependency Service -------------*/
        private void RegisterDependencyService()
        {
            /*------------- Register Singleton Flurl -------------*/
            DependencyService.RegisterSingleton<IFlurlAPI<User>>(new FlurlAPI<User>());
            DependencyService.RegisterSingleton<IFlurlAPI<Post>>(new FlurlAPI<Post>());

            /*------------- Register Singleton SQLite -------------*/
            DependencyService.RegisterSingleton<ISQLiteRetositoryBase<User>>(new SQLiteRetositoryBase<User>());
            DependencyService.RegisterSingleton<ISQLiteRetositoryBase<Address>>(new SQLiteRetositoryBase<Address>());
            DependencyService.RegisterSingleton<ISQLiteRetositoryBase<GeoLocation>>(new SQLiteRetositoryBase<GeoLocation>());
            DependencyService.RegisterSingleton<ISQLiteRetositoryBase<Company>>(new SQLiteRetositoryBase<Company>());
            DependencyService.RegisterSingleton<ISQLiteRetositoryBase<Post>>(new SQLiteRetositoryBase<Post>());

            /*------------- Register Singleton Acr.UserDialogs -------------*/
            DependencyService.RegisterSingleton<IDialogService>(new DialogService());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
