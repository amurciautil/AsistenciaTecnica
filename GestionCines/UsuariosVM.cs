using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace AsistenciaTecnica
{
    class UsuariosVM : INotifyPropertyChanged
    {
        public Usuario SELECCIONADA { get; set; }
        public Usuario FORMULARIO { get; set; }
        public ObservableCollection<Usuario> USUARIOS { get; set; }
        public ObservableCollection<Perfil> PERFILES { get; set; }
        public ObservableCollection<Empleado> EMPLEADOS { get; set; }
        public ObservableCollection<string> PERFILESNOMBRE { get; set; }
        public ObservableCollection<string> EMPLEADOSNOMBRE { get; set; }
        public Modo ACCION { get; set; }

        private readonly ServicioBaseDatos bbdd;
        public UsuariosVM()
        {
            bbdd = new ServicioBaseDatos();
            USUARIOS = bbdd.ObtenerUsuarios(false);
            FORMULARIO = new Usuario();
            ACCION = Modo.Insertar;
            // Obtener valores para los combobox
            ObtenerDatosCombo();
        }
        public void ObtenerDatosCombo()
        {
            PERFILES = bbdd.ObtenerPerfiles(false);
            PERFILESNOMBRE = new ObservableCollection<string>();
            foreach (var perfil in PERFILES)
            {
                PERFILESNOMBRE.Add(perfil.NOMBRE);
            }
            EMPLEADOS = bbdd.ObtenerEmpleados(false);
            EMPLEADOSNOMBRE = new ObservableCollection<string>();
            foreach (var empleado in EMPLEADOS)
            {
                EMPLEADOSNOMBRE.Add(empleado.IDEMPLEADO + "-" + empleado.NOMBREYAPELLIDOS);
            }
        }
        public void AñadirUsuario()
        {
            FORMULARIO = new Usuario();
            ACCION = Modo.Insertar;
        }
        public void EditarUsuario()
        {
            FORMULARIO = new Usuario(SELECCIONADA);
            ACCION = Modo.Actualizar;
        }
        public string BorrarUsuario()
        {
            string mensajeBorre = SELECCIONADA.LOGIN + " " + SELECCIONADA.NOMBREEMPLEADO;
            FORMULARIO = new Usuario(SELECCIONADA);
            bbdd.BorrarUsuario(FORMULARIO);
            FORMULARIO = new Usuario();
            USUARIOS = bbdd.ObtenerUsuarios(false);
            ACCION = Modo.Borrar;
            return mensajeBorre;
        }
        public bool HayUsuarioSeleccionada()
        {
            return SELECCIONADA != null;
        }
        public bool FormularioOk()
        {
            bool correcto = false;
            int longitudMinima = Properties.Settings.Default.minLengthPasswd;
            int longitudMaxima = Properties.Settings.Default.maxLengthPasswd;
            // valido solo los obligatorios (no admiten nulos en la base de datos)
            if (FORMULARIO.LOGIN != null &&
                FORMULARIO.LOGIN.Length > 0 &&
                FORMULARIO.NOMBREPERFIL != null &&
                FORMULARIO.NOMBREEMPLEADO != null)
                correcto = true;
            if (correcto && ACCION == Modo.Insertar)
                if (FORMULARIO.PASSWORD != null &&
                    FORMULARIO.PASSWORD.Length >= longitudMinima && FORMULARIO.PASSWORD.Length <= longitudMaxima)
                    correcto = true;
                else
                    correcto = false;
            return correcto;
        }
        public void GuardarCambios()
        {
            string[] valores = FORMULARIO.NOMBREEMPLEADO.Split('-');
            FORMULARIO.IDEMPLEADO = Int32.Parse(valores[0]); 
            if (ACCION == Modo.Insertar)
            {
                bbdd.InsertarUsuario(FORMULARIO);
            }
            else
                bbdd.ActualizarUsuario(FORMULARIO);
            FORMULARIO = new Usuario();
            USUARIOS = bbdd.ObtenerUsuarios(false);
        }
        public void Cancelar()
        {
            FORMULARIO = new Usuario();
        }
        public bool HayDatos()
        {
            return FORMULARIO.LOGIN != null && FORMULARIO.LOGIN.Length > 0;
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
