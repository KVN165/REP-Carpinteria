using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace brendacarpinteria
{
    public class Clsinsumos
    {
        private string cadenaconexion = "Data Source =localhost\\SQLEXPRESS; Initial Catalog = Carpinteria_BD; Integrated Security=True";
        SqlDataAdapter da;
        SqlCommand cmd;
        DataTable dt;
        private string bus;
        private int emp;
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
        public int Emp { get => emp; set => emp = value; }

        public void cargarDatain(DataGridView dgv)
        {
            SqlConnection conexion = new SqlConnection(cadenaconexion);
            try
            {
                da = new SqlDataAdapter("select [id_insumo],[nombre_insumo],p.[nombre_empresa],[descripcion_insumo],[precio_compra],[cantidad_disponible] from [dbo].[insumos] s inner join [dbo].[proveedores] p on p.id_proveedor=s.id_proveedor", conexion);
                dt = new DataTable();
                da.Fill(dt);
                dgv.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pueden cargar los datos:" +ex);
            }

            finally
            {
                conexion.Close();
            }
        }

        public void Insertar()
        {
            if(string.IsNullOrEmpty(Name) && string.IsNullOrEmpty(Desc))
            {
                MessageBox.Show("por favor llenar todos los campos");
            }
            else
            {
                SqlConnection conexion = new SqlConnection(cadenaconexion);
                conexion.Open();
                try
                {

                    cmd = new SqlCommand("insert into insumos values(@empe,@name,@desc,@precio,@cantidad)", conexion);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@empe", Emp);
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
           
        }

        public DataTable cargarcom()
        {
            SqlConnection conexion = new SqlConnection(cadenaconexion);
            conexion.Open();
            SqlDataAdapter da = new SqlDataAdapter("sp_cargarcomboboxee", conexion);
            da.SelectCommand.CommandType=CommandType.StoredProcedure;
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public void buscar(DataGridView dgv)
        {
            SqlConnection conexion = new SqlConnection(cadenaconexion);
            if (cons == "Nombre")
            {
                string consulta = "select [id_insumo],[nombre_insumo],p.[nombre_empresa],[descripcion_insumo],[precio_compra],[cantidad_disponible] from [dbo].[insumos] s inner join [dbo].[proveedores] p on p.id_proveedor=s.id_proveedor where nombre_insumo like '%" + bus + "%'";
                SqlDataAdapter da = new SqlDataAdapter(consulta, conexion);
                conexion.Open();
                DataSet ds = new DataSet();
                da.Fill(ds, "insumos");

                dgv.DataSource = ds;
                dgv.DataMember = "insumos";

            }
            else if (cons == "id")
            {
                string consulta = "select [id_insumo],[nombre_insumo],p.[nombre_empresa],[descripcion_insumo],[precio_compra],[cantidad_disponible] from [dbo].[insumos] s inner join [dbo].[proveedores] p on p.id_proveedor=s.id_proveedor where id_insumo = '" + bus + "'";
                SqlDataAdapter da = new SqlDataAdapter(consulta, conexion);
                conexion.Open();
                DataSet ds = new DataSet();
                da.Fill(ds, "insumos");

                dgv.DataSource = ds;
                dgv.DataMember = "insumos";
            }
            else
            {
                string consulta = "select [id_insumo],[nombre_insumo],p.[nombre_empresa],[descripcion_insumo],[precio_compra],[cantidad_disponible] from [dbo].[insumos] s inner join [dbo].[proveedores] p on p.id_proveedor=s.id_proveedor where [nombre_empresa] like '%" + bus + "%'";
                SqlDataAdapter da = new SqlDataAdapter(consulta, conexion);
                conexion.Open();
                DataSet ds = new DataSet();
                da.Fill(ds, "insumos");

                dgv.DataSource = ds;
                dgv.DataMember = "insumos";
            }
        }

        public void Actualizar(DataGridView dgv)
        {
            SqlConnection conexion = new SqlConnection(cadenaconexion);
            try
            {
                conexion.Open();
                cmd = new SqlCommand("UPDATE [dbo].[insumos] set nombre_insumo = @name, id_proveedor= @empe ,[descripcion_insumo]= @desc,[precio_compra] = @precio,[cantidad_disponible] = @cantidad where [id_insumo] = @id ", conexion);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@empe", Emp);
                cmd.Parameters.AddWithValue("@desc", Desc);
                cmd.Parameters.AddWithValue("@precio", Precio);
                cmd.Parameters.AddWithValue("@cantidad", cantidad);
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
