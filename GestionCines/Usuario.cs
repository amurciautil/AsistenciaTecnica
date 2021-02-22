using System.ComponentModel;

namespace AsistenciaTecnica
{
    public class Usuario : INotifyPropertyChanged
    {
        public string LOGIN { get; set; }
        public string PASSWORD { get; set; }
        public int EMPLEADO { get; set; }

        public Usuario()
        {
        }
        public Usuario(string LOGIN, string PASSWORD, int EMPLEADO)
        {
            this.LOGIN = LOGIN;
            this.PASSWORD = PASSWORD;
            this.EMPLEADO = EMPLEADO;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
