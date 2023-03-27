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
using System.Text.RegularExpressions;
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
        Proyecto_Carpinteria.Clases.ClsDetalle clsdetalle = new Proyecto_Carpinteria.Clases.ClsDetalle();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cargarlistaclientes();
            cargarlistaproductos();
            brendacarpinteria.ClsUsuario usuariosinfo = new brendacarpinteria.ClsUsuario();
            txtnombreempleado.Text = Proyecto_Carpinteria.ClsFactura.Usuario;
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

        public void cargarlistaclientes()
        {
            SqlConnection conexion = new SqlConnection(@"Data Source=localhost\sqlexpress; Initial Catalog=Carpinteria_BD; Integrated Security=True;");
            try
            {
                conexion.Open();
                SqlCommand cm = new SqlCommand("SELECT * FROM clientes order by nombre", conexion);
                SqlDataReader dr = cm.ExecuteReader();
                controlslistclientes.Items.Clear();
                while (dr.Read())
                {
                    string nombre, apellido, nombrecompleto;
                    nombre = dr[1].ToString();
                    apellido = dr[2].ToString();
                    nombrecompleto = nombre + " " + apellido;
                    controlslistclientes.Items.Add(nombrecompleto);
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
        }

        public void cargarlistaproductos()
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

        public void Enviardatos_factura()
        {
            clfactura.Id_cliente = Convert.ToInt32(txtidcliente.Text);
            clfactura.Id_usuario = Convert.ToInt32(ClsFactura.Id_obtenido_factura);
            clfactura.Fecha_hora = DateTime.Now;
            clfactura.Subtotal = Convert.ToDecimal(txtsubtotalfactura.Text);
            clfactura.Iva = Convert.ToDecimal(txtiva.Text);
            clfactura.Total = Convert.ToDecimal(txttotal.Text);
        }

        private void Btnrealizarfactura_Click(object sender, RoutedEventArgs e)
        {
            if (txtidcliente.Text == "")
            {
                MessageBox.Show("Debe seleccionar a un cliente...");
            }
            else
            {
                SqlConnection conexion = new SqlConnection(@"Data Source=localhost\sqlexpress; Initial Catalog=Carpinteria_BD; Integrated Security=True;");
                try
                {
                    //Crear Factura
                    Enviardatos_factura();
                    clfactura.crear_factura();
                    //Crear Detalles
                    crear_detalles();
                    //Limpiar campos
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
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error en facturas");
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    conexion.Close();
                } 
            }
        }

        public void crear_detalles()
        {
            SqlConnection conexion = new SqlConnection("Data Source = localhost\\sqlexpress; Initial Catalog = Carpinteria_BD; Integrated Security=True");
            try
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO detalles(id_factura, id_producto, cantidad, precio_total) VALUES(@id_factura, @id_producto, @cantidad, @precio_total);", conexion);
                foreach (System.Windows.Forms.DataGridViewRow row in dgvCarrito.Rows)
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@id_factura", Convert.ToInt32(ClsFactura.Id_obtenido_factura));
                    cmd.Parameters.AddWithValue("@id_producto", Convert.ToInt32(row.Cells["id_producto"].Value));
                    cmd.Parameters.AddWithValue("@cantidad", Convert.ToInt32(row.Cells["cantidad_comprada"].Value));
                    cmd.Parameters.AddWithValue("@precio_total", Convert.ToDecimal(row.Cells["subtotal_producto"].Value));
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al crear detalles:" + ex);
            }
            finally
            {
                conexion.Close();
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

        public void cargar_id_usuario()
        {
            SqlConnection conexion = new SqlConnection(@"Data Source=localhost\sqlexpress; Initial Catalog=Carpinteria_BD; Integrated Security=True;");
            try
            {
                conexion.Open();
                SqlCommand cm = new SqlCommand("SELECT id_usuario FROM usuarios WHERE nombre='" + txtnombreempleado.Text + "'", conexion);
                SqlDataReader dr = cm.ExecuteReader();
                dr.Read();
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

        List<string> listaproductos = new List<string>();

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            controlslistproductos.SelectedValue = false;
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
            if(controlslistproductos.SelectedValue == null)
            {
                //nada
            }
            else
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
                txtcantidadproducto.IsEnabled = true;
            }
        }

        private void Controlslistclientes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(controlslistclientes.SelectedValue == null)
            {
                //nada
            }
            else
            {
                SqlConnection conexion = new SqlConnection(@"Data Source=localhost\sqlexpress; Initial Catalog=Carpinteria_BD; Integrated Security=True;");
                try
                {
                    conexion.Open();
                    string nombrecompleto = controlslistclientes.SelectedValue.ToString();
                    string[] dividir_nombre = nombrecompleto.Split(' ');
                    SqlCommand cm = new SqlCommand("SELECT * FROM clientes WHERE nombre='" + dividir_nombre[0] + "' ;", conexion);
                    SqlDataReader dr = cm.ExecuteReader();
                    while (dr.Read())
                    {
                        txtidcliente.Text = dr["id_cliente"].ToString();
                        txtnombrecliente.Text = dr["nombre"].ToString();
                        txtapellidocliente.Text = dr["apellido"].ToString();
                        txtdireccion.Text = dr["direccion"].ToString();
                        txttelefonocliente.Text = dr["telefono"].ToString();
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

        private void Txtcantidadproducto_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtcantidadproducto.Text = txtcantidadproducto.Text.Replace(".", "");
            bool numeros = Regex.IsMatch(txtcantidadproducto.Text, "^[0-9]+$");
            if(numeros)
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
            posicion = dgvCarrito.CurrentRow.Index;
            dgvCarrito[2, posicion].Value = txtcantidadproducto.Text;
            dgvCarrito[3, posicion].Value = txtPrecioCantidad.Text;
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

        private void Btnrefrescarclientes_Click(object sender, RoutedEventArgs e)
        {
            cargarlistaclientes();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            brendacarpinteria.productos productosform = new brendacarpinteria.productos();
            productosform.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            controlslistproductos.SelectedValue = false;
        }
    }
}
