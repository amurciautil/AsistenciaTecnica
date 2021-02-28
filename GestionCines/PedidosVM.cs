using System.Collections.ObjectModel;
using System.ComponentModel;

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
            SITUACIONESPEDIDO = bbdd.ObtenerSituacionPedidos(true);
            TIPOSPEDIDO = bbdd.ObtenerTipoPedido(true);
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
