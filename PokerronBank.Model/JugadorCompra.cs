namespace PokerronBank.Model
{
    public class JugadorCompra : Entity
    {
        public virtual Jugador Jugador { get; set; }
        public virtual Compra Compra { get; set; }

        public JugadorCompra()
        {

        }
        public JugadorCompra(Jugador jugador, Compra compra)
        {
            Jugador = jugador;
            Compra = compra;
        }
    }
}