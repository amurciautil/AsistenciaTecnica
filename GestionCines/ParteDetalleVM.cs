using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsistenciaTecnica
{
    class ParteDetalleVM : INotifyPropertyChanged
    {
        public const string CONDICION_FIJA = " WHERE 1=1";
        public Parte FORMULARIO { get; set; }
        public Parte SELECCIONADA { get; set; }
        public ObservableCollection<string> EMPLEADONOMBRE { get; set; }
        public ObservableCollection<Empleado> EMPLEADOS { get; set; }
        private readonly ServicioBaseDatos bbdd;
        public Modo ACCION { get; set; }
        public ObservableCollection<Parte> PARTES { get; set; }
        public ParteDetalleVM(Parte parte)
        {
            try
            {
                bbdd = new ServicioBaseDatos();
                if (parte.IDPARTE == 0)
                {
                 //   FORMULARIO = new Parte();
                    FORMULARIO = new Parte(parte);

                    ACCION = Modo.Insertar;
                }
                else
                {
                    FORMULARIO = new Parte(parte);
                    SELECCIONADA = new Parte(parte);
                    ACCION = Modo.Actualizar;
                }
                // Obtener valores para los combobox
                ObtenerDatosCombo();
            }
            catch (Exception ex)
            {
                throw new MisExcepciones(ex.Message);
            }
        }
        public void ObtenerDatosCombo()
        {
            int departamentoTecnico = Properties.Settings.Default.departamentoTecnico;
            EMPLEADOS = bbdd.ObtenerEmpleados(false,departamentoTecnico);
            EMPLEADONOMBRE = new ObservableCollection<string>();
            foreach (Empleado empleado in EMPLEADOS)
            {
                EMPLEADONOMBRE.Add(empleado.NOMBRECOMBOBOX);
            }
        }
        public bool FormularioOK()
        {
            bool correcto = false;
            // valido solo los obligatorios (no admiten nulos en la base de datos)
            if (FORMULARIO.NOMBREEMPLEADO != "0-" &&
                FORMULARIO.FECHAPREVISTA != null)
                correcto = true;
            return correcto;
        }
        public ObservableCollection<Parte> GuardarCambios()
        {
            string[] valores = FORMULARIO.NOMBREEMPLEADO.Split('-');
            FORMULARIO.IDEMPLEADO = Int32.Parse(valores[0]);
            if (ACCION == Modo.Insertar)
            {
                bbdd.InsertarParte(FORMULARIO);
            }
            else
                bbdd.ActualizarParte(FORMULARIO);
            FORMULARIO = new Parte();

            PARTES = bbdd.ObtenerPartes(CONDICION_FIJA,FORMULARIO.IDPEDIDO, false);
            return PARTES;
        }
        public void Ayuda(string codigoAyuda)
        {
            Ayuda ayuda = new Ayuda(codigoAyuda);
            ayuda.ShowDialog();
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
