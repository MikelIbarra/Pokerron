using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerronBank.Model;

namespace Pokerronbank.Logic.Test
{
    [TestClass]
    public class UnitTest1
    {
        //    [TestMethod]
        //    public void TestMethod1()
        //    {
        //        var core = new Core("DataBaseTest.db");
        //         var partida1 = new Partida();
        //        var partida2 = new Partida();
        //        var jugador1 = new Jugador("Mikel",false, partida1);
        //        var jugador2 = new Jugador("Fernando", false, partida1);
        //        var jugador3 = new Jugador("Oskar", false, partida1);
        //        var jugador4 = new Jugador("Iker", false, partida1);
        //        var jugador5 = new Jugador("Miguel", false, partida1);
        //        core.Repository.Add(jugador1);
        //        core.Repository.Add(jugador2);
        //        core.Repository.Add(jugador3);
        //        core.Repository.Add(jugador4);
        //        core.Repository.Add(jugador5);

        //        core.Repository.Add(partida1);
        //        core.Repository.Add(partida2);

        //        core.Repository.SaveAll();

        //        var par = core.Repository.GetAll<Partida>().ToList();

        //        List<Jugador> jugadores = core.Repository.GetAll<Jugador>().ToList();





        //        var ingreso1 = new Ingreso(jugador1, 100, false, partida1);
        //        var ingreso2 = new Ingreso(jugador2, 150, true, partida1);




        //        core.Repository.Add(ingreso1);
        //        core.Repository.Add(ingreso2);

        //        //core.Repository.Add(jugadorEnPartida1);
        //        //core.Repository.Add(jugadorEnPartida2);
        //        //core.Repository.Add(jugadorEnPartida3);
        //        //core.Repository.Add(jugadorEnPartida4);


        //        core.Repository.SaveAll();

        //        var partidas = core.Repository.GetAll<Partida>().ToList();
        //        var jug = core.Repository.GetAll<Jugador>().ToList();
        //        var ingresos = core.Repository.GetAll<Ingreso>().ToList();
        //    }

        //    [TestMethod]
        //    public void TestMethod2()
        //    {
        //        var core = new Core("DataBaseTest2.db");



        //        var  partida = core.Services.GetPartida(); 



        //        var partidas = core.Repository.GetAll<Partida>().ToList();
        //        var ingresos = core.Repository.GetAll<Ingreso>().ToList();
        //        var jug = core.Repository.GetAll<Jugador>().ToList();



        //        var jugador = core.Services.AddNewJugador("Mikel", false, partida);
        //        var ingreso = core.Services.AddNewIngreso(jugador, 100,false, partida);

        //        var b = core.Repository.GetAll<Partida>().ToList()[0];


        //    }



        //    [TestMethod]
        //    public void TestCalcularDeudas()
        //    {
        //        var core = new Core("DataBaseTest.db");
        //        var partida1 = new Partida();
        //        var partida2 = new Partida();
        //        var jugador1 = new Jugador("Mikel", false, partida1);
        //        var jugador2 = new Jugador("Fernando", false, partida1);
        //        var jugador3 = new Jugador("Oskar", false, partida1);
        //        var jugador4 = new Jugador("Iker", false, partida1);
        //        var jugador5 = new Jugador("Miguel", false, partida1);
        //        core.Repository.Add(jugador1);
        //        core.Repository.Add(jugador2);
        //        core.Repository.Add(jugador3);
        //        core.Repository.Add(jugador4);
        //        core.Repository.Add(jugador5);

        //        core.Repository.Add(partida1);


        //        core.Repository.SaveAll();

        //        var par = core.Repository.GetAll<Partida>().ToList();

        //        List<Jugador> jugadores = core.Repository.GetAll<Jugador>().ToList();





        //        var ingreso1 = new Ingreso(jugador1, 100, false, partida1);
        //        var ingreso2 = new Ingreso(jugador2, 150, true, partida1);
        //        var ingreso3 = new Ingreso(jugador3, 150, false, partida1);
        //        var ingreso4 = new Ingreso(jugador4, 150, true, partida1);




        //        core.Repository.Add(ingreso1);
        //        core.Repository.Add(ingreso2);
        //        core.Repository.Add(ingreso3);
        //        core.Repository.Add(ingreso4);

        //        //core.Repository.Add(jugadorEnPartida1);
        //        //core.Repository.Add(jugadorEnPartida2);
        //        //core.Repository.Add(jugadorEnPartida3);
        //        //core.Repository.Add(jugadorEnPartida4);


        //        core.Repository.SaveAll();


        //        jugador1.DineroAlFinal = 0;
        //        jugador2.DineroAlFinal = 200;
        //        jugador3.DineroAlFinal = 330;
        //        jugador4.DineroAlFinal = 20;


        //        core.Services.CalcularDeudas(partida1);

        //        var partidas = core.Repository.GetAll<Partida>().ToList();
        //        var jug = core.Repository.GetAll<Jugador>().ToList();
        //        var ingresos = core.Repository.GetAll<Ingreso>().ToList();
        //    }

        //}


        [TestMethod]
        public void TestCalcularCompras()
        {
            var core = new Core("DataBaseTest.db");
            var partida1 = new Partida();

            var jugador1 = new Jugador("Mikel", false, true, "", false, partida1);
            var jugador2 = new Jugador("Fernando", false, false, "", false, partida1);
            var jugador3 = new Jugador("Iker", false, false, "", false, partida1);
            var jugador4 = new Jugador("Miguel", false, false, "", false, partida1);
            var jugador5 = new Jugador("Oskar", false, false, "", false, partida1);
            core.Repository.Add(jugador1);
            core.Repository.Add(jugador2);
            core.Repository.Add(jugador3);
            core.Repository.Add(jugador4);
            core.Repository.Add(jugador5);

            core.Repository.Add(partida1);


            core.Repository.SaveAll();

            var par = core.Repository.GetAll<Partida>().ToList();

            List<Jugador> jugadores = core.Repository.GetAll<Jugador>().ToList();





            var ingreso1 = new Ingreso(jugador1, 100, false, partida1);
            var ingreso2 = new Ingreso(jugador2, 150, true, partida1);
            var ingreso3 = new Ingreso(jugador3, 150, false, partida1);
            var ingreso4 = new Ingreso(jugador4, 150, true, partida1);




            core.Repository.Add(ingreso1);
            core.Repository.Add(ingreso2);
            core.Repository.Add(ingreso3);
            core.Repository.Add(ingreso4);

            //core.Repository.Add(jugadorEnPartida1);
            //core.Repository.Add(jugadorEnPartida2);
            //core.Repository.Add(jugadorEnPartida3);
            //core.Repository.Add(jugadorEnPartida4);


            core.Repository.SaveAll();


            var compra1 = core.Services.AddCompra("Ron", 55, partida1, jugador2,new List<Jugador> {jugador1, jugador3, jugador4});
            var compras = core.Repository.GetAll<Compra>().ToList();

            var compra2 = core.Services.AddCompra("hielo", 11, partida1, jugador2,new List<Jugador> {jugador1, jugador2, jugador3, jugador4, jugador5});
            compras = core.Repository.GetAll<Compra>().ToList();

            var compra3 = core.Services.AddCompra("pizza", 26, partida1, jugador2,new List<Jugador> {jugador1, jugador3});
             compras = core.Repository.GetAll<Compra>().ToList();

            //    var compraDirect1 = new Compra("Ron", 4332, partida1, jugador4);

          
            var partidas = core.Repository.GetAll<Partida>().ToList();
            var jug = core.Repository.GetAll<Jugador>().ToList();
            var ingresos = core.Repository.GetAll<Ingreso>().ToList();
          //  var compras = core.Repository.GetAll<Compra>().ToList();
        }
    }
}

