using System;
using PokerronBank.UI.ViewModels.Helper;
using PokerronBank.UI.Views.Popups;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PokerronBank.UI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ComprasView : ContentView
    {
        public ComprasView()
        {
            BindingContext = ViewModelViewManager.MainViewModel;
            InitializeComponent();
        }

        private bool canClickBotonAnadirCompra= true;

        private async void BotonAnadirCompras(object sender, EventArgs e)
        {

            if (!canClickBotonAnadirCompra) return;
            canClickBotonAnadirCompra = false;
            if (ViewModelViewManager.MainViewModel.ComprasIngresoView)
            {
                await PopupNavigation.PushAsync(new PopupAddCompra());
            }
            else
            {
                if (await Application.Current.MainPage.DisplayAlert("Confirmar enviar deudas", "", "Aceptar", "Cancelar"))
                {
                    ViewModelViewManager.MainViewModel.EnviarTodasLasDeudasCompra();
                }
            }
            canClickBotonAnadirCompra = true;
        }

    
        private bool canClickBotonCambioVista = true;
        private  void BotonCambioVista(object sender, EventArgs e)
        {
            if (!canClickBotonCambioVista) return;
            canClickBotonCambioVista = false;
            ViewModelViewManager.MainViewModel.ComprasIngresoView =!ViewModelViewManager.MainViewModel.ComprasIngresoView;
            

            canClickBotonCambioVista = true;
        }

        private bool canClickOnCambiar = true;
        private async void OnCambiar(object sender, EventArgs e)
        {
            if (!canClickOnCambiar) return;
            canClickOnCambiar = false;
            var mi = ((MenuItem)sender);
            if (mi.CommandParameter == null)
            {
                canClickOnCambiar = true;
                return;
            }

            var compra = mi.CommandParameter as CompraViewItem;
            ComprasList.SelectedItem = compra;
            await PopupNavigation.PushAsync(new PopupAddCompra(compra));
            canClickOnCambiar = true;
        }



        private bool canClickOnBorrar = true;
        private async void OnBorrar(object sender, EventArgs e)
        {
            if (!canClickOnBorrar) return;
            canClickOnBorrar = false;
            var mi = ((MenuItem)sender);
            if (mi.CommandParameter == null)
            {
                canClickOnBorrar = true;
                return;
            }

            var compra = (mi.CommandParameter as CompraViewItem);
            ComprasList.SelectedItem = compra;
            var nombre = compra?.Reference.Nombre;
            var confirm = await Application.Current.MainPage.DisplayAlert("Borrar a compra " + nombre + "?", "", "Aceptar", "Cancelar");
            if (confirm)
            {
                ViewModelViewManager.MainViewModel.BorrarCompra(compra?.Reference);
            }
            canClickOnBorrar = true;
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
                ViewModelViewManager.MainViewModel.EnviarDeudaCompra(jugador?.Reference);
            }
            canClickReenviarDeuda = true;
        }
    }
}