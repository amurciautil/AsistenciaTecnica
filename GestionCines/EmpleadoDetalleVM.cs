using System.Collections.ObjectModel;
using System.ComponentModel;

namespace AsistenciaTecnica
{
    class EmpleadoDetalleVM : INotifyPropertyChanged
    {
        public Empleado FORMULARIO { get; set; }
        public Empleado SELECCIONADA { get; set; }
        public Modo ACCION { get; set; }
        public ObservableCollection<Cargo> CARGOS { get; set; }
        public ObservableCollection<string> CARGOSNOMBRE { get; set; }
        public ObservableCollection<Departamento> DEPARTAMENTOS { get; set; }
        public ObservableCollection<string> DEPARTAMENTOSNOMBRE { get; set; }
        public ObservableCollection<Provincia> PROVINCIAS { get; set; }
        public ObservableCollection<string> PROVINCIASNOMBRE { get; set; }
        public ObservableCollection<Empleado> EMPLEADOS { get; set; }
        private readonly ServicioBaseDatos bbdd;
        public EmpleadoDetalleVM(Empleado empleado)
        {
            bbdd = new ServicioBaseDatos();
            if (empleado.IDEMPLEADO == 0)
            {
                FORMULARIO = new Empleado();
                ACCION = Modo.Insertar;
            }
            else
            {
                FORMULARIO = new Empleado(empleado);
                SELECCIONADA = new Empleado(empleado);
                ACCION = Modo.Actualizar;
            }
            // Obtener valores para los combobox
            ObtenerDatosCombo();
        }
        public void ObtenerDatosCombo()
        {
            CARGOS = bbdd.ObtenerCargos(false);
            CARGOSNOMBRE = new ObservableCollection<string>();
            foreach (var cargo in CARGOS)
            {
                CARGOSNOMBRE.Add(cargo.NOMBRE);
            }
            DEPARTAMENTOS = bbdd.ObtenerDepartamentos(false);
            DEPARTAMENTOSNOMBRE = new ObservableCollection<string>();
            foreach (var departamento in DEPARTAMENTOS)
            {
                DEPARTAMENTOSNOMBRE.Add(departamento.NOMBRE);
            }
            PROVINCIAS = bbdd.ObtenerProvincias(false);
            PROVINCIASNOMBRE = new ObservableCollection<string>();
            foreach (var departamento in PROVINCIAS)
            {
                PROVINCIASNOMBRE.Add(departamento.NOMBRE);
            }
        }
        public bool FormularioOK()
        {
            bool correcto = false;
            // valido solo los obligatorios (no admiten nulos en la base de datos)
            if (FORMULARIO.NOMBRE != null &&
                FORMULARIO.NOMBRE.Length > 0 &&
                FORMULARIO.NOMBREPROVINCIA != null &&
                FORMULARIO.NOMBRECARGO != null &&
                FORMULARIO.NOMBREDEPARTAMENTO != null)
                correcto = true;
            return correcto;
        }
        public ObservableCollection<Empleado> GuardarCambios()
        {
            if (ACCION == Modo.Insertar)
            {
                bbdd.InsertarEmpleado(FORMULARIO);
            }
            else
                bbdd.ActualizarEmpleado(FORMULARIO);
            FORMULARIO = new Empleado();
            
            EMPLEADOS = bbdd.ObtenerEmpleados(false,0);
            return EMPLEADOS;
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
