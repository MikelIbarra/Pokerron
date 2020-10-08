using System;
using System.Collections.ObjectModel;
using System.Linq;
using Pokerronbank.Logic;
using PokerronBank.Model;
using PokerronBank.Model.Contracts;
using PokerronBank.UI.ViewModels.Common;
using PokerronBank.UI.ViewModels.Helper;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.OpenWhatsApp;

namespace PokerronBank.UI.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
 
        private bool _comprasIngresoView;

        #region Collections

        public ObservableCollection<JugadorViewItem> Jugadores { get; set; } = new ObservableCollection<JugadorViewItem>();

        public ObservableCollection<JugadorViewItem> JugadoresCompra { get; set; } = new ObservableCollection<JugadorViewItem>();


        public ObservableCollection<IngresoViewItem> Ingresos { get; set; } = new ObservableCollection<IngresoViewItem>();

        public ObservableCollection<ContactViewItem> Contacts { get; set; } = new ObservableCollection<ContactViewItem>();
                
        public ObservableCollection<CompraViewItem> Compras { get; set; } = new ObservableCollection<CompraViewItem>();


        #endregion

        public PartidaViewItem Partida { get; set; }

        #region Commands

        public DelegateCommand AddJugador { get; set; }
        public DelegateCommand CambiarJugador { get; set; }
        public DelegateCommand AddIngreso { get; set; }
        public DelegateCommand AddCompra { get; set; }
        public DelegateCommand CambiarCompra { get; set; }
        public DelegateCommand CalcularDeudas { get; set; }
        public DelegateCommand FinPartida { get; set; }
        public DelegateCommand ReiniciarPartida { get; set; }
        public DelegateCommand PartidaNueva { get; set; }



        #endregion

        #region Selected items

        public JugadorViewItem SelectedJugadorPicker { get; set; }
        public bool FocusOnIngresosPicker { get; set; }




        #endregion

        public string DineroEnCaja => Partida.Reference.Ingresos?.Sum(x => x.Cantidad) + "€ Pot (" +
                                      Partida.Reference.Ingresos?.Where(x => x.EsCash).Sum(x => x.Cantidad) + "€ cash)";

        public bool CajaCuadra => Partida.Reference.Ingresos?.Sum(x => x.Cantidad) ==
                                  Partida.Reference.Jugadores?.Sum(x => x.DineroAlFinal);

        public bool PartidaTerminada => Partida.Reference.Terminada;
        public string DineroParaCambiar => Partida.Reference.Ingresos?.Sum(x => x.Cantidad)  - Partida.Reference.Jugadores?.Sum(x => x.DineroAlFinal) + "€ para cuadrar";
        public string JugadoresPartida => Partida.Reference.Jugadores?.Count + " jugadores";


        public bool ComprasIngresoView
        {
            get => _comprasIngresoView;
            set
            {
                
                _comprasIngresoView = value;
                ComprasDeudasView = !value;
                if (ComprasDeudasView)
                {
                    Core.Services.CalcularDeudasCompras(Partida.Reference);
                    Jugadores.ForEach(x => x.PropertiesUpdate());
                }
            }
        }

        public bool ComprasDeudasView { get; set; }

        public string TiempoPartida
        {
            get
            {
                if (Partida.Reference.Terminada)
                {
                    return (Partida.Reference.FechaInicio - Partida.Reference.FechaFinal).ToString(@"hh\:mm") +
                           " horas";
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
            AddCompra = new DelegateCommand(UserWantToAddCompra);
            CambiarCompra = new DelegateCommand(UserWantToCambiarCompra);


            var db = DependencyService.Get<IConfigDataBase>().GetFullPath("PokerronDataBase.db");
            Core = new Core(db);


            Partida = new PartidaViewItem(Core.Services.GetPartida());

            Jugadores.Clear();
            Partida.Reference.Jugadores.OrderBy(x => Core.Services.GetDineroAlFinal(x, Partida.Reference))
                .ForEach(x => Jugadores.Insert(0, new JugadorViewItem(x, this)));

            Ingresos.Clear();
            Partida.Reference.Ingresos.ForEach(x => Ingresos.Insert(0, new IngresoViewItem(x)));

            Compras.Clear();
            Partida.Reference.Compras.ForEach(x => Compras.Insert(0, new CompraViewItem(x)));

            Device.StartTimer(new TimeSpan(0, 0, 60), () =>
            {
                // do something every 60 seconds
                Device.BeginInvokeOnMainThread(UpdateJugadores);
                return true; // runs again, or false to stop
            });

            ComprasIngresoView = true;
        }

        private void UserWantToCambiarCompra(object obj)
        {
            var cantidad = ((Tuple<decimal, string, CompraViewItem>)obj).Item1;
            var concepto = ((Tuple<decimal, string, CompraViewItem>)obj).Item2;
            var compra = ((Tuple<decimal, string, CompraViewItem>)obj).Item3;

            Core.Services.CambiarCompra(compra.Reference, concepto, cantidad, Partida.Reference, SelectedJugadorPicker.Reference, JugadoresCompra.Where(x => x.ParticipaEnCompra).Select(x => x.Reference).ToList());

            compra.PropertiesUpdate();
        }

        private void UserWantToAddCompra(object obj)
        {
            var cantidad = ((Tuple<decimal, string>)obj).Item1;
            var concepto = ((Tuple<decimal, string>)obj).Item2;

            var newCompra = Core.Services.AddCompra(concepto, cantidad, Partida.Reference, SelectedJugadorPicker.Reference, JugadoresCompra.Where(x => x.ParticipaEnCompra).Select(x=>x.Reference).ToList());
 
            Compras.Insert(0, new CompraViewItem(newCompra));
        }


        public void UpdateJugadoresCompra()
        {
            JugadoresCompra.Clear();
            Jugadores.ForEach(x => JugadoresCompra.Add(new JugadorViewItem(x.Reference, this)));
        }

        private void UserWantToCambiarJugador(object obj)
        {
            var nombre = ((Tuple<string, bool, string, JugadorViewItem>) obj).Item1;
            var escaja = ((Tuple<string, bool, string, JugadorViewItem>) obj).Item2;
            var dineroAlFinal = ((Tuple<string, bool, string, JugadorViewItem>) obj).Item3;
            var jugador = ((Tuple<string, bool, string, JugadorViewItem>) obj).Item4;

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

        public void BorrarCompra(Compra compra)
        {

            Core.Services.DeleteCompra(compra, Partida.Reference);
            Compras.Remove(Compras.FirstOrDefault(x => x.Reference == compra));

        }

        private void UserWantToReiniciarPartida(object obj)
        {
            Partida.Reference.Terminada = false;
            Core.Repository.SaveAll();
            OnPropertyChanged("");
        }

        private async void UserWantToFinPartida(object obj)
        {
            Partida.Reference.Terminada = true;
            Partida.Reference.FechaFinal = DateTime.Now;
            Core.Repository.SaveAll();
            OnPropertyChanged("");


            var contacts = await Plugin.ContactService.CrossContactService.Current.GetContactListAsync();

        }

        private void UserWantToCalcularDeudas(object obj)
        {
            Jugadores.Clear();
            Partida.Reference.Jugadores.OrderBy(x => Core.Services.GetDineroAlFinal(x, Partida.Reference))
                .ForEach(x => Jugadores.Insert(0, new JugadorViewItem(x, this)));
            Core.Services.CalcularDeudas(Partida.Reference);
            Partida.Reference.Jugadores.ForEach(x => x.DeudaDetalle = x.DeudaDetalleHelp);
           

            UpdateJugadores();
            OnPropertyChanged("");
            // return ret;
        }



        public void EnviarTodasLasDeudas()
        {
            Jugadores.ForEach(EnviarDeuda);
        }

        public void EnviarTodasLasDeudasCompra()
        {
            Jugadores.ForEach(x => EnviarDeudaCompra(x.Reference));
       
        }

        

        public void EnviarDeudaCompra(Jugador jugador)
        {

            if (!string.IsNullOrEmpty(jugador.DeudaCompraDetalle))
            {
                var ret = "***Deudas de las compras*** \n" +
                          jugador.DeudaCompraDetalle.Replace("Recibe", "Recibes")
                              .Replace("Debe", "Debes");

                ret += "\n\n***Resumen***\n" +
                       Core.Services.GetJugadorCompraDetalle(jugador, Partida.Reference)
                           .Replace("Participa", "Participas")
                           .Replace("Ha pagado", "Has pagado");

                if (!string.IsNullOrEmpty(jugador?.NumeroTelefono))
                {
                    SendWhatsApp(jugador.NumeroTelefono, ret);
                }
            }
        }

        public void EnviarDeuda(JugadorViewItem jugador)
        {
            if (!string.IsNullOrEmpty(jugador.Reference.DeudaDetalle))
            {
                 var ret = "***Final partida*** \n" +
                           jugador.Reference.DeudaDetalle.Replace("Ha", "Has")
                                  .Replace("Cancela", "Cancelas")
                                  .Replace("Recibe", "Recibes")
                                  .Replace("Debe", "Debes");

                    ret += "\n\n***Resumen***\n" + "Has ingresado:" + jugador.DineroIngresado + "\n"
                           + "Chips al final:" + jugador.DineroAlFinal
                           + "\n" + "Total ganancias:" + jugador.Total;

                    if (!string.IsNullOrEmpty(jugador?.Reference?.NumeroTelefono))
                    {
                        SendWhatsApp(jugador.Reference.NumeroTelefono, ret);
                    }
            }
        }

        public void EnviarIngreso(IngresoViewItem ingreso)
        {
            if (!string.IsNullOrEmpty(ingreso?.Reference.Jugador?.NumeroTelefono))
            {
                var ret = "*Reenvio*\nHas ingresado " + ingreso.DetalleIngreso;
                SendWhatsApp(SelectedJugadorPicker?.Reference.NumeroTelefono, ret);
            }

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

            var cantidad = ((Tuple<Decimal, bool>) obj).Item1;
            var esCash = ((Tuple<Decimal, bool>) obj).Item2;

            var newItem =Core.Services.AddNewIngreso(SelectedJugadorPicker?.Reference, cantidad, esCash, Partida.Reference);
            var newViewItem = new IngresoViewItem(newItem);
            Ingresos.Insert(0, newViewItem);

            

            if (!string.IsNullOrEmpty(newItem?.Jugador?.NumeroTelefono))
            {
                var ret = "Has ingresado " + newViewItem.DetalleIngreso;
                SendWhatsApp(SelectedJugadorPicker?.Reference.NumeroTelefono, ret);
            }
            

            UpdateJugadores();



            SelectedJugadorPicker = null;
        }

        private void UserWantToAddJugador(object obj)
        {
            var nombre = ((Tuple<string, bool, bool,bool, ContactViewItem>) obj).Item1;
            var escaja = ((Tuple<string, bool, bool, bool, ContactViewItem>) obj).Item2;
            var esAnfitrion = ((Tuple<string, bool, bool, bool, ContactViewItem>) obj).Item3;
            var whatsAppFunciona = ((Tuple<string, bool, bool, bool, ContactViewItem>) obj).Item4;
            var contacto = ((Tuple<string, bool, bool, bool, ContactViewItem>) obj).Item5;

            var newItem = Core.Services.AddNewJugador(nombre, escaja, esAnfitrion, contacto?.Numero, whatsAppFunciona, Partida.Reference);

            Jugadores.Insert(0, new JugadorViewItem(newItem,this));



        }




        public  bool SendWhatsApp(string numero, string mensaje)
        {
            var header = "💰💰💰Pokerron🍸🍸🍸\n\n" + mensaje;
            try
            {
                Chat.Open(numero, header);
                return true;
               
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}
