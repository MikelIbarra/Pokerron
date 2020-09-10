using PokerronBank.UI.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using PokerronBank.Model;

namespace PokerronBank.UI.ViewModels.Helper
{
    public class JugadorViewItem : ViewItem
    {
        private string _dineroAlFinal;

        public Jugador Reference { get; set; }
        public MainViewModel MainViewModel { get; set; }

        public string Total => MainViewModel.Core.Services.GetDineroAlFinal(Reference, MainViewModel.Partida.Reference) + "€";
        public string DineroIngresado => MainViewModel.Core.Services.GetDineroIngresado(Reference, MainViewModel.Partida.Reference) + "€";

        public string DineroAlFinalYTotal => Reference.DineroAlFinal + "€ (" + Total + ")";
        public bool PartidaTerminada => MainViewModel.PartidaTerminada;
       

        public string DineroAlFinal
        {
            get => Reference.DineroAlFinal + "€";
            set
            {
                
                if (decimal.TryParse(value.Replace("€",""), out var cantidad))
                {
                    Reference.DineroAlFinal = cantidad;
                }

                MainViewModel.UpdateJugadores();
            }
        }


        public JugadorViewItem(Jugador reference, MainViewModel mainViewModel)
    {
        Reference = reference;
        MainViewModel = mainViewModel;
    }

    }
}
