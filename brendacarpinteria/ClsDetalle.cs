using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.Data.SqlClient;
using System.Data;
using MessageBox = System.Windows.Forms.MessageBox;

namespace Proyecto_Carpinteria.Clases

{
    class ClsDetalle
    {

        private int id_factura;
        private int id_producto;
        private decimal cantidad_comprada;
        private decimal precio_total;

        public int Id_factura { get => id_factura; set => id_factura = value; }
        public int Id_producto { get => id_producto; set => id_producto = value; }
        public decimal Cantidad_comprada { get => cantidad_comprada; set => cantidad_comprada = value; }
        public decimal Precio_total { get => precio_total; set => precio_total = value; }


        private string conexioncadena = "Data Source = localhost\\sqlexpress; Initial Catalog = Carpinteria_BD; Integrated Security=True";

        //int id_factura, int id_producto, decimal cantidad_comprada, decimal precio_total
        public void Insertar_datos(int id_factura, int id_producto, decimal cantidad_comprada, decimal precio_total)
        {
            
            SqlConnection conexion = new SqlConnection(conexioncadena);
            conexion.Open();
            try
            {
                //SqlCommand cmd;
                /*
                cmd.CommandText = "InsertarDetalles";
                
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@idfactura", id_factura);
                cmd.Parameters.AddWithValue("@idproducto", id_producto);
                cmd.Parameters.AddWithValue("@cantidad_comprada", cantidad_comprada);
                cmd.Parameters.AddWithValue("precio_total", precio_total);
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                */
                MessageBox.Show("Datos insertados");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
    }
}
