using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsistenciaTecnica
{
    class MantenimientoAyudaVM : INotifyPropertyChanged
    {
        public TablaAyuda SELECCIONADO { get; set; }
        public TablaAyuda FORMULARIO { get; set; }
        public ObservableCollection<TablaAyuda> LISTA { get; set; }
        public Modo ACCION { get; set; }
        private readonly ServicioBaseDatos bbdd;


        public MantenimientoAyudaVM()
        {
            bbdd = new ServicioBaseDatos();
            LISTA = bbdd.ObtenerTablaAyuda(false);
            FORMULARIO = new TablaAyuda();
            ACCION = Modo.Insertar;
        }
        public void Añadir()
        {
            FORMULARIO = new TablaAyuda();
            ACCION = Modo.Insertar;
        }
        public void Editar()
        {
            FORMULARIO = new TablaAyuda(SELECCIONADO);
            ACCION = Modo.Actualizar;
        }

        public bool HaySeleccionado()
        {
            return SELECCIONADO != null;
        }
        public bool FormularioOk()
        {
            return FORMULARIO.CODIGO != null && 
                FORMULARIO.CODIGO.Length > 0 && 
                FORMULARIO.DESCRIPCION != null &&
                FORMULARIO.DESCRIPCION.Length > 0;
        }
        public void GuardarCambios()
        {
            if (ACCION == Modo.Insertar)
            {
                try
                {
                    bbdd.InsertarTablaAyuda(FORMULARIO);
                }catch(Exception e)
                {
                    throw new MisExcepciones(e.Message);
                }
            }
            else
                bbdd.ActualizarTablaAyuda(FORMULARIO);
            FORMULARIO = new TablaAyuda();
            LISTA = bbdd.ObtenerTablaAyuda(false);
        }
        public void Cancelar()
        {
            FORMULARIO = new TablaAyuda();
        }
        public bool HayDatos()
        {
            return FORMULARIO.DESCRIPCION != null;
        }
        public void Ayuda(string codigoAyuda)
        {
            Ayuda ayuda = new Ayuda(codigoAyuda);
            ayuda.ShowDialog();
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
