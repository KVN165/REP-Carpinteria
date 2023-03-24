using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
    /// Lógica de interacción para Insumos.xaml
    /// </summary>
    public partial class Insumos : Window
    {
        private string cadenaconexion = "Data Source =MENDEZ\\SQLEXPRESS; Initial Catalog = Carpinteria_BD; Integrated Security=True";
        Clsinsumos ins = new Clsinsumos();
        int tipop;
        
        public Insumos()
        {
            InitializeComponent();
        }

        private void btnagregar_Click(object sender, RoutedEventArgs e)
        {

        }

        public void envdatos()
        {
            ins.Name = txtnombre.Text;
            ins.Desc = txtnombre.Text;
            ins.Precio = Convert.ToDecimal(txtprecio.Text);
            ins.Cantidad = Convert.ToInt64(txtcantidad1.Text);
            ins.Emp = Convert.ToInt32(cmbemp.SelectedValue);

        }
        private void WindowsFormsHost_ChildChanged(object sender, System.Windows.Forms.Integration.ChildChangedEventArgs e)
        {
            ins.cargarDatain(dgvinsumos);
          
        }

        private void btneliminar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnActualizar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnregresar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            envdatos();
            ins.Insertar();
            ins.cargarDatain(dgvinsumos);
            txtbuscar1.Clear();
            txtnombre.Clear();
            txtprecio.Clear();
            txtprecio1.Clear();
            txtcantidad1.Clear();
            txtid.Clear();
            
        }
       
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
      
        }

        private void WindowsFormsHost_Loaded(object sender, RoutedEventArgs e)
        {

            cmbemp.DataSource = ins.cargarcom();
            cmbemp.DisplayMember = "nombre_empresa";
            cmbemp.ValueMember = "id_proveedor";
            tipop = Convert.ToInt16(cmbemp.SelectedValue);
        }

        private void btnbuscar_Click(object sender, RoutedEventArgs e)
        {
            ins.Bus = txtbuscar1.Text;
            ins.Cons = Convert.ToString(cobbus.Text);
            ins.buscar(dgvinsumos);
            txtbuscar1.Clear();
        }

        private void btnrefrescar_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection conexion = new SqlConnection(cadenaconexion);
            conexion.Open();
            ins.cargarDatain(dgvinsumos);
            txtbuscar1.Clear();
            conexion.Close();
            txtbuscar1.Clear();
            txtnombre.Clear();
            txtprecio.Clear();
            txtprecio1.Clear();
            txtcantidad1.Clear();
            txtid.Clear();
            btnagregar.IsEnabled = true;
            btnactualizar.IsEnabled = false;
        }
        private void seleccionar()
        {
            txtid.Text = dgvinsumos.CurrentRow.Cells[0].Value.ToString();
            txtnombre.Text = dgvinsumos.CurrentRow.Cells[1].Value.ToString();
            cmbemp.Text = dgvinsumos.CurrentRow.Cells[2].Value.ToString();
            txtprecio1.Text = dgvinsumos.CurrentRow.Cells[3].Value.ToString();
            txtcantidad1.Text = dgvinsumos.CurrentRow.Cells[5].Value.ToString();
            txtprecio.Text = dgvinsumos.CurrentRow.Cells[4].Value.ToString();
           

        }

        private void dgvinsumos_CellMouseClick(object sender, System.Windows.Forms.DataGridViewCellMouseEventArgs e)
        {
            seleccionar();
            btnagregar.IsEnabled=false;
            btnactualizar.IsEnabled=true;
        }

        private void btnactualizar_Click_1(object sender, RoutedEventArgs e)
        {
            ins.Id = Convert.ToInt32(txtid.Text);
            ins.Name = txtnombre.Text;
            ins.Desc = txtprecio1.Text;
            ins.Precio = Convert.ToDecimal(txtprecio.Text);
            ins.Cantidad = Convert.ToInt32(txtcantidad1.Text);
            ins.Emp = Convert.ToInt32(cmbemp.SelectedValue);
            ins.Actualizar(dgvinsumos);
            ins.cargarDatain(dgvinsumos);
            txtbuscar1.Clear();
            txtnombre.Clear();
            txtprecio.Clear();
            txtprecio1.Clear();
            txtcantidad1.Clear();
            txtid.Clear();
            btnagregar.IsEnabled = true;
            btnactualizar.IsEnabled = false;
        }

        private void btnregresar_Click_1(object sender, RoutedEventArgs e)
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
