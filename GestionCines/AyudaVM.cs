using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsistenciaTecnica
{
    class AyudaVM : INotifyPropertyChanged
    {
        public AyudaEnLinea FORMULARIO { get; set; }
        public string CODIGO { get; set; }
        private readonly ServicioBaseDatos bbdd;
        public AyudaVM(string Codigo)
        {
            bbdd = new ServicioBaseDatos();
            CODIGO = Codigo;
            FORMULARIO = bbdd.LeerAyuda(CODIGO);
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
