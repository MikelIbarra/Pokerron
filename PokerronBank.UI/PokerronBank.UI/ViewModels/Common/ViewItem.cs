using System;
using System;
using System.Windows;


namespace PokerronBank.UI.ViewModels.Common
{
    public class ViewItem : ViewModelBase
    {
        public bool Visibility { get; set; }

        public void PropertiesUpdate()
        {
            OnPropertyChanged("");
        }
    }

    
}