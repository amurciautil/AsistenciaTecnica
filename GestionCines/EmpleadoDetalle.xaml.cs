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
    /// Lógica de interacción para EmpleadoDetalle.xaml
    /// </summary>
    partial class EmpleadoDetalle : Window
    {
        public Empleado FORMULARIO { get; set; }
        public Empleado SELECCIONADA { get; set; }
        public EmpleadoDetalle(Empleado empleado)
        {
            if (empleado.IDEMPLEADO == 0)
            {
                FORMULARIO = empleado;
                SELECCIONADA = new Empleado();
            }
            else
            {
                FORMULARIO = new Empleado(empleado);
                SELECCIONADA = empleado;
            }
            InitializeComponent();
            DataContext = this;
        }
    }
}
