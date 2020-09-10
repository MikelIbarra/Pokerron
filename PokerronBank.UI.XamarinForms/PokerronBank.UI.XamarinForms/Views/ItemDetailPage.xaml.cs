using System.ComponentModel;
using Xamarin.Forms;
using PokerronBank.UI.XamarinForms.ViewModels;

namespace PokerronBank.UI.XamarinForms.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}