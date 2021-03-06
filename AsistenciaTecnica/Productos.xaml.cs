using System;
using System.Windows;
using System.Windows.Input;

namespace AsistenciaTecnica
{
    /// <summary>
    /// Lógica de interacción para Productos.xaml
    /// </summary>
    public partial class Productos : Window
    {
        private readonly ProductosVM _vm;
        public Productos()
        {
            _vm = new ProductosVM();
            InitializeComponent();
            DataContext = _vm;
        }
        private void CommandBinding_Executed_Añadir(object sender, ExecutedRoutedEventArgs e)
        {
            _vm.AñadirProducto(this);
        }
        private void CommandBinding_Executed_Editar(object sender, ExecutedRoutedEventArgs e)
        {
            _vm.EditarProducto(this);
        }

        private void CommandBinding_CanExecute_Editar(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _vm.HaySelecionada();
        }

        private void CommandBinding_Executed_Borrar(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                MessageBoxResult result = MessageBox.Show("¿Esta seguro que quiere borrar el registro?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        string borrado = _vm.BorrarProducto();
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

        private void CommandBinding_Executed_Salir(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }
    }
}
