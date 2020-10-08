using System;
using System.Linq;
using PokerronBank.UI.ViewModels.Helper;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PokerronBank.UI.Views.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopupBuscarContacto : Rg.Plugins.Popup.Pages.PopupPage
    {
        public PopupAddJugador parent { get; set; }
        public PopupBuscarContacto(PopupAddJugador popupAddJugador)
        {
            InitializeComponent();
            parent = popupAddJugador;
            ListContactos.ItemsSource = ViewModelViewManager.MainViewModel.Contacts;
    
        }

        protected override void OnAppearing()
        {
            searchContactos.Unfocus();
            searchContactos.Focus();
            base.OnAppearing();
        }

        protected  override bool OnBackButtonPressed()
        {
             PopupNavigation.Instance.PopAsync();
            // Return true if you don't want to close this popup page when a back button is pressed
            return base.OnBackButtonPressed();
        }

        public bool InputStart { get; set; }
        private async void ButtonOk(object sender, EventArgs e)
        {
            if (InputStart) return;
            InputStart = true;


            if (ListContactos.SelectedItem is ContactViewItem contact)
            {
                parent.Nombre = contact.Nombre;
                parent.Contact = contact;
                await PopupNavigation.Instance.PopAsync();
                InputStart = false;
            }
            else
            {
                await DisplayAlert("", "Ningun contacto seleccionado", "Ok");
                InputStart = false;
            }
            
        }

       

        private async void ButtonCancel(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.NewTextValue))
            {
                ListContactos.ItemsSource = ViewModelViewManager.MainViewModel.Contacts;
            }

            else
            {
                ListContactos.ItemsSource =  ViewModelViewManager.MainViewModel.Contacts.Where(x => x.Nombre.ToLower().Contains(e.NewTextValue.ToLower()));
            }
        }


        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var stackLayout = sender as StackLayout;
            ListContactos.SelectedItem = stackLayout?.BindingContext;

        }

        private  void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            var stackLayout = sender as StackLayout; 
            ListContactos.SelectedItem = stackLayout?.BindingContext;
            ButtonOk(null,null);
        }
    }
}