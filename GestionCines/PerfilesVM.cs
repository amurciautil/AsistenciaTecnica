using System.Collections.ObjectModel;
using System.ComponentModel;

namespace AsistenciaTecnica
{   
    class PerfilesVM : INotifyPropertyChanged
    {
        public Perfil SELECCIONADA { get; set; }
        public Perfil FORMULARIO { get; set; }
        public ObservableCollection<Perfil> PERFILES { get; set; }
        public Modo ACCION { get; set; }

        private readonly ServicioBaseDatos bbdd;
        public PerfilesVM()
        {
            bbdd = new ServicioBaseDatos();
            PERFILES = bbdd.ObtenerPerfiles(false);
            FORMULARIO = new Perfil();
            ACCION = Modo.Insertar;
        }
        public void AñadirPerfil()
        {
            FORMULARIO = new Perfil();
            ACCION = Modo.Insertar;
        }
        public void EditarPerfil()
        {
            FORMULARIO = new Perfil(SELECCIONADA);
            ACCION = Modo.Actualizar;
        }
        public string BorrarPerfil()
        {
            string mensajeBorre = SELECCIONADA.IDPERFIL + " " + SELECCIONADA.NOMBRE;
            FORMULARIO = new Perfil(SELECCIONADA);
            bbdd.BorrarPerfil(FORMULARIO);
            FORMULARIO = new Perfil();
            PERFILES = bbdd.ObtenerPerfiles(false);
            ACCION = Modo.Borrar;
            return mensajeBorre;
        }
        public bool HayPerfilSeleccionada()
        {
              return SELECCIONADA != null;
        }
        public bool FormularioOk()
        {
            return (FORMULARIO.NOMBRE != null && FORMULARIO.NOMBRE.Length > 0);
        }
        public void GuardarCambios()
        {
            if (ACCION == Modo.Insertar)
            {
                bbdd.InsertarPerfil(FORMULARIO);
            }
            else
                bbdd.ActualizarPerfil(FORMULARIO);
            FORMULARIO = new Perfil();
            PERFILES = bbdd.ObtenerPerfiles(false);
        }
        public void Cancelar()
        {
            FORMULARIO = new Perfil();
        }
        public bool HayDatos()
        {
            return FORMULARIO.NOMBRE != null;
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
