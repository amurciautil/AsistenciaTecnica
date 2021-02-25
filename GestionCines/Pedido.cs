using System;
using System.ComponentModel;

namespace AsistenciaTecnica
{
    public class Pedido : INotifyPropertyChanged
    {
        public int IDPEDIDO { get; set; }
        public string DESCRIPCION { get; set; }
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
        public string USUARIO { get; set; }
        public DateTime FECHAINTRODUCCION { get; set; }
        public DateTime FECHACIERRE { get; set; }
        public SituacionPedido SITUACION { get; set; }
        public string NOMBRESITUACION { get; set; }
        public int IDSITUACION { get; set; }
        public TipoPedido TIPOPEDIDO { get; set; }
        public string NOMBRETIPOPEDIDO { get; set; }
        public int IDTIPOPEDIDO { get; set; }
        public bool ENGARANTIA { get; set; }
        public string FECHAALTAFORMATO { get; set; }
        public string FECHACIERREFORMATO { get; set; }
        public string NOMBREYAPELLIDOS { get; set; }

        public Pedido()
        {
        }

        public Pedido(int iDpedido, string descripcion, string telefono, string nombre, 
                      string apellidos, string direccion, string poblacion, 
                      string codigoPostal, Provincia provincia, string mail, 
                      string usuario, DateTime fechaIntroduccion, DateTime fechacierre, 
                      SituacionPedido situacion, TipoPedido tipoPedido, bool enGarantia)
        {
            IDPEDIDO = iDpedido;
            DESCRIPCION = descripcion;
            TELEFONO = telefono;
            NOMBRE = nombre;
            APELLIDOS = apellidos;
            DIRECCION = direccion;
            POBLACION = poblacion;
            CODIGOPOSTAL = codigoPostal;
            PROVINCIA = provincia;
            MAIL = mail;
            USUARIO = usuario;
            FECHAINTRODUCCION = fechaIntroduccion;
            FECHACIERRE = fechacierre;
            SITUACION = situacion;
            TIPOPEDIDO = tipoPedido;
            ENGARANTIA = enGarantia;
            NOMBREPROVINCIA = provincia.IDPROVINCIA+"-"+provincia.NOMBRE;
            NOMBRESITUACION = situacion.IDSITUACION+"-"+ situacion.DESCRIPCION;
            NOMBRETIPOPEDIDO = tipoPedido.IDTIPO+"-"+tipoPedido.DESCRIPCION;
            IDPROVINCIA = provincia.IDPROVINCIA;
            IDSITUACION = situacion.IDSITUACION;
            IDTIPOPEDIDO = tipoPedido.IDTIPO;
            FECHAALTAFORMATO = FECHAINTRODUCCION.ToString("dd/MM/yyyy");
            FECHACIERREFORMATO = FECHACIERRE.ToString("dd/MM/yyyy");
            NOMBREYAPELLIDOS = NOMBRE + " " + APELLIDOS;
        }

        public Pedido(Pedido pedido)
        {
            IDPEDIDO = pedido.IDPEDIDO;
            DESCRIPCION = pedido.DESCRIPCION;
            TELEFONO = pedido.TELEFONO;
            NOMBRE = pedido.NOMBRE;
            APELLIDOS = pedido.APELLIDOS;
            DIRECCION = pedido.DIRECCION;
            POBLACION = pedido.POBLACION;
            CODIGOPOSTAL = pedido.CODIGOPOSTAL;
            PROVINCIA = pedido.PROVINCIA;
            MAIL = pedido.MAIL;
            USUARIO = pedido.USUARIO;
            FECHAINTRODUCCION = pedido.FECHAINTRODUCCION;
            FECHACIERRE = pedido.FECHACIERRE;
            SITUACION = pedido.SITUACION;
            TIPOPEDIDO = pedido.TIPOPEDIDO;
            ENGARANTIA = pedido.ENGARANTIA;
            NOMBREPROVINCIA = pedido.PROVINCIA.IDPROVINCIA+" "+pedido.PROVINCIA.NOMBRE;
            NOMBRESITUACION = pedido.SITUACION.IDSITUACION+" "+pedido.SITUACION.DESCRIPCION;
            NOMBRETIPOPEDIDO = pedido.TIPOPEDIDO.IDTIPO+" "+pedido.TIPOPEDIDO.DESCRIPCION;
            IDPROVINCIA = pedido.PROVINCIA.IDPROVINCIA;
            IDSITUACION = pedido.SITUACION.IDSITUACION;
            IDTIPOPEDIDO = pedido.TIPOPEDIDO.IDTIPO;
            FECHAALTAFORMATO = FECHAINTRODUCCION.ToString("dd/MM/yyyy");
            FECHACIERREFORMATO = FECHACIERRE.ToString("dd/MM/yyyy");
            NOMBREYAPELLIDOS = NOMBRE + " " + APELLIDOS;

        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
