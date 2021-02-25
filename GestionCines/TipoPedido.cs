using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsistenciaTecnica
{
    public class TipoPedido : INotifyPropertyChanged
    {
        public int IDTIPO { set; get; }
        public string DESCRIPCION { set; get; }
        public string TIPODESCRIPCION { set; get; }

        public TipoPedido(int idTipo, string descripcion)
        {
            IDTIPO = idTipo;
            DESCRIPCION = descripcion;
            TIPODESCRIPCION = idTipo + "-" + descripcion;
        }

        public TipoPedido()
        {
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
