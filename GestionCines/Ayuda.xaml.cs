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
    /// Lógica de interacción para Ayuda.xaml
    /// </summary>
    public partial class Ayuda : Window
    {
        private readonly AyudaVM _vm;
        public Ayuda(string codigoAyuda)
        {
            _vm = new AyudaVM(codigoAyuda);
            InitializeComponent();
            DataContext = _vm;
        }

        private void CommandBinding_Executed_Aceptar(object sender, ExecutedRoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
