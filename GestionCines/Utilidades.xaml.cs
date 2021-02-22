using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace AsistenciaTecnica
{
    /// <summary>
    /// Lógica de interacción para Utilidades.xaml
    /// </summary>
    public partial class Utilidades : Window
    {
        public string FormaDePago { get; set; }
        public Utilidades()
        {
            ServicioBaseDatos bbdd = new ServicioBaseDatos();
            InitializeComponent();
            DataContext = this;
        //    ObservableCollection<string> formasPago = bbdd.ObtenerFormaDePago();
       //     formaPagoComboBox.ItemsSource = formasPago;
        }
        private void CommandBinding_Executed_Aceptar(object sender, ExecutedRoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
