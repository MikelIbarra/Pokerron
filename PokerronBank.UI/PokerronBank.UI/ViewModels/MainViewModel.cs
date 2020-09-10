using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
using Pokerronbank.Logic;
using PokerronBank.Model;
using PokerronBank.Model.Contracts;
using PokerronBank.UI.ViewModels.Common;
using PokerronBank.UI.ViewModels.Helper;
using Sharpnado.Presentation.Forms.Commands;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace PokerronBank.UI.ViewModels
{
    public class MainViewModel : ViewModelBase 
    {
        private int _selectedViewModelIndex;

        #region Collections

        public ObservableCollection<JugadorViewItem> Jugadores { get; set; } = new ObservableCollection<JugadorViewItem>();

        public ObservableCollection<IngresoViewItem> Ingresos { get; set; } = new ObservableCollection<IngresoViewItem>();


        #endregion

        public PartidaViewItem Partida{ get; set; }

        #region Commands

        public DelegateCommand AddJugador { get; set; }
        public DelegateCommand CambiarJugador { get; set; }
        public DelegateCommand AddIngreso { get; set; }
        public DelegateCommand CalcularDeudas { get; set; }
        public DelegateCommand FinPartida { get; set; }
        public DelegateCommand ReiniciarPartida { get; set; }
        public DelegateCommand PartidaNueva { get; set; }
    


        #endregion

        #region Selected items

        public JugadorViewItem SelectedJugadorPicker { get; set; }
  


        #endregion

        public string DineroEnCaja => Partida.Reference.Ingresos?.Sum(x => x.Cantidad) +"€ Pot (" + Partida.Reference.Ingresos?.Where(x=>x.EsCash).Sum(x => x.Cantidad) + "€ cash)";
        public bool CajaCuadra => Partida.Reference.Ingresos?.Sum(x => x.Cantidad) == Partida.Reference.Jugadores?.Sum(x => x.DineroAlFinal);
        public bool PartidaTerminada => Partida.Reference.Terminada;
        public string DineroParaCambiar =>  Partida.Reference.Jugadores?.Sum(x => x.DineroAlFinal) + "€ para cambiar";
        public string JugadoresPartida => Partida.Reference.Jugadores?.Count +" jugadores";
        public string TiempoPartida
        {
            get
            {
                if (Partida.Reference.Terminada)
                {
                    return (Partida.Reference.FechaInicio - Partida.Reference.FechaFinal).ToString(@"hh\:mm") + " horas";
                }
                else
                {
                    return (Partida.Reference.FechaInicio - DateTime.Now).ToString(@"hh\:mm") + " horas";
                }
                
            }
        }


        public Core Core { get; set; }


        public MainViewModel()
        {
            AddJugador = new DelegateCommand(UserWantToAddJugador);
            CambiarJugador = new DelegateCommand(UserWantToCambiarJugador);
            AddIngreso = new DelegateCommand(UserWantToAddIngreso);
            CalcularDeudas = new DelegateCommand(UserWantToCalcularDeudas);
            FinPartida = new DelegateCommand(UserWantToFinPartida);
            ReiniciarPartida = new DelegateCommand(UserWantToReiniciarPartida);
            PartidaNueva = new DelegateCommand(UserWantToPartidaNueva);
        

            var db = DependencyService.Get<IConfigDataBase>().GetFullPath("PokerronDataBase.db");
            Core = new Core(db);

     
            Partida = new PartidaViewItem( Core.Services.GetPartida());

            Jugadores.Clear();
            Ingresos.Clear();
            Partida.Reference.Jugadores.OrderBy(x => Core.Services.GetDineroAlFinal(x, Partida.Reference)).ForEach(x => Jugadores.Insert(0,new JugadorViewItem(x, this)));
            Partida.Reference.Ingresos.ForEach(x => Ingresos.Insert(0,new IngresoViewItem(x)));

            Device.StartTimer(new TimeSpan(0, 0, 60), () =>
            {
                // do something every 60 seconds
                Device.BeginInvokeOnMainThread(UpdateJugadores);
                return true; // runs again, or false to stop
            });
        }

        private void UserWantToCambiarJugador(object obj)
        {
            var nombre = ((Tuple<string, bool,string,JugadorViewItem>)obj).Item1;
            var escaja = ((Tuple<string, bool, string, JugadorViewItem>)obj).Item2;
            var dineroAlFinal = ((Tuple<string, bool, string, JugadorViewItem>)obj).Item3;
            var jugador = ((Tuple<string, bool, string, JugadorViewItem>)obj).Item4;

            jugador.Reference.Nombre = nombre;
            jugador.Reference.EsCaja = escaja;
            jugador.DineroAlFinal = dineroAlFinal;
            Core.Repository.SaveAll();
            UpdateJugadores();
        }

        private void UserWantToPartidaNueva(object obj)
        {
            Jugadores.Clear();
            Ingresos.Clear();

            Partida = new PartidaViewItem(Core.Services.CreatePartida()) {Reference = {Terminada = false}};


            OnPropertyChanged("");
        }

        public bool BorrarJugador(Jugador jugador)
        {

            if (Partida.Reference.Ingresos.All(x => x.Jugador != jugador))
            {
                Core.Services.DeleteJugador(jugador, Partida.Reference);
                Jugadores.Remove(Jugadores.FirstOrDefault(x => x.Reference == jugador));
                UpdateJugadores();
                return true;
            }

            return false;
    
        }

        private void UserWantToReiniciarPartida(object obj)
        {
            Partida.Reference.Terminada = false;
            Core.Repository.SaveAll();
            OnPropertyChanged("");
        }

        private void UserWantToFinPartida(object obj)
        {
            Partida.Reference.Terminada = true;
            Partida.Reference.FechaFinal = DateTime.Now;
            Core.Repository.SaveAll();
            OnPropertyChanged("");

        }

        private void UserWantToCalcularDeudas(object obj)
        {
            Jugadores.Clear();
            Partida.Reference.Jugadores.OrderBy(x => Core.Services.GetDineroAlFinal(x, Partida.Reference)).ForEach(x => Jugadores.Insert(0, new JugadorViewItem(x, this)));
            var ret = Core.Services.CalcularDeudas(Partida.Reference);
            UpdateJugadores();
            OnPropertyChanged("");
            // return ret;
        }

        


        public void UpdateJugadores()
        {
            Jugadores.ForEach(x => x.PropertiesUpdate());
            OnPropertyChanged("");
        }


        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            OnPropertyChanged("TiempoPartida");
        }

       
        public bool CheckExistNombreJugador(string nombre)
        {
            return Core.Services.CheckExistNombreJugador(nombre, Partida.Reference);
        }



        private void UserWantToAddIngreso(object obj)
        {
           
            var cantidad = ((Tuple<Decimal, bool>)obj).Item1;
            var esCash = ((Tuple<Decimal, bool>)obj).Item2;

            var newItem = Core.Services.AddNewIngreso(SelectedJugadorPicker?.Reference, cantidad, esCash, Partida.Reference);
            if (Ingresos.Count > 0)
            {
                Ingresos.Insert(0,new IngresoViewItem(newItem));
            }
            else
            {
                Ingresos.Add(new IngresoViewItem(newItem));
            }
            
            UpdateJugadores();



            SelectedJugadorPicker = null;
        }

        private void UserWantToAddJugador(object obj)
        {
            var nombre = ((Tuple<string, bool>)obj).Item1;
            var escaja = ((Tuple<string, bool>)obj).Item2;

            var newItem = Core.Services.AddNewJugador(nombre, escaja, Partida.Reference);
            if (Jugadores.Count > 0)
            {
                Jugadores.Insert(0, new JugadorViewItem(newItem, this));
            }
            else
            {
                Jugadores.Add(new JugadorViewItem(newItem, this));
            }

          
        }

    }
}
