using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace AsistenciaTecnica
{
    /// <summary>
    /// Lógica de interacción para EmpleadoDetalle.xaml
    /// </summary>
    partial class EmpleadoDetalle : Window
    {
        private readonly EmpleadoDetalleVM _vm;
        public ObservableCollection<Empleado> EMPLEADOS { get; set; }
        public EmpleadoDetalle(Empleado empleado)
        {
            _vm = new EmpleadoDetalleVM(empleado);
            InitializeComponent();
            DataContext = _vm;
        }

        private void CommandBinding_Executed_CuardarCambios(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                EMPLEADOS = _vm.GuardarCambios();
                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Errores", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CommandBinding_CanExecute_GuardarCambios(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _vm.FormularioOK();
        }
        private void CommandBinding_Executed_Cancelar(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
            MessageBoxResult result = MessageBox.Show("¿Esta seguro desea salir y volver a lista de empleados?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    this.Close();
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }
        private void CommandBinding_Executed_Salir(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }
        private void CommandBinding_Executed_Ayuda(object sender, ExecutedRoutedEventArgs e)
        {
            _vm.Ayuda("MANTEMPLEADOSDETALLE");
        }
    }
}
