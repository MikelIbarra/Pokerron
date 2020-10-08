using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PokerronBank.Model
{
    public class Jugador: Entity
    {
        public string Nombre { get; set; } 
        public string DeudaDetalle { get; set; }
        public string DeudaCompraDetalle { get; set; }
        [NotMapped]
        public string DeudaDetalleHelp { get; set; }
        [NotMapped]
        public Decimal DeudaHelp { get; set; }
        public Decimal DineroAlFinal { get; set; }
        public bool EsCaja { get; set; }
        public bool EsAnfitrion { get; set; }
        public string NumeroTelefono { get; set; }
        public bool WhatsAppFunciona { get; set; }

        


        public Jugador()
        {
                
        }
        public Jugador(string nombre, bool esCaja, bool esAnfitrion, string numeroTelefono, bool whatsAppFunciona, Partida partida)
        {
            Nombre = nombre;
            EsCaja = esCaja;
            EsAnfitrion = esAnfitrion;
            NumeroTelefono = numeroTelefono;
            WhatsAppFunciona = whatsAppFunciona;
            partida.Jugadores.Add(this);
        }
    }
}
