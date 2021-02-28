using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace AsistenciaTecnica
{
    class PartesVM : INotifyPropertyChanged
    {
        public Modo ACCION { get; set; }
        public const string CONDICION_FIJA = " WHERE 1=1";
        private readonly ServicioBaseDatos bbdd;
        public string NUMEROPEDIDO { get; set; }
        public string NOMBRE { get; set; }
        public string APELLIDOS { get; set; }
        public string AÑOSELECCIONADO { get; set; }
        public ObservableCollection<string> AÑOS { get; set; }
        public string MESSELECCIONADO { get; set; }
        public ObservableCollection<string> MESES { get; set; }
        public string SITUACIONESELECCIONADA { get; set; }
        public ObservableCollection<string> SITUACIONESPARTE { get; set; }
        public ObservableCollection<Parte> PARTES { get; set; }
        public Parte SELECCIONADA { get; set; }
        public Parte FORMULARIO { get; set; }
        public Pedido PEDIDO { get; set; }
        public PartesVM(Pedido pedido)
        {
            bbdd = new ServicioBaseDatos();
            PEDIDO = pedido;
            PARTES = bbdd.ObtenerPartes(CONDICION_FIJA,PEDIDO.IDPEDIDO, false);
            FORMULARIO = new Parte();
            SITUACIONESPARTE = new ObservableCollection<string>{ "Todos","Abierto", "Cerrado" };
            AÑOS = new ObservableCollection<string>();
            AÑOS.Add("");
            AÑOS.Add(DateTime.Now.Year.ToString());
            for (int i = 2020; i <= 2040; i++)
                if (i.ToString() != DateTime.Now.Year.ToString())
                    AÑOS.Add(i.ToString());
            MESES = new ObservableCollection<string>();
            MESES.Add("");
            MESES.Add(DateTime.Now.Month.ToString());
            for (int i = 1; i <= 12; i++)
                if (i.ToString() != DateTime.Now.Month.ToString())
                    MESES.Add(i.ToString());
        }
        public bool HaySelecionada()
        {
            return SELECCIONADA != null;
        }
        public void Añadir(Partes partesWindow,Pedido pedido)
        {
            Parte parte = new Parte(pedido);
            ParteDetalle parteDetalle = new ParteDetalle(parte);
            parteDetalle.Owner = partesWindow;
            if (parteDetalle.ShowDialog() == true)
                PARTES = parteDetalle.PARTES;
        }
        public void Editar(Partes partesWindow)
        {
            ParteDetalle parteDetalle = new ParteDetalle(SELECCIONADA);
            parteDetalle.Owner = partesWindow;
            if (parteDetalle.ShowDialog() == true)
                PARTES = parteDetalle.PARTES;
        }
        public string Borrar()
        {
            string mensajeBorre = SELECCIONADA.IDPARTE + " " + SELECCIONADA.OBSERVACIONES;
            bbdd.BorrarParte(SELECCIONADA);
            PARTES = bbdd.ObtenerPartes(CONDICION_FIJA, PEDIDO.IDPEDIDO, false);
            return mensajeBorre;
        }
        public void RefrescarFiltrado()
        {
            string condicion_filtro = CONDICION_FIJA;
            switch (SITUACIONESELECCIONADA)
            {
                case "Abierto":
                    condicion_filtro += " AND pa.cerrado = 0";
                    break;
                case "Cerrado":
                    condicion_filtro += " AND pa.cerrado = 1";
                    break;
                default:
                    break;
            }
            string patron = @"^[0-9]*$";
            Regex pa = new Regex(patron);
            if (NUMEROPEDIDO != null && NUMEROPEDIDO.Trim(' ').Length > 0 && pa.IsMatch(NUMEROPEDIDO))
                condicion_filtro += " AND pe.idPedido = " + NUMEROPEDIDO;
            if (NOMBRE != null && NOMBRE.Trim(' ').Length > 0)
                condicion_filtro += " AND em.nombre LIKE '%" + NOMBRE + "%'";
            if (APELLIDOS != null && APELLIDOS.Trim(' ').Length > 0)
                condicion_filtro += " AND em.apellidos LIKE '%" + APELLIDOS + "%'";
            if (AÑOSELECCIONADO != null && AÑOSELECCIONADO.Length > 0)
                condicion_filtro += " AND YEAR(pa.fechaIntroduccion) = "+ Convert.ToInt32(AÑOSELECCIONADO);
            if (MESSELECCIONADO != null && MESSELECCIONADO.Length > 0)
                condicion_filtro += " AND MONTH(pa.fechaIntroduccion) = " + Convert.ToInt32(MESSELECCIONADO);
            PARTES = bbdd.ObtenerPartes(condicion_filtro, PEDIDO.IDPEDIDO, false);
        }
        public void Ayuda(string codigoAyuda)
        {
            Ayuda ayuda = new Ayuda(codigoAyuda);
            ayuda.ShowDialog();
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
