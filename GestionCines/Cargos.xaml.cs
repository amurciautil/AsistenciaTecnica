using System.Windows;
using System.Windows.Input;


namespace AsistenciaTecnica
{
    /// <summary>
    /// Lógica de interacción para Cargos.xaml
    /// </summary>
    public partial class Cargos : Window
    {
        private readonly CargosVM _vm;
        public Cargos()
        {
            _vm = new CargosVM();
            InitializeComponent();
            DataContext = _vm;
        }
        private void CommandBinding_Executed_EditarCargo(object sender, ExecutedRoutedEventArgs e)
        {
            _vm.EditarCargo();
        }

        private void CommandBinding_CanExecute_EditarCargo(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _vm.HayCargoSeleccionada();
        }

        private void CommandBinding_Executed_AñadirCargo(object sender, ExecutedRoutedEventArgs e)
        {
            _vm.AñadirCargo();
        }

        private void CommandBinding_CanExecute_GuardarCambiosCargo(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _vm.FormularioOk();
        }

        private void CommandBinding_Executed_GuardarCambiosCargo(object sender, ExecutedRoutedEventArgs e)
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
        private void CommandBinding_Executed_Borrar(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("¿Esta seguro que quiere borrar el registro?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    string borrado = _vm.BorrarCargo();
                    MessageBox.Show("Registro ("+borrado+") borrado", "Baja",MessageBoxButton.OK,MessageBoxImage.Exclamation);
                    break;
                case MessageBoxResult.No:
                    break;
            }
            
        }
        private void CommandBinding_CanExecute_Borrar(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _vm.HayCargoSeleccionada();
        }
        private void CommandBinding_Executed_Salir(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }
    }
}
