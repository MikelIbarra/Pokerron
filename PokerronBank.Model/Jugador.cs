using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PokerronBank.Model
{
    public class Jugador: Entity
    {
        public string Nombre { get; set; } 
        public string DeudaDetalle { get; set; }
        [NotMapped]
        public Decimal DeudaHelp { get; set; } 
        public Decimal DineroAlFinal { get; set; }
        public bool EsCaja { get; set; }
        public Jugador()
        {
            
        }
        public Jugador(string nombre, bool esCaja, Partida partida)
        {
            Nombre = nombre;
            EsCaja = esCaja;
            partida.Jugadores.Add(this);
        }
    }
}
