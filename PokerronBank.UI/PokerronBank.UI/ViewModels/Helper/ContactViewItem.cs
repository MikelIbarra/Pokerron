using Plugin.ContactService.Shared;
using PokerronBank.UI.ViewModels.Common;

namespace PokerronBank.UI.ViewModels.Helper
{
    public class ContactViewItem : ViewItem
    {



        public Contact Reference { get; set; }

        public bool Usar { get; set; }
        public string Nombre { get; set; }
        public string Numero { get; set; }
        
        public ContactViewItem(Contact reference, string numero)
        {
            Reference = reference;
            Nombre = reference.Name;
            Numero = numero;
        }

    }
}