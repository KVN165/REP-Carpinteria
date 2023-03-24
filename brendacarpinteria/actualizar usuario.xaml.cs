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
    /// Lógica de interacción para actualizar_usuario.xaml
    /// </summary>
    public partial class actualizar_usuario : Window
    {
        ClsUsuario a = new ClsUsuario();
        public actualizar_usuario()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(txtcontraseña.Text == txtconfirmar.Text)
            {
               
                a.Cusuario = txtusuario.Text;
                a.Ccon = txtconfirmar.Text;
                a.actualizarusuario();

            }
            else
            {
                MessageBox.Show("Error las contraseñas no coinciden ");
            }
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
   
        }

        private void btnregresar2_Click(object sender, RoutedEventArgs e)
        {


            ClsUsuario us = new ClsUsuario();
            if (us.Tipousuario == "Administrador")
            {
                admin ad = new admin();
                ad.Show();
                this.Close();
            }
            else if (us.Tipousuario == "Cajero")
            {
                cajero ca = new cajero();
                ca.Show();
                this.Close();
            }
            else if (us.Tipousuario == "Bodeguero")
            {
                Bodeguero bo = new Bodeguero();
                bo.Show();
                this.Close();
            }
        }
    }
    }

