using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace AsistenciaTecnica
{
    class PedidosVM : INotifyPropertyChanged
    {
        public const string CONDICION_FIJA = " WHERE 1=1";
        public SituacionPedido SITUACIONESELECCIONADA { get; set; }
        public ObservableCollection<SituacionPedido> SITUACIONESPEDIDO { get; set; }
        public TipoPedido TIPOPEDIDOESELECCIONADA { get; set; }
        public ObservableCollection<TipoPedido> TIPOSPEDIDO { get; set; }
        public string TELEFONO { get; set; }
        public string POBLACION { get; set; }
        public string NUMEROPEDIDO { get; set; }
        public string AÑOSELECCIONADO { get; set; }
        public ObservableCollection<string> AÑOS { get; set; }
        public string MESSELECCIONADO { get; set; }
        public ObservableCollection<string> MESES { get; set; }
        public Pedido SELECCIONADA { get; set; }
        public Pedido FORMULARIO { get; set; }
        public ObservableCollection<Pedido> PEDIDOS { get; set; }
        public Modo ACCION { get; set; }
        private readonly ServicioBaseDatos bbdd;
        public PedidosVM()
        {
            bbdd = new ServicioBaseDatos();
            PEDIDOS = bbdd.ObtenerPedidos(CONDICION_FIJA,false);
            FORMULARIO = new Pedido();
            // Datos para filtrado
            SITUACIONESELECCIONADA = new SituacionPedido();
            TIPOPEDIDOESELECCIONADA = new TipoPedido();
            SITUACIONESPEDIDO = bbdd.ObtenerSituacionPedidos(false);
            TIPOSPEDIDO = bbdd.ObtenerTipoPedido(true);
            AÑOS = new ObservableCollection<string>();
            AÑOS.Add("");
            AÑOS.Add(DateTime.Now.Year.ToString());
            for (int i = 2020; i <= 2040; i++)
                if (i.ToString() != DateTime.Now.Year.ToString())
                    AÑOS.Add(i.ToString());
            MESES = new ObservableCollection<string>();
            MESES.Add("");
            MESES.Add(DateTime.Now.Month.ToString());
            for (int i = 1; i <= 12; i++)
                if (i.ToString() != DateTime.Now.Month.ToString())
                    MESES.Add(i.ToString());
        }
        public bool HaySelecionada()
        {
            return SELECCIONADA != null;
        }
        public void Añadir(Pedidos pedidosWindow)
        {
            PedidoDetalle pedidoDetalle = new PedidoDetalle(new Pedido());
            pedidoDetalle.Owner = pedidosWindow;
            if (pedidoDetalle.ShowDialog() == true)
                PEDIDOS = pedidoDetalle.PEDIDOS;
        }
        public void Editar(Pedidos pedidosWindow)
        {
            PedidoDetalle pedidoDetalle = new PedidoDetalle(SELECCIONADA);
            pedidoDetalle.Owner = pedidosWindow;
            if (pedidoDetalle.ShowDialog() == true)
                PEDIDOS = pedidoDetalle.PEDIDOS;
        }
        public string Borrar()
        {
            string mensajeBorre = SELECCIONADA.IDPEDIDO + " " + SELECCIONADA.DESCRIPCION;
            bbdd.BorrarPedido(SELECCIONADA);
            PEDIDOS = bbdd.ObtenerPedidos(CONDICION_FIJA,false);
            return mensajeBorre;
        }
        public void RefrescarFiltrado()
        {
            string condicion_filtro = CONDICION_FIJA;
            string patron = @"^[0-9]*$";
            Regex pa = new Regex(patron);
            if (NUMEROPEDIDO != null && NUMEROPEDIDO.Trim(' ').Length > 0 && pa.IsMatch(NUMEROPEDIDO))
                condicion_filtro += " AND pe.idPedido = " + NUMEROPEDIDO;
            if (AÑOSELECCIONADO != null && AÑOSELECCIONADO.Length > 0)
                condicion_filtro += " AND YEAR(pe.fechaIntroduccion) = " + Convert.ToInt32(AÑOSELECCIONADO);
            if (MESSELECCIONADO != null && MESSELECCIONADO.Length > 0)
                condicion_filtro += " AND MONTH(pe.fechaIntroduccion) = " + Convert.ToInt32(MESSELECCIONADO);
            if (SITUACIONESELECCIONADA.IDSITUACION != 0)
                condicion_filtro += " AND pe.situacion = '" + SITUACIONESELECCIONADA.IDSITUACION + "'";
            if (TIPOPEDIDOESELECCIONADA.IDTIPO != 0)
               condicion_filtro += " AND pe.tipoPedido = '" + TIPOPEDIDOESELECCIONADA.IDTIPO + "'";
            if (TELEFONO != null && TELEFONO.Trim(' ').Length > 0)
                condicion_filtro += " AND pe.telefono LIKE '%" + TELEFONO + "%'";
            if (POBLACION != null && POBLACION.Trim(' ').Length > 0)
                condicion_filtro += " AND pe.poblacion LIKE '%" + POBLACION + "%'";
            PEDIDOS = bbdd.ObtenerPedidos(condicion_filtro,false);
        }
        public void Partes(Pedidos pedidosWindow)
        {
            Partes partes = new Partes(SELECCIONADA,"P");
            partes.Owner = pedidosWindow;
            if (partes.ShowDialog() == true)
            {
                PEDIDOS = bbdd.ObtenerPedidos(CONDICION_FIJA, false);
            }
        }
        public void Ayuda(string codigoAyuda)
        {
            Ayuda ayuda = new Ayuda(codigoAyuda);
            ayuda.ShowDialog();
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
