using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace AsistenciaTecnica
{
    class ProductoDetalleVM : INotifyPropertyChanged
    {
        public Producto FORMULARIO { get; set; }
        public Producto SELECCIONADA { get; set; }
        public ObservableCollection<TipoProducto> TIPOPRODUCTO { get; set; }
        public ObservableCollection<string> TIPOSNOMBRE { get; set; }
        public Modo ACCION { get; set; }

        public ObservableCollection<Producto> PRODUCTOS { get; set; }
        private readonly ServicioBaseDatos bbdd;
        public ProductoDetalleVM(Producto producto)
        {
            try
            {
                bbdd = new ServicioBaseDatos();
                if (producto.DESCRIPCION == null)
                {
                    FORMULARIO = new Producto();
                    ACCION = Modo.Insertar;
                }
                else
                {
                    FORMULARIO = new Producto(producto);
                    SELECCIONADA = new Producto(producto);
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
            TIPOPRODUCTO = bbdd.ObtenerTipoProducto(false);
            TIPOSNOMBRE = new ObservableCollection<string>();
            foreach (var tipos in TIPOPRODUCTO)
            {
                TIPOSNOMBRE.Add(tipos.NOMBRETIPO);
            }
        }
        public bool FormularioOK()
        {
            bool correcto = false;
            // valido solo los obligatorios (no admiten nulos en la base de datos)
            if (FORMULARIO.IDCODIGO != null &&
                FORMULARIO.IDCODIGO.Length > 0 &&
                FORMULARIO.DESCRIPCION != null &&
                FORMULARIO.NOMBRETIPO != null)
                correcto = true;
            // control de duplicado
            if (ACCION == Modo.Insertar && correcto)
                if (bbdd.ExisteProducto(FORMULARIO))
                    correcto = false;
            return correcto;
        }
        public ObservableCollection<Producto> GuardarCambios()
        {
            string[] valores = FORMULARIO.NOMBRETIPO.Split('-');
            FORMULARIO.IDTIPOPRODUCTO = Int32.Parse(valores[0]);
            if (ACCION == Modo.Insertar)
            {
                bbdd.InsertarProducto(FORMULARIO);
            }
            else
                bbdd.ActualizarProducto(FORMULARIO);
            FORMULARIO = new Producto();

            PRODUCTOS = bbdd.ObtenerProductos(false, false);
            return PRODUCTOS;
        }
        public void Ayuda(string codigoAyuda)
        {
            Ayuda ayuda = new Ayuda(codigoAyuda);
            ayuda.ShowDialog();
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
