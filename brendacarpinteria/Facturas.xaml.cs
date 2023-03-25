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
// Agregados
using DataGridView = System.Windows.Forms;
using System.Data.SqlClient;
using System.Windows.Threading;
//

namespace Proyecto_Carpinteria
{
    /// <summary>
    /// Lógica de interacción para Facturas.xaml
    /// </summary>
    /// 
    public partial class Facturas : Window
    {
        public Facturas()
        {
            InitializeComponent();

            //Reloj
            DispatcherTimer oDispacherTimer = new DispatcherTimer();

            oDispacherTimer.Interval = new TimeSpan(800);
            oDispacherTimer.Tick += (a, b) =>
            {
                txtfecha.Text = DateTime.Now.ToString();
            };
            oDispacherTimer.Start();


        }
        // variables
        int cantidad_producto;
        decimal precio_producto;
        decimal cantidad_subtotal;
        decimal total_productos;
        decimal iva;
        decimal total_factura;
        static int posicion;
        //Clases
        ClsFactura clfactura = new ClsFactura();
        Clases.ClsDetalle detalleinfo = new Clases.ClsDetalle();


        

        public void dgvCarrito_CellClick(object sender, DataGridView.DataGridViewCellEventArgs e)
        {
            posicion = dgvCarrito.CurrentRow.Index;
            MessageBox.Show("Row numero" + posicion);
            txtidproducto.Text = dgvCarrito.CurrentRow.Cells[0].Value.ToString();
            txtnombreproducto.Text = dgvCarrito.CurrentRow.Cells[1].Value.ToString();
            txtcantidadproducto.Text = dgvCarrito.CurrentRow.Cells[2].Value.ToString();
            txtPrecioCantidad.Text = dgvCarrito.CurrentRow.Cells[3].Value.ToString();
            btnEliminarCarrito.IsEnabled = true;
            btnEditarCarrito.IsEnabled = true;
            btnagregarcompra.IsEnabled = false;
        }

        public void enviar_datos()
        {
            clfactura.Id_cliente = Convert.ToInt32(txtidcliente.Text);
            clfactura.Id_usuario = Convert.ToInt32(txtidusuario.Text);
            clfactura.Fecha_hora = DateTime.Today;
            clfactura.Subtotal = Convert.ToDecimal(txtsubtotalfactura.Text);
            clfactura.Iva = Convert.ToDecimal(txtiva.Text);
            clfactura.Total = Convert.ToDecimal(txttotal.Text);
        }

        private void Btnrealizarfactura_Click(object sender, RoutedEventArgs e)
        {
            if (txtidcliente.Text == "0" || txtidusuario.Text == "0")
            {
                MessageBox.Show("Debe seleccionar a un cliente...");
            }
            else
            {
                SqlConnection conexion = new SqlConnection(@"Data Source=localhost\sqlexpress; Initial Catalog=Carpinteria_BD; Integrated Security=True;");
                try
                {
                    conexion.Open();
                    enviar_datos();
                    clfactura.ingresar_datos();
                    txtidfactura.Text = ClsFactura.Id_obtenido_factura.ToString();
                    MessageBox.Show("Factura agregada con éxito!!!");
                    btnrealizarfactura.IsEnabled = false;
                    txtidproducto.Clear();
                    txtnombreproducto.Clear();
                    txtprecioproducto.Clear();
                    txtcantidaddisponible.Clear();
                    txtdescripproducto.Clear();
                    txtcantidadproducto.Clear();
                    txtsubtotalfactura.Clear();
                    txttotal.Clear();
                    txtiva.Clear();
                    dgvCarrito.Rows.Clear();
                    txtfecha.Text = Convert.ToString(DateTime.Now);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error en facturas");
                    MessageBox.Show(ex.Message);
                }
                conexion.Close();
                agregar_tabla_detalles();
            }
        }

        public void agregar_tabla_detalles()
        {
            SqlConnection conexion = new SqlConnection(@"Data Source=localhost\sqlexpress; Initial Catalog=Carpinteria_BD; Integrated Security=True;");
            SqlCommand insertar_detalles = new SqlCommand("INSERT into detalles(id_factura, id_producto, cantidad, precio_total) values(@id_factura, @id_producto, @cantidad, @precio_total)", conexion);
            try
            {
                conexion.Open();
                foreach (System.Windows.Forms.DataGridViewRow row in dgvCarrito.Rows)
                {
                    insertar_detalles.Parameters.Clear();
                    insertar_detalles.Parameters.AddWithValue("@id_factura", Convert.ToInt32(txtidfactura.Text));   
                    insertar_detalles.Parameters.AddWithValue("@id_producto", Convert.ToInt32(row.Cells["id_producto"].Value));
                    insertar_detalles.Parameters.AddWithValue("@cantidad", Convert.ToInt32(row.Cells["cantidad_comprada"].Value));
                    insertar_detalles.Parameters.AddWithValue("@precio_total", Convert.ToDecimal(row.Cells["subtotal_producto"].Value));
                    insertar_detalles.ExecuteNonQuery();
                    conexion.Close();
                }
                conexion.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en detalles");
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvCarrito_CellMouseClick(object sender, System.Windows.Forms.DataGridViewCellMouseEventArgs e)
        {
            if (dgvCarrito.CurrentRow == null)
            {
                btnEditarCarrito.IsEnabled = false;
                btnEliminarCarrito.IsEnabled = false;
            }
            else
            {
                btnEditarCarrito.IsEnabled = true;
                btnEliminarCarrito.IsEnabled = true;
                btnagregarcompra.IsEnabled = false;
                txtidproducto.Text = dgvCarrito.CurrentRow.Cells[0].Value.ToString();
                txtnombreproducto.Text = dgvCarrito.CurrentRow.Cells[1].Value.ToString();
                txtcantidadproducto.Text = dgvCarrito.CurrentRow.Cells[2].Value.ToString();
                txtPrecioCantidad.Text = dgvCarrito.CurrentRow.Cells[3].Value.ToString();

                SqlConnection conexion = new SqlConnection(@"Data Source=localhost\sqlexpress; Initial Catalog=Carpinteria_BD; Integrated Security=True;");
                try
                {
                    conexion.Open();
                    SqlCommand cm = new SqlCommand("SELECT * FROM productos WHERE id_producto='" + txtidproducto.Text + "' ;", conexion);
                    SqlDataReader dr = cm.ExecuteReader();
                    while (dr.Read())
                    {
                        txtidproducto.Text = dr["id_producto"].ToString();
                        txtnombreproducto.Text = dr["nombre_producto"].ToString();
                        txtdescripproducto.Text = dr["descripcion"].ToString();
                        txtprecioproducto.Text = dr["precio_venta"].ToString();
                        txtcantidaddisponible.Text = dr["cantidad"].ToString();
                    }
                    conexion.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    conexion.Close();

                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // cargar lista de datos clientes
            SqlConnection conexion = new SqlConnection(@"Data Source=localhost\sqlexpress; Initial Catalog=Carpinteria_BD; Integrated Security=True;");
            try
            {
                conexion.Open();
                SqlCommand cm = new SqlCommand("SELECT * FROM clientes order by nombre", conexion);
                SqlDataReader dr = cm.ExecuteReader();
                controlslistclientes.Items.Clear();
                while (dr.Read())
                {
                    controlslistclientes.Items.Add(dr[1].ToString());
                } 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conexion.Close();
            }
            // cargar lista de datos productos
            cargar_lista_productos();

            System.Windows.Forms.Integration.WindowsFormsHost
                host = new System.Windows.Forms.Integration.WindowsFormsHost();
            DataGrid dgv = new DataGrid();

            txtfecha.Text = Convert.ToString(DateTime.Now);
            brendacarpinteria.ClsUsuario usuariosinfo = new brendacarpinteria.ClsUsuario();
            txtnombreempleado.Text = Proyecto_Carpinteria.ClsFactura.Usuario;
            txtidusuario.Text = Proyecto_Carpinteria.ClsFactura.Id_obtenido_factura;


            dgvCarrito.Columns.Add("id_producto", "Id del producto");
            dgvCarrito.Columns.Add("nombre_producto", "Nombre del producto");
            dgvCarrito.Columns.Add("cantidad_comprada", "Cantidad");
            dgvCarrito.Columns.Add("subtotal_producto", "Precio total");
            dgvCarrito.Columns["id_producto"].Visible = false;
            dgvCarrito.AllowUserToAddRows = false;
            dgvCarrito.AllowUserToDeleteRows = false;
            dgvCarrito.ReadOnly = true;
            dgvCarrito.Rows.Clear();
            dgvCarrito.AutoResizeColumns();
            dgvCarrito.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
        }

        public void cargar_lista_productos()
        {
            SqlConnection conexion = new SqlConnection(@"Data Source=localhost\sqlexpress; Initial Catalog=Carpinteria_BD; Integrated Security=True;");
            try
            {
                conexion.Open();
                SqlCommand cm = new SqlCommand("SELECT * FROM productos order by nombre_producto", conexion);
                SqlDataReader dr = cm.ExecuteReader();
                controlslistproductos.Items.Clear();
                while (dr.Read())
                {
                    controlslistproductos.Items.Add(dr[1].ToString());
                    listaproductos.Sort();
                }
                conexion.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conexion.Close();
            }
        }

        public void cargar_id_usuario()
        {
            SqlConnection conexion = new SqlConnection(@"Data Source=localhost\sqlexpress; Initial Catalog=Carpinteria_BD; Integrated Security=True;");
            try
            {
                conexion.Open();
                SqlCommand cm = new SqlCommand("SELECT id_usuario FROM usuarios WHERE nombre='" + txtnombreempleado.Text + "'", conexion);
                SqlDataReader dr = cm.ExecuteReader();
                dr.Read();
                txtidusuario.Text = dr["id_usuario"].ToString();
                string nombre1, apellido1, nombre_largo;
                nombre1 = dr["nombre"].ToString();
                apellido1 = dr["apellido"].ToString();
                nombre_largo = nombre1 + apellido1;
                conexion.Close();
                txtnombreempleado.Text = nombre_largo;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conexion.Close();
            }
        }

        private void WindowsFormsHost_ChildChanged(object sender, System.Windows.Forms.Integration.ChildChangedEventArgs e)
        {
            

        //facturainfo.cargarDatapro(dgvfactura); 
        }

        // ??

        List<string> listaproductos = new List<string>();


        public void ordenar_lista()
        {
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SqlConnection conexion = new SqlConnection(@"Data Source=localhost\sqlexpress; Initial Catalog=Carpinteria_BD; Integrated Security=True;");
            try
            {
                conexion.Open();
                SqlCommand cm = new SqlCommand("SELECT * FROM productos order by nombre_producto", conexion);
                SqlDataReader dr = cm.ExecuteReader();
                controlslistproductos.Items.Clear();
                while (dr.Read())
                {
                    controlslistproductos.Items.Add(dr[1].ToString());
                    listaproductos.Sort();
                }
                conexion.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conexion.Close();
            }
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void Btnagregarcompra_Click(object sender, RoutedEventArgs e)
        {

            if (txtcantidadproducto.Text == "")
            {
                MessageBox.Show("Debe de agregar una cantidad al producto");
            }else if (txtprecioproducto.Text == "")
            {
                MessageBox.Show("Debe seleccionar un producto...");
            }else{
                btnrealizarfactura.IsEnabled = true;
                dgvCarrito.Rows.Add(txtidproducto.Text, txtnombreproducto.Text, txtcantidadproducto.Text, txtPrecioCantidad.Text);
                total_productos = cantidad_subtotal + total_productos;
                iva = total_productos * Convert.ToDecimal(0.15);
                total_factura = total_productos + iva;
                txtsubtotalfactura.Text = Convert.ToString(total_productos);
                txtiva.Text = Convert.ToString(iva);
                txttotal.Text = Convert.ToString(total_factura);

                MessageBox.Show("Producto agregado al carrito");
                txtidproducto.Clear();
                txtnombreproducto.Clear();
                txtprecioproducto.Clear();
                txtcantidaddisponible.Clear();
                txtdescripproducto.Clear();
                txtcantidadproducto.Clear();
                txtPrecioCantidad.Clear();
            }
        }

        private void Controlslistproductos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SqlConnection conexion = new SqlConnection(@"Data Source=localhost\sqlexpress; Initial Catalog=Carpinteria_BD; Integrated Security=True;");
            try
            {
                conexion.Open();
                SqlCommand cm = new SqlCommand("SELECT * FROM productos WHERE nombre_producto='" + controlslistproductos.SelectedValue.ToString() + "' ;", conexion);
                SqlDataReader dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    txtidproducto.Text = dr["id_producto"].ToString();
                    txtnombreproducto.Text = dr["nombre_producto"].ToString();
                    txtdescripproducto.Text = dr["descripcion"].ToString();
                    txtprecioproducto.Text = dr["precio_venta"].ToString();
                    txtcantidaddisponible.Text = dr["cantidad"].ToString();
                }
                conexion.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conexion.Close();
            }

        }

        private void Btntest_Click(object sender, RoutedEventArgs e)
        {
            controlslistclientes.SelectedItem = "";
            SqlConnection conexion = new SqlConnection(@"Data Source=localhost\sqlexpress; Initial Catalog=Carpinteria_BD; Integrated Security=True;");
            try
            {
                conexion.Open();
                SqlCommand cm = new SqlCommand("SELECT * FROM clientes order by nombre", conexion);
                SqlDataReader dr = cm.ExecuteReader();
                controlslistclientes.Items.Clear();
                while (dr.Read())
                {
                    controlslistclientes.Items.Add(dr[1].ToString());
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
            finally
            {
                conexion.Close();

            }



            /*


            SqlConnection conexion = new SqlConnection(@"Data Source=localhost\sqlexpress; Initial Catalog=Carpinteria_BD; Integrated Security=True;");
            try
            {
                conexion.Open();
                SqlCommand cm = new SqlCommand("SELECT * FROM clientes order by nombre", conexion);
                SqlDataReader dr = cm.ExecuteReader();


                controlslistclientes.Items.Clear();
                while (dr.Read())
                {
                    //listaproductos.Add(dr[1].ToString());
                    controlslistclientes.Items.Add(dr[1].ToString());
                    //controlslistproductos.Items.Add(listaproductos);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Refrescar");
                MessageBox.Show(ex.Message);

            }
            finally
            {
                conexion.Close();

            }
            */
        }

        private void Controlslistclientes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SqlConnection conexion = new SqlConnection(@"Data Source=localhost\sqlexpress; Initial Catalog=Carpinteria_BD; Integrated Security=True;");
            try
            {
                conexion.Open();
                SqlCommand cm = new SqlCommand("SELECT * FROM clientes WHERE nombre='" + controlslistclientes.SelectedValue.ToString() + "' ;", conexion);
                SqlDataReader dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    txtidcliente.Text = dr["id_cliente"].ToString();
                    txtnombrecliente.Text = dr["nombre"].ToString();
                    txtapellidocliente.Text = dr["apellido"].ToString();
                    txtdireccion.Text = dr["direccion"].ToString();
                    txttelefonocliente.Text = dr["telefono"].ToString();

                    //controlslistproductos.Items.Add(dr[1].ToString());
                }
                conexion.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
            finally
            {
                conexion.Close();

            }

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }

        private void Txtnombreempleado_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void Txtcantidadproducto_TextChanged(object sender, TextChangedEventArgs e)
        {

            if(System.Text.RegularExpressions.Regex.IsMatch(txtcantidadproducto.Text, "^[0-9]"))
            {

            }
            else
            {
                txtcantidadproducto.Text = "";
            }



            if(String.IsNullOrEmpty(txtcantidadproducto.Text))
            {
                txtPrecioCantidad.Clear();
            }
            else
            {
                if (txtprecioproducto.Text == "")
                {

                }
                else
                {
                    precio_producto = Convert.ToDecimal(txtprecioproducto.Text);
                    cantidad_producto = Convert.ToInt32(txtcantidadproducto.Text);
                }
            }
            if (cantidad_producto == 0)
            {
                txtPrecioCantidad.Text = "0";
            }
            else
            {
                cantidad_subtotal = cantidad_producto * precio_producto;
                txtPrecioCantidad.Text = Convert.ToString(cantidad_subtotal);
            }    
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            // boton editar datagrid carrito 4 5

            posicion = dgvCarrito.CurrentRow.Index;

            MessageBox.Show("Posicon " + posicion);

            dgvCarrito[2, posicion].Value = txtcantidadproducto.Text;
            dgvCarrito[3, posicion].Value = txtPrecioCantidad.Text;
        }

        private void WindowsFormsHost_GotFocus(object sender, RoutedEventArgs e)
        {
            /*
            posicion = dgvCarrito.CurrentRow.Index;
            txtidproducto.Text = dgvCarrito[0, posicion].Value.ToString();
            txtnombreproducto.Text = dgvCarrito[1, posicion].Value.ToString();
            txtcantidadproducto.Text = dgvCarrito[2, posicion].Value.ToString();
            txtsubtotal.Text = dgvCarrito[3, posicion].Value.ToString();

            //txtidproducto.Template = dgvCarrito.SelectedRows.Cells[0].Value.ToString();

            //btnagregarcompra.IsEnabled = false;

            MessageBox.Show("Linea numero " + posicion);
            */

        }

        private void WindowsFormsHost_LostFocus(object sender, RoutedEventArgs e)
        {
            btnagregarcompra.IsEnabled = true;
        }

        private void Btncalcularsubtotal_Click(object sender, RoutedEventArgs e)
        {
            if (txtcantidadproducto.Text == "")
            {

            }
            else
            {
                precio_producto = Convert.ToDecimal(txtprecioproducto.Text);
                cantidad_producto = Convert.ToInt16(txtcantidadproducto.Text);
            }

            if (txtcantidadproducto.Text == "")
            {
                //txtcantidadproducto.Text = "0";
            }




            if (cantidad_producto == 0)
            {
                txtPrecioCantidad.Text = "0";
            }
            else
            {
                cantidad_subtotal = cantidad_producto * precio_producto;
                txtPrecioCantidad.Text = Convert.ToString(cantidad_subtotal);
            }
        }

        private void Btnlimpiar_Click(object sender, RoutedEventArgs e)
        {
            txtidproducto.Clear();
            txtnombreproducto.Clear();
            txtprecioproducto.Clear();
            txtcantidaddisponible.Clear();
            txtdescripproducto.Clear();
            txtcantidadproducto.Clear();
            txtPrecioCantidad.Clear();
            txtiva.Clear();
            txttotal.Clear();
            txtsubtotalfactura.Clear();
            txtfecha.Text = Convert.ToString(DateTime.Now);
        }


        private void WindowsFormsHost_GotMouseCapture(object sender, MouseEventArgs e)
        {
            posicion = dgvCarrito.CurrentRow.Index;
            txtidproducto.Text = dgvCarrito[0, posicion].Value.ToString();
            txtnombreproducto.Text = dgvCarrito[1, posicion].Value.ToString();
            txtcantidadproducto.Text = dgvCarrito[2, posicion].Value.ToString();
            txtPrecioCantidad.Text = dgvCarrito[3, posicion].Value.ToString();

            //btnagregarcompra.IsEnabled = false;

            MessageBox.Show("Linea numero " + posicion);
            

        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            dgvCarrito.Rows.RemoveAt(posicion);
            MessageBox.Show("Producto eliminado del carrito");

            if(dgvCarrito.Rows.Count == 0)
            {
                btnrealizarfactura.IsEnabled = false;
            }
        }

        private void Btncerrarsesion_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult resultado = MessageBox.Show("Estas seguro de querer regresar al menú?", "Regresar al menú", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);

            switch (resultado)
            {
                case MessageBoxResult.Yes:
                    //Inicio_sesion inicioform = new Inicio_sesion();
                    //inicioform.Show();
                    

                    brendacarpinteria. ClsUsuario us = new brendacarpinteria. ClsUsuario();
                    if (us.Tipousuario == "Administrador")
                    {
                        brendacarpinteria.admin ad = new brendacarpinteria.admin();
                        ad.Show();
                        this.Close();
                    }
                    else if (us.Tipousuario == "Cajero")
                    {
                        brendacarpinteria.cajero ca = new brendacarpinteria.cajero();
                        ca.Show();
                        this.Close();
                    }
                    else if (us.Tipousuario == "Bodeguero")
                    {
                        brendacarpinteria.Bodeguero bo = new brendacarpinteria.Bodeguero();
                        bo.Show();
                        this.Close();
                    }
                    break;
                case MessageBoxResult.No:
                    break;

            }

        }


        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            brendacarpinteria.clientes clientesform = new brendacarpinteria.clientes();
            clientesform.Show();
        }

        private void BtnNuevo_Click(object sender, RoutedEventArgs e)
        {
            dgvCarrito.ClearSelection();
            btnagregarcompra.IsEnabled = true;
            btnEditarCarrito.IsEnabled = false;
            btnEliminarCarrito.IsEnabled = false;
        }
    }
}
