using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
            EMPLEADOS =_vm.GuardarCambios();
            DialogResult = true;
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

    }
}
