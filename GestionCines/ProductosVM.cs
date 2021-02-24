using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsistenciaTecnica
{
    class ProductosVM : INotifyPropertyChanged
    {
        public Producto SELECCIONADA { get; set; }
        public Producto FORMULARIO { get; set; }
        public ObservableCollection<Producto> PRODUCTOS { get; set; }
        public Modo ACCION { get; set; }
        private readonly ServicioBaseDatos bbdd;
        public ProductosVM()
        {
            bbdd = new ServicioBaseDatos();
            PRODUCTOS = bbdd.ObtenerProductos(false,false);
            FORMULARIO = new Producto();
        }
        public bool HaySelecionada()
        {
            return SELECCIONADA != null;
        }
        public void AñadirProducto(Productos productosWindow)
        {
            ProductoDetalle productoDetalle = new ProductoDetalle(new Producto());
            productoDetalle.Owner = productosWindow;
            if (productoDetalle.ShowDialog() == true)
                PRODUCTOS = productoDetalle.PRODUCTOS;
        }
        public void EditarProducto(Productos productosWindow)
        {
            ProductoDetalle productoDetalle = new ProductoDetalle(SELECCIONADA);
            productoDetalle.Owner = productosWindow;
            if (productoDetalle.ShowDialog() == true)
                PRODUCTOS = productoDetalle.PRODUCTOS;
        }
        public string BorrarProducto()
        {
            string mensajeBorre = SELECCIONADA.IDCODIGO + " " + SELECCIONADA.DESCRIPCION;
            bbdd.BorrarProducto(SELECCIONADA);
            PRODUCTOS = bbdd.ObtenerProductos(false,false);
            return mensajeBorre;
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
