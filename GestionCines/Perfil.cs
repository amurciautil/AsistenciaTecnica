using System.ComponentModel;

namespace AsistenciaTecnica
{
    class Perfil : INotifyPropertyChanged
    {
        public int IDPERFIL { get; set; }
        public string NOMBRE { get; set; }
        public string DESCRIPCIONAMPLIADA { get; set; }

        public Perfil()
        {  
        }
        public Perfil(int idPerfil, string nombre,string descripcionAmpliada)
        {
            IDPERFIL = idPerfil;
            NOMBRE = nombre;
            DESCRIPCIONAMPLIADA = descripcionAmpliada;
        }

        public Perfil(Perfil perfil)
        {
            IDPERFIL = perfil.IDPERFIL;
            NOMBRE = perfil.NOMBRE;
            DESCRIPCIONAMPLIADA = perfil.DESCRIPCIONAMPLIADA;
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
