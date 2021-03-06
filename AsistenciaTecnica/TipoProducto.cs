using System.ComponentModel;

namespace AsistenciaTecnica
{
    public class TipoProducto : INotifyPropertyChanged
    {
        public int IDTIPO { get; set; }
        public string NOMBRE { get; set; }
        public string NOMBRETIPO { get; set; }

        public TipoProducto()
        {  
        }
        public TipoProducto(int idTipo, string nombre)
        {
            IDTIPO = idTipo;
            NOMBRE = nombre;
            NOMBRETIPO = IDTIPO + "-" + NOMBRE;
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
