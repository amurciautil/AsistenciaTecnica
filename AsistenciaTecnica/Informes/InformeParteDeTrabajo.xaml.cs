using System.Windows;

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
