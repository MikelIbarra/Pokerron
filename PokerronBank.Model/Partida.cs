using System;
using System.Collections.Generic;

namespace PokerronBank.Model
{
    public class Partida : Entity
    {

        public DateTime FechaInicio { get; set; }
        public DateTime FechaFinal { get; set; }
        public virtual  List<Jugador> Jugadores { get; set; } = new List<Jugador>();
        public virtual List<Ingreso> Ingresos { get; set; } = new List<Ingreso>();
        public bool Terminada { get; set; }

        public Partida()
        {
            FechaInicio = DateTime.Now;
        }
       
    }
}