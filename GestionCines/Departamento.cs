using System.ComponentModel;
namespace AsistenciaTecnica
{
    public class Departamento : INotifyPropertyChanged
    {
        public int IDDEPARTAMENTO { get; set; }
        public string NOMBRE { get; set; }

        public Departamento(int IDDEPARTAMENTO, string NOMBRE)
        {
            this.IDDEPARTAMENTO = IDDEPARTAMENTO;
            this.NOMBRE = NOMBRE;
        }
        public Departamento()
        {
        }

        public Departamento(Departamento departamento)
        {
            IDDEPARTAMENTO = departamento.IDDEPARTAMENTO;
            NOMBRE = departamento.NOMBRE;

        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
