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
    /// Lógica de interacción para proveedores.xaml
    /// </summary>
    public partial class proveedores : Window
    {
        Clsproveedores p = new Clsproveedores();
        public proveedores()
        {
            InitializeComponent();
        }

        private void WindowsFormsHost_ChildChanged(object sender, System.Windows.Forms.Integration.ChildChangedEventArgs e)
        {
            p.cargarDatapro(dgvproveedores);
        }

        
        private void btnagregar_Click(object sender, RoutedEventArgs e)
        {
            p.Name = txtnombre.Text;
            p.Direccion = txtdireccion.Text;
            p.Correo = txtcorreo.Text;
            p.Tel = txttel1.Text;
            p.Es = Convert.ToString(cmbestado.Text);
            p.Insertar();
            p.cargarDatapro(dgvproveedores);
            limpiar();
        }

        private void btnbuscar_Click(object sender, RoutedEventArgs e)
        {
            p.Cons = Convert.ToString(cobbus.Text);
            p.Bus = txtbuscar1.Text;
            p.buscar(dgvproveedores);
        }

        private void cobbus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void seleccion()
        {
            txtid.Text = dgvproveedores.CurrentRow.Cells[0].Value.ToString();
            txtnombre.Text = dgvproveedores.CurrentRow.Cells[1].Value.ToString();
            txtdireccion.Text = dgvproveedores.CurrentRow.Cells[2].Value.ToString();
            txtcorreo.Text = dgvproveedores.CurrentRow.Cells[3].Value.ToString();
            txttel1.Text = dgvproveedores.CurrentRow.Cells[4].Value.ToString();
            cmbestado.Text = dgvproveedores.CurrentRow.Cells[5].Value.ToString();

        }
        private void btnactualizar_Click(object sender, RoutedEventArgs e)
        {
            p.Name = txtnombre.Text;
            p.Direccion = txtdireccion.Text;
            p.Correo = txtcorreo.Text;
            p.Tel = txttel1.Text;
            p.Es = Convert.ToString(cmbestado.Text);
            p.Id = Convert.ToInt32(txtid.Text);
            p.Actualizar(dgvproveedores);
            p.cargarDatapro(dgvproveedores);
            btnagregar.IsEnabled = true;
            limpiar();

        }

        private void dgvproveedores_CellMouseClick(object sender, System.Windows.Forms.DataGridViewCellMouseEventArgs e)
        {
            seleccion();
            btnagregar.IsEnabled = false;
            btnactualizar.IsEnabled = true;
        }

        private void btnrefrescar_Click(object sender, RoutedEventArgs e)
        {
            limpiar();
            btnactualizar.IsEnabled = false; 
            btnagregar.IsEnabled=true;
        }

        public void limpiar()
        {
            txtbuscar1.Clear();
            txtnombre.Clear();
            txtdireccion.Clear();
            txtcorreo.Clear();
            txttel1.Clear();
            txtid.Clear();
            p.cargarDatapro(dgvproveedores);
        }

        private void btnregresar1_Click(object sender, RoutedEventArgs e)
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
