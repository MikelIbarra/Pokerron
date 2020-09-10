using PokerronBank.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
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

            partida.Jugadores.ForEach(x => x.DeudaDetalle = "");
            partida.Jugadores.ForEach(x => x.DeudaHelp = GetDineroIngresadoSinCash(x,partida));
            partida.Jugadores.ForEach(x => x.DeudaHelp -= x.DineroAlFinal);
            if (partida.Ingresos.Sum(x => x.Cantidad) != partida.Jugadores.Sum(x => x.DineroAlFinal)) return false;
            var cash = partida.Ingresos.Where(x => x.EsCash).Sum(x => x.Cantidad);
          
            foreach (var item in partida.Jugadores)
            {
                if (item.DineroAlFinal > 0 && partida.Ingresos.Any(x => x.Jugador == item && !x.EsCash))
                {
                    if (partida.Ingresos.Where(x => x.Jugador == item && !x.EsCash).Sum(x=>x.Cantidad) >= item.DineroAlFinal)
                    {
                        item.DeudaDetalle = "Cancela " + item.DineroAlFinal + "€ en pagares";
                    }
                    else
                    {
                        item.DeudaDetalle = "Cancela " + partida.Ingresos.Where(x => x.Jugador == item && !x.EsCash).Sum(x => x.Cantidad) + "€ en pagares";
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
                    if (jugador.DeudaDetalle != "") newLine = "\n";
                    jugador.DeudaDetalle +=  newLine +"Recibe " + (-jugador.DeudaHelp )+ "€ en cash";
                    ret += jugador.DeudaHelp;
                    jugador.DeudaHelp = 0;
                    return ret;
                }
                if (jugador.DeudaDetalle != "") newLine = "\n";
                jugador.DeudaHelp += cash;
                jugador.DeudaDetalle += newLine + "Recibe " + cash + "€ en cash";
                ret = 0;
            }

            while (jugador.DeudaHelp < 0)
            {
                foreach (var item in perdedores)
                {
                    if (item.DeudaHelp > 0)
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
            if (ganador.DeudaDetalle == "")
            {
                ganador.DeudaDetalle = "Recibe " + cantidad + "€ de " + perdedor.Nombre;

            }
            else
            {
                ganador.DeudaDetalle += "\nRecibe " + cantidad + "€ de " + perdedor.Nombre;
            }
            if (perdedor.DeudaDetalle == "")
            {
                perdedor.DeudaDetalle = "Debe " + cantidad + "€ a " + ganador.Nombre;

            }
            else
            {
                perdedor.DeudaDetalle += "\nDebe " + cantidad + "€ a " + ganador.Nombre;
            }
        }

        public Partida GetPartida()
        {
           var partidaList =  Core.Repository.GetAll<Partida>().ToList();
            Core.Repository.GetAll<Jugador>();
            Core.Repository.GetAll<Ingreso>();
            return partidaList.Count == 0 ? CreatePartida() : partidaList.First();
        }

        public Partida CreatePartida()
        {
            var newItem = new Partida();

            //Nueva partida -> se borra todo
            Core.Repository.GetAll<Partida>().ToList().ForEach(x => Core.Repository.Delete(x));
            Core.Repository.GetAll<Jugador>().ToList().ForEach(x => Core.Repository.Delete(x));
            Core.Repository.GetAll<Ingreso>().ToList().ForEach(x => Core.Repository.Delete(x));

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


        public Jugador AddNewJugador(string nombre, bool esCaja, Partida partida)
        {
            var newItem = new Jugador(nombre, esCaja, partida);
            Core.Repository.Add(newItem);
            Core.Repository.SaveAll();
            return newItem;
        }

        public void DeleteJugador(Jugador jugador, Partida partida)
        {
            partida.Jugadores.Remove(jugador);
            Core.Repository.Delete(jugador);
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
