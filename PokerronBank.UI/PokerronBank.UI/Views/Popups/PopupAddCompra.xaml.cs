using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using PokerronBank.UI.ViewModels.Helper;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using static Rg.Plugins.Popup.Services.PopupNavigation;

namespace PokerronBank.UI.Views.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopupAddCompra : Rg.Plugins.Popup.Pages.PopupPage
    {
        public PopupAddCompra()
        {
            InitializeComponent();
            BindingContext = ViewModelViewManager.MainViewModel;

            ViewModelViewManager.MainViewModel.UpdateJugadoresCompra();

        }

        private CompraViewItem Compra { get; set; }
        public PopupAddCompra(CompraViewItem compra)
        {
            InitializeComponent();
            BindingContext = ViewModelViewManager.MainViewModel;

            ViewModelViewManager.MainViewModel.UpdateJugadoresCompra();
            Compra = compra;

        }

        private bool started { get; set; }
        protected override void OnAppearing()
        {
            

            if (Compra == null)
            {
                InputConcepto.Unfocus();
                InputConcepto.Focus();
                CheckBoxTodosMenosAnfitrion.IsChecked = true;
            }
            else
            {

                ListCompras.Unfocus();
                ListCompras.Focus();
                BotonAnadir.Text = "Cambiar";
                InputConcepto.Text = Compra.Reference.Nombre;
                InputCantidad.Text = Compra.Reference.Cantidad.ToString() ?? "";
                ViewModelViewManager.MainViewModel.SelectedJugadorPicker = ViewModelViewManager.MainViewModel.Jugadores.FirstOrDefault(x => x.Reference == Compra.Reference.JugadorQueHaPagadoCompra);
                ViewModelViewManager.MainViewModel.JugadoresCompra.ForEach(x => x.ParticipaEnCompra = Compra.Reference.JugadoresCompra.Any(y => x.Reference == y.Jugador));
                CheckBoxTodos.IsChecked = ViewModelViewManager.MainViewModel.JugadoresCompra.All(x => x.ParticipaEnCompra);
                CheckBoxTodosMenosAnfitrion.IsChecked = ViewModelViewManager.MainViewModel.JugadoresCompra.Where(x => !x.Reference.EsAnfitrion).All(x => x.ParticipaEnCompra && !CheckBoxTodos.IsChecked);
            }


            base.OnAppearing();
            started = true;
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
            }
            else if(string.IsNullOrEmpty(InputConcepto.Text))
            {
                await DisplayAlert("", "Escriba algo en concepto", "Ok");
            }
            else if (decimal.TryParse(InputCantidad.Text, NumberStyles.Any, new CultureInfo("en-US"), out cantidad))
            {
                
                var ret = "";
                ViewModelViewManager.MainViewModel.JugadoresCompra.Where(x=>x.ParticipaEnCompra).ForEach(x =>
                {
                    var newLine = ret == "" ? "" : ", ";
                    ret += newLine + x.Reference.Nombre;
                });
                var confirm = await DisplayAlert(ViewModelViewManager.MainViewModel.SelectedJugadorPicker .Reference.Nombre + " ha comprado " + InputConcepto.Text + "\nPrecio : " + InputCantidad.Text + "€", 
                    "Pagan: " + ret, "Ok", "Cancelar");
                if (confirm)
                {
                    if (Compra == null)
                    {
                        ViewModelViewManager.MainViewModel.AddCompra.Execute(new Tuple<decimal, string>(cantidad, InputConcepto.Text));
                        ViewModelViewManager.MainViewModel.SelectedJugadorPicker = null;
                        await PopAsync();
                    }
                    else
                    {
                        ViewModelViewManager.MainViewModel.CambiarCompra.Execute(new Tuple<decimal, string, CompraViewItem>(cantidad, InputConcepto.Text, Compra));
                        ViewModelViewManager.MainViewModel.SelectedJugadorPicker = null;
                        await PopAsync();
                    }
                }
                
               
            }
            else
            {
                await DisplayAlert("", "Cantidad no valida", "Ok");
                InputStart = false;
            }


           
            InputStart = false;




        }

        private void ButtonCancel(object sender, EventArgs e)
        {
            Instance.PopAsync();
            ViewModelViewManager.MainViewModel.SelectedJugadorPicker = null;

        }

        private void CheckBoxTodos_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (e.Value)
            {
                ViewModelViewManager.MainViewModel.JugadoresCompra.ForEach(x => x.ParticipaEnCompra = true);
                CheckBoxTodosMenosAnfitrion.IsChecked = false;
            }
        }
    

        private void CheckBoxTodosMenosAnfitrion_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (e.Value)
            {
                ViewModelViewManager.MainViewModel.JugadoresCompra.ForEach(x => x.ParticipaEnCompra = !x.Reference.EsAnfitrion);
                CheckBoxTodos.IsChecked = false;
            }
        }

      
        private  void ListBoxCheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            
            if (started)
            {
                CheckBoxTodos.IsChecked = ViewModelViewManager.MainViewModel.JugadoresCompra.All(x => x.ParticipaEnCompra);
                CheckBoxTodosMenosAnfitrion.IsChecked = ViewModelViewManager.MainViewModel.JugadoresCompra.Where(x => !x.Reference.EsAnfitrion).All(x => x.ParticipaEnCompra && !CheckBoxTodos.IsChecked);
            }
            
        }
    }
}