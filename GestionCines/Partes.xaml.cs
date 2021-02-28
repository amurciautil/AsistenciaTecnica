using System.Windows;
using System.Windows.Input;


namespace AsistenciaTecnica
{
    /// <summary>
    /// Lógica de interacción para Partes.xaml
    /// </summary>
    public partial class Partes : Window
    {
        private readonly PartesVM _vm;
        public Pedido PEDIDO { get; set; }
        public string ORIGEN { get; set; } // para saber si venfo desde menu principal o desde pedidos (M/P)
        public Partes(Pedido PEDIDO, string ORIGEN)
        {
            _vm = new PartesVM(PEDIDO);
            this.PEDIDO = PEDIDO;
            this.ORIGEN = ORIGEN;
            InitializeComponent();
            DataContext = _vm;
        }

        private void CommandBinding_Executed_Añadir(object sender, ExecutedRoutedEventArgs e)
        {
            _vm.Añadir(this,PEDIDO);
        }
        private void CommandBinding_CanExecute_Añadir(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = PEDIDO.IDPEDIDO > 0;
            // si venimos a partes desde el menu principal no se pueden añadir partes porque no tenemos un pedido 
            // de referencia del cual colgar los partes
        }
        private void CommandBinding_Executed_Editar(object sender, ExecutedRoutedEventArgs e)
        {
            _vm.Editar(this);
        }

        private void CommandBinding_CanExecute_Editar(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _vm.HaySelecionada();
        }

        private void CommandBinding_Executed_Borrar(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("¿Esta seguro que quiere borrar el registro?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    string borrado = _vm.Borrar();
                    MessageBox.Show("Registro (" + borrado + ") borrado", "Baja", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }
        private void CommandBinding_Executed_Filtrar(object sender, ExecutedRoutedEventArgs e)
        {
            _vm.RefrescarFiltrado();
        }
        private void CommandBinding_Executed_Salir(object sender, ExecutedRoutedEventArgs e)
        {
            if (ORIGEN == "M")
                this.Close();
            else
                DialogResult = true;
        }
        private void CommandBinding_Executed_Ayuda(object sender, ExecutedRoutedEventArgs e)
        {
            _vm.Ayuda("MANTPARTES");
        }


    }
}
