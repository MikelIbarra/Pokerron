using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


        private bool canClickCalcularDeudas = true;

        private async void CalcularDeudas(object sender, EventArgs e)
        {
            if (!canClickCalcularDeudas) return;
            canClickCalcularDeudas = false;
            if (!ViewModelViewManager.MainViewModel.CajaCuadra)
            {
                await Application.Current.MainPage.DisplayAlert("","No es posible cacular deudas \nla caja no cuadra", "Ok");
            }
            ViewModelViewManager.MainViewModel.CalcularDeudas.Execute(null);
            canClickCalcularDeudas = true;
        }
    }
}