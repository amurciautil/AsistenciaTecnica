using System.ComponentModel;

namespace AsistenciaTecnica
{
    public class Usuario : INotifyPropertyChanged
    {
        public string LOGIN { get; set; }
        public string PASSWORD { get; set; }
        public Perfil PERFIL { get; set; }
        public Empleado EMPLEADO { get; set; }
        public int IDEMPLEADO { get; set; }
        public string NOMBREEMPLEADO { get; set; }
        public string NOMBREPERFIL { get; set; }
        public bool ACTIVO { get; set; }
        public Usuario()
        {
            ACTIVO = true;
        }
        public Usuario(string LOGIN, string PASSWORD, Perfil PERFIL, Empleado EMPLEADO, bool ACTIVO)
        {
            this.LOGIN = LOGIN;
            this.PASSWORD = PASSWORD;
            this.PERFIL = PERFIL;
            this.EMPLEADO = EMPLEADO;
            this.ACTIVO = ACTIVO;
            IDEMPLEADO = EMPLEADO.IDEMPLEADO;
            NOMBREEMPLEADO = EMPLEADO.IDEMPLEADO+"-"+EMPLEADO.NOMBREYAPELLIDOS;
            NOMBREPERFIL = PERFIL.NOMBRE;
        }
        public Usuario(Usuario usuario)
        {
            this.LOGIN = usuario.LOGIN;
            this.PASSWORD = usuario.PASSWORD;
            this.PERFIL = usuario.PERFIL;
            this.EMPLEADO = usuario.EMPLEADO;
            this.ACTIVO = usuario.ACTIVO;
            NOMBREEMPLEADO = usuario.EMPLEADO.IDEMPLEADO + "-" + usuario.EMPLEADO.NOMBREYAPELLIDOS;
            NOMBREPERFIL = usuario.PERFIL.NOMBRE;
            IDEMPLEADO = usuario.EMPLEADO.IDEMPLEADO;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
