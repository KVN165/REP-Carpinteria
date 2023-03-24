using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.AccessControl;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Xml;
using MessageBox = System.Windows.Forms.MessageBox;

namespace brendacarpinteria
{
    internal class Clsproducto1
    {
        private string bus;
        private string name;
        private string desc;
        private decimal precio;
        private decimal cantidad;
        private string cons;
        private int id;
        
        public string Name { get => name; set => name = value; }
        public string Desc { get => desc; set => desc = value; }
        public decimal Precio { get => precio; set => precio = value; }
        public decimal Cantidad { get => cantidad; set => cantidad = value; }
        public string Bus { get => bus; set => bus = value; }
        public string Cons { get => cons; set => cons = value; }
        public int Id { get => id; set => id = value; }

        private string cadenaconexion = "Data Source =localhost\\SQLEXPRESS; Initial Catalog = Carpinteria_BD; Integrated Security=True";
        SqlDataAdapter da;
        SqlCommand cmd;
        DataTable dt;
        public void Insertar()
        {

            SqlConnection conexion = new SqlConnection(cadenaconexion);
            conexion.Open();
            try
            {
                cmd = new SqlCommand("insert into [dbo].[productos] values(@name,@desc,@precio,@cantidad)", conexion);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@desc", Desc);
                cmd.Parameters.AddWithValue("@precio", Precio);
                cmd.Parameters.AddWithValue("@cantidad", cantidad);
                cmd.ExecuteNonQuery();
                conexion.Close();
                MessageBox.Show("Registro guardado correctamente");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void cargarDatapro(DataGridView dgv)
        {
                SqlConnection conexion = new SqlConnection(cadenaconexion);
                try
                {
                    da = new SqlDataAdapter("Select * from productos ", conexion);
                    dt = new DataTable();
                    da.Fill(dt);
                    dgv.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("No se pueden cargar los datos");
                }
          
                 finally
                {
                 conexion.Close();
                }
        }
        public void buscar(DataGridView dgv)
        {
            SqlConnection conexion = new SqlConnection(cadenaconexion);
            if (cons == "Nombre")
            {
                string consulta = "select * from productos where nombre_producto like  '%" + bus + "%'";
                SqlDataAdapter da = new SqlDataAdapter(consulta, conexion);
                conexion.Open();
                DataSet ds = new DataSet();
                da.Fill(ds, "producto");

                dgv.DataSource = ds;
                dgv.DataMember = "producto";

            }
            else if (cons == "id")
            {
                string consulta = "select * from productos where [id_producto] = '" + bus + "'";
                SqlDataAdapter da = new SqlDataAdapter(consulta, conexion);
                conexion.Open();
                DataSet ds = new DataSet();
                da.Fill(ds, "producto");

               dgv.DataSource = ds;
                dgv.DataMember = "producto";
            }
           
        }
        public void eliminar()
        {

        }
        public void Actualizar(DataGridView dgv)
        {
            SqlConnection conexion = new SqlConnection(cadenaconexion);
            try
            {
                conexion.Open();
                cmd = new SqlCommand("UPDATE productos set nombre_producto = @name,descripcion= @desc,precio_venta = @precio,cantidad = @cantidad where [id_producto] = @id ",conexion);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@desc", Desc);
                cmd.Parameters.AddWithValue("@precio", Precio);
                cmd.Parameters.AddWithValue("@cantidad", cantidad);
                cmd.Parameters.AddWithValue("@id",Id);
                cmd.ExecuteNonQuery();
                conexion.Close();
                MessageBox.Show("Registro guardado correctamente");
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
         
        }
     }
}
