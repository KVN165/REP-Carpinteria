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

//Agregados
using System.Data;

using System.Data.Sql;
using System.Data.SqlClient;
using System.Windows.Forms;
//using MessageBox = System.Windows.Forms;
using MessageBox = System.Windows.Forms.MessageBox;


namespace brendacarpinteria
{
    /// <summary>
    /// Lógica de interacción para Ver_facturas.xaml
    /// </summary>
    public partial class Ver_facturas : Window
    {
        public Ver_facturas()
        {
            InitializeComponent();
        }

        //variables
        static int posicion;

        string identidad_cliente;
        string nombre_cliente;
        string apellido_cliente;

        private string cadenaconexion = "Data Source = localhost\\sqlexpress; Initial Catalog = Carpinteria_BD; Integrated Security=True";

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dgvFactura.ReadOnly = true;
            dgvFactura.AllowUserToAddRows = false;
            dgvFactura.AllowUserToDeleteRows = false;
            dgvDetalles.ReadOnly = true;
            dgvDetalles.AllowUserToAddRows = false;
            dgvDetalles.AllowUserToDeleteRows = false;
            
            Cargar_facturas();
        }

        public void Cargar_facturas()
        {
            SqlConnection conexion = new SqlConnection(cadenaconexion);
            conexion.Open();
            try
            {
                SqlDataAdapter cmd = new SqlDataAdapter("SELECT f.id_factura AS 'Id Factura', CONCAT(u.nombre,' ',u.apellido) AS 'Atendido por', CONCAT(c.nombre, ' ', c.apellido) AS 'Cliente',  f.fecha_hora AS 'Fecha', f.efectivo_pagado AS 'Efectivo pagado', f.subtotal AS 'Subtotal', f.iva AS 'IVA 15%', f.total AS 'Total de factura'FROM factura f inner join usuarios u on f.id_usuario = u.id_usuario inner join clientes c on f.id_cliente = c.id_cliente", conexion);
                DataTable tablafactura = new DataTable();
                cmd.Fill(tablafactura);
                dgvFactura.DataSource = tablafactura;

                //dgvFactura.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCells);
                dgvFactura.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("No se pueden cargar los datos:" + ex);
            }
            finally
            {
                conexion.Close();
                dgvFactura.ClearSelection();
                dgvDetalles.ClearSelection();
            }
            posicion = -1;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void WindowsFormsHost_GotMouseCapture(object sender, System.Windows.Input.MouseEventArgs e)
        {
            posicion = dgvFactura.CurrentRow.Index;
            txtidfactura.Text = dgvFactura[0, posicion].Value.ToString();
        }


        private void dgvFactura_CellMouseClick(object sender, System.Windows.Forms.DataGridViewCellMouseEventArgs e)
        {
            if (dgvFactura.CurrentRow == null)
            {
                //nada
            }
            else
            {
                posicion = dgvFactura.CurrentRow.Index;
                txtidfactura.Text = dgvFactura[0, posicion].Value.ToString();
            }
        }

        private void Txtidfactura_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(txtidfactura.Text == "")
            {
                //nada
            }
            else
            {
                SqlConnection conexion = new SqlConnection(cadenaconexion);
                conexion.Open();
                try
                {
                    SqlDataAdapter cmd = new SqlDataAdapter("SELECT d.id_detalle AS 'ID Detalle', d.id_factura AS 'Id Factura', p.nombre_producto AS 'Nombre Producto', d.cantidad AS 'Cantidad Comprada', p.precio_venta AS 'Precio Unitario', (p.precio_venta * d.cantidad) AS 'Precio Total' FROM detalles d inner join productos p on d.id_producto = p.id_producto WHERE d.id_factura='" + txtidfactura.Text + "';", conexion);
                    DataTable tabladetalles = new DataTable();
                    cmd.Fill(tabladetalles);
                    dgvDetalles.DataSource = tabladetalles;
                    dgvDetalles.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("No se pueden cargar los datos:" + ex);
                }
                finally
                {
                    conexion.Close();
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Cargar_facturas();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(txtbuscador.Text != "")
            {
                dgvFactura.CurrentCell = null;
                foreach(DataGridViewRow r in dgvFactura.Rows)
                {
                    r.Visible = false;
                }
                foreach(DataGridViewRow r in dgvFactura.Rows)
                {
                    foreach(DataGridViewCell c in r.Cells)
                    {
                        if ((c.Value.ToString().ToUpper()).IndexOf(txtbuscador.Text.ToUpper()) == 0)
                        {
                            r.Visible = true;
                            break;
                        }

                    }
                }
            }
            else
            {
                Cargar_facturas();
            }
        }

        private void btnregresar2_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            
            if (dgvFactura.CurrentRow == null)
            {
                MessageBox.Show("Seleccione una celda primero");
            }
            else
            {
                imprimirfact();
            }

        }

        private void imprimirfact()
        {
            if (posicion == -1)
            {
                MessageBox.Show("Seleccione una factura para imprimir");
            }
            else
            {
                mostrar_factura();
            }   
        }

        private void mostrar_factura()
        {
            brendacarpinteria.climprimirfactura.CreaTicket Ticket1 = new brendacarpinteria.climprimirfactura.CreaTicket();

            Ticket1.TextoCentro("**********************************");
            Ticket1.TextoCentro("Carpinteria Brenda"); //imprime una linea de descripcion
            Ticket1.TextoCentro("**********************************");

            Ticket1.TextoCentro("Factura de Venta"); //imprime una linea de descripcion
            Ticket1.TextoIzquierda("No Factura: " + dgvFactura[0, posicion].Value.ToString());

            //convertir fecha hora
            string fechahorastring = dgvFactura[3, posicion].Value.ToString();

            DateTime fechahora = DateTime.Parse(fechahorastring);

            DateTime fecha = fechahora.Date;
            TimeSpan hora = fechahora.TimeOfDay;
            // fin

            Ticket1.TextoIzquierda("Fecha: " + fecha.ToString("dd/mm/yyyy") + "        Hora:" + hora.ToString());
            Ticket1.TextoIzquierda("Le Atendió: " + dgvFactura[1, posicion].Value.ToString());


            //Buscando cual es la id del cliente
            SqlConnection conexion = new SqlConnection(@"Data Source=localhost\sqlexpress; Initial Catalog=Carpinteria_BD; Integrated Security=True;");
                conexion.Open();
                string nombrecompleto = dgvFactura[2, posicion].Value.ToString();
                string[] dividir_nombre = nombrecompleto.Split(' ');
                SqlCommand cm = new SqlCommand("SELECT * FROM clientes WHERE nombre='" + dividir_nombre[0] + "' ;", conexion);
                SqlDataReader dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    identidad_cliente = dr["id_cliente"].ToString();
                    nombre_cliente = dr["nombre"].ToString();
                    apellido_cliente = dr["apellido"].ToString();
                }
                conexion.Close();
           

            //POINT
            Ticket1.TextoIzquierda("Identidad Cliente: "+ identidad_cliente.ToString());
            Ticket1.TextoIzquierda("Cliente: "+ nombre_cliente.ToString()+ " " + apellido_cliente.ToString());
            Ticket1.TextoIzquierda("");
            brendacarpinteria.climprimirfactura.CreaTicket.LineasGuion();

            brendacarpinteria.climprimirfactura.CreaTicket.EncabezadoVenta();
            brendacarpinteria.climprimirfactura.CreaTicket.LineasGuion();

            
            foreach (System.Windows.Forms.DataGridViewRow r in dgvDetalles.Rows)
            {
                // Articulo                     //Precio                                    cantidad                            Subtotal
                Ticket1.AgregaArticulo(r.Cells[2].Value.ToString(), double.Parse(r.Cells[4].Value.ToString()), int.Parse(r.Cells[3].Value.ToString()), double.Parse(r.Cells[5].Value.ToString())); //imprime una linea de descripcion
            }


            brendacarpinteria.climprimirfactura.CreaTicket.LineasGuion();
            Ticket1.AgregaTotales("Sub-Total", double.Parse(dgvFactura[5, posicion].Value.ToString())); // imprime linea con Subtotal
            Ticket1.AgregaTotales("15%. ISV", double.Parse(dgvFactura[6, posicion].Value.ToString()));
            Ticket1.AgregaTotales("Total", double.Parse(dgvFactura[7, posicion].Value.ToString())); // imprime linea con total
            Ticket1.AgregaTotales("Entrega:", double.Parse(dgvFactura[4, posicion].Value.ToString()));
            //calculo cambio
            decimal cambio;
            cambio = Convert.ToDecimal(dgvFactura[4, posicion].Value.ToString()) - Convert.ToDecimal(dgvFactura[7, posicion].Value.ToString());
            //fin
            Ticket1.AgregaTotales("Cambio:", double.Parse(Convert.ToString(cambio)));
           

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

        private void limpiar(object sender, RoutedEventArgs e)
        {
            posicion = -1;
            dgvFactura.ClearSelection();

            dgvDetalles.Refresh();
        }

        private void btncerrar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
