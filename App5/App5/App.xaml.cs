using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace App5
{

    public partial class App : Application
    {
        public const string DATABASE_NAME = "TestDataBase.db";
        public App()
        {
            //string databasePath = DependencyService.Get<IPath>().GetDatabasePath(DATABASE_NAME);
            string databasePath = DependencyService.Get<IPath>().GetDatabasePath(DATABASE_NAME);
            InitializeComponent();
            MainPage = new MainPage();
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
