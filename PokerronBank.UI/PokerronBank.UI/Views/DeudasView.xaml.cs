using System;
using PokerronBank.UI.ViewModels.Helper;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PokerronBank.UI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeudasView : ContentView
    {
        public DeudasView()
        {
            BindingContext = ViewModelViewManager.MainViewModel;
            InitializeComponent();

        }


        public void PageAppearing()
        {
        }
        public void PageDisappearing()
        {
        }


        private bool canClickCalcularDeudas = true;

        private async void CalcularDeudas(object sender, EventArgs e)
        {
            if (!canClickCalcularDeudas) return;
            canClickCalcularDeudas = false;
            if (!ViewModelViewManager.MainViewModel.CajaCuadra)
            {
                await Application.Current.MainPage.DisplayAlert("","No es posible cacular deudas \nLa caja no cuadra", "Ok");
            }
            ViewModelViewManager.MainViewModel.CalcularDeudas.Execute(null);
            canClickCalcularDeudas = true;
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {

        }
        private bool canClickEnviarDeudas = true;
        private async void EnviarDeudas(object sender, EventArgs e)
        {
            if (!canClickEnviarDeudas) return;
            canClickEnviarDeudas = false;
            if (!ViewModelViewManager.MainViewModel.CajaCuadra)
            {
                await Application.Current.MainPage.DisplayAlert("", "No es posible enviar deudas \nLa caja no cuadra", "Ok");
            }
            else if (await Application.Current.MainPage.DisplayAlert("Confirmar enviar deudas", "", "Aceptar", "Cancelar"))
            {
                ViewModelViewManager.MainViewModel.EnviarTodasLasDeudas();
            }
            canClickEnviarDeudas = true;
        }

        private bool canClickReenviarDeuda = true;
        private async void ReenviarDeuda(object sender, EventArgs e)
        {
            if (!canClickReenviarDeuda) return;
            canClickReenviarDeuda = false;
            var mi = ((MenuItem)sender);
            var jugador = (mi.CommandParameter as JugadorViewItem);
            DeudasList.SelectedItem = jugador;
            var nombre = jugador?.Reference.Nombre;
            if (await Application.Current.MainPage.DisplayAlert("Reenviar deuda a " + nombre + "?", "", "Aceptar", "Cancelar"))
            {
                ViewModelViewManager.MainViewModel.EnviarDeuda(jugador);
            }
            canClickReenviarDeuda = true;
        }
    }
}