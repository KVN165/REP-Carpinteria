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

namespace brendacarpinteria
{
    /// <summary>
    /// Lógica de interacción para recuperacion.xaml
    /// </summary>
    public partial class recuperacion : Window
    {
        public recuperacion()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var user = new NRecuperarcontra();
            var result = user.recoverpass(txtusuario.Text);
            lblcon.Content= result;
        }

        private void btnregresar_Click(object sender, RoutedEventArgs e)
        {
            MainWindow m = new MainWindow();
            m.Show();
            this.Close();
        }
    }
}
