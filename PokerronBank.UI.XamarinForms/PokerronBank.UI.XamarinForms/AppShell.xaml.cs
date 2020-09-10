using System;
using System.Collections.Generic;
using PokerronBank.UI.XamarinForms.ViewModels;
using PokerronBank.UI.XamarinForms.Views;
using Xamarin.Forms;

namespace PokerronBank.UI.XamarinForms
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
        }

    }
}
