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
    /// Lógica de interacción para GraficosPartesTecnico.xaml
    /// </summary>
    public partial class GraficosPartesTecnico : Window
    {
        public GraficosPartesTecnico()
        {
            InitializeComponent();
            reportViewer.Owner = this;
            GraficoPartesTecnico rep = new GraficoPartesTecnico();
            rep.SetDatabaseLogon("adminserver", "Damfp2019");
            reportViewer.ViewerCore.ReportSource = rep;
        }
    }
}
