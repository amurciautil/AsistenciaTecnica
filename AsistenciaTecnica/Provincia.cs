using System.ComponentModel;

namespace AsistenciaTecnica
{
    public class Provincia : INotifyPropertyChanged
    {
        public string IDPROVINCIA { get; set; }
        public string NOMBRE { get; set; }
        public string CODIGONOMBRE { get; set; }
        public Provincia()
        {  
        }
        public Provincia(string idProvincia, string nombre)
        {
            IDPROVINCIA = idProvincia;
            NOMBRE = nombre;
            CODIGONOMBRE = idProvincia + "-" + nombre;
        }

        public Provincia(Provincia provincia)
        {
            IDPROVINCIA = provincia.IDPROVINCIA;
            NOMBRE = provincia.NOMBRE;
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
