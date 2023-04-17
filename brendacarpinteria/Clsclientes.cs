using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;

namespace brendacarpinteria
{
    internal class Clsclientes
    {
        private string cadenaconexion = "Data Source =localhost\\SQLEXPRESS; Initial Catalog = Carpinteria_BD; Integrated Security=True";
        //SqlDataAdapter da;
        //SqlCommand cmd;
        //DataTable dt;


        private long identidad;
        private string cons;
        private string bus;
        private static bool validarc;
        public string Nombre { get; set; }

        public string Apellido { get; set; }

        public int id { get; set; }

        public string DireccionC { get; set; }

        public string Tl { get; set; }
        public string Cons { get => cons; set => cons = value; }
        public string Bus { get => bus; set => bus = value; }
        public long Identidad { get => identidad; set => identidad = value; }

        public bool validarinsert(string nombre , string apellido )
        {

            SqlConnection conexion = new SqlConnection(cadenaconexion);
            conexion.Open();
            SqlCommand cmd = new SqlCommand("select * from clientes where nombre = '"+nombre+ "'", conexion);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                validarc = true;
            }
            dr.Close();
            return validarc;
        }


        public void cargardatos(DataGridView dgv)
        {
            SqlConnection conexion = new SqlConnection(cadenaconexion);
            conexion.Open();
            try
            {
                SqlDataAdapter clie = new SqlDataAdapter("select * from clientes", conexion);
                DataTable dtclie = new DataTable();
                clie.Fill(dtclie);
                dgv.DataSource = dtclie;
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pueden cargar los datos:" +ex);
            }
        }

        public void Insertar()
        {

            SqlConnection conexion   = new SqlConnection(cadenaconexion);
            conexion.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("insert into clientes values(@id,@name,@ape,@dir,@tel)", conexion);
                cmd.Parameters.AddWithValue("@id", Identidad);
                cmd.Parameters.AddWithValue("@name", Nombre);
                cmd.Parameters.AddWithValue("@ape", Apellido);
                cmd.Parameters.AddWithValue("@dir", DireccionC);
                cmd.Parameters.AddWithValue("@tel", Tl);
                cmd.ExecuteNonQuery();
                conexion.Close();
                MessageBox.Show("Registro guardado correctamente");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Actualizar()
        {
            SqlConnection con = new SqlConnection(cadenaconexion);
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("UPDATE clientes set nombre = @nam,apellido = @ape,direccion = @dir,telefono = @tel where id_cliente = @id ", con);
                cmd.Parameters.AddWithValue("@nam", Nombre);
                cmd.Parameters.AddWithValue("@ape", Apellido);
                cmd.Parameters.AddWithValue("@dir", DireccionC);
                cmd.Parameters.AddWithValue("@tel", Tl);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
                con.Close();
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
                  string consulta = "select * from [dbo].[clientes] where nombre like '%" + Bus + "%'";
                  SqlDataAdapter da = new SqlDataAdapter(consulta, conexion);
                  conexion.Open();
                  DataSet ds = new DataSet();
                  da.Fill(ds, "insumos");

                  dgv.DataSource = ds;
                  dgv.DataMember = "insumos";

              }
              else if (Cons == "id")
              {
                  string consulta = "select * from [dbo].[clientes] where[id_cliente] = '" + Bus + "'";
                  SqlDataAdapter da = new SqlDataAdapter(consulta, conexion);
                  conexion.Open();
                  DataSet ds = new DataSet();
                  da.Fill(ds, "insumos");

                  dgv.DataSource = ds;
                  dgv.DataMember = "insumos";
              }

        }
    }
}
