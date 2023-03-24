using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MessageBox = System.Windows.Forms.MessageBox;


namespace brendacarpinteria
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
           
            
        }
        ClsUsuario us = new ClsUsuario();

        public void envdatosp()
        {
           us.Usuarios = txtusuario.Text;
           us.Contra = passbox.Password;
        }


        private void btnacceder_Click(object sender, RoutedEventArgs e)
        {
            envdatosp();
            us.login();

          
                if (us.Tipousuario == "Administrador")
                {
                    if (us.Tipo == "Activo")
                    {
                        admin ad = new admin();
                        ad.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Usuario bloqueado, comunicarse con el administrador", "Usuario Bloqueado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }


                }
                else if (us.Tipousuario == "Cajero")
                {
                    if (us.Tipo == "Activo") { 
                    cajero ca = new cajero();
                    ca.Show();
                    this.Close();
                    }
                    else
                    {
                     MessageBox.Show("Usuario bloqueado, comunicarse con el administrador", "Usuario Bloqueado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                     }

            }
                else if (us.Tipousuario == "Bodeguero")
                {
                     if (us.Tipo == "Activo") { 
                    Bodeguero bo = new Bodeguero();
                    bo.Show();
                    this.Close();
                     }
                        else
                        {
                          MessageBox.Show("Usuario bloqueado, comunicarse con el administrador", "Usuario Bloqueado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                         }
                 }


        }

        private void btnsalir_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnrecu_Click(object sender, RoutedEventArgs e)
        {
            recuperacion r = new recuperacion();
            r.Show();
            this.Close();
        }
    }
}
