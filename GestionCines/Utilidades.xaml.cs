using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace AsistenciaTecnica
{
    /// <summary>
    /// Lógica de interacción para Utilidades.xaml
    /// </summary>
    public partial class Utilidades : Window
    {
        public int BOTONEXTERNO { get; set; }
        public int BOTONINTERNO { get; set; }
        public string MODODESARROLLO { get; set; }
        public int SITUACIONCIERRE { get; set; }
        public int SITUACIONREPARACION { get; set; }
        public int DEPARTAMENTOTECNICO { get; set; }
        public int PERFILADMINISTRADOR { get; set; }
        public string LOGIN { get; set; }
        public string NOMBREUSUARIO { get; set; }
        public int PERFILUSUARIO { get; set; }
        public int MINLONGITUDPASSWD { get; set; }
        public int MAXLONGITUDPASSWD { get; set; }
        public int FUENTETEXTBLOCK { get; set; }
        public int FUENTETEXTBOX { get; set; }
        public int FECHAPREVISTA { get; set; }
        public Utilidades()
        {
            ServicioBaseDatos bbdd = new ServicioBaseDatos();
            InitializeComponent();
            DataContext = this;
        }
        private void CommandBinding_Executed_CuardarCambios(object sender, ExecutedRoutedEventArgs e)
        {
            DialogResult = true;
        }
        private void CommandBinding_CanExecute_GuardarCambios(object sender, CanExecuteRoutedEventArgs e)
        {
            string patron = @"^[0-9]*$";
            Regex pa = new Regex(patron);
            if (pa.IsMatch(BOTONINTERNO.ToString()) && 
                pa.IsMatch(BOTONEXTERNO.ToString()) &&
                pa.IsMatch(FUENTETEXTBLOCK.ToString()) &&
                pa.IsMatch(FUENTETEXTBOX.ToString()))  
                e.CanExecute = true;
            else
                e.CanExecute = false;
        }
        private void CommandBinding_Executed_Cancelar(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }
    }
}
