using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsistenciaTecnica
{
    class AyudaEnLinea
    {
        public string CODIGO { get; set; }
        public string DESCRIPCION { get; set; }
        public string AMPLIACION { get; set; }

        public AyudaEnLinea(string codigo, string descripcion, string ampliacion)
        {
            CODIGO = codigo;
            DESCRIPCION = descripcion;
            AMPLIACION = ampliacion;
        }
        public AyudaEnLinea()
        {
        }
    }
}
