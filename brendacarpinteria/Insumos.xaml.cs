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
            ins.Name = txtnombre.Text.Trim();
            ins.Desc = txtprecio1.Text.Trim();
            ins.Precio = Convert.ToDecimal(txtprecio.Text);
            ins.Cantidad = Convert.ToDecimal(txtcantidad1.Text);
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
            if (txtnombre.Text == String.Empty || txtprecio.Text == String.Empty || txtprecio1.Text == String.Empty || txtcantidad1.Text == String.Empty)
            {
                MessageBox.Show("Por favor llene todos los campos");
            }
            else
            {
                if (txtnombre.Text.Length < 3 || txtprecio.Text.Length < 1 || txtprecio1.Text.Length < 3 || txtcantidad1.Text.Length < 1)
                {
                    MessageBox.Show("el minimo de caracteres para el nombre y descripcion es 3 ");
                }
                else
                {
                    if (txtnombre.Text== null || txtprecio1.Text== null || txtprecio.Text.Contains(" ") || txtcantidad1.Text.Contains(" "))
                    {
                        MessageBox.Show("espacios en blanco");
                        
                    }
                    else
                    {
                        if(txtprecio.Text == "0" || txtcantidad1.Text== "0")
                        {
                            MessageBox.Show("el precio y cantidad no puede ser 0");
                        }
                        else
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
                      
                    }


                }
            }



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
            btnagregar.IsEnabled = false;
            btnactualizar.IsEnabled = true;
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

        //validaciones
        private void WindowsFormsHost_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void txtprecio_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9 || e.Key == Key.OemComma)
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void txtcantidad1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void txtnombre_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, "^^[a-zA-Z0-9]"))
            {
                e.Handled = true;
            }
        }

        private void txtprecio1_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, "^[a-zA-Z]"))
            {
                e.Handled = true;
            }
        }

        private void txtbuscar1_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, "^[a-zA-Z0-9]"))
            {
                e.Handled = true;
            }
        }
    }
}
