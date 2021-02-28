using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsistenciaTecnica
{
    public enum Modo
    {
        Insertar, Actualizar, Borrar
    }
    public class DepartamentoVM : INotifyPropertyChanged
    {

        public Departamento DEPARTAMENTOSELECCIONADO { get; set; }
        public Departamento DEPARTAMENTOFORMULARIO { get; set; }
        public ObservableCollection<Departamento> DEPARTAMENTOS { get; set; }
        public Modo ACCION { get; set; }
        private readonly ServicioBaseDatos bbdd;
        public DepartamentoVM()
        {
            bbdd = new ServicioBaseDatos();
            DEPARTAMENTOS = bbdd.ObtenerDepartamentos(false);
            DEPARTAMENTOFORMULARIO = new Departamento();
            ACCION = Modo.Insertar;
        }
        public void AñadirDepartamento()
        {
            DEPARTAMENTOFORMULARIO = new Departamento();
            ACCION = Modo.Insertar;
        }
        public void EditarDepartamento()
        {
            DEPARTAMENTOFORMULARIO = new Departamento(DEPARTAMENTOSELECCIONADO);
            ACCION = Modo.Actualizar;
        }
        public string BorrarDepartamento()
        {
            string mensajeBorre = DEPARTAMENTOSELECCIONADO.IDDEPARTAMENTO + " " + DEPARTAMENTOSELECCIONADO.NOMBRE;
            DEPARTAMENTOFORMULARIO = new Departamento(DEPARTAMENTOSELECCIONADO);
            bbdd.BorrarDepartamento(DEPARTAMENTOFORMULARIO);
            DEPARTAMENTOFORMULARIO = new Departamento();
            DEPARTAMENTOS = bbdd.ObtenerDepartamentos(false);
            ACCION = Modo.Borrar;
            return mensajeBorre;
        }
        public bool HayDepartamentoSeleccionado()
        {
            return DEPARTAMENTOSELECCIONADO != null;
        }
        public bool SePuedeBorrar()
        {
            // no se puede borrar el departamento parametrizado como técnico
            int departamentoTecnico = Properties.Settings.Default.departamentoTecnico;
            return DEPARTAMENTOSELECCIONADO != null && DEPARTAMENTOSELECCIONADO.IDDEPARTAMENTO != departamentoTecnico;
        }
        public bool FormularioOk()
        {
            return DEPARTAMENTOFORMULARIO.NOMBRE != "";
        }
        public void GuardarCambios()
        {
            if (ACCION == Modo.Insertar)
            {
                bbdd.InsertarDepartamento(DEPARTAMENTOFORMULARIO);
            }
            else
                bbdd.ActualizarDepartamento(DEPARTAMENTOFORMULARIO);
            DEPARTAMENTOFORMULARIO = new Departamento();
            DEPARTAMENTOS = bbdd.ObtenerDepartamentos(false);
        }
        public void Cancelar()
        {
            DEPARTAMENTOFORMULARIO = new Departamento();
        }
        public bool HayDatos()
        {
            return DEPARTAMENTOFORMULARIO.NOMBRE != null;
        }
        public void Ayuda(string codigoAyuda)
        {
            Ayuda ayuda = new Ayuda(codigoAyuda);
            ayuda.ShowDialog();
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
