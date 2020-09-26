using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PokerronBank.Model
{
    public class Compra : Entity
    {
        public string Nombre { get; set; }
        public Decimal Cantidad { get; set; }
        public string DeudaDetalle { get; set; }
        [NotMapped]
        public Decimal DeudaHelp { get; set; }

        public virtual List<Jugador> Jugadores { get; set; } = new List<Jugador>();

        public Compra()
        {

        }
        public Compra(string nombre, Decimal cantidad)
        {
            Nombre = nombre;
            Cantidad = cantidad;

        }
    }
}