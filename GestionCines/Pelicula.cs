using System.ComponentModel;

namespace AsistenciaTecnica
{
    class Pelicula : INotifyPropertyChanged
    {
        public int ID { get; set; }
        public string TITULO { get; set; }
        public string CARTEL { get; set; }
        public int AÑO { get; set; }
        public string GENERO { get; set; }
        public string CALIFICACION { get; set; }

        public Pelicula(Pelicula pelicula)
        {
            ID = pelicula.ID;
            TITULO = pelicula.TITULO;
            CARTEL = pelicula.CARTEL;
            AÑO = pelicula.AÑO;
            GENERO = pelicula.GENERO;
            CALIFICACION = pelicula.CALIFICACION;
        }

        public Pelicula()
        {
        }

        public Pelicula(int idPelicula, string titulo, string cartel, int año, string genero, string calificacion)
        {
            ID = idPelicula;
            TITULO = titulo;
            CARTEL = cartel;
            AÑO = año;
            GENERO = genero;
            CALIFICACION = calificacion;
        }


        public event PropertyChangedEventHandler PropertyChanged;
    }
}
