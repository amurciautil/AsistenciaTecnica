using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsistenciaTecnica
{
    class ProductosPedidoVM : INotifyPropertyChanged
    {
        public ProductoPedido SELECCIONADA { get; set; }
        public ProductoPedido FORMULARIO { get; set; }
        public ObservableCollection<Producto> PRODUCTOS { get; set; }
        public ObservableCollection<String> PRODUCTOSNOMBRE { get; set; }
        public Pedido PEDIDO { get; set; }
        private readonly ServicioBaseDatos bbdd;
        public ObservableCollection<ProductoPedido> PRODUCTOSPEDIDO { get; set; }
        public Modo ACCION { get; set; }
        public ProductosPedidoVM(Pedido pedido)
        {
            PEDIDO = pedido;
            bbdd = new ServicioBaseDatos();
            PRODUCTOSPEDIDO = bbdd.ObtenerProductoPedido(false, PEDIDO.IDPEDIDO);
            FORMULARIO = new ProductoPedido(PEDIDO);
            ACCION = Modo.Insertar;
            // Obtener valores para los combobox
            ObtenerDatosCombo();
        }
        public void ObtenerDatosCombo()
        {
            PRODUCTOS = bbdd.ObtenerProductos(false, true);
            PRODUCTOSNOMBRE = new ObservableCollection<string>();
            foreach (Producto producto in PRODUCTOS)
            {
                PRODUCTOSNOMBRE.Add(producto.NOMBRECOMBO);
            }
        }
        public void Añadir()
        {
            FORMULARIO = new ProductoPedido(PEDIDO);
            ACCION = Modo.Insertar;
        }
        public void Editar()
        {
            FORMULARIO = new ProductoPedido(SELECCIONADA);
            ACCION = Modo.Actualizar;
        }
        public string Borrar()
        {
            try
            {
                string mensajeBorre = SELECCIONADA.ID + " " + SELECCIONADA.PRODUCTO.DESCRIPCION;
                FORMULARIO = new ProductoPedido(SELECCIONADA);
                bbdd.BorrarProductoPedido(FORMULARIO);
                FORMULARIO = new ProductoPedido(PEDIDO);
                PRODUCTOSPEDIDO = bbdd.ObtenerProductoPedido(false, FORMULARIO.PEDIDO.IDPEDIDO);
                ACCION = Modo.Borrar;
                return mensajeBorre;
            }
            catch (Exception e)
            {
                throw new MisExcepciones(e.Message);
            }
        }
        public bool HaySeleccionada()
        {
            return SELECCIONADA != null;
        }
        public bool FormularioOk()
        {
            bool correcto;
            if (FORMULARIO.NOMBREPRODUCTO != null)
                correcto = true;
            else
                correcto = false;
            return correcto;
        }
        public void GuardarCambios()
        {
            try
            {
                string[] valores = FORMULARIO.NOMBREPRODUCTO.Split('-');
                FORMULARIO.PRODUCTO.IDCODIGO = valores[0];
                if (ACCION == Modo.Insertar)
                {
                    bbdd.InsertarProductoPedido(FORMULARIO);
                }
                else
                    bbdd.ActualizarProductoPedido(FORMULARIO);
                FORMULARIO = new ProductoPedido(PEDIDO);
                PRODUCTOSPEDIDO = bbdd.ObtenerProductoPedido(false, FORMULARIO.PEDIDO.IDPEDIDO);
            }
            catch (Exception e)
            {
                throw new MisExcepciones(e.Message);
            }
        }
        public void Cancelar()
        {
            FORMULARIO = new ProductoPedido();
        }
        public bool HayDatos()
        {
            return FORMULARIO.NOMBREPRODUCTO != null && FORMULARIO.NOMBREPRODUCTO.Length > 0;
        }
        public void Ayuda(string codigoAyuda)
        {
            Ayuda ayuda = new Ayuda(codigoAyuda);
            ayuda.ShowDialog();
        }

        public double Buscar()
        {
            if (FORMULARIO.NOMBREPRODUCTO != null)
            {
                string[] valores = FORMULARIO.NOMBREPRODUCTO.Split('-');
                FORMULARIO.PRECIOVENTA = bbdd.ObtenerProducto(valores[0]);
            }
            return FORMULARIO.PRECIOVENTA;
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
