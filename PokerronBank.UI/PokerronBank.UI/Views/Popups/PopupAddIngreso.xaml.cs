using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Rg.Plugins.Popup.Services.PopupNavigation;

namespace PokerronBank.UI.Views.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopupAddIngreso : Rg.Plugins.Popup.Pages.PopupPage
    {
        public PopupAddIngreso()
        {
            InitializeComponent();
            BindingContext = ViewModelViewManager.MainViewModel;
            

        }

        protected override void OnAppearing()
        {
            InputCantidad.Unfocus();
            InputCantidad.Focus();
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
            // Return true if you don't want to close this popup page when a back button is pressed
            return base.OnBackButtonPressed();
        }

        // Invoked when background is clicked
        protected override bool OnBackgroundClicked()
        {
            // Return false if you don't want to close this popup page when a background of the popup page is clicked
            return base.OnBackgroundClicked();
        }




        public bool InputStart { get; set; }

        private async void ButtonOk(object sender, EventArgs e)
        {
            decimal cantidad;

            if (InputStart) return;
            InputStart = true;


            if (ViewModelViewManager.MainViewModel.SelectedJugadorPicker == null)
            {
                await DisplayAlert("", "Seleccione un jugador", "Ok");
                InputStart = false;
            }
            else if (decimal.TryParse(InputCantidad.Text, out cantidad))
            {
                var text = ViewModelViewManager.MainViewModel.SelectedJugadorPicker.Reference.Nombre + " ingresa " + cantidad + "€";
                if (CheckBoxEsCash.IsChecked)
                {
                    text += " en ca$h?";
                }
                else
                {
                    text += " en un pagare?";
                }
                var confirm = await DisplayAlert("Confirmar ingreso", text, "Ok", "Cancelar");
                if (confirm)
                {
                    ViewModelViewManager.MainViewModel.AddIngreso.Execute(new Tuple<decimal, bool>(cantidad, CheckBoxEsCash.IsChecked));
                    await PopAsync();
                }
                else
                {
                    InputStart = false;
                }
                ViewModelViewManager.MainViewModel.SelectedJugadorPicker = null;
            }
            else
            {
                await DisplayAlert("", "Cantidad no valida", "Ok");
                InputStart = false;
            }
            


        }

        private void ButtonCancel(object sender, EventArgs e)
        {
            Instance.PopAsync();
            ViewModelViewManager.MainViewModel.SelectedJugadorPicker = null;
        }
    }
}