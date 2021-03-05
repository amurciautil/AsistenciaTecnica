using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AsistenciaTecnica
{
    /// <summary>
    /// Lógica de interacción para ParteDetalle.xaml
    /// </summary>
    public partial class ParteDetalle : Window
    {
        const int MAXIMA_LONGITUD_TEXTO = 500;
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
            try
            {
                PARTES = _vm.GuardarCambios();
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
        private void DescripcionTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int longitud = observacionesTextBox.Text.Length;
            contadorOTextBlock.Text = longitud + "/(" + MAXIMA_LONGITUD_TEXTO + " max.caracteres)";
            if (longitud > MAXIMA_LONGITUD_TEXTO)
                contadorOTextBlock.Foreground = (Brush)new BrushConverter().ConvertFromString("#F00");
            else
                contadorOTextBlock.Foreground = (Brush)new BrushConverter().ConvertFromString("#000");
        }
        private void IncidenciasTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int longitud = incidenciasTextBox.Text.Length;
            contadorITextBlock.Text = longitud + "/(" + MAXIMA_LONGITUD_TEXTO + " max.caracteres)";
            if (longitud > MAXIMA_LONGITUD_TEXTO)
                contadorITextBlock.Foreground = (Brush)new BrushConverter().ConvertFromString("#F00");
            else
                contadorITextBlock.Foreground = (Brush)new BrushConverter().ConvertFromString("#000");
        }
    }
}
