using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsistenciaTecnica
{
    class Telefono
    {
        public string TELEFONO { get; set; }
        public string NOMBRE { get; set; }
        public string APELLIDOS { get; set; }
        public string DIRECCION { get; set; }
        public string POBLACION { get; set; }
        public string CODIGOPOSTAL { get; set; }
        public Provincia PROVINCIA { get; set; }
        public string NOMBREPROVINCIA { get; set; }
        public string IDPROVINCIA { get; set; }
        public string MAIL { get; set; }
        public Telefono()
        {
        }

        public Telefono(string telefono, string nombre,
                      string apellidos, string direccion, string poblacion,
                      string codigoPostal, Provincia provincia, string mail)
        {
            TELEFONO = telefono;
            NOMBRE = nombre;
            APELLIDOS = apellidos;
            DIRECCION = direccion;
            POBLACION = poblacion;
            CODIGOPOSTAL = codigoPostal;
            PROVINCIA = provincia;
            MAIL = mail;
            NOMBREPROVINCIA = provincia.IDPROVINCIA + "-" + provincia.NOMBRE;
            IDPROVINCIA = provincia.IDPROVINCIA;
        }

        public Telefono(Telefono pedido)
        {
            TELEFONO = pedido.TELEFONO;
            NOMBRE = pedido.NOMBRE;
            APELLIDOS = pedido.APELLIDOS;
            DIRECCION = pedido.DIRECCION;
            POBLACION = pedido.POBLACION;
            CODIGOPOSTAL = pedido.CODIGOPOSTAL;
            PROVINCIA = pedido.PROVINCIA;
            MAIL = pedido.MAIL;
            NOMBREPROVINCIA = pedido.PROVINCIA.IDPROVINCIA + "-" + pedido.PROVINCIA.NOMBRE;
        }
    }
}
