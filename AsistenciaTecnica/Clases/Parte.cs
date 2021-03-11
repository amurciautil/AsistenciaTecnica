using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsistenciaTecnica
{
    public class Parte
    {
        public int IDPARTE { get; set; }
        public Pedido PEDIDO { get; set; }
        public int IDPEDIDO { get; set; }
        public DateTime FECHAALTA { get; set; }
        public DateTime FECHACIERRE { get; set; }
        public bool CERRADO { get; set; }
        public Empleado EMPLEADO { get; set; }
        public int IDEMPLEADO { get; set; }
        public string EMPLEADONOMBRE { get; set; }
        public string NOMBREEMPLEADO { get; set; }
        public string OBSERVACIONES { get; set; }
        public string INCIDENCIAS { get; set; }
        public string DIRECCION { get; set; }
        public string POBLACION { get; set; }
        public string NOMBRECLIENTE { get; set; }
        public string TELEFONO { get; set; }
        public DateTime FECHAPREVISTA { get; set; }
        public string FECHAPREVISTAFORMATO { get; set; }
        public string FECHAALTAPARTEFORMATO { get; set; }
        public string NOMBRETIPOPEDIDO { get; set; }
        public string NOMBRETIPOPEDIDOSIN { get; set; }
        public string FECHAALTAPEDIDOFORMATO { get; set; }
        public string FECHACIERREPEDIDOFORMATO { get; set; }
        public string NOMBRESITUACIONSIN { get; set; }
        public string PROVINCIA { get; set; }
        public string DESCRIPCION { get; set; }
        public string MAIL { get; set; }
        public string FECHACIERREPARTEFORMATO { get; set; }

        public Parte()
        {
        }
        // Constructor para altas
        public Parte(Pedido pedido)
        {
            Empleado empleado = new Empleado();
            Parte parte = new Parte();
            DateTime fechaPrevista = DateTime.Now.AddDays(Properties.Settings.Default.fechaPrevista);
            switch (fechaPrevista.ToString())
            {
                case "Saturday":
                    FECHAPREVISTA = fechaPrevista.AddDays(2);
                    break;
                case "Sunday":
                    FECHAPREVISTA = fechaPrevista.AddDays(1);
                    break;
                default:
                    FECHAPREVISTA = fechaPrevista;
                    break;
            }
            FECHAALTA = DateTime.Now;
            FECHACIERREPARTEFORMATO = "";
            IDPARTE = 0;
            PEDIDO = pedido;
            IDPEDIDO = pedido.IDPEDIDO;
            DESCRIPCION = pedido.DESCRIPCION;
            FECHACIERRE = parte.FECHACIERRE;
            CERRADO = false;
            EMPLEADO = empleado;
            IDEMPLEADO = empleado.IDEMPLEADO;
            EMPLEADONOMBRE = empleado.NOMBREYAPELLIDOS;
            NOMBREEMPLEADO = empleado.IDEMPLEADO + "-" + empleado.NOMBREYAPELLIDOS;
            OBSERVACIONES = parte.OBSERVACIONES;
            DIRECCION = pedido.DIRECCION;
            POBLACION = pedido.POBLACION;
            PROVINCIA = pedido.NOMBREPROVINCIASIN;
            TELEFONO = pedido.TELEFONO;
            MAIL = pedido.MAIL;
            FECHAALTAPARTEFORMATO = FECHAALTA.ToString("dd/MM/yyyy");
            FECHAPREVISTAFORMATO = FECHAPREVISTA.ToString("dd/MM/yyyy");
            FECHAALTAPEDIDOFORMATO = pedido.FECHAALTAFORMATO;
            FECHACIERREPEDIDOFORMATO = pedido.FECHACIERREFORMATO;
            NOMBRECLIENTE = pedido.NOMBREYAPELLIDOS;
            NOMBRETIPOPEDIDO = pedido.NOMBRETIPOPEDIDO;
            NOMBRETIPOPEDIDOSIN = pedido.NOMBRETIPOPEDIDOSIN;
            NOMBRESITUACIONSIN = pedido.NOMBRESITUACIONSIN;
        }

        public Parte(int idParte, Pedido pedido, DateTime fechaAlta, DateTime fechaCierre, 
                     bool cerrado, Empleado empleado,
                     string observaciones, DateTime fechaPrevista,string incidencias)
        {
            IDPARTE = idParte;
            PEDIDO = pedido;
            IDPEDIDO = pedido.IDPEDIDO;
            DESCRIPCION = pedido.DESCRIPCION;
            FECHAALTA = fechaAlta;
            FECHACIERRE = fechaCierre;
            CERRADO = cerrado;
            EMPLEADO = empleado;
            IDEMPLEADO = empleado.IDEMPLEADO;
            EMPLEADONOMBRE = empleado.NOMBREYAPELLIDOS;
            NOMBREEMPLEADO = empleado.IDEMPLEADO + "-" + empleado.NOMBREYAPELLIDOS;
            OBSERVACIONES = observaciones;
            FECHAPREVISTA = fechaPrevista;
            DIRECCION = pedido.DIRECCION;
            POBLACION = pedido.POBLACION;
            PROVINCIA = pedido.NOMBREPROVINCIASIN;
            TELEFONO = pedido.TELEFONO;
            MAIL = pedido.MAIL;
            FECHAALTAPARTEFORMATO = FECHAALTA.ToString("dd/MM/yyyy");
            if (FECHACIERRE.Year == 1)
                FECHACIERREPARTEFORMATO = "";
            else
                FECHACIERREPARTEFORMATO = FECHACIERRE.ToString("dd/MM/yyyy");
            FECHAPREVISTAFORMATO = FECHAPREVISTA.ToString("dd/MM/yyyy");
            FECHAALTAPEDIDOFORMATO = pedido.FECHAALTAFORMATO;
            FECHACIERREPEDIDOFORMATO = pedido.FECHACIERREFORMATO;
            NOMBRECLIENTE = pedido.NOMBREYAPELLIDOS;
            NOMBRETIPOPEDIDO = pedido.NOMBRETIPOPEDIDO;
            NOMBRETIPOPEDIDOSIN = pedido.NOMBRETIPOPEDIDOSIN;
            NOMBRESITUACIONSIN = pedido.NOMBRESITUACIONSIN;
            INCIDENCIAS = incidencias;
        }
        public Parte(Parte parte)
        {
            IDPARTE = parte.IDPARTE;
            PEDIDO = parte.PEDIDO;
            IDPEDIDO= parte.IDPEDIDO;
            DESCRIPCION = parte.DESCRIPCION;
            FECHAALTA = parte.FECHAALTA;
            FECHACIERRE = parte.FECHACIERRE;
            CERRADO = parte.CERRADO;
            EMPLEADO = parte.EMPLEADO;
            IDEMPLEADO = parte.IDEMPLEADO;
            EMPLEADONOMBRE = parte.EMPLEADONOMBRE;
            NOMBREEMPLEADO = parte.NOMBREEMPLEADO;
            OBSERVACIONES = parte.OBSERVACIONES;
            FECHAPREVISTA = parte.FECHAPREVISTA.Date;
            DIRECCION = parte.DIRECCION;
            POBLACION = parte.POBLACION;
            PROVINCIA = parte.PEDIDO.NOMBREPROVINCIASIN;
            TELEFONO = parte.TELEFONO;
            MAIL = parte.MAIL;
            FECHAALTAPARTEFORMATO = parte.FECHAALTAPARTEFORMATO;
            FECHACIERREPARTEFORMATO = parte.FECHACIERREPARTEFORMATO;
            FECHAPREVISTAFORMATO = parte.FECHAPREVISTAFORMATO;
            FECHAALTAPEDIDOFORMATO = parte.PEDIDO.FECHAALTAFORMATO;
            FECHACIERREPEDIDOFORMATO = parte.PEDIDO.FECHACIERREFORMATO;
            NOMBRECLIENTE = parte.NOMBRECLIENTE;
            NOMBRETIPOPEDIDO = parte.NOMBRETIPOPEDIDO;
            NOMBRETIPOPEDIDOSIN = parte.NOMBRETIPOPEDIDOSIN;
            NOMBRESITUACIONSIN = parte.NOMBRESITUACIONSIN;
            INCIDENCIAS = parte.INCIDENCIAS;
        }

    }
}
