using System.Data.SqlTypes;
using System.Windows;

namespace AsistenciaTecnica
{
    /// <summary>
    /// Lógica de interacción para CambioPasswd.xaml
    /// </summary>
    public partial class CambioPasswd : Window
    {
        public string PasswdNueva { get; set; }
        public string PasswdRepetir { get; set; }
        public string Login { get; set; }
        public bool HayCambio { get; set; }
        SqlBinary PasswordAnterior;
        private readonly CambioPasswdVM _vm;
        public CambioPasswd(string login)
        {
            _vm = new CambioPasswdVM();
            InitializeComponent();
            DataContext = this;
            // Obtener password antigua para comprobar si ha cambiado
            Login = login;
            PasswordAnterior = _vm.BuscarPassword(Login);
        }

        private void Button_Click_Aceptar(object sender, RoutedEventArgs e)
        {
            int longitudMinima = Properties.Settings.Default.minLengthPasswd;
            int longitudMaxima = Properties.Settings.Default.maxLengthPasswd;
            if (nuevaPasswordBox.Password != null && repetirPasswordBox.Password != null)
            {
                PasswdNueva = nuevaPasswordBox.Password;
                PasswdRepetir = repetirPasswordBox.Password;
                if (PasswdNueva != PasswdRepetir)
                    MessageBox.Show("Las password no son iguales", "Conformidad", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                {
                    if (PasswdNueva.Length < longitudMinima || PasswdNueva.Length > longitudMaxima)
                        MessageBox.Show("Las longitud de la password debe estar entre " + longitudMinima + " y " 
                                      + longitudMaxima, "Conformidad", MessageBoxButton.OK, MessageBoxImage.Error);
                    else
                    {
                        _vm.CambiarPassword(Login, PasswdNueva);  //cambiamos
                        SqlBinary PasswordNueva = _vm.BuscarPassword(Login); // leemos nueva
                        if (!Equals(PasswordNueva, PasswordAnterior)) // comparamos las claves binarias
                            HayCambio = true;
                        DialogResult = true;
                    }
                }
            }
            else
                MessageBox.Show("Debe indicar la password a cambiar", "Conformidad", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}
