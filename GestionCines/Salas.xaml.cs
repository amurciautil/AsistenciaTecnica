using System;
using System.Collections.Generic;
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
    /// Lógica de interacción para Salas.xaml
    /// </summary>
    public partial class Salas : Window
    {
        private readonly SalasVM _vm;
        public Salas()
        {
            _vm = new SalasVM();
            InitializeComponent();
            try
            {
                DataContext = _vm;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + ". Pulse Aceptar para Salir.", "Errores", MessageBoxButton.OK, MessageBoxImage.Error);
                App.Current.Shutdown();
            }
        }

        private void CommandBinding_Executed_EditarSala(object sender, ExecutedRoutedEventArgs e)
        {
            _vm.EditarSala();
        }

        private void CommandBinding_CanExecute_EditarSala(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _vm.HaySalaSeleccionada();
        }

        private void CommandBinding_Executed_AñadirSala(object sender, ExecutedRoutedEventArgs e)
        {
            _vm.AñadirSala();
        }

        private void CommandBinding_CanExecute_GuardarCambiosSala(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _vm.FormularioOk();
        }

        private void CommandBinding_Executed_GuardarCambiosSala(object sender, ExecutedRoutedEventArgs e)
        {
            _vm.GuardarCambios();
        }

        private void CommandBinding_Executed_Cancelar(object sender, ExecutedRoutedEventArgs e)
        {
            _vm.Cancelar();
        }

        private void CommandBinding_CanExecute_Cancelar(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _vm.HayDatos();
        }
    }
}
