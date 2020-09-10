using System;
using System.Collections.Generic;
using System.Text;
using PokerronBank.UI.ViewModels;
using PokerronBank.UI.Views;
using Xamarin.Forms;

namespace PokerronBank.UI
{
    public static class ViewModelViewManager
    {
        //Views
        public static MainPage MainPage { get; set; }
        public static PartidaView PartidaView { get; set; }
    


        //ViewModels
        public static MainViewModel MainViewModel { get; set; } = new MainViewModel();


        static ViewModelViewManager()
        {
            if (!(Application.Current.MainPage is MainPage mainPage)) return;
            //Views----
            MainPage = mainPage;
            PartidaView = new PartidaView();

            //ViewModel
            MainPage.BindingContext = MainViewModel;
            PartidaView.BindingContext = MainViewModel;
           

        }
    }
}
