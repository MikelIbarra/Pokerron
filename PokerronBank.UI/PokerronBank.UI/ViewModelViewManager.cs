using PokerronBank.UI.ViewModels;
using Xamarin.Forms;

namespace PokerronBank.UI
{
    public static class ViewModelViewManager
    {
        //Views
        public static MainPage MainPage { get; set; }
        //public static PartidaView PartidaView { get; set; }
        //public static IngresosView IngresosView { get; set; }
        //public static DeudasView DeudasView { get; set; }
        //public static ComprasView ComprasView { get; set; }
    


        //ViewModels
        public static MainViewModel MainViewModel { get; set; } = new MainViewModel();


        static ViewModelViewManager()
        {
            if (!(Application.Current.MainPage is MainPage mainPage)) return;
            //Views----
            MainPage = mainPage;
            //PartidaView = new PartidaView();
            //PartidaView = new PartidaView();
            //PartidaView = new PartidaView();

            //ViewModel
            MainPage.BindingContext = MainViewModel;
            //PartidaView.BindingContext = MainViewModel;
            //IngresosView.BindingContext = MainViewModel;
            //DeudasView.BindingContext = MainViewModel;
            //ComprasView.BindingContext = MainViewModel;
           

        }
    }
}
