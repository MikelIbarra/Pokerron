using System;
using System.Collections.Generic;

namespace PokerronBank.Model
{
    public class PartidaArchivo : Entity
    {
        public DateTime FechaInicio { get; set; }

        public DateTime Duracion { get; set; }
        public virtual List<JugadorArchivo> Jugadores { get; set; } = new List<JugadorArchivo>();
       
        public int DineroJugado { get; set; }

        public PartidaArchivo()
        {
                
        }
        public PartidaArchivo(Partida partida)
        {
            FechaInicio = partida.FechaInicio;
            // Duracion = partida.FechaFinal - partida.FechaInicio;
            partida.Jugadores.ForEach(x => Jugadores.Add(new JugadorArchivo(x)));
        }

    }
}