using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokerronBank.UI.Views.Popups;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PokerronBank.UI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IngresosView : ContentView
    {
        public IngresosView()
        {
            BindingContext = ViewModelViewManager.MainViewModel;
            InitializeComponent();
        }
        private bool canClickBotonNuevoIngreso = true;

        [Obsolete]
        private  async void BotonNuevoIngreso(object sender, EventArgs e)
        {
            if (!canClickBotonNuevoIngreso) return;
            canClickBotonNuevoIngreso = false;
            if (ViewModelViewManager.MainViewModel.Partida.Reference.Terminada)
            {
                await Application.Current.MainPage.DisplayAlert("", "No es posible hacer el ingreso \nLa partida esta termniada", "Ok");
            }
            else
            {
                await PopupNavigation.PushAsync(new PopupAddIngreso());
            }
            
            canClickBotonNuevoIngreso = true;
          
        }
    }
}