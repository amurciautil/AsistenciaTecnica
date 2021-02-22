using System.ComponentModel;

namespace AsistenciaTecnica
{
    class Sala : INotifyPropertyChanged 
    {
        
        public int IDSALA { get; set; }
        public string NUMERO { get; set; }
        public int CAPACIDAD { get; set; }
        public bool DISPONIBLE { get; set; }

        public Sala()
        {
            DISPONIBLE = true;
        }
        public Sala(int iDSala, string numero, int capacidad, bool disponible)
        {
            IDSALA = iDSala;
            NUMERO = numero;
            CAPACIDAD = capacidad;
            DISPONIBLE = disponible;
        }

        public Sala(Sala sala)
        {
            IDSALA = sala.IDSALA;
            NUMERO = sala.NUMERO;
            CAPACIDAD = sala.CAPACIDAD;
            DISPONIBLE = sala.DISPONIBLE;
        }

        public event PropertyChangedEventHandler PropertyChanged;


    }
}
