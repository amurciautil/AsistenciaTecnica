using System;
using System.Windows;
using System.Windows.Input;


namespace AsistenciaTecnica
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowVM _vm;
        private Usuario usuario;
        public MainWindow()
        {
            // Instanciamos la clase VistaModelo
            try
            {
                _vm = new MainWindowVM();
            }catch(Exception e)
            {
                MessageBox.Show(e.Message+ ". Pulse Aceptar para Salir.", "Errores", MessageBoxButton.OK, MessageBoxImage.Error);
                App.Current.Shutdown();
            }
            InitializeComponent();
            DataContext = _vm;
            // Logado COMENTAMOS EN FASE DE PRUEBAS
            //usuario = _vm.Logado(this);
            //if (usuario != null && usuario.EMPLEADO > 0)
            //    // Guardar Propiedades de sistema para mostrarlas en el StatusBar y conocer el usuario logado
            //    _vm.GuardarPropiedadesDeSistema(usuario);
            //else
            //    App.Current.Shutdown();
        }

        private void CommandBinding_Executed_Ayuda(object sender, ExecutedRoutedEventArgs e)
        {
            _vm.Ayuda();
        }
        private void CommandBinding_Executed_Utilidades(object sender, ExecutedRoutedEventArgs e)
        {
            _vm.Utilidades(this);
        }

        private void CommandBinding_Executed_Salir(object sender, ExecutedRoutedEventArgs e)
        {
            App.Current.Shutdown();
        }

        private void CommandBinding_CanExecute_Salir(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CommandBinding_Executed_Salas(object sender, ExecutedRoutedEventArgs e)
        {
            _vm.Salas(this);
        }
        private void CommandBinding_Executed_Cargo(object sender, ExecutedRoutedEventArgs e)
        {
            _vm.Cargo(this);
        }
        private void CommandBinding_Executed_Departamento(object sender, ExecutedRoutedEventArgs e)
        {
            _vm.Departamentos(this);
        }
        private void CommandBinding_Executed_Perfil(object sender, ExecutedRoutedEventArgs e)
        {
            _vm.Perfil(this);
        }
        private void CommandBinding_Executed_Provincia(object sender, ExecutedRoutedEventArgs e)
        {
            _vm.Provincia(this);
        }
        private void CommandBinding_Executed_Empleado(object sender, ExecutedRoutedEventArgs e)
        {
            _vm.Empleado(this);
        }
    }
}
