using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Data;

namespace brendacarpinteria
{
    
    internal class Clsproveedores
    {
        private int id;
        private string name;
        private string direccion;
        private string correo;
        private string tel;
        private string es;
        private string cons;
        private string bus;

        private string cadenaconexion = "Data Source =localhost\\SQLEXPRESS; Initial Catalog = Carpinteria_BD; Integrated Security=True";
        SqlDataAdapter da;
        SqlCommand cmd;
        DataTable dt;

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string Direccion { get => direccion; set => direccion = value; }
        public string Correo { get => correo; set => correo = value; }
        public string Tel { get => tel; set => tel = value; }
        public string Es { get => es; set => es = value; }
        public string Cons { get => cons; set => cons = value; }
        public string Bus { get => bus; set => bus = value; }

        public void cargarDatapro(DataGridView dgv)
        {
            SqlConnection conexion = new SqlConnection(cadenaconexion);
            conexion.Open();
            try
            {
                da = new SqlDataAdapter("Select * from [dbo].[proveedores] ", conexion);
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

        public void Insertar()
        {

            SqlConnection conexion = new SqlConnection(cadenaconexion);
            conexion.Open();
            try
            {
                cmd = new SqlCommand("insert into [dbo].[proveedores] values(@name,@dic,@cor,@tel,@es)", conexion);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@dic", direccion );
                cmd.Parameters.AddWithValue("@cor", correo);
                cmd.Parameters.AddWithValue("@tel", tel);
                cmd.Parameters.AddWithValue("@es", es);
                cmd.ExecuteNonQuery();
                conexion.Close();
                MessageBox.Show("Registro guardado correctamente");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void buscar(DataGridView dgv)
        {
            SqlConnection conexion = new SqlConnection(cadenaconexion);
            if (Cons == "Nombre")
            {
                string consulta = "select * from [dbo].[proveedores] where [nombre_empresa] like  '%" + Bus + "%'";
                SqlDataAdapter da = new SqlDataAdapter(consulta, conexion);
                conexion.Open();
                DataSet ds = new DataSet();
                da.Fill(ds, "proveedores");

                dgv.DataSource = ds;
                dgv.DataMember = "proveedores";

            }
            else if (Cons == "id")
            {
                string consulta = "select * from [dbo].[proveedores] where [id_proveedor] = '" + Bus + "'";
                SqlDataAdapter da = new SqlDataAdapter(consulta, conexion);
                conexion.Open();
                DataSet ds = new DataSet();
                da.Fill(ds, "proveedores");

                dgv.DataSource = ds;
                dgv.DataMember = "proveedores";
            }

        }

        public void Actualizar(DataGridView dgv)
        {
            SqlConnection conexion = new SqlConnection(cadenaconexion);
            try
            {
                conexion.Open();
                cmd = new SqlCommand("UPDATE [dbo].[proveedores] set [nombre_empresa] = @name, [direccion]= @empe ,[telefono]= @desc,[email] = @precio,[estado_proveedor] = @cantidad where [id_proveedor] = @id ", conexion);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@empe", direccion);
                cmd.Parameters.AddWithValue("@desc", tel);
                cmd.Parameters.AddWithValue("@precio",correo);
                cmd.Parameters.AddWithValue("@cantidad",es);
                cmd.Parameters.AddWithValue("@id", Id);
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
