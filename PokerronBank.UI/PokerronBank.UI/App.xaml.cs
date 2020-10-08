using PokerronBank.UI.Views;
using Xamarin.Forms;

namespace PokerronBank.UI
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();



            // MainPage = new MainPage();

            MainPage = new MainView();
            //App.Current.Resources = new DarkTheme();
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
