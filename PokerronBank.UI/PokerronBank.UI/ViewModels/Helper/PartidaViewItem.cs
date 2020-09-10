using PokerronBank.Model;
using PokerronBank.UI.ViewModels.Common;

namespace PokerronBank.UI.ViewModels.Helper
{
    public class PartidaViewItem : ViewItem
    {
        public Partida Reference { get; set; }
        public PartidaViewItem(Partida reference)
        {
            Reference = reference;
        }

    }
}