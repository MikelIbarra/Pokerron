using System;
using PokerronBank.UI.ViewModels.Helper;
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


        private bool canClickReenviarDeuda = true;
        private async void ReenviarIngreso(object sender, EventArgs e)
        {
            if (!canClickReenviarDeuda) return;
            canClickReenviarDeuda = false;
            var mi = ((MenuItem)sender);
            var ingreso = (mi.CommandParameter as IngresoViewItem);
            ListIngresos.SelectedItem = ingreso;
            var nombre = ingreso?.Reference.Jugador.Nombre;
            if (await Application.Current.MainPage.DisplayAlert("Reenviar ingreso a " + nombre + "?", "", "Aceptar", "Cancelar"))
            {
                ViewModelViewManager.MainViewModel.EnviarIngreso(ingreso);
            }
            canClickReenviarDeuda = true;
        }
       
    }
}