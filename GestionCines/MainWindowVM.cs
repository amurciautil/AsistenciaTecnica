using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Forms;

namespace AsistenciaTecnica
{
    class MainWindowVM : INotifyPropertyChanged
    {
        public bool HAYPELICULASCARGADAS { get; set; }
        public ObservableCollection<Pelicula> PELICULAS { get; set; }
        private readonly ServicioBaseDatos bbdd;
        public MainWindowVM()
        {
            bbdd = new ServicioBaseDatos();
     


        }

        public void Ayuda()
        {
            Help.ShowHelp(null, "GestionCines.chm");
        }

        public void Salas(MainWindow mainWindow)
        {
            Salas salas = new Salas();
            salas.Owner = mainWindow;
            salas.Show();
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
            Empleado empleado = bbdd.BuscarEmpleado(usuario.EMPLEADO);
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
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
