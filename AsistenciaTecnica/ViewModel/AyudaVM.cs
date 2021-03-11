using System.ComponentModel;

namespace AsistenciaTecnica
{
    class AyudaVM : INotifyPropertyChanged
    {
        public AyudaEnLinea FORMULARIO { get; set; }
        public string CODIGO { get; set; }
        private readonly ServicioBaseDatos bbdd;
        public AyudaVM(string Codigo)
        {
            bbdd = new ServicioBaseDatos();
            CODIGO = Codigo;
            FORMULARIO = bbdd.LeerAyuda(CODIGO);
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
