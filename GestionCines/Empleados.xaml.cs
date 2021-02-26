using System.Windows;
using System.Windows.Input;

namespace AsistenciaTecnica
{
    /// <summary>
    /// Lógica de interacción para Empleados.xaml
    /// </summary>
    public partial class Empleados : Window
    {
        private readonly EmpleadosVM _vm;
        public Empleados()
        {
            _vm = new EmpleadosVM();
            InitializeComponent();
            DataContext = _vm;
        }
        private void CommandBinding_Executed_AñadirEmpleado(object sender, ExecutedRoutedEventArgs e)
        {
            _vm.AñadirEmpleado(this);
        }
        private void CommandBinding_Executed_EditarEmpleado(object sender, ExecutedRoutedEventArgs e)
        {
            _vm.EditarEmpleado(this);
        }

        private void CommandBinding_CanExecute_EditarEmpleado(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _vm.HaySelecionada();
        }

        private void CommandBinding_Executed_Borrar(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("¿Esta seguro que quiere borrar el registro?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    string borrado = _vm.BorrarEmpleado();
                    MessageBox.Show("Registro (" + borrado + ") borrado", "Baja", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    break;
                case MessageBoxResult.No:
                    break;
            } 
        }
        private void CommandBinding_Executed_Salir(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }
    }
}
