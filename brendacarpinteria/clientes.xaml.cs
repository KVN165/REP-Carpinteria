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
    /// Lógica de interacción para clientes.xaml
    /// </summary>
    public partial class clientes : Window
    {
        Clsclientes c = new Clsclientes();
        public clientes()
        {
            InitializeComponent();
        }

        private void WindowsFormsHost_ChildChanged(object sender, System.Windows.Forms.Integration.ChildChangedEventArgs e)
        {
            c.cargardatos(dgvclientes);
        }

        private void btnagregar_Click(object sender, RoutedEventArgs e)
        {
            c.Nombre = txtnombre.Text;
            c.DireccionC = txtdireccion.Text;
            c.Tl = txttel1.Text;
            c.Apellido = txtapellido.Text;
            c.Insertar();
            c.cargardatos(dgvclientes);
        }

        private void btnactualizar_Click(object sender, RoutedEventArgs e)
        {
            c.Nombre = txtnombre.Text;
            c.DireccionC = txtdireccion.Text;
            c.Tl = txttel1.Text;
            c.Apellido = txtapellido.Text;
            c.id = Convert.ToInt32(txtid.Text);
            c.Actualizar();
            c.cargardatos(dgvclientes);
            btnagregar.IsEnabled = true;
            btnactualizar.IsEnabled = false;

            limpiar();
        }

        public void limpiar()
        {
            txtbuscar1.Clear();
            txtnombre.Clear();
            txtapellido.Clear();
            txtnombre.Clear();
            txtbuscar1.Clear();
            txtdireccion.Clear();
            txttel1.Clear();
            txtid.Clear();
            btnactualizar.IsEnabled=false;
            btnagregar.IsEnabled=true;
        }

        private void dgvclientes_CellMouseClick(object sender, System.Windows.Forms.DataGridViewCellMouseEventArgs e)
        {
            txtid.Text = dgvclientes.CurrentRow.Cells[0].Value.ToString();
           txtnombre.Text = dgvclientes.CurrentRow.Cells[1].Value.ToString();
            txtapellido.Text = dgvclientes.CurrentRow.Cells[2].Value.ToString();
            txtdireccion.Text = dgvclientes.CurrentRow.Cells[3].Value.ToString();
            txttel1.Text = dgvclientes.CurrentRow.Cells[4].Value.ToString();
            btnactualizar.IsEnabled = true;
            btnagregar.IsEnabled = false;
        }

        private void btnrefresca_Click(object sender, RoutedEventArgs e)
        {
            limpiar();
        }

        private void btnbuscar_Click(object sender, RoutedEventArgs e)
        {
            c.Bus = txtbuscar1.Text;
            c.Cons = Convert.ToString(cobbus.Text);
            c.buscar(dgvclientes);
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
