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
using MessageBox = System.Windows.Forms;


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
            }
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
    }
}
