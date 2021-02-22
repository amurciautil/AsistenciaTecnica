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

        public Empleado()
        {
        }

        public Empleado(int IDEMPLEADO, string NOMBRE, string APELLIDOS)
        {
            this.IDEMPLEADO = IDEMPLEADO;
            this.NOMBRE = NOMBRE;
            this.APELLIDOS = APELLIDOS;
        }
        public string ObtenerNombreCompleto()
        {
            return NOMBRE + " " + APELLIDOS;
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
