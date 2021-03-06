﻿using System;
using System.Windows;
using System.Windows.Input;


namespace AsistenciaTecnica
{
    /// <summary>
    /// Lógica de interacción para Departamento.xaml
    /// </summary>
    public partial class Departamentos : Window
    {
        private readonly DepartamentoVM _vm;
        public Departamentos()
        {
            _vm = new DepartamentoVM();
            InitializeComponent();
            DataContext = _vm;
        }

        private void CommandBinding_Executed_Editar(object sender, ExecutedRoutedEventArgs e)
        {
            _vm.EditarDepartamento();
        }

        private void CommandBinding_CanExecute_Editar(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _vm.HayDepartamentoSeleccionado();
        }

        private void CommandBinding_Executed_AñadirDepartamento(object sender, ExecutedRoutedEventArgs e)
        {
            _vm.AñadirDepartamento();
        }
        private void CommandBinding_CanExecute_GuardarCambiosDepartamento(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _vm.FormularioOk();
        }

        private void CommandBinding_Executed_GuardarCambiosDepartamento(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                _vm.GuardarCambios();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Errores", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CommandBinding_Executed_Cancelar(object sender, ExecutedRoutedEventArgs e)
        {
            _vm.Cancelar();
        }

        private void CommandBinding_CanExecute_Cancelar(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _vm.HayDatos();
        }
        private void CommandBinding_Executed_Borrar(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                MessageBoxResult result = MessageBox.Show("¿Esta seguro que quiere borrar el registro?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        string borrado = _vm.BorrarDepartamento();
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
        private void CommandBinding_CanExecute_Borrar(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _vm.SePuedeBorrar();
        }
        private void CommandBinding_Executed_Salir(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }
        private void CommandBinding_Executed_Ayuda(object sender, ExecutedRoutedEventArgs e)
        {
            _vm.Ayuda("MANTDEPARTAMENTO");
        }
    }
}
