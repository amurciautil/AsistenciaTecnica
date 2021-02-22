using System.Collections.ObjectModel;
using System.ComponentModel;

namespace AsistenciaTecnica
{

    class SalasVM : INotifyPropertyChanged
    {
        public Sala SALASELECCIONADA { get; set; }
        public Sala SALAFORMULARIO { get; set; }
        public ObservableCollection<Sala> SALAS { get; set; }
        public Modo ACCION { get; set; }

        private readonly ServicioBaseDatos bbdd;

        public SalasVM()
        {
            bbdd = new ServicioBaseDatos();
            SALAS = bbdd.ObtenerSalas(false,false);
            SALAFORMULARIO = new Sala();
            ACCION = Modo.Insertar;
        }
        public void AñadirSala()
        {
            SALAFORMULARIO = new Sala();
            ACCION = Modo.Insertar;
        }
        public void EditarSala()
        {
            SALAFORMULARIO = new Sala(SALASELECCIONADA);
            ACCION = Modo.Actualizar;
        }
        public bool HaySalaSeleccionada()
        {
            return SALASELECCIONADA != null;
        }
        public bool FormularioOk()
        {
            return SALAFORMULARIO.NUMERO != "" && SALAFORMULARIO.CAPACIDAD != 0 && !bbdd.ExisteSala(SALAFORMULARIO);
        }
        public void GuardarCambios()
        {
            if (ACCION == Modo.Insertar)
            {
                bbdd.InsertarSala(SALAFORMULARIO);
            }
            else
                bbdd.ActualizarSala(SALAFORMULARIO);
            SALAFORMULARIO = new Sala();
            SALAS = bbdd.ObtenerSalas(false,false);
        }
        public void Cancelar()
        {
            SALAFORMULARIO = new Sala();
        }
        public bool HayDatos()
        {
            return SALAFORMULARIO.NUMERO != null || SALAFORMULARIO.CAPACIDAD != 0;
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
