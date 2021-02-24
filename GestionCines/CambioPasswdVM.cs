using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsistenciaTecnica
{
    class CambioPasswdVM : INotifyPropertyChanged
    {
        private readonly ServicioBaseDatos bbdd;
        public CambioPasswdVM()
        {
            bbdd = new ServicioBaseDatos();
        }
        public void CambiarPassword(string login, string password)
        {
            bbdd.ActualizarPassword(login, password);
        }
        public SqlBinary BuscarPassword(string login)
        {
            return bbdd.BuscarPassword(login);
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
