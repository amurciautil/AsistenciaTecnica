using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AsistenciaTecnica
{
    /// <summary>
    /// Lógica de interacción para ParteDetalle.xaml
    /// </summary>
    public partial class ParteDetalle : Window
    {
        const int MAXIMA_LONGITUD_TEXTO = 400;
        public ObservableCollection<Parte> PARTES { get; set; }
        private readonly ParteDetalleVM _vm;
        public ParteDetalle(Parte parte)
        {
            _vm = new ParteDetalleVM(parte);
            InitializeComponent();
            DataContext = _vm;
        }
        private void CommandBinding_Executed_CuardarCambios(object sender, ExecutedRoutedEventArgs e)
        {
            PARTES = _vm.GuardarCambios();
            DialogResult = true;
        }

        private void CommandBinding_CanExecute_GuardarCambios(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _vm.FormularioOK();
        }
        private void CommandBinding_Executed_Cancelar(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBoxResult result = System.Windows.MessageBox.Show("¿Esta seguro desea salir y volver a lista de partes?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    this.Close();
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }
        private void CommandBinding_Executed_Ayuda(object sender, ExecutedRoutedEventArgs e)
        {
            _vm.Ayuda("MANTPARTEDETALLE");
        }


        private void descripcionTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int longitud = observacionesTextBox.Text.Length;
            contadorTextBlock.Text = longitud + "/(" + MAXIMA_LONGITUD_TEXTO + " max.caracteres)";
            if (longitud >= MAXIMA_LONGITUD_TEXTO)
                observacionesTextBox.IsReadOnly = true;
        }
    }
}
