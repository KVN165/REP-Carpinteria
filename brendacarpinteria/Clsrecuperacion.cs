using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using System.Data;
using System.Data.SqlClient;

namespace brendacarpinteria
{
    public class Clsrecuperacion
    {
        private SmtpClient smtpClient;
        private string cadenaconexion = "Data Source =MENDEZ\\SQLEXPRESS; Initial Catalog = Carpinteria_BD; Integrated Security=True";
        SqlDataAdapter da;
        SqlCommand cmd;
        DataTable dt;
        protected string remitentecorreo { get; set; }
        protected string pass { get; set; }
         
        protected string host { get; set; } 
        protected int port { get; set; }
        protected bool ssl { get; set; }

        protected void initializestmpcliente()
        {
            smtpClient = new SmtpClient();
            smtpClient.Credentials = new NetworkCredential(remitentecorreo, pass);
            smtpClient.Host = host;
            smtpClient.Port = port; 
            smtpClient.EnableSsl = ssl;
        }

        public void sendmail(string subject, string body , List<string> destinatariocorreo)
        {
            var mailmessage = new MailMessage();
            try
            {
                mailmessage.From = new MailAddress(remitentecorreo);
                foreach(string mail in destinatariocorreo)
                {
                    mailmessage.To.Add(mail);
                }
                mailmessage.Subject = subject;  
                mailmessage.Body = body;
                mailmessage.Priority = MailPriority.Normal;
                smtpClient.Send(mailmessage);
            }
            catch (Exception ex) { }
            finally
            {
                mailmessage.Dispose();
                smtpClient.Dispose();
            }
        }

        public string  recoverpassword(string usuariosolicitado)
        {
            SqlConnection conexion = new SqlConnection(cadenaconexion);
            conexion.Open();
            cmd = new SqlCommand("select * from usuarios where [usuario] = @us or [correo] =@correo ", conexion);
            cmd.Parameters.AddWithValue("@us", usuariosolicitado);
            cmd.Parameters.AddWithValue("@correo",usuariosolicitado);
            cmd.CommandType = System.Data.CommandType.Text;
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read() == true)
            {
                string nombre = reader.GetString(1);
                string correo = reader.GetString(7);
                string contra = reader.GetString(4);

                var mailservice = new Dcorreosoporte();
                mailservice.sendmail(subject: "SISTEMA DE CARPINTERIA BRENDA : SOLICITUD DE RECUPERACION DE CONTRASEÑA ",
                    body: "HOLA , " + nombre + "\n Usted solicito recuperar contraseña.\n" + "\n sin envargo le pedimos que se comunique con el administrador para cambiar la contraseña..." + "\n su contraseña es : "+ contra,
                    destinatariocorreo: new List<string> { correo });

                return "hola, " + nombre + " Usted a solicitado recuperar su contraseña.\n" + "porfavor revise su correo: " + correo + " \nsin envargo le pedimos cambiar la contraseña...";
            }
            else
            {
                return "lo sentimos , no tiene una cuenta con ese correo o nombre de usuario";
            }
            
        }

    }
}
