using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AsistenciaTecnica
{
    /// <summary>
    /// Lógica de interacción para ProductoDetalle.xaml
    /// </summary>
    public partial class ProductoDetalle : Window
    {
        private readonly ProductoDetalleVM _vm;
        public ObservableCollection<Producto> PRODUCTOS { get; set; }
        public ProductoDetalle(Producto producto)
        {
            try
            {
                _vm = new ProductoDetalleVM(producto);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + ". Pulse Aceptar para Salir.", "Errores", MessageBoxButton.OK, MessageBoxImage.Error);
                App.Current.Shutdown();
            }
            InitializeComponent();
            DataContext = _vm;
        }

        private void CommandBinding_Executed_CuardarCambios(object sender, ExecutedRoutedEventArgs e)
        {
            PRODUCTOS = _vm.GuardarCambios();
            DialogResult = true;
        }

        private void CommandBinding_CanExecute_GuardarCambios(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _vm.FormularioOK();
        }
        private void CommandBinding_Executed_Cancelar(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
            MessageBoxResult result = MessageBox.Show("¿Esta seguro desea salir y volver a lista de productos?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    this.Close();
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }
    }
}
