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
    }
}
