using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsistenciaTecnica
{
    class TablaAyuda
    {
        public string CODIGO { get; set; }
        public string DESCRIPCION { get; set; }
        public string AMPLIACION { get; set; }

        public TablaAyuda(string codigo, string descripcion, string ampliacion)
        {
            CODIGO = codigo;
            DESCRIPCION = descripcion;
            AMPLIACION = ampliacion;
        }

        public TablaAyuda()
        {
        }
        public TablaAyuda(TablaAyuda tablaayuda)
        {
            CODIGO = tablaayuda.CODIGO;
            DESCRIPCION = tablaayuda.DESCRIPCION;
            AMPLIACION = tablaayuda.AMPLIACION;
        }

    }
}
