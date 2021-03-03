using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace AsistenciaTecnica
{
    class CargosVM : INotifyPropertyChanged
    {
        public Cargo CARGOSELECCIONADA { get; set; }
        public Cargo CARGOFORMULARIO { get; set; }
        public ObservableCollection<Cargo> CARGOS { get; set; }
        public Modo ACCION { get; set; }

        private readonly ServicioBaseDatos bbdd;
        public CargosVM()
        {
            bbdd = new ServicioBaseDatos();
            CARGOS = bbdd.ObtenerCargos(false);
            CARGOFORMULARIO = new Cargo();
            ACCION = Modo.Insertar;
        }
        public void AñadirCargo()
        {
            CARGOFORMULARIO = new Cargo();
            ACCION = Modo.Insertar;
        }
        public void EditarCargo()
        {
            CARGOFORMULARIO = new Cargo(CARGOSELECCIONADA);
            ACCION = Modo.Actualizar;
        }
        public string BorrarCargo()
        {
            try
            {
                string mensajeBorre = CARGOSELECCIONADA.IDCARGO + " " + CARGOSELECCIONADA.NOMBRE;
                CARGOFORMULARIO = new Cargo(CARGOSELECCIONADA);
                bbdd.BorrarCargo(CARGOFORMULARIO);
                CARGOFORMULARIO = new Cargo();
                CARGOS = bbdd.ObtenerCargos(false);
                ACCION = Modo.Borrar;
                return mensajeBorre;
            }
            catch (Exception e)
            {
                throw new MisExcepciones(e.Message);
            }
        }
        public bool HayCargoSeleccionada()
        {
            return CARGOSELECCIONADA != null;
        }
        public bool FormularioOk()
        {
            return (CARGOFORMULARIO.NOMBRE != null && CARGOFORMULARIO.NOMBRE.Length > 0);
        }
        public void GuardarCambios()
        {
            try
            {
                if (ACCION == Modo.Insertar)
                {
                    bbdd.InsertarCargo(CARGOFORMULARIO);
                }
                else
                {
                    bbdd.ActualizarCargo(CARGOFORMULARIO);
                }
                CARGOFORMULARIO = new Cargo();
                CARGOS = bbdd.ObtenerCargos(false);
            }
            catch (Exception e)
            {
                throw new MisExcepciones(e.Message);
            }
        }
        public void Cancelar()
        {
            CARGOFORMULARIO = new Cargo();
        }
        public bool HayDatos()
        {
            return CARGOFORMULARIO.NOMBRE != null;
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
