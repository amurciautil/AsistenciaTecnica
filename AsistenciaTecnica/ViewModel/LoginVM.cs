using System;
using System.ComponentModel;

namespace AsistenciaTecnica
{
    public class LoginVM : INotifyPropertyChanged
    {
        private readonly ServicioBaseDatos bbdd;
        public Usuario USUARIOSELECCIONADO { get; set; }

        public LoginVM()
        {
            bbdd = new ServicioBaseDatos();
            USUARIOSELECCIONADO = new Usuario();
        }

        public void Cancelar()
        {
            USUARIOSELECCIONADO = new Usuario();
        }
        public void BuscarUsuario(string login, string password)
        {
            try
            {
                if (bbdd.BuscarUsuario(login, password) == 1)
                {
                    USUARIOSELECCIONADO = bbdd.LeerUsuario(login);
                }
            }
            catch (Exception e)
            {
                throw new MisExcepciones(e.Message);
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
