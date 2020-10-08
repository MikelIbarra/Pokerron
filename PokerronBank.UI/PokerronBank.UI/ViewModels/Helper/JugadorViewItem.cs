using System.Globalization;
using PokerronBank.UI.ViewModels.Common;
using PokerronBank.Model;

namespace PokerronBank.UI.ViewModels.Helper
{
    public class JugadorViewItem : ViewItem
    {
        

        public Jugador Reference { get; set; }
        public MainViewModel MainViewModel { get; set; }

        public bool ParticipaEnCompra { get; set; }

        public string Total => MainViewModel.Core.Services.GetDineroAlFinal(Reference, MainViewModel.Partida.Reference) + "€";
        public string DineroIngresado => MainViewModel.Core.Services.GetDineroIngresado(Reference, MainViewModel.Partida.Reference) + "€";

        public string DineroAlFinalYTotal => Reference.DineroAlFinal + "€ (" + Total + ")";
        public bool PartidaTerminada => MainViewModel.PartidaTerminada;

        public string DetalleCompra => MainViewModel.Core.Services.GetJugadorCompraDetalle(Reference, MainViewModel.Partida.Reference);


        private string dineroFinal;
        public string DineroAlFinal
        {
            get
            {
                if (dineroFinal?.Length > 1 && dineroFinal?.Substring(dineroFinal.Length - 1, 1) == ".")
                {
                    return dineroFinal;
                }
                return Reference.DineroAlFinal.ToString().Replace(",", ".");
            }
            set
            {
                if (value?.Length > 1 && value?.Substring(value.Length - 1, 1) == ".")
                {
                    dineroFinal = value;
                   
                }
                else if (string.IsNullOrEmpty(value))
                {
                    dineroFinal = "0";
                    Reference.DineroAlFinal = 0;
                }
                else if (decimal.TryParse(value, NumberStyles.Any, new CultureInfo("en-US"), out var cantidad))
                {
                    dineroFinal = cantidad.ToString().Replace(",", ".");
                    Reference.DineroAlFinal = cantidad;
                }
                MainViewModel.UpdateJugadores();
                
            }
        }


        public JugadorViewItem(Jugador reference, MainViewModel mainViewModel)
        {
            Reference = reference;
            MainViewModel= mainViewModel;


        }

    }
}
