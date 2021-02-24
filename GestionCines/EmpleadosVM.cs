using System.Collections.ObjectModel;
using System.ComponentModel;

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
        public bool HaySelecionada()
        {
            return SELECCIONADA != null;
        }
        public void AñadirEmpleado(Empleados empleadosWindow)
        {
            EmpleadoDetalle empleadoDetalle = new EmpleadoDetalle(new Empleado());
            empleadoDetalle.Owner = empleadosWindow;
            if (empleadoDetalle.ShowDialog() == true)
                EMPLEADOS = empleadoDetalle.EMPLEADOS;         
        }
        public void EditarEmpleado(Empleados empleadosWindow)
        {
            EmpleadoDetalle empleadoDetalle = new EmpleadoDetalle(SELECCIONADA);
            empleadoDetalle.Owner = empleadosWindow;
            if (empleadoDetalle.ShowDialog() == true)
                EMPLEADOS = empleadoDetalle.EMPLEADOS;
        }
        public string BorrarEmpleado()
        {
            string mensajeBorre = SELECCIONADA.IDEMPLEADO + " " + SELECCIONADA.NOMBREYAPELLIDOS;
            bbdd.BorrarEmpleado(SELECCIONADA);
            EMPLEADOS = bbdd.ObtenerEmpleados(false);
            return mensajeBorre;
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
