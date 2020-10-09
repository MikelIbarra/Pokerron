using System.Linq;
using PokerronBank.Model;
using PokerronBank.UI.ViewModels.Common;

namespace PokerronBank.UI.ViewModels.Helper
{
    public class CompraViewItem : ViewItem
    {
        


        public Compra Reference { get; set; }

        public CompraViewItem(Compra reference)
        {
            Reference = reference;
        }

        public string Cantidad
        {
            get
            {
                if (Reference?.JugadoresCompra?.Count() == 0)
                {
                    return Reference?.Cantidad + "€";
                }

                if (Reference?.Cantidad > 0)
                {
                    return Reference.Cantidad + "€ -> " + Reference.JugadoresCompra.Count + " x " + (Reference.Cantidad / Reference.JugadoresCompra.Count).ToString("0.##") + "€";
                }
                return "0€";


            }
        }

        public string JugadoresQuePagan
        {
            get
            {
                var ret = "";
                Reference?.JugadoresCompra?.ForEach(x =>
                {
                    var newLine = ret == "" ? "" : ", ";
                    ret += newLine + x.Jugador.Nombre;
                });
                return ret;
            }
        }

        public string JugadorQueHaPagadoCompra => "(" + Reference.JugadorQueHaPagadoCompra.Nombre + ")";


    }
}