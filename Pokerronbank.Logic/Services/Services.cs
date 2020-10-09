using PokerronBank.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.Math;

namespace Pokerronbank.Logic.Services
{
    public class Services
    {
        public Core Core { get; set; }
        public Services(Core core)
        {
            Core = core;
        }

        public bool CalcularDeudas(Partida partida)
        {
            Core.Repository.SaveAll();

            partida.Jugadores.ForEach(x => x.DeudaDetalleHelp = "");
            partida.Jugadores.ForEach(x => x.DeudaHelp = GetDineroIngresadoSinCash(x,partida));
            partida.Jugadores.ForEach(x => x.DeudaHelp -= x.DineroAlFinal);
            if (partida.Ingresos.Sum(x => x.Cantidad) != partida.Jugadores.Sum(x => x.DineroAlFinal)) return false;
            var cash = partida.Ingresos.Where(x => x.EsCash).Sum(x => x.Cantidad);

            

            foreach (var item in partida.Jugadores)
            {
                //cash ingresado
                var cashIngresado = partida.Ingresos.Where(x => x.Jugador == item && x.EsCash).Sum(x => x.Cantidad);
                if (cashIngresado > 0)
                {
                    item.DeudaDetalleHelp = "Ha ingresado " + cashIngresado.ToString("0.##") + "€ en cash";
                }
                //cancelar pagares
                if (item.DineroAlFinal > 0 && partida.Ingresos.Any(x => x.Jugador == item && !x.EsCash))
                {
                    if (partida.Ingresos.Where(x => x.Jugador == item && !x.EsCash).Sum(x=>x.Cantidad) >= item.DineroAlFinal)
                    {
                        if (item.DeudaDetalleHelp != "") item.DeudaDetalleHelp += "\n";
                        item.DeudaDetalleHelp += "Cancela " + item.DineroAlFinal.ToString("0.##") + "€ en pagares";
                    }
                    else
                    {
                        if (item.DeudaDetalleHelp != "") item.DeudaDetalleHelp += "\n";
                        item.DeudaDetalleHelp += "Cancela " + partida.Ingresos.Where(x => x.Jugador == item && !x.EsCash).Sum(x => x.Cantidad).ToString("0.##") + "€ en pagares";
                    }
                }
            }
           

            var ganadores = partida.Jugadores.Where(x => x.DeudaHelp < 0).ToList().OrderBy(x => x.DeudaHelp);
            var perdedores = partida.Jugadores.Where(x => x.DeudaHelp > 0).ToList().OrderByDescending(x => x.DeudaHelp);

            //primero cobra los caja
            foreach (var item in ganadores.Where(x=>x.EsCaja))
            {
                cash = CalcularDeudaJugador(item, perdedores.Where(x => x.DeudaHelp > 0).ToList(), cash);
            }
            foreach (var item in ganadores.Where(x => !x.EsCaja))
            {
                cash = CalcularDeudaJugador(item, perdedores.Where(x => x.DeudaHelp > 0).ToList(), cash);
            }

            Core.Repository.SaveAll();
            return true;
        }

        public decimal CalcularDeudaJugador(Jugador jugador, List<Jugador> perdedores, decimal cash)
        {
          
            var ret = cash;
            if (cash > 0)
            {
                var newLine = "";
                if (cash >= -jugador.DeudaHelp)
                {
                    if (jugador.DeudaDetalleHelp != "") newLine = "\n";
                    jugador.DeudaDetalleHelp +=  newLine +"Recibe " + (-jugador.DeudaHelp ).ToString("0.##") + "€ en cash";
                    ret += jugador.DeudaHelp;
                    jugador.DeudaHelp = 0;
                    return ret;
                }
                if (jugador.DeudaDetalleHelp != "") newLine = "\n";
                jugador.DeudaHelp += cash;
                jugador.DeudaDetalleHelp += newLine + "Recibe " + cash.ToString("0.##") + "€ en cash";
                ret = 0;
            }

            while (Abs(decimal.ToDouble(jugador.DeudaHelp)) > 0.01)
            {
                foreach (var item in perdedores)
                {
                    if (decimal.ToDouble(item.DeudaHelp) > 0.01)
                    {
                        if (item.DeudaHelp >= -(jugador.DeudaHelp))
                        {
                            item.DeudaHelp += jugador.DeudaHelp;
                            JugadorDetalleDeuda(jugador, item, Abs(jugador.DeudaHelp));
                            jugador.DeudaHelp = 0;
                            return ret;
                        }

                        jugador.DeudaHelp += item.DeudaHelp;
                        JugadorDetalleDeuda(jugador, item, Abs(item.DeudaHelp));
                        item.DeudaHelp = 0;
                    }
                }
            }


            return ret;
        }

        public void JugadorDetalleDeuda(Jugador ganador, Jugador perdedor, decimal cantidad)
        {
            if (ganador.DeudaDetalleHelp == "")
            {
                ganador.DeudaDetalleHelp = "Recibe " + cantidad.ToString("0.##") + "€ de " + perdedor.Nombre;

            }
            else
            {
                ganador.DeudaDetalleHelp += "\nRecibe " + cantidad.ToString("0.##") + "€ de " + perdedor.Nombre;
            }
            if (perdedor.DeudaDetalleHelp == "")
            {
                perdedor.DeudaDetalleHelp = "Debe " + cantidad.ToString("0.##") + "€ a " + ganador.Nombre;

            }
            else
            {
                perdedor.DeudaDetalleHelp += "\nDebe " + cantidad.ToString("0.##") + "€ a " + ganador.Nombre;
            }
        }

        public bool CalcularDeudasCompras(Partida partida)
        {
            Core.Repository.SaveAll();

            partida.Jugadores.ForEach(x => x.DeudaDetalleHelp = "");
            partida.Jugadores.ForEach(x => x.DeudaHelp =0);

            //Cuanto debe cada jugador
            partida.Compras.ForEach(x =>
            {
                x.JugadorQueHaPagadoCompra.DeudaHelp += -x.Cantidad;

                if (x.JugadoresCompra.Count > 0)
                {
                    var pagarPorPersona = x.Cantidad / x.JugadoresCompra.Count;
                    x.JugadoresCompra.ForEach(y => y.Jugador.DeudaHelp += pagarPorPersona);
                }


            });

            var total = Abs(decimal.ToDouble(partida.Jugadores.Sum(x => x.DeudaHelp)));
            if (total > 0.01 )
            {
                var fallo = 1;
            }

            var aDevolver = partida.Jugadores.Where(x => x.DeudaHelp < 0).ToList().OrderBy(x => x.DeudaHelp);
            var aPagar = partida.Jugadores.Where(x => x.DeudaHelp > 0).ToList().OrderByDescending(x => x.DeudaHelp);


            //primero cobra los caja
            foreach (var item in aDevolver)
            {
                 CalcularDeudaJugador(item, aPagar.Where(x => x.DeudaHelp > 0).ToList(), 0);
            }
            
            partida.Jugadores.ForEach(x => x.DeudaCompraDetalle = x.DeudaDetalleHelp);
            return true;
        }

        public string GetJugadorCompraDetalle(Jugador jugador, Partida partida)
        {
            var ret = "";


            if (partida.Compras.Any(x => x.JugadoresCompra.Select(y=>y.Jugador).Contains(jugador)))
            {
                ret += "Participa:";
                partida.Compras.Where(x => x.JugadoresCompra.Select(y=>y.Jugador).Contains(jugador)).ToList().ForEach(x =>
                {

                    ret += "\n -" + x.Nombre + " " + (x.Cantidad / x.JugadoresCompra.Count).ToString("0.##") + "€";
                });
            }


            if (partida.Compras.Any(x => x.JugadorQueHaPagadoCompra == jugador))
            {
                ret += "\n\nHa pagado:";
                partida.Compras.Where(x => x.JugadorQueHaPagadoCompra == jugador).ToList().ForEach(x =>
                {
                    ret += "\n -" + x.Nombre + " " + x.Cantidad + "€";
                });

            }


            return ret;
        }

        public Partida GetPartida()
        {
           var partidaList =  Core.Repository.GetAll<Partida>().ToList();
            var jugadores = Core.Repository.GetAll<Jugador>();
            var ingresos = Core.Repository.GetAll<Ingreso>();
            var compras = Core.Repository.GetAll<Compra>();
            var jugadorCompra = Core.Repository.GetAll<JugadorCompra>();
            return partidaList.Count == 0 ? CreatePartida() : partidaList.First();
        }

        public Partida CreatePartida()
        {
            var newItem = new Partida();

            //Nueva partida -> se borra todo
            Core.Repository.GetAll<Partida>().ToList().ForEach(x => Core.Repository.Delete(x));
            Core.Repository.GetAll<Jugador>().ToList().ForEach(x => Core.Repository.Delete(x));
            Core.Repository.GetAll<Ingreso>().ToList().ForEach(x => Core.Repository.Delete(x));
            Core.Repository.GetAll<Compra>().ToList().ForEach(x => Core.Repository.Delete(x));
            Core.Repository.GetAll<JugadorCompra>().ToList().ForEach(x => Core.Repository.Delete(x));

            Core.Repository.Add(newItem);
            Core.Repository.SaveAll();

            return Core.Repository.GetAll<Partida>().FirstOrDefault();
        }

        public decimal GetDineroAlFinalsinCash(Jugador jugador, Partida partida)
        {
            return jugador.DineroAlFinal - GetDineroIngresadoSinCash(jugador, partida);
        }

        public decimal GetDineroAlFinal(Jugador jugador, Partida partida)
        {
            return jugador.DineroAlFinal - GetDineroIngresado(jugador,partida);
        }


        public decimal GetDineroIngresadoSinCash(Jugador jugador, Partida partida)
        {
            return partida.Ingresos.Where(x => x.Jugador == jugador && !x.EsCash).Sum(x => x.Cantidad);
        }
        public decimal GetDineroIngresado(Jugador jugador, Partida partida)
        {
            return partida.Ingresos.Where(x => x.Jugador == jugador).Sum(x => x.Cantidad);
        }


        public Jugador AddNewJugador(string nombre, bool esCaja, bool esAnfitrion, string numeroTelefono, bool whatsAppFunciona, Partida partida)
        {
            var newItem = new Jugador(nombre, esCaja,esAnfitrion,numeroTelefono, whatsAppFunciona, partida);
            Core.Repository.Add(newItem);
            Core.Repository.SaveAll();
            return newItem;
        }

        public Compra AddCompra(string nombre, Decimal cantidad, Partida partida,Jugador jugadorQuePagaCompra ,List<Jugador> jugadores)
        {
            var newItem = new Compra(nombre, cantidad, partida, jugadorQuePagaCompra);
            foreach (var jug in jugadores)
            {
                var newItem2 = new JugadorCompra(jug, newItem);
                newItem.JugadoresCompra.Add(newItem2);
                Core.Repository.Add(newItem2);
            }
            
            Core.Repository.Add(newItem);
         
            Core.Repository.SaveAll();
            return newItem;
        }

        public void CambiarCompra(Compra compra, string nombre, Decimal cantidad, Partida partida, Jugador jugadorQuePagaCompra, List<Jugador> jugadores)
        {

            compra.Cantidad = cantidad;
            compra.Nombre = nombre;
            compra.JugadorQueHaPagadoCompra = jugadorQuePagaCompra;

            compra.JugadoresCompra.Where(x => !jugadores.Contains(x.Jugador)).ToList().ForEach(x => Core.Repository.Delete(x));
            compra.JugadoresCompra.Where(x => !jugadores.Contains(x.Jugador)).ToList().ForEach(x => compra.JugadoresCompra.Remove(x));
            jugadores.Where(x => compra.JugadoresCompra.All(y => y.Jugador != x)).ToList().ForEach(x =>
            {
                var newItem = new JugadorCompra(x, compra);
                compra.JugadoresCompra.Add(newItem);
                Core.Repository.Add(newItem);
            });

            Core.Repository.SaveAll();

        }

        public void DeleteJugador(Jugador jugador, Partida partida)
        {
            partida.Jugadores.Remove(jugador);
            Core.Repository.Delete(jugador);
            Core.Repository.SaveAll();
           
        }

        public void DeleteCompra(Compra compra, Partida partida)
        {
            partida.Compras.Remove(compra);
            compra.JugadoresCompra.ForEach(x => Core.Repository.Delete(x));
            Core.Repository.Delete(compra);
            Core.Repository.SaveAll();

        }
        public bool CheckExistNombreJugador(string nombre,  Partida partida)
        {
            
            return partida.Jugadores.Any(x=>x.Nombre==nombre);
        }
        public Ingreso AddNewIngreso(Jugador jugador, Decimal cantidad, bool esCash, Partida partida)
        {
            var newItem = new Ingreso(jugador,cantidad, esCash, partida);
            Core.Repository.Add(newItem);
            Core.Repository.SaveAll();
            return newItem;
        }

      
    }
}
