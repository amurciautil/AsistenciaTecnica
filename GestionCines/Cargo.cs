using System.ComponentModel;

namespace AsistenciaTecnica
{
    class Cargo : INotifyPropertyChanged
    {
        public int IDCARGO { get; set; }
        public string NOMBRE { get; set; }

        public Cargo()
        {  
        }
        public Cargo(int idCargo, string nombre)
        {
            IDCARGO = idCargo;
            NOMBRE = nombre;
        }

        public Cargo(Cargo cargo)
        {
            IDCARGO = cargo.IDCARGO;
            NOMBRE = cargo.NOMBRE;
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
