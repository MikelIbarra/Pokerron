using PokerronBank.Model;
using PokerronBank.UI.ViewModels.Common;

namespace PokerronBank.UI.ViewModels.Helper
{
    public class IngresoViewItem : ViewItem
    {
        public Ingreso Reference { get; set; }
        public string DetalleIngreso
        {
            get
            {
                var ret = Reference.Cantidad + "€";
                if (Reference.EsCash)
                {
                    ret += " en ca$h";
                }
                else
                {
                    ret += " en un pagare";
                }
                ret += " , hora: " + Reference.Fecha.ToString("HH:mm") ;
                return ret;
            }
        }

        public IngresoViewItem(Ingreso reference)
        {
            Reference = reference;
        }

    }
}