using AsistenciaTecnica.Informes;
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
            utilidades.BOTONEXTERNO = Properties.Settings.Default.botonExterno;
            utilidades.BOTONINTERNO = Properties.Settings.Default.botonInterno;
            utilidades.MODODESARROLLO = Properties.Settings.Default.modoDesarrollo;
            utilidades.SITUACIONCIERRE = Properties.Settings.Default.situacionCierre;
            utilidades.DEPARTAMENTOTECNICO = Properties.Settings.Default.departamentoTecnico;
            utilidades.SITUACIONREPARACION = Properties.Settings.Default.situacionEnReparacion;
            utilidades.PERFILADMINISTRADOR = Properties.Settings.Default.perfilAdministrador;
            utilidades.LOGIN = Properties.Settings.Default.login;
            utilidades.NOMBREUSUARIO = Properties.Settings.Default.nombreUsuario;
            utilidades.PERFILUSUARIO = Properties.Settings.Default.perfilUsuario;
            utilidades.MINLONGITUDPASSWD = Properties.Settings.Default.minLengthPasswd;
            utilidades.MAXLONGITUDPASSWD = Properties.Settings.Default.maxLengthPasswd;
            utilidades.FUENTETEXTBLOCK = Properties.Settings.Default.fuenteTextBlock;
            utilidades.FUENTETEXTBOX = Properties.Settings.Default.fuenteTextBox;
            utilidades.FECHAPREVISTA = Properties.Settings.Default.fechaPrevista;

            if (utilidades.ShowDialog() == true)
            {
                Properties.Settings.Default.botonExterno = utilidades.BOTONEXTERNO;
                Properties.Settings.Default.botonInterno = utilidades.BOTONINTERNO;
                Properties.Settings.Default.fuenteTextBlock = utilidades.FUENTETEXTBLOCK;
                Properties.Settings.Default.fuenteTextBox = utilidades.FUENTETEXTBOX;
                Properties.Settings.Default.fechaPrevista = utilidades.FECHAPREVISTA;
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
            Properties.Settings.Default.perfilUsuario = usuario.PERFIL.IDPERFIL;
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
        public void Pedidos(MainWindow mainWindow)
        {
            Pedidos pedidos = new Pedidos();
            pedidos.Owner = mainWindow;
            pedidos.Show();
        }
        public void Partes(MainWindow mainWindow)
        {

            Partes partes = new Partes(new Pedido(),"M");
            partes.Owner = mainWindow;
            partes.Show();
        }
        public void MantenimientoAyuda(MainWindow mainWindow)
        {
            MantenimientoAyuda ayuda = new MantenimientoAyuda();
            ayuda.Owner = mainWindow;
            ayuda.Show();
        }
        public void InformeParteTrabajo(MainWindow mainWindow)
        {
            InformeParteDeTrabajo informeParteDeTrabajo = new InformeParteDeTrabajo();
            informeParteDeTrabajo.Show();
        }
        public void GraficoPartesTecnico(MainWindow mainWindow)
        {
            GraficosPartesTecnico graficoPartesTecnico = new GraficosPartesTecnico();
            graficoPartesTecnico.Show();
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
