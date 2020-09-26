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

    }
}