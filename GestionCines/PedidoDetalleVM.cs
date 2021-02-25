using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace AsistenciaTecnica
{
    class PedidoDetalleVM : INotifyPropertyChanged
    {
        public Pedido FORMULARIO { get; set; }
        public Pedido SELECCIONADA { get; set; }
        public const string CONDICION_FIJA = " WHERE 1=1";
        public ObservableCollection<TipoPedido> TIPOPEDIDO { get; set; }
        public ObservableCollection<string> TIPOPEDIDODESCRIPCION { get; set; }
        public ObservableCollection<SituacionPedido> SITUACION { get; set; }
        public ObservableCollection<string> SITUACIONDESCRIPCION { get; set; }
        public ObservableCollection<Provincia> PROVINCIA { get; set; }
        public ObservableCollection<string> PROVINCIANOMBRE { get; set; }
        public Modo ACCION { get; set; }

        public ObservableCollection<Pedido> PEDIDOS { get; set; }
        private readonly ServicioBaseDatos bbdd;
        public PedidoDetalleVM(Pedido pedido)
        {
            try
            {
                bbdd = new ServicioBaseDatos();
                if (pedido.IDPEDIDO == 0)
                {
                    FORMULARIO = new Pedido();
                    ACCION = Modo.Insertar;
                }
                else
                {
                    FORMULARIO = new Pedido(pedido);
                    SELECCIONADA = new Pedido(pedido);
                    ACCION = Modo.Actualizar;
                }
                // Obtener valores para los combobox
                ObtenerDatosCombo();
            }
            catch (Exception ex)
            {
                throw new MisExcepciones(ex.Message);
            }
        }
        public void ObtenerDatosCombo()
        {
            TIPOPEDIDO = bbdd.ObtenerTipoPedido(false);
            TIPOPEDIDODESCRIPCION = new ObservableCollection<string>();
            foreach (TipoPedido tipo in TIPOPEDIDO)
            {
                TIPOPEDIDODESCRIPCION.Add(tipo.TIPODESCRIPCION);
            }
            SITUACION = bbdd.ObtenerSituacionPedidos(false);
            SITUACIONDESCRIPCION = new ObservableCollection<string>();
            foreach (SituacionPedido situacion in SITUACION)
            {
                SITUACIONDESCRIPCION.Add(situacion.SITUACIONDESCRIPCION);
            }
            PROVINCIA = bbdd.ObtenerProvincias(false);
            PROVINCIANOMBRE = new ObservableCollection<string>();
            foreach (Provincia provincia in PROVINCIA)
            {
                PROVINCIANOMBRE.Add(provincia.CODIGONOMBRE);
            }
        }
        public bool FormularioOK()
        {
            bool correcto = false;
            // valido solo los obligatorios (no admiten nulos en la base de datos)
            if (FORMULARIO.NOMBRE != null &&
                FORMULARIO.NOMBRE.Length > 0 &&
                FORMULARIO.DESCRIPCION != null &&
                FORMULARIO.DESCRIPCION.Length > 0 &&
                FORMULARIO.TELEFONO != null &&
                FORMULARIO.TELEFONO.Length > 0 &&
                FORMULARIO.DIRECCION != null &&
                FORMULARIO.DIRECCION.Length > 0 &&
                FORMULARIO.POBLACION != null &&
                FORMULARIO.POBLACION.Length > 0 &&
                FORMULARIO.NOMBREPROVINCIA != null &&
                FORMULARIO.SITUACION != null &&
                FORMULARIO.TIPOPEDIDO != null)
                correcto = true;
            return correcto;
        }
        public ObservableCollection<Pedido> GuardarCambios()
        {
            string[] valores = FORMULARIO.NOMBRESITUACION.Split('-');
            FORMULARIO.IDSITUACION = Int32.Parse(valores[0]);
            valores = FORMULARIO.NOMBREPROVINCIA.Split('-');
            FORMULARIO.IDPROVINCIA = valores[0];
            valores = FORMULARIO.NOMBRETIPOPEDIDO.Split('-');
            FORMULARIO.IDTIPOPEDIDO = Int32.Parse(valores[0]);
            if (ACCION == Modo.Insertar)
            {
                FORMULARIO.USUARIO = Properties.Settings.Default.login;
                bbdd.InsertarPedido(FORMULARIO);
            }
            else
                bbdd.ActualizarPedido(FORMULARIO);
            FORMULARIO = new Pedido();

            PEDIDOS = bbdd.ObtenerPedidos(CONDICION_FIJA,false);
            return PEDIDOS;
        }
        public void Ayuda(string codigoAyuda)
        {
            Ayuda ayuda = new Ayuda(codigoAyuda);
            ayuda.ShowDialog();
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
