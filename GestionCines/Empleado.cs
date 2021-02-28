using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsistenciaTecnica
{
    public class Empleado : INotifyPropertyChanged
    {
        public int IDEMPLEADO { get; set; }
        public string NOMBRE { get; set; }
        public string APELLIDOS { get; set; }
        public string TELEFONO { get; set; }
        public string DIRECCION { get; set; }
        public string POBLACION { get; set; }
        public string CODIGOPOSTAL { get; set; }
        public Provincia PROVINCIA { get; set; }
        public string MAIL { get; set; }
        public Cargo CARGO { get; set; }
        public Departamento DEPARTAMENTO { get; set; }
        public string NOMBREDEPARTAMENTO { get; set; }
        public string NOMBREPROVINCIA { get; set; }
        public string NOMBRECARGO { get; set; }
        public string NOMBREYAPELLIDOS { get; set; }
        public string IMAGEN { get; set; }
        public string NOMBRECOMBOBOX { get; set; }
        public Empleado()
        {
        }
        public Empleado(int idEmpleado, string nombre, string apellidos)
        {
            IDEMPLEADO = idEmpleado;
            NOMBRE = nombre;
            APELLIDOS = apellidos;
            NOMBREYAPELLIDOS = ObtenerNombreCompleto();
            NOMBRECOMBOBOX = IDEMPLEADO + "-" + NOMBREYAPELLIDOS;
        }
        public Empleado(int idEmpleado, string nombre, string apellidos, string telefono, 
                        string direccion, string poblacion, string codigoPostal, 
                        Provincia provincia, string mail, Cargo cargo, Departamento departamento,
                        string imagen)
        {
            IDEMPLEADO = idEmpleado;
            NOMBRE = nombre;
            APELLIDOS = apellidos;
            TELEFONO = telefono;
            DIRECCION = direccion;
            POBLACION = poblacion;
            CODIGOPOSTAL = codigoPostal;
            PROVINCIA = provincia;
            MAIL = mail;
            CARGO = cargo;
            DEPARTAMENTO = departamento;
            NOMBREDEPARTAMENTO = departamento.NOMBRE;
            NOMBRECARGO = cargo.NOMBRE;
            NOMBREPROVINCIA = provincia.NOMBRE;
            NOMBREYAPELLIDOS = ObtenerNombreCompleto();
            NOMBRECOMBOBOX = IDEMPLEADO + "-" + NOMBREYAPELLIDOS;
            IMAGEN = imagen;
        }
        public Empleado(Empleado empleado)
        {
            IDEMPLEADO = empleado.IDEMPLEADO;
            NOMBRE = empleado.NOMBRE;
            APELLIDOS = empleado.APELLIDOS;
            TELEFONO = empleado.TELEFONO;
            DIRECCION = empleado.DIRECCION;
            POBLACION = empleado.POBLACION;
            CODIGOPOSTAL = empleado.CODIGOPOSTAL;
            PROVINCIA = empleado.PROVINCIA;
            MAIL = empleado.MAIL;
            CARGO = empleado.CARGO;
            DEPARTAMENTO = empleado.DEPARTAMENTO;
            NOMBREDEPARTAMENTO = empleado.DEPARTAMENTO.NOMBRE;
            NOMBRECARGO = empleado.CARGO.NOMBRE;
            NOMBREPROVINCIA = empleado.PROVINCIA.NOMBRE;
            NOMBREYAPELLIDOS = empleado.ObtenerNombreCompleto();
            NOMBRECOMBOBOX = IDEMPLEADO+"-"+ NOMBREYAPELLIDOS;
            IMAGEN = empleado.IMAGEN;
        }

        public string ObtenerNombreCompleto()
        {
            return NOMBRE + " " + APELLIDOS;
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
