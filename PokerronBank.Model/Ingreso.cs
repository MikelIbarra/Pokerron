using System;

namespace PokerronBank.Model
{
    public class Ingreso : Entity
    {
        public DateTime Fecha { get; set; }
        public virtual Jugador Jugador { get; set; }
        public virtual Partida Partida { get; set; }
        public bool EsCash { get; set; }
        public Decimal Cantidad { get; set; }

        public Ingreso()
        {
                
        }
        public Ingreso(Jugador jugador, Decimal cantidad, bool esCash, Partida partida )
        {
            Fecha = DateTime.Now;
            Cantidad = cantidad;
            EsCash = esCash;
            Partida = partida;
            Jugador = jugador;
            partida.Ingresos.Add(this);
        }
    
    }
}