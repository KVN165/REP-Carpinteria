using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace brendacarpinteria
{
  
    internal class ClsUsuario
    {
        private string usuarios;
        private string contra;
        private static string tipousuario;

        private string cusuario;
        private string ccon;
        private string bus;
        private string name;
        private string ape;
        private string tip;
        private string Nusu;
        private string contr;
        private static int id;
        private string cons;
        private string estado;
        private string tipo;
        private static string no;
        private string correo;
        private static int idusuario;
        public string Usuarios { get => usuarios; set => usuarios = value; }
        public string Contra { get => contra; set => contra = value; }
        public  string Tipousuario { get => tipousuario; set => tipousuario = value; }
        public string Bus { get => bus; set => bus = value; }
        public string Name { get => name; set => name = value; }
        public string Ape { get => ape; set => ape = value; }
        public string Tip { get => tip; set => tip = value; }
        public string Nusu1 { get => Nusu; set => Nusu = value; }
        public string Contr { get => contr; set => contr = value; }
        public  int Id { get => id; set => id = value; }
        public string Cons { get => cons; set => cons = value; }
        public string Estado { get => estado; set => estado = value; }
        public string Tipo { get => tipo; set => tipo = value; }
        public string No { get => no; set => no = value; }
        public string Correo { get => correo; set => correo = value; }
        public string Cusuario { get => cusuario; set => cusuario = value; }
        public string Ccon { get => ccon; set => ccon = value; }
        public  int Idusuario { get => idusuario; set => idusuario = value; }

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
                cmd = new SqlCommand("insert into [dbo].[usuarios] values(@name,@ape,@tip,@cont,@usu,@es,@cor)", conexion);
                cmd.Parameters.AddWithValue("@name", Name);
                cmd.Parameters.AddWithValue("@ape", Ape);
                cmd.Parameters.AddWithValue("@tip", Tip);
                cmd.Parameters.AddWithValue("@cont", contr);
                cmd.Parameters.AddWithValue("@usu", Nusu1);
                cmd.Parameters.AddWithValue("@es",Estado);
                cmd.Parameters.AddWithValue("@cor", correo);
                cmd.ExecuteNonQuery();
                conexion.Close();
                MessageBox.Show("Registro guardado correctamente");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void cargarDatausu(DataGridView dgv)
        {
            SqlConnection conexion = new SqlConnection(cadenaconexion);
            try
            {
                da = new SqlDataAdapter("Select * from usuarios ", conexion);
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
            if (Cons == "Nombre")
            {
                string consulta = "select [id_usuario] , [nombre] , apellido , [tipo_usuario] , [contraseña],[usuario] from [dbo].[usuarios] where [nombre] like  '%" + Bus + "%'";
                SqlDataAdapter da = new SqlDataAdapter(consulta, conexion);
                conexion.Open();
                DataSet ds = new DataSet();
                da.Fill(ds, "id_usuario");

                dgv.DataSource = ds;
                dgv.DataMember = "id_usuario";
                conexion.Close();
            }
            else if (Cons == "id")
            {
                string consulta = "select [id_usuario] , [nombre] , apellido , [tipo_usuario] , [contraseña],[usuario] from [dbo].[usuarios] where [id_usuario]  = '" + Bus + "'";
                SqlDataAdapter da = new SqlDataAdapter(consulta, conexion);
                conexion.Open();
                DataSet ds = new DataSet();
                da.Fill(ds, "id_usuario");

                dgv.DataSource = ds;
                dgv.DataMember = "id_usuario";
                conexion.Close();
            }
            else if (Cons == "tipo de usuario")
            {
                string consulta = "select [id_usuario] , [nombre] , apellido , [tipo_usuario] , [contraseña],[usuario] from [dbo].[usuarios] where [tipo_usuario] like '%" + Bus + "%'";
                SqlDataAdapter da = new SqlDataAdapter(consulta, conexion);
                conexion.Open();
                DataSet ds = new DataSet();
                da.Fill(ds, "id_usuario");

                dgv.DataSource = ds;
                dgv.DataMember = "id_usuario";
                conexion.Close();
            }

        }

        public void Actualizar(DataGridView dgv)
        {
            SqlConnection conexion = new SqlConnection(cadenaconexion);
            try
            {
                conexion.Open();
                cmd = new SqlCommand("UPDATE [dbo].[usuarios] set [nombre] = @n, [apellido]= @a ,[tipo_usuario]= @t,[contraseña] = @c,[usuario] = @u, [Estadousuario] = @es,correo = @cor where [id_usuario] = @id ", conexion);
                cmd.Parameters.AddWithValue("@n", name??(Object)DBNull.Value);
                cmd.Parameters.AddWithValue("@a", ape ?? (Object)DBNull.Value);
                cmd.Parameters.AddWithValue("@t", tip ?? (Object)DBNull.Value);
                cmd.Parameters.AddWithValue("@c", contr ?? (Object)DBNull.Value);
                cmd.Parameters.AddWithValue("@u", Nusu ?? (Object)DBNull.Value);
                cmd.Parameters.AddWithValue("@es", estado ?? (Object)DBNull.Value);
                cmd.Parameters.AddWithValue("@id", Id);
                cmd.Parameters.AddWithValue("@cor", correo);
                cmd.ExecuteNonQuery();
                conexion.Close();
                MessageBox.Show("Registro guardado correctamente");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        public void login()
        {
            SqlConnection conexion = new SqlConnection(cadenaconexion);
            conexion.Open();
            
            SqlCommand cmd = new SqlCommand("select  usuario,contraseña,tipo_usuario,[Estadousuario],concat(nombre,' ',apellido),[id_usuario] from [dbo].[usuarios] where usuario='" + usuarios + "'and contraseña = '" + contra + "'", conexion);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                tipousuario = reader.GetString(2);
                Tipo=reader.GetString(3);
                No = reader.GetString(4);
                idusuario = reader.GetInt32(5);
                obtener_idusuario();
            }
            else
            {
                MessageBox.Show("Usuario o contraseña incorrecta", "Error de inicio", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
            }
        }

        public void obtener_idusuario()
        {
            SqlConnection conexion = new SqlConnection(cadenaconexion);
            conexion.Open();

            SqlCommand cmd = new SqlCommand("select id_usuario,nombre,apellido,usuario,contraseña,tipo_usuario,[Estadousuario]from [dbo].[usuarios] where usuario='" + usuarios + "'and contraseña = '" + contra + "'", conexion);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                Proyecto_Carpinteria.ClsFactura.Id_obtenido_factura = reader[0].ToString();




                string nombre1, apellido1, nombre_largo;
                nombre1 = reader[1].ToString();
                apellido1 = reader[2].ToString(); 
                nombre_largo = nombre1 + " " + apellido1;
                Proyecto_Carpinteria.ClsFactura.Usuario = nombre_largo;
            }
            else
            {
            }
        }


        public void actualizarusuario()
        {
            SqlConnection conexion = new SqlConnection(cadenaconexion);
            try
            {
                conexion.Open();
                cmd = new SqlCommand("UPDATE [dbo].[usuarios] set [contraseña] = @c,[usuario] = @u where [id_usuario] = @id ", conexion);
                cmd.Parameters.AddWithValue("@c", Ccon ?? (Object)DBNull.Value);
                cmd.Parameters.AddWithValue("@u", cusuario ?? (Object)DBNull.Value); 
                cmd.Parameters.AddWithValue("@id", idusuario);
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
