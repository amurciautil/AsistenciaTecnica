using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsistenciaTecnica
{
    public class SituacionPedido : INotifyPropertyChanged
    {
        public int IDSITUACION { set; get; }
        public string DESCRIPCION { set; get; }
        public string SITUACIONDESCRIPCION { set; get; }

        public SituacionPedido(int idTipo, string descripcion)
        {
            IDSITUACION = idTipo;
            DESCRIPCION = descripcion;
            SITUACIONDESCRIPCION = idTipo+"-"+descripcion;
        }

        public SituacionPedido()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
