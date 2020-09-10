using System;

namespace PokerronBank.Model
{
    public class JugadorArchivo : Entity
    {
        public string Nombre { get; set; }
        public Decimal DineroAlFinal { get; set; }
        public string Deudas { get; set; }
        public JugadorArchivo()
        {

        }
        public JugadorArchivo(Jugador jugador)
        {
            Nombre = jugador.Nombre;
            DineroAlFinal = jugador.DineroAlFinal;
            //Deudas = jugador.Deudas;
        }
    }
}