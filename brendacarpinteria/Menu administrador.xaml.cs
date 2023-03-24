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
    /// Lógica de interacción para admin.xaml
    /// </summary>
    public partial class admin : Window
    {
        public admin()
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
            productos po = new productos();
            po.Show();
            this.Close();   
            
        }

        private void btnregresar_Click(object sender, RoutedEventArgs e)
        {
            admin ad = new admin();
            ad.Show();
            this.Close();
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btninsumos_Click(object sender, RoutedEventArgs e)
        {
            Insumos insu  = new Insumos();
            insu.Show();
            this.Close();
        }

        private void btnusuario_Click(object sender, RoutedEventArgs e)
        {
            usuarios u = new usuarios();
            u.Show();
            this.Close();
            
        }

        private void btnproveedores_Click(object sender, RoutedEventArgs e)
        {
            proveedores p = new proveedores();
            p.Show();
            this.Close();
        }

        private void btnClientes_Click(object sender, RoutedEventArgs e)
        {
            clientes c = new clientes();
            c.Show();
            this.Close();
        }
        private void btnregresar2_Click(object sender, RoutedEventArgs e)
        {
            MainWindow m = new MainWindow();
            m.Show();
            this.Close();
        }

        private void btnfactura_Click(object sender, RoutedEventArgs e)
        {
            Proyecto_Carpinteria.Facturas f = new Proyecto_Carpinteria.Facturas();
            f.Show();
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
