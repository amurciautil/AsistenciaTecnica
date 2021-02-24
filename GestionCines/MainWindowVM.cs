using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Forms;

namespace AsistenciaTecnica
{
    class MainWindowVM : INotifyPropertyChanged
    {
        private readonly ServicioBaseDatos bbdd;
        public MainWindowVM()
        {
            bbdd = new ServicioBaseDatos();
        }

        public void Ayuda()
        {
            Help.ShowHelp(null, "AsistenciaTecnica.chm");
        }      
        public void Utilidades(MainWindow mainWindow)
        {
            Utilidades utilidades = new Utilidades();
            utilidades.Owner = mainWindow;
            utilidades.FormaDePago = Properties.Settings.Default.formaDePago;
            if (utilidades.ShowDialog() == true)
            {
                Properties.Settings.Default.formaDePago = utilidades.FormaDePago;
                Properties.Settings.Default.Save();
            }
        }
        public Usuario Logado(MainWindow mainWindow)
        {
            Login login = new Login();
            login.ShowDialog();
            return login.USUARIO;
        }
        public void GuardarPropiedadesDeSistema(Usuario usuario)
        {
            Empleado empleado = bbdd.BuscarEmpleado(usuario.EMPLEADO.IDEMPLEADO);
            Properties.Settings.Default.login = usuario.LOGIN;
            Properties.Settings.Default.nombreUsuario = empleado.ObtenerNombreCompleto();
            Properties.Settings.Default.Save();
        }
        public void Cargo(MainWindow mainWindow)
        {
            Cargos cargos = new Cargos();
            cargos.Owner = mainWindow;
            cargos.Show();
        }
        public void Departamentos(MainWindow mainWindow)
        {
            Departamentos departamentos = new Departamentos();
            departamentos.Owner = mainWindow;
            departamentos.Show();
        }
        public void Perfil(MainWindow mainWindow)
        {
            Perfiles perfiles = new Perfiles();
            perfiles.Owner = mainWindow;
            perfiles.Show();
        }
        public void Provincia(MainWindow mainWindow)
        {
            Provincias provincias = new Provincias();
            provincias.Owner = mainWindow;
            provincias.Show();
        }
        public void Empleado(MainWindow mainWindow)
        {
            Empleados empleados = new Empleados();
            empleados.Owner = mainWindow;
            empleados.Show();
        }
        public void Usuario(MainWindow mainWindow)
        {
            Usuarios usuarios = new Usuarios();
            usuarios.Owner = mainWindow;
            usuarios.Show();
        }
        public bool CambioPassword(MainWindow mainWindow)
        {
            bool hayCambio = false;
            CambioPasswd cambioPasswd = new CambioPasswd(Properties.Settings.Default.login);
            cambioPasswd.Owner = mainWindow;
            cambioPasswd.Login = Properties.Settings.Default.login;
            cambioPasswd.HayCambio = false;
            if (cambioPasswd.ShowDialog() == true)
                hayCambio = cambioPasswd.HayCambio;
            return hayCambio;
        }
        public void Productos(MainWindow mainWindow)
        {
            Productos productos = new Productos();
            productos.Owner = mainWindow;
            productos.Show();
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
