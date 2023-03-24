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
using System.Windows.Threading;

namespace brendacarpinteria
{
    /// <summary>
    /// Lógica de interacción para cajero.xaml
    /// </summary>
    public partial class cajero : Window
    {
        public cajero()
        {
            InitializeComponent();
            DispatcherTimer oDispacherTimer = new DispatcherTimer();

            oDispacherTimer.Interval = new TimeSpan(800);
            oDispacherTimer.Tick += (a, b) =>
            {
                lblHora.Content = DateTime.Now.ToLongTimeString();

            };

            oDispacherTimer.Start();
        }

        private void WindowsFormsHost_ChildChanged(object sender, System.Windows.Forms.Integration.ChildChangedEventArgs e)
        {
           
        }

        private void btnClientes_Click(object sender, RoutedEventArgs e)
        {
            clientes c = new clientes();
            c.Show();
            this.Close();
        }
        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            

        }
        private void btnregresar2_Click(object sender, RoutedEventArgs e)
        {
            MainWindow m = new MainWindow();
            m.Show();
            this.Close();
        }

        private void btnFactura_Click(object sender, RoutedEventArgs e)
        {

            Proyecto_Carpinteria.Facturas f = new Proyecto_Carpinteria.Facturas();
            f.Show();
            this.Close();
        }

        private void btncambiar_Click(object sender, RoutedEventArgs e)
        {
            actualizar_usuario a = new actualizar_usuario();
            a.Show();
            this.Close();
        }

        private void Grid_Loaded_1(object sender, RoutedEventArgs e)
        {
            ClsUsuario a = new ClsUsuario();
            lblus.Content = a.No;
            lblFechaa.Content = Convert.ToString(DateTime.Now.ToLongDateString());
        }
    }
}
