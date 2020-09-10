using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PokerronBank.UI.XamarinForms.Services;
using PokerronBank.UI.XamarinForms.Views;

namespace PokerronBank.UI.XamarinForms
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
