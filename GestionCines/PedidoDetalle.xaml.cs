using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AsistenciaTecnica
{
    /// <summary>
    /// Lógica de interacción para PedidoDetalle.xaml
    /// </summary>
    public partial class PedidoDetalle : Window
    {
        const int MAXIMA_LONGITUD_TEXTO = 500;
        private readonly PedidoDetalleVM _vm;
        public ObservableCollection<Pedido> PEDIDOS { get; set; }
        public PedidoDetalle(Pedido pedido)
        {
            _vm = new PedidoDetalleVM(pedido);
            InitializeComponent();
            DataContext = _vm;
        }
        private void CommandBinding_Executed_CuardarCambios(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                PEDIDOS = _vm.GuardarCambios();
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
            MessageBoxResult result = System.Windows.MessageBox.Show("¿Esta seguro desea salir y volver a lista de pedidos?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);
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
            _vm.Ayuda("MANTPEDIDODETALLE");
        }

        private void CommandBinding_Executed_Buscar(object sender, ExecutedRoutedEventArgs e)
        {
            _vm.Buscar();
        }

        private void CommandBinding_CanExecute_Buscar(object sender, CanExecuteRoutedEventArgs e)
        {

            e.CanExecute = _vm.HayTelefono();
        }

        private void descripcionTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int longitud = descripcionTextBox.Text.Length;
            contadorTextBlock.Text = longitud + "/(" + MAXIMA_LONGITUD_TEXTO + " max.caracteres)";
            if (longitud >= MAXIMA_LONGITUD_TEXTO)
                descripcionTextBox.IsReadOnly = true;
        }
    }
}
