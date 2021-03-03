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
    /// Lógica de interacción para Provincias.xaml
    /// </summary>
    public partial class Provincias : Window
    {
        private readonly ProvinciasVM _vm;
        public Provincias()
        {
            _vm = new ProvinciasVM();
            InitializeComponent();
            DataContext = _vm;
        }
        private void CommandBinding_Executed_EditarProvincia(object sender, ExecutedRoutedEventArgs e)
        {
            _vm.EditarProvincia();
        }

        private void CommandBinding_CanExecute_EditarProvincia(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _vm.HayProvinciaSeleccionada();
        }

        private void CommandBinding_Executed_AñadirProvincia(object sender, ExecutedRoutedEventArgs e)
        {
            _vm.AñadirProvincia();
        }

        private void CommandBinding_CanExecute_GuardarCambiosProvincia(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _vm.FormularioOk();
        }

        private void CommandBinding_Executed_GuardarCambiosProvincia(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                _vm.GuardarCambios();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Errores", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CommandBinding_Executed_Cancelar(object sender, ExecutedRoutedEventArgs e)
        {
            _vm.Cancelar();
        }

        private void CommandBinding_CanExecute_Cancelar(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _vm.HayDatos();
        }
        private void CommandBinding_Executed_Borrar(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                MessageBoxResult result = MessageBox.Show("¿Esta seguro que quiere borrar el registro?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        string borrado = _vm.BorrarProvincia();
                        MessageBox.Show("Registro (" + borrado + ") borrado", "Baja", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        break;
                    case MessageBoxResult.No:
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Errores", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        private void CommandBinding_CanExecute_Borrar(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _vm.HayProvinciaSeleccionada();
        }
        private void CommandBinding_Executed_Salir(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }
        private void CommandBinding_Executed_Ayuda(object sender, ExecutedRoutedEventArgs e)
        {
            _vm.Ayuda("MANTPROVINCIAS");
        }
    }
}
