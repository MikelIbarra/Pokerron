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

        public virtual Jugador JugadorQueHaPagadoCompra { get; set; }

        //Deberia ser solo Jugador pero da error en el Lazy loading
        public virtual List<JugadorCompra> JugadoresCompra { get; set; } = new List<JugadorCompra>();

        public Compra()
        {

        }
        public Compra(string nombre, Decimal cantidad, Partida partida, Jugador jugadorQueHaPagadoCompra)
        {
            Nombre = nombre;
            Cantidad = cantidad; 
            JugadorQueHaPagadoCompra = jugadorQueHaPagadoCompra;
            partida.Compras.Add(this);
            
        }
    }
}