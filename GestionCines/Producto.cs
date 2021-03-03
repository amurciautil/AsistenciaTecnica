using System.ComponentModel;

namespace AsistenciaTecnica
{
    public class Producto : INotifyPropertyChanged
    {
        public string IDCODIGO { get; set; }
        public string DESCRIPCION { get; set; }
        public string DESCRIPCIONAMPLIADA { get; set; }
        public double PRECIOVENTA { get; set; }
        public TipoProducto TIPOPRODUCTO { get; set; }
        public int IDTIPOPRODUCTO { get; set; }
        public string NOMBRETIPO { get; set; }
        public string NOMBRECOMBO { get; set; }
        public bool CONTROLEXISTENCIAS { get; set; }
        public double EXISTENCIAS { get; set; }
        public bool ACTIVO { get; set; }


        public Producto()
        {
            ACTIVO = true;
        }
        public Producto(string idCodigo, string descripcion,string descripcionAmpliada,double precioVenta,
            TipoProducto tipoProducto,bool controlExistencias,double existencias, bool activo)
        {
            IDCODIGO = idCodigo;
            DESCRIPCION = descripcion;
            DESCRIPCIONAMPLIADA = descripcionAmpliada;
            PRECIOVENTA = precioVenta;
            TIPOPRODUCTO = tipoProducto;
            CONTROLEXISTENCIAS = controlExistencias;
            EXISTENCIAS = existencias;
            ACTIVO = activo;
            IDTIPOPRODUCTO = TIPOPRODUCTO.IDTIPO;
            NOMBRETIPO = TIPOPRODUCTO.IDTIPO + "-" + TIPOPRODUCTO.NOMBRE;
            NOMBRECOMBO = IDCODIGO + "-" + DESCRIPCION;
        }

        public Producto(Producto producto)
        {
            IDCODIGO = producto.IDCODIGO;
            DESCRIPCION = producto.DESCRIPCION;
            DESCRIPCIONAMPLIADA = producto.DESCRIPCIONAMPLIADA;
            PRECIOVENTA = producto.PRECIOVENTA;
            TIPOPRODUCTO = producto.TIPOPRODUCTO;
            CONTROLEXISTENCIAS = producto.CONTROLEXISTENCIAS;
            EXISTENCIAS = producto.EXISTENCIAS;
            ACTIVO = producto.ACTIVO;
            IDTIPOPRODUCTO = producto.TIPOPRODUCTO.IDTIPO;
            NOMBRETIPO = producto.NOMBRETIPO;
            NOMBRECOMBO = producto.NOMBRECOMBO;
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
