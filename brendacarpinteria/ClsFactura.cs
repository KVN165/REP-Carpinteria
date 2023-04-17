using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;


using MessageBox = System.Windows.Forms.MessageBox;



namespace Proyecto_Carpinteria
{
    class ClsFactura
    {
        // factura --- id_factur, id_cliente, id_usuario, id_detalle, fecha_hora, subtotal, iva, total
        //detalles --- id_detalle, id_factura, id_producto, cantidad, precio_total

        private int id_factura;
        private long id_cliente;
        private int id_usuario;
        private DateTime fecha_hora = DateTime.Today;
        private decimal subtotal;
        private decimal iva;
        private decimal total;

        private static string id_obtenido_factura;

        private static string usuario;

        private decimal efectivo_pagado;

        

        SqlDataAdapter da;
        //SqlCommand cmd;
        DataTable dt;

        public int Id_factura { get => id_factura; set => id_factura = value; }
        public long Id_cliente { get => id_cliente; set => id_cliente = value; }
        public int Id_usuario { get => id_usuario; set => id_usuario = value; }
        public DateTime Fecha_hora { get => fecha_hora; set => fecha_hora = value; }
        public decimal Subtotal { get => subtotal; set => subtotal = value; }
        public decimal Iva { get => iva; set => iva = value; }
        public decimal Total { get => total; set => total = value; }
        public static string Usuario { get => usuario; set => usuario = value; }
        public static string Id_obtenido_factura { get => id_obtenido_factura; set => id_obtenido_factura = value; }
        public decimal Efectivo_pagado { get => efectivo_pagado; set => efectivo_pagado = value; }

        private string conexioncadena = "Data Source = localhost\\sqlexpress; Initial Catalog = Carpinteria_BD; Integrated Security=True";

        public void crear_factura()
        {
            SqlConnection conexion = new SqlConnection(conexioncadena);
            try
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO factura(id_cliente, id_usuario, fecha_hora, subtotal, iva, total, efectivo_pagado) VALUES(@id_cliente, @id_usuario, @fecha_hora, @subtotal, @iva, @total, @efectivo_pagado) SELECT SCOPE_IDENTITY();", conexion);
                cmd.Parameters.AddWithValue("@id_cliente", Id_cliente);
                cmd.Parameters.AddWithValue("@id_usuario", Id_usuario);
                cmd.Parameters.AddWithValue("@fecha_hora", Fecha_hora);
                cmd.Parameters.AddWithValue("@subtotal", Subtotal);
                cmd.Parameters.AddWithValue("@iva", Iva);
                cmd.Parameters.AddWithValue("@total", Total);
                cmd.Parameters.AddWithValue("@efectivo_pagado", Efectivo_pagado);
                SqlDataReader reader = cmd.ExecuteReader();
                reader.Read();
                string id_factura_lectura = reader[0].ToString();
                id_obtenido_factura = id_factura_lectura;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error al crear factura: " + ex);
            }
            finally
            {
                conexion.Close();
            }
        }

        public void cargarDatapro(DataGridView dgv)
        {
            SqlConnection conexion = new SqlConnection(conexioncadena);
            try
            {
                da = new SqlDataAdapter("SELECT CONCAT(u.nombre,' ', u.apellido) AS nombre_usuario, CONCAT(c.nombre1,' ', c.nombre2, ' ',c.apellido1, ' ',c.apellido2) AS nombre_cliente, f.fecha_hora, f.iva, f.subtotal, f.total from factura f INNER JOIN usuarios u ON u.id_usuario = f.id_usuario INNER JOIN clientes c ON c.id_cliente = f.id_cliente", conexion);
                dt = new DataTable();
                da.Fill(dt);

                dgv.DataSource = dt;
                

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }

        }


    }
}
