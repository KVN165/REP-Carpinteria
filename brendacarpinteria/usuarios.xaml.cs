using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace brendacarpinteria
{
    /// <summary>
    /// Lógica de interacción para usuarios.xaml
    /// </summary>
    public partial class usuarios : Window
    {
        ClsUsuario us = new ClsUsuario();
        public usuarios()
        {
            InitializeComponent();
        }

        public void endatos()
        {
            us.Name = txtnombre.Text;
            us.Ape = txtapellido.Text;
            us.Contr = txtcontra.Text;
            us.Nusu1 = txtusuario.Text;
            us.Tip = Convert.ToString(combo.Text);
            us.Estado = Convert.ToString(cmbes.Text);
            us.Correo = txtcorreo.Text;
            
        }

        private void WindowsFormsHost_Loaded(object sender, RoutedEventArgs e)
        {
            us.cargarDatausu(dgvusuarios);
        }

        public void limpiar(){
            txtbuscar1.Clear();
            txtnombre.Clear();
            txtapellido.Clear();
            txtcontra.Clear();
            txtnombre.Clear();
            txtbuscar1.Clear();
            txtusuario.Clear();
            txtcorreo.Clear();
            txtid.Clear();
        }
    
        
        private void btnbuscar_Click(object sender, RoutedEventArgs e)
        {
            us.Bus = txtbuscar1.Text;
            us.Cons = Convert.ToString(cobbus.Text);
            us.buscar(dgvusuarios);
            
        }

        private void seleccion()
        {
            txtid.Text = dgvusuarios.CurrentRow.Cells[0].Value.ToString();
            txtnombre.Text = dgvusuarios.CurrentRow.Cells[1].Value.ToString();
            txtapellido.Text = dgvusuarios.CurrentRow.Cells[2].Value.ToString();
            combo.Text = dgvusuarios.CurrentRow.Cells[3].Value.ToString();
            txtcontra.Text = dgvusuarios.CurrentRow.Cells[4].Value.ToString();
            txtusuario.Text = dgvusuarios.CurrentRow.Cells[5].Value.ToString();
            cmbes.Text = dgvusuarios.CurrentRow.Cells[6].Value.ToString();
            txtcorreo.Text = dgvusuarios.CurrentRow.Cells[7].Value.ToString();

        }

        private void btnactualizar_Click(object sender, RoutedEventArgs e)
        {
            us.Id = Convert.ToInt32(txtid.Text);
            us.Name = txtnombre.Text;
            us.Ape = txtapellido.Text;
            us.Contr = txtcontra.Text;
            us.Nusu1 = txtusuario.Text;
            us.Tip = Convert.ToString(combo.Text);
            us.Estado = Convert.ToString(cmbes.Text);
            us.Correo = txtcorreo.Text;
            us.Actualizar(dgvusuarios);
            limpiar();
            us.cargarDatausu(dgvusuarios);
            btnagregar1.IsEnabled = true;
            btnactualizar.IsEnabled = false;
        }

        private void dgvusuarios_CellMouseClick(object sender, System.Windows.Forms.DataGridViewCellMouseEventArgs e)
        {
            seleccion();
            btnagregar1.IsEnabled = false;
            btnactualizar.IsEnabled = true;
        }

        private void btnrefrescar_Click(object sender, RoutedEventArgs e)
        {
            us.cargarDatausu(dgvusuarios);
            btnagregar1.IsEnabled = true;
            btnactualizar.IsEnabled = false;
            limpiar();
        }

        private void btnregresar_Click(object sender, RoutedEventArgs e)
        {
            ClsUsuario us = new ClsUsuario();
            if (us.Tipousuario == "Administrador")
            {
                    admin ad = new admin();
                    ad.Show();
                    this.Close();
            }
            else if (us.Tipousuario == "Cajero")
            {
                    cajero ca = new cajero();
                    ca.Show();
                    this.Close();
            }
            else if (us.Tipousuario == "Bodeguero")
            {
                    Bodeguero bo = new Bodeguero();
                    bo.Show();
                    this.Close();
            }
        }

        //validaciones


        public static bool ValidarEmai1(string comprobarEmai1)
        {
            string emailFormato;
            emailFormato = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+({-.]\\w+)*";
            if (Regex.IsMatch(comprobarEmai1, emailFormato))
            {
                if (Regex.Replace(comprobarEmai1, emailFormato, string.Empty).Length == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private bool AlgoritmoContraseñaSegura(string password)
        {
            bool mayuscula, minuscula, numero, carespecial;
            mayuscula = false; minuscula = false; numero = false; carespecial = false;
            for (int i = 0; i < password.Length; i++)
            {
                if (Char.IsUpper(password, i))
                {
                    mayuscula = true;
                }
                else if (Char.IsLower(password, i))
                {
                    minuscula = true;
                }
                else if (Char.IsDigit(password, i))
                {
                    numero = true;
                }
                else
                {
                    carespecial = true;
                }
            }
            if (mayuscula && minuscula && numero && carespecial && password.Length >= 8)
            {
                return true;
            }
            return false;
        }
      
        
        private void txtnombre_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, "^[a-zA-Z]"))
            {
                e.Handled = true;
            }
        }

        private void txtapellido_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, "^[a-zA-Z]"))
            {
                e.Handled = true;
            }
        }



        private void txtusuario_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, "^[A-Za-z0-9]+$"))
            {
                e.Handled = true;
            }
        }

        

        private void btnagregar1_Click(object sender, RoutedEventArgs e)
        {
            if (txtnombre.Text == String.Empty || txtcorreo.Text == String.Empty || txtapellido.Text == String.Empty || txtcontra.Text == String.Empty)
            {
                MessageBox.Show("Por favor llene todos los campos");
            }
            else
            {
                if (us.validarinsert(txtusuario.Text) == true)
                {

                    MessageBox.Show("el usuario ya existe");
                }
                else
                {
                    if (AlgoritmoContraseñaSegura(txtcontra.Text) == false)
                    {
                        MessageBox.Show("contraseña no valida");
                    }
                    else
                    {
                        if (ValidarEmai1(txtcorreo.Text) == false)
                        {
                            MessageBox.Show("direccion de correo no valida");
                        }
                        else
                        {
                            endatos();
                            us.Insertar();
                            us.cargarDatausu(dgvusuarios);
                            limpiar();
                            btnactualizar.IsEnabled = true;

                        }
                    }

                }
                

            }
        }
    }
 }

