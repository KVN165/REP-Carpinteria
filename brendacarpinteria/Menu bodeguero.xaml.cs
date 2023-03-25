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
    /// Lógica de interacción para Bodeguero.xaml
    /// </summary>
    public partial class Bodeguero : Window
    {
        public Bodeguero()
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

        private void btnagregar_Click(object sender, RoutedEventArgs e)
        {
            productos p = new productos();
            p.Show();
            this.Close();
        }

        private void btnproveedores_Click(object sender, RoutedEventArgs e)
        {
            proveedores p = new proveedores();  
            p.Show();
            this.Close();
        }

        private void btninsumos_Click(object sender, RoutedEventArgs e)
        {
            Insumos i = new Insumos();
            i.Show();
            this.Close();
        }

        private void btnregresar2_Click(object sender, RoutedEventArgs e)
        {
            MainWindow m = new MainWindow();
            m.Show();
            this.Close();
        }

        private void btncambiar_Click(object sender, RoutedEventArgs e)
        {
            actualizar_usuario a = new actualizar_usuario();
            a.Show();
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ClsUsuario a = new ClsUsuario();
            lbus.Content = a.No;
            lblFecha.Content = Convert.ToString(DateTime.Now.ToLongDateString());
        }
    }
}
