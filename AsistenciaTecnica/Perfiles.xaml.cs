using System;
using System.Windows;
using System.Windows.Input;

namespace AsistenciaTecnica
{
    /// <summary>
    /// Lógica de interacción para Perfiles.xaml
    /// </summary>
    public partial class Perfiles : Window
    {
        private readonly PerfilesVM _vm;
        public Perfiles()
        {
            _vm = new PerfilesVM();
            InitializeComponent();
            DataContext = _vm;
        }
        private void CommandBinding_Executed_EditarPerfil(object sender, ExecutedRoutedEventArgs e)
        {
            _vm.EditarPerfil();
        }

        private void CommandBinding_CanExecute_EditarPerfil(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _vm.SePuedeBorrar();
        }

        private void CommandBinding_Executed_AñadirPerfil(object sender, ExecutedRoutedEventArgs e)
        {
            _vm.AñadirPerfil();
        }

        private void CommandBinding_CanExecute_GuardarCambiosPerfil(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _vm.FormularioOk();
        }

        private void CommandBinding_Executed_GuardarCambiosPerfil(object sender, ExecutedRoutedEventArgs e)
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
                        string borrado = _vm.BorrarPerfil();
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
            e.CanExecute = _vm.SePuedeBorrar();
        }
        private void CommandBinding_Executed_Salir(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }
        private void CommandBinding_Executed_Ayuda(object sender, ExecutedRoutedEventArgs e)
        {
            _vm.Ayuda("MANTPERFILES");
        }
    }
}
