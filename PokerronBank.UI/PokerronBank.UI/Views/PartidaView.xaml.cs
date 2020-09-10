using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokerronBank.UI.ViewModels.Helper;
using PokerronBank.UI.Views.Popups;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Rg.Plugins.Popup.Services.PopupNavigation;


namespace PokerronBank.UI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PartidaView : ContentView
    {
        public PartidaView()
        {
            BindingContext = ViewModelViewManager.MainViewModel;

            InitializeComponent();


        }

        private bool canClickBotonNuevoJugador = true;

        [Obsolete]
        private async void BotonNuevoJugador(object sender, EventArgs e)
        {
            if (!canClickBotonNuevoJugador) return;
            canClickBotonNuevoJugador = false;
            if (!ViewModelViewManager.MainViewModel.Partida.Reference.Terminada)
            {
                await PopupNavigation.PushAsync(new PopupAddJugador());
            }
            else
            {
                var confirm = await Application.Current.MainPage.DisplayAlert("Confirmar nueva partida \nTodos los datos se borraran", "", "Aceptar", "Cancelar");
                if (confirm)
                {
                    ViewModelViewManager.MainViewModel.PartidaNueva.Execute(null);
                }
            }


            canClickBotonNuevoJugador = true;
        }

        private bool canClickBotonTerminarPartida = true;
        private async void BotonTerminarPartida(object sender, EventArgs e)
        {
            if (!canClickBotonTerminarPartida) return;
            canClickBotonTerminarPartida = false;

            if (!ViewModelViewManager.MainViewModel.Partida.Reference.Terminada)
            {
                if (!ViewModelViewManager.MainViewModel.CajaCuadra)
                {
                    await Application.Current.MainPage.DisplayAlert("", "No es posible finalizar partida \nLa caja no cuadra", "Ok");
                }
                else
                {
                    var confirm = await Application.Current.MainPage.DisplayAlert("Confirmar fin partida", "", "Aceptar", "Cancelar");
                    if (confirm)
                    {
                        ViewModelViewManager.MainViewModel.FinPartida.Execute(null);
                    }
                }
            }
            else
            {
                var confirm = await Application.Current.MainPage.DisplayAlert("Confirmar reiniciar partida", "", "Aceptar", "Cancelar");
                if (confirm)
                {
                    ViewModelViewManager.MainViewModel.ReiniciarPartida.Execute(null);
                }
            }
            
            canClickBotonTerminarPartida = true;


        }

        public int ButtonCount { get; set; }
        private void JugadoresList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (ButtonCount < 1)
            {
                TimeSpan tt = new TimeSpan(0, 0, 1);
                Device.StartTimer(tt, TestHandleFunc);
            }
            ButtonCount++;
        }

        private bool TestHandleFunc()
        {
            if (ButtonCount > 1)
            {
                
            }
            else
            {
                
            }
            ButtonCount = 0;
            return false;
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

            var jugador = (mi.CommandParameter as JugadorViewItem);
            JugadoresList.SelectedItem = jugador;
            await PushAsync(new PopupCambiarJugador(jugador));
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

            var jugador = (mi.CommandParameter as JugadorViewItem);
            JugadoresList.SelectedItem = jugador;
            var nombre = jugador?.Reference.Nombre;
            var confirm = await Application.Current.MainPage.DisplayAlert("Borrar a jugador " + nombre + "?", "", "Aceptar", "Cancelar");
            if (confirm)
            {
                if (!ViewModelViewManager.MainViewModel.BorrarJugador(jugador?.Reference))
                {
                    await Application.Current.MainPage.DisplayAlert("", "Error\nSolo es posible borrar jugadores sin ingresos", "Ok");
                }
            }
            canClickOnBorrar = true;
        }

       
    }
}