using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AsistenciaTecnica.Informes
{
    /// <summary>
    /// Lógica de interacción para InformeParteDeTrabajo.xaml
    /// </summary>
    public partial class InformeParteDeTrabajo : Window
    {
        public InformeParteDeTrabajo()
        {
            InitializeComponent();
            reportViewer.Owner = this;
            CrystalReport1 rep = new CrystalReport1();
            rep.SetDatabaseLogon("adminserver", "Damfp2019");
            reportViewer.ViewerCore.ReportSource = rep;
        }
    }
}
