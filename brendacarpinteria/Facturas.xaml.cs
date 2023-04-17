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


        const string DECIMAL_FORMAT = "N2";

        // variables
        int cantidad_producto;
        decimal precio_producto;
        decimal cantidad_subtotal;
        decimal total_productos;
        decimal iva;
        decimal total_factura;
        static int posicion;
        static bool ya_esta = false;

        static int id_producto;
        static int cantidad_comprada;

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
            dgvCarrito.Columns.Add("precio_producto", "Precio");
            dgvCarrito.Columns.Add("cantidad_comprada", "Cantidad");
            dgvCarrito.Columns.Add("subtotal_producto", "Subtotal");
            dgvCarrito.Columns["id_producto"].Visible = false;
            dgvCarrito.AllowUserToAddRows = false;
            dgvCarrito.AllowUserToDeleteRows = false;
            dgvCarrito.ReadOnly = true;
            dgvCarrito.Rows.Clear();
            dgvCarrito.AutoResizeColumns();
            dgvCarrito.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;

            dgvCarrito.Sort(dgvCarrito.Columns[1], System.ComponentModel.ListSortDirection.Ascending);
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
            clfactura.Id_cliente = Convert.ToInt64(txtidcliente.Text);
            clfactura.Id_usuario = Convert.ToInt32(ClsFactura.Id_obtenido_factura);
            clfactura.Fecha_hora = DateTime.Now;
            clfactura.Subtotal = Convert.ToDecimal(txtsubtotalfactura.Text);
            clfactura.Iva = Convert.ToDecimal(txtiva.Text);
            clfactura.Total = Convert.ToDecimal(txttotal.Text);
            clfactura.Efectivo_pagado = Convert.ToDecimal(txtcantidadpagada.Text);
        }

        private void Btnrealizarfactura_Click(object sender, RoutedEventArgs e)
        {
            /*
             if(txtcantidadproducto.Text == "" || txtcantidadproducto.Text.TrimStart('0') == "")
            {
                MessageBox.Show("Debe ingresar una cantidad para comprar", "Faltan Datos!!!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            } else if (txtprecioproducto.Text == "" || txtidproducto.Text == "")
            {
                MessageBox.Show("Debe seleccionar un producto!", "Faltan Datos!!!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            } else if (Convert.ToInt32(txtcantidadproducto.Text) > Convert.ToInt32(txtcantidaddisponible.Text))
            {
                MessageBox.Show("No hay suficiente STOCK del producto solicitado", "Sin Stock en Inventario", MessageBoxButton.OK, MessageBoxImage.Warning);

            } else if (Convert.ToInt32(txtcantidadpagada.Text) < Convert.ToInt32(txttotal.Text))
            {
                MessageBox.Show("Solicite más efectivo para continuar con la compra", "Monto no válido", MessageBoxButton.OK, MessageBoxImage.Warning);
            } else
             */



            if (txtidcliente.Text == "")
            {
                MessageBox.Show("Debe de seleccionar a un cliente", "Faltan datos", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }else if (txtcantidadpagada.Text == "")
            {
                MessageBox.Show("Debe de ingresar una cantidad a paga", "Faltan datos", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }else if (Convert.ToDecimal(txtcambio.Text) < 0)
            {
                MessageBox.Show("Se necesita pagar mas para realizar la factura!!!");
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
                    //Actulizar inventario va dentro de crear_detlles()
                    //Limpiar campos
                    MessageBox.Show("La factura a sido creada!!", "Carrito editado", MessageBoxButton.OK, MessageBoxImage.Information);

                    MessageBox.Show("Mostrando factura");
                    mostrar_factura();


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

                    txtidcliente.Clear();
                    txtnombrecliente.Clear();
                    txtapellidocliente.Clear();
                    txttelefonocliente.Clear();
                    txtdireccion.Clear();
                    txtcantidadpagada.Text = "0";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error en facturas: " + ex.Message);
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


                    id_producto = Convert.ToInt32(row.Cells["id_producto"].Value);
                    cantidad_comprada = Convert.ToInt32(row.Cells["cantidad_comprada"].Value);
                    actualizar_inventario();

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

        public void actualizar_inventario()
        {
            SqlConnection conexion = new SqlConnection("Data Source = localhost\\sqlexpress; Initial Catalog = Carpinteria_BD; Integrated Security=True");
            try
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("UPDATE productos SET cantidad= cantidad - '"+cantidad_comprada+"'WHERE id_producto='"+id_producto+"';", conexion);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar stock:" + ex);
            }
            finally
            {
                conexion.Close();
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
                txtcantidadproducto.Text = dgvCarrito.CurrentRow.Cells[3].Value.ToString();
                txtPrecioCantidad.Text = dgvCarrito.CurrentRow.Cells[4].Value.ToString();
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
            MessageBox.Show("Lista Actualizada");
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

        private void SubTotalFactura()
        {
            float costoTotal = 0;
            int Totalfilas = 0;

            costoTotal = 0;
            Totalfilas = 0;
            total_productos = 0;
            total_factura = 0;
            iva = 0;

            Totalfilas = dgvCarrito.RowCount;

            if (dgvCarrito.RowCount < 1)
            {

            }
            else
            {
                for (int i = 0; i < Totalfilas; i++)
                {
                    costoTotal += float.Parse(dgvCarrito.Rows[i].Cells[4].Value.ToString());
                }
            }


            total_productos = Convert.ToDecimal(costoTotal) + total_productos;
            iva = total_productos * Convert.ToDecimal(0.15);
            iva = Math.Round(iva, 2);
            total_factura = total_productos + iva;

            txtsubtotalfactura.Text = costoTotal.ToString();
            txtiva.Text = Convert.ToString(iva);
            txttotal.Text = Convert.ToString(total_factura);


           
            dgvCarrito.ClearSelection();

        }

        private void Btnagregarcompra_Click(object sender, RoutedEventArgs e)
        {

            //Comprobando que algunos campos no estén vacios

            if(txtcantidadproducto.Text == "" || txtcantidadproducto.Text.TrimStart('0') == "")
            {
                MessageBox.Show("Debe ingresar una cantidad para comprar", "Faltan Datos!!!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            } else if (txtprecioproducto.Text == "" || txtidproducto.Text == "")
            {
                MessageBox.Show("Debe seleccionar un producto!", "Faltan Datos!!!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            } else if (Convert.ToInt32(txtcantidadproducto.Text) > Convert.ToInt32(txtcantidaddisponible.Text))
            {
                MessageBox.Show("No hay suficiente STOCK del producto solicitado", "Sin Stock en Inventario", MessageBoxButton.OK, MessageBoxImage.Warning);
            } else
            {
                //Comprobar si el producto ya está en el carrito

                ya_esta = false;
                foreach (System.Windows.Forms.DataGridViewRow row in dgvCarrito.Rows)
                {
                    int valor_id = Convert.ToInt32(row.Cells["id_producto"].Value);
                    if (valor_id == Convert.ToInt32(txtidproducto.Text))
                    { 
                        ya_esta = true;
                        break;
                    }
                }

                //Fin comprobación producto carrito

                if (ya_esta == true)
                {
                    MessageBox.Show("El producto ya está en el carrito! \n Edítelo si quiere agregar mas cantidad comprada", "Producto ya en carrito", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    //Eliminar 0 al comienzo
                    string numerotxtbox = txtcantidadproducto.Text;
                    string numerosincero = numerotxtbox.TrimStart('0');
                    //Fin

                    ///total_productos = 0;
                    ///total_factura = 0;
                    ///iva = 0;
                    btnrealizarfactura.IsEnabled = true;
                    dgvCarrito.Rows.Add(txtidproducto.Text, txtnombreproducto.Text, txtprecioproducto.Text, numerosincero, txtPrecioCantidad.Text);
                    ///total_productos = cantidad_subtotal + total_productos;
                    ///iva = total_productos * Convert.ToDecimal(0.15);
                    ///iva = Math.Round(iva, 2);
                    ///total_factura = total_productos + iva;
                    //txtsubtotalfactura.Text = Convert.ToString(total_productos);
                    ///txtiva.Text = Convert.ToString(iva);
                    ///txttotal.Text = Convert.ToString(total_factura);
                    ///dgvCarrito.ClearSelection();
                    ///
                    SubTotalFactura();

                    dgvCarrito.Sort(dgvCarrito.Columns[1], System.ComponentModel.ListSortDirection.Ascending);

                    MessageBox.Show("Producto agregado al carrito!!", "Producto Agregado", MessageBoxButton.OK, MessageBoxImage.Information);
                    txtPrecioCantidad.Clear();
                    txtcantidadproducto.Clear();
                    txtidproducto.Clear();
                    txtnombreproducto.Clear();
                    txtprecioproducto.Clear();
                    txtcantidaddisponible.Clear();
                    txtdescripproducto.Clear();
                    txtcantidadproducto.Clear();
                    txtPrecioCantidad.Clear();

                    SubTotalFactura();
                    
                }
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
                    txtPrecioCantidad.Text = "0";
                    txtcantidadproducto.Text = "0";
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
            if(txtcantidadproducto.Text == "" || txtprecioproducto.Text == "")
            {
                //nada
            }
            else
            {
                if(txtcantidadproducto.Text.Length > 0)
                {
                    if (int.TryParse(txtcantidadproducto.Text, out int num))
                    {
                        if(num.ToString().Length > 5)
                        {
                            MessageBox.Show("La cantidad a comprar no debe exeder los 5 dígitos!!", "Cantidad solicitada", MessageBoxButton.OK, MessageBoxImage.Warning);
                            txtcantidadproducto.Text = "";
                            txtPrecioCantidad.Text = "";
                        }
                        else
                        {
                            precio_producto = Convert.ToDecimal(txtprecioproducto.Text);
                            cantidad_producto = Convert.ToInt32(txtcantidadproducto.Text);
                            cantidad_subtotal = cantidad_producto * precio_producto;
                            txtPrecioCantidad.Text = Convert.ToString(cantidad_subtotal);
                        }
                    }
                    
                }
                else
                {
                    //MessageBox.Show("El numero es muy grande!!!");
                }
                
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            posicion = dgvCarrito.CurrentRow.Index;
            dgvCarrito[3, posicion].Value = txtcantidadproducto.Text;
            dgvCarrito[4, posicion].Value = txtPrecioCantidad.Text;
            MessageBox.Show("Producto editado del carrito!!", "Carrito editado", MessageBoxButton.OK, MessageBoxImage.Information);
            dgvCarrito.ClearSelection();
            SubTotalFactura();
        }
        private void Btncalcularsubtotal_Click(object sender, RoutedEventArgs e)
        {
            if (txtcantidadproducto.Text == "")
            {

            }
            else
            {
                
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
                
            }
        }

        private void Btnlimpiar_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult resultado = MessageBox.Show("Estas seguro de querer lipiar los datos?", "Limpiar Datos", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
            switch (resultado)
            {
                case MessageBoxResult.Yes:
                    controlslistproductos.SelectedValue = false;
                    txtidproducto.Clear();
                    txtnombreproducto.Clear();
                    txtprecioproducto.Clear();
                    txtcantidaddisponible.Clear();
                    txtdescripproducto.Clear();
                    txtcantidadproducto.Clear();
                    txtPrecioCantidad.Clear();
                    txtfecha.Text = Convert.ToString(DateTime.Now);
                    break;
                case MessageBoxResult.No:
                    break;

            }
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
            MessageBox.Show("Producto ELIMINADO del carrito!!", "Carrito eliminado", MessageBoxButton.OK, MessageBoxImage.Information);
            if (dgvCarrito.Rows.Count == 0)
            {
                btnrealizarfactura.IsEnabled = false;
                MessageBox.Show("Sin datos en carrito");
                txtsubtotalfactura.Text = "0";
                txttotal.Text = "0";
                txtiva.Text = "0";
                
            }
            SubTotalFactura();
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
            MessageBox.Show("Lista Actualizada");
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

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            brendacarpinteria.Ver_facturas verfacturasform = new brendacarpinteria.Ver_facturas();
            verfacturasform.Show();
        }

        private void Txtcantidadproducto_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, "^[0-9]+$"))
            {
                e.Handled = true;
            }


        }

        private void Txtcantidadpagada_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                txtcambio.Text = (float.Parse(txtcantidadpagada.Text) - float.Parse(txttotal.Text)).ToString();
            }
            catch
            {
                txtcambio.Text = "0";
            }
        }

        private void Txttotal_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                txtcambio.Text = (float.Parse(txttotal.Text) - float.Parse(txtcantidadpagada.Text)).ToString();
            }
            catch
            {
                txtcambio.Text = "0";
            }
        }

        private void mostrar_factura()
        {
                brendacarpinteria.climprimirfactura.CreaTicket Ticket1 = new brendacarpinteria.climprimirfactura.CreaTicket();

                Ticket1.TextoCentro("**********************************");
                Ticket1.TextoCentro("Carpinteria Brenda" ); //imprime una linea de descripcion
                Ticket1.TextoCentro("**********************************");

                Ticket1.TextoCentro("Factura de Venta"); //imprime una linea de descripcion
                Ticket1.TextoIzquierda("No Factura: " + ClsFactura.Id_obtenido_factura.ToString());
                Ticket1.TextoIzquierda("Fecha: " + DateTime.Now.ToShortDateString() + "        Hora:" + DateTime.Now.ToString("hh:mm tt"));
                Ticket1.TextoIzquierda("Le Atendió: " + txtnombreempleado.Text);
                Ticket1.TextoIzquierda("Identidad Cliente: " +txtidcliente.Text);
                Ticket1.TextoIzquierda("Cliente: " + txtnombrecliente.Text);
                Ticket1.TextoIzquierda("");
                brendacarpinteria.climprimirfactura.CreaTicket.LineasGuion();

                brendacarpinteria.climprimirfactura.CreaTicket.EncabezadoVenta();
                brendacarpinteria.climprimirfactura.CreaTicket.LineasGuion();
                foreach (System.Windows.Forms.DataGridViewRow r in dgvCarrito.Rows)
                {
                                            // Articulo                     //Precio                                    cantidad                            Subtotal
                    Ticket1.AgregaArticulo(r.Cells[1].Value.ToString(), double.Parse(r.Cells[2].Value.ToString()), int.Parse(r.Cells[3].Value.ToString()), double.Parse(r.Cells[4].Value.ToString())); //imprime una linea de descripcion
                }


                brendacarpinteria.climprimirfactura.CreaTicket.LineasGuion();
                Ticket1.AgregaTotales("Sub-Total", double.Parse(clfactura.Subtotal.ToString())); // imprime linea con Subtotal
                Ticket1.AgregaTotales("15%. ISV", double.Parse(clfactura.Iva.ToString()));
                Ticket1.AgregaTotales("Total", double.Parse(txttotal.Text)); // imprime linea con total
                Ticket1.AgregaTotales("Entrega:", double.Parse(txtcantidadpagada.Text));
                Ticket1.AgregaTotales("Cambio:", double.Parse(txtcambio.Text));


                // Ticket1.LineasTotales(); // imprime linea 

                Ticket1.TextoIzquierda(" ");
                Ticket1.TextoCentro("**********************************");
                Ticket1.TextoCentro("*     Gracias por preferirnos    *");    
                Ticket1.TextoCentro("**********************************");
                Ticket1.TextoIzquierda(" ");
                string impresora = "Microsoft XPS Document Writer";
                Ticket1.ImprimirTiket(impresora);



                /*
                Fila = 0;
                while (dataGridView1.RowCount > 0)//limpia el dgv
                { dataGridView1.Rows.Remove(dataGridView1.CurrentRow); }
                //LBLIDnuevaFACTURA.Text = ClaseFunciones.ClsFunciones.IDNUEVAFACTURA().ToString();

                txtIdProducto.Text = lblNombre.Text =  txtCantidad.Text =textBox3.Text= "";
                lblCostoApagar.Text = lbldevolucion.Text = lblPrecio.Text = "0";
                txtIdProducto.Focus();
                MessageBox.Show("Gracias por preferirnos");
                */
        }

    }
}
