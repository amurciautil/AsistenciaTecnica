using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsistenciaTecnica
{
    class EmpleadosVM : INotifyPropertyChanged
    {
        public Empleado SELECCIONADA { get; set; }
        public Empleado FORMULARIO { get; set; }
        public ObservableCollection<Empleado> EMPLEADOS { get; set; }
        public Modo ACCION { get; set; }
        private readonly ServicioBaseDatos bbdd;
        public EmpleadosVM()
        {
            bbdd = new ServicioBaseDatos();
            EMPLEADOS = bbdd.ObtenerEmpleados(false);
            FORMULARIO = new Empleado();
        }
        public void AñadirEmpleado(Empleados empleadosWindow)
        {
            EmpleadoDetalle empleadoDetalle = new EmpleadoDetalle(new Empleado());
            empleadoDetalle.Owner = empleadosWindow;
            empleadoDetalle.Show();
        }
        public void EditarEmpleado(Empleados empleadosWindow)
        {
            EmpleadoDetalle empleadoDetalle = new EmpleadoDetalle(SELECCIONADA);
            empleadoDetalle.Owner = empleadosWindow;
            empleadoDetalle.Show();
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
