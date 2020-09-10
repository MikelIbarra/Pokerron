using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Rg.Plugins.Popup.Services.PopupNavigation;

namespace PokerronBank.UI.Views.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopupAddJugador : Rg.Plugins.Popup.Pages.PopupPage
    {
        public PopupAddJugador()
        {
            
            InitializeComponent();

        }

        protected override void OnAppearing()
        {
            InputNombre.Unfocus();
            InputNombre.Focus();
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
            else if (ViewModelViewManager.MainViewModel.CheckExistNombreJugador(InputNombre.Text))
            {
                await DisplayAlert("", "Nombre no valido, ya existe", "Ok");
                InputStart = false;
            }
            else
            {
                ViewModelViewManager.MainViewModel.AddJugador.Execute(new Tuple<string, bool>(InputNombre.Text, CheckBoxEsCaja.IsChecked));
                await PopAsync();

                var confirm = await Application.Current.MainPage.DisplayAlert("Añadir ingreso a " + InputNombre.Text + "?", "", "Aceptar", "Cancelar");
                if (confirm)
                {
                    ViewModelViewManager.MainViewModel.SelectedJugadorPicker = ViewModelViewManager.MainViewModel.Jugadores.FirstOrDefault(x => x.Reference.Nombre == InputNombre.Text);
                    await PushAsync(new PopupAddIngreso());
                }
                else
                {
                    ViewModelViewManager.MainViewModel.SelectedJugadorPicker = null;
                }
            }
           

        }

        private void ButtonCancel(object sender, EventArgs e)
        {
            Instance.PopAsync();
        }
    }
}