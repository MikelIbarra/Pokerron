using System;
using System.Threading.Tasks;
using PokerronBank.UI.ViewModels.Helper;
using Xamarin.Forms.Xaml;
using static Rg.Plugins.Popup.Services.PopupNavigation;

namespace PokerronBank.UI.Views.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopupCambiarJugador : Rg.Plugins.Popup.Pages.PopupPage
    {
        private readonly JugadorViewItem _jugador;

        public PopupCambiarJugador(JugadorViewItem jugador)
        {
            _jugador = jugador;

            InitializeComponent();
        }

        public string NombreOld { get; set; }
        protected override void OnAppearing()
        {
            InputNombre.Text = _jugador.Reference.Nombre;
            NombreOld = _jugador.Reference.Nombre;
            InputDinero.Text = _jugador.DineroAlFinal;
            CheckBoxEsCaja.IsChecked = _jugador.Reference.EsCaja;

            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        // ### Methods for supporting animations in your popup page ###

        // Invoked before an animation appearing
        protected override void OnAppearingAnimationBegin()
        {
            base.OnAppearingAnimationBegin();
        }

        // Invoked after an animation appearing
        protected override void OnAppearingAnimationEnd()
        {
            base.OnAppearingAnimationEnd();
        }

        // Invoked before an animation disappearing
        protected override void OnDisappearingAnimationBegin()
        {
            base.OnDisappearingAnimationBegin();
        }

        // Invoked after an animation disappearing
        protected override void OnDisappearingAnimationEnd()
        {
            base.OnDisappearingAnimationEnd();
        }

        protected override Task OnAppearingAnimationBeginAsync()
        {
            return base.OnAppearingAnimationBeginAsync();
        }

        protected override Task OnAppearingAnimationEndAsync()
        {
            return base.OnAppearingAnimationEndAsync();
        }

        protected override Task OnDisappearingAnimationBeginAsync()
        {
            return base.OnDisappearingAnimationBeginAsync();
        }

        protected override Task OnDisappearingAnimationEndAsync()
        {
            return base.OnDisappearingAnimationEndAsync();
        }

        // ### Overrided methods which can prevent closing a popup page ###

        // Invoked when a hardware back button is pressed
        protected override bool OnBackButtonPressed()
        {
            Instance.PopAsync();
            // Return true if you don't want to close this popup page when a back button is pressed
            return base.OnBackButtonPressed();
        }



        // Invoked when background is clicked
        protected override bool OnBackgroundClicked()
        {
            // Return false if you don't want to close this popup page when a background of the popup page is clicked

            return true;
        }

        public bool InputStart { get; set; }

        private async void ButtonOk(object sender, EventArgs e)
        {

            if (InputStart) return;
            InputStart = true;
            if (string.IsNullOrEmpty(InputNombre.Text))
            {
                await DisplayAlert("", "Nombre no valido", "Ok");
                InputStart = false;
            }
            else if (ViewModelViewManager.MainViewModel.CheckExistNombreJugador(InputNombre.Text) && NombreOld!= InputNombre.Text)
            {
                await DisplayAlert("", "Nombre no valido, ya existe", "Ok");
                InputStart = false;
            }
            else
            {
                ViewModelViewManager.MainViewModel.CambiarJugador.Execute(new Tuple<string, bool, string, JugadorViewItem>(InputNombre.Text, CheckBoxEsCaja.IsChecked, InputDinero.Text,_jugador));
                await PopAsync();

                
            }


        }

        private void ButtonCancel(object sender, EventArgs e)
        {
            

            Instance.PopAsync();
        }
    }
}