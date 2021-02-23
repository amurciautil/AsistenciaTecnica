using System.Collections.ObjectModel;
using System.ComponentModel;

namespace AsistenciaTecnica
{   
    class ProvinciasVM : INotifyPropertyChanged
    {
        public Provincia SELECCIONADA { get; set; }
        public Provincia FORMULARIO { get; set; }
        public ObservableCollection<Provincia> PROVINCIAS { get; set; }
        public Modo ACCION { get; set; }

        private readonly ServicioBaseDatos bbdd;
        public ProvinciasVM()
        {
            bbdd = new ServicioBaseDatos();
            PROVINCIAS = bbdd.ObtenerProvincias(false);
            FORMULARIO = new Provincia();
            ACCION = Modo.Insertar;
        }
        public void AñadirProvincia()
        {
            FORMULARIO = new Provincia();
            ACCION = Modo.Insertar;
        }
        public void EditarProvincia()
        {
            FORMULARIO = new Provincia(SELECCIONADA);
            ACCION = Modo.Actualizar;
        }
        public string BorrarProvincia()
        {
            string mensajeBorre = SELECCIONADA.IDPROVINCIA + " " + SELECCIONADA.NOMBRE;
            FORMULARIO = new Provincia(SELECCIONADA);
            bbdd.BorrarProvincia(FORMULARIO);
            FORMULARIO = new Provincia();
            PROVINCIAS = bbdd.ObtenerProvincias(false);
            ACCION = Modo.Borrar;
            return mensajeBorre;
        }
        public bool HayProvinciaSeleccionada()
        {
              return SELECCIONADA != null;
        }
        public bool FormularioOk()
        {
            bool estado = false;
            estado = (FORMULARIO.NOMBRE != null && FORMULARIO.NOMBRE.Length > 0 && 
                      FORMULARIO.IDPROVINCIA != null && FORMULARIO.IDPROVINCIA.Length > 0 &&
                      FORMULARIO.IDPROVINCIA.Length < 3);
            if (estado && ACCION == Modo.Insertar && !bbdd.ExisteProvincia(FORMULARIO))
                estado = true;
            else
                estado = false;
            return estado;
        }
        public void GuardarCambios()
        {
            if (ACCION == Modo.Insertar)
            {
                bbdd.InsertarProvincia(FORMULARIO);
            }
            else
                bbdd.ActualizarProvincia(FORMULARIO);
            FORMULARIO = new Provincia();
            PROVINCIAS = bbdd.ObtenerProvincias(false);
        }
        public void Cancelar()
        {
            FORMULARIO = new Provincia();
        }
        public bool HayDatos()
        {
            return FORMULARIO.NOMBRE != null;
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
