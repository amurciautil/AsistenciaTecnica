using System.Windows;
using System.Windows.Input;


namespace AsistenciaTecnica
{
    /// <summary>
    /// Lógica de interacción para Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private readonly LoginVM _vm;
        public Usuario USUARIO { get; set; }
        public string LoginUsuario { get; set; }
        public string PasswordUsuario { get; set; }
        public string ModoDesarrollo { get; set; }
        public Login()
        {
            _vm = new LoginVM();
            InitializeComponent();
            DataContext = this;
            // Para periodo de desarrollo
            ModoDesarrollo = Properties.Settings.Default.modoDesarrollo;
        }
        private void CommandBinding_Executed_Cancelar(object sender, ExecutedRoutedEventArgs e)
        {
            _vm.Cancelar();
            USUARIO = _vm.USUARIOSELECCIONADO;
            DialogResult = true;
        }
        private void CommandBinding_Executed_Aceptar(object sender, ExecutedRoutedEventArgs e)
        {
            PasswordUsuario = passwordPasswordBox.Password;
            // Para fase de pruebas
            if (ModoDesarrollo == "S")
                _vm.BuscarUsuario("amurcia", "damfp2019");
            else
                _vm.BuscarUsuario(LoginUsuario, PasswordUsuario);

            USUARIO = _vm.USUARIOSELECCIONADO;
            if (USUARIO.LOGIN != null)
                DialogResult = true;
            else
                MessageBox.Show("Usuario o password incorrecto", "Fallo de identificación", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        private void CommandBinding_CanExecute_Aceptar(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ModoDesarrollo == "S")
                e.CanExecute = true;
            else
            {
                if (LoginUsuario != null && passwordPasswordBox.Password != null &&
                LoginUsuario.Length > 0 && passwordPasswordBox.Password.Length > 0)
                    e.CanExecute = true;
                else
                    e.CanExecute = false;
            }
        }
    }
}
