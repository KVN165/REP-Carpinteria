using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Lógica de interacción para clientes.xaml
    /// </summary>
    public partial class clientes : Window
    {
        Clsclientes c = new Clsclientes();
        public clientes()
        {
            InitializeComponent();
        }

        private void WindowsFormsHost_ChildChanged(object sender, System.Windows.Forms.Integration.ChildChangedEventArgs e)
        {
            c.cargardatos(dgvclientes);
        }

        private void btnagregar_Click(object sender, RoutedEventArgs e)
        {
            if (txtnombre.Text == String.Empty || txtapellido.Text == String.Empty || txtdireccion .Text == String.Empty ||txttel1.Text == String.Empty)
            {
                MessageBox.Show("Por favor llene todos los campos");
            }
            else
            {
                if(c.validarinsert(txtnombre.Text, txtapellido.Text) == false)
                {
                    if(Validartel(txttel1.Text) == false)
                    {
                        MessageBox.Show("numero de telefono no valida");
                    }
                    else
                    {
                        c.Identidad = Convert.ToInt64(txtid.Text);
                        MessageBox.Show("long " + c.Identidad);
                        c.Nombre = txtnombre.Text;
                        c.DireccionC = txtdireccion.Text;
                        c.Tl = txttel1.Text;
                        c.Apellido = txtapellido.Text;
                        c.Insertar();
                        c.cargardatos(dgvclientes);
                    }
                
                }
                else
                {
                    MessageBox.Show("datos ya existen ", "Datos existentes");
                }
            }
            }
          

        private void btnactualizar_Click(object sender, RoutedEventArgs e)
        {
            c.Nombre = txtnombre.Text;
            c.DireccionC = txtdireccion.Text;
            c.Tl = txttel1.Text;
            c.Apellido = txtapellido.Text;
            c.Identidad = Convert.ToInt64(txtid.Text);
            c.Actualizar();
            c.cargardatos(dgvclientes);
            btnagregar.IsEnabled = true;
            btnactualizar.IsEnabled = false;

            limpiar();
        }

        public void limpiar()
        {
            txtbuscar1.Clear();
            txtnombre.Clear();
            txtapellido.Clear();
            txtnombre.Clear();
            txtbuscar1.Clear();
            txtdireccion.Clear();
            txttel1.Clear();
            txtid.Clear();
            btnactualizar.IsEnabled=false;
            btnagregar.IsEnabled=true;
        }

        private void dgvclientes_CellMouseClick(object sender, System.Windows.Forms.DataGridViewCellMouseEventArgs e)
        {
            txtid.Text = dgvclientes.CurrentRow.Cells[0].Value.ToString();
           txtnombre.Text = dgvclientes.CurrentRow.Cells[1].Value.ToString();
            txtapellido.Text = dgvclientes.CurrentRow.Cells[2].Value.ToString();
            txtdireccion.Text = dgvclientes.CurrentRow.Cells[3].Value.ToString();
            txttel1.Text = dgvclientes.CurrentRow.Cells[4].Value.ToString();
            btnactualizar.IsEnabled = true;
            btnagregar.IsEnabled = false;
        }

        private void btnrefresca_Click(object sender, RoutedEventArgs e)
        {
            limpiar();
        }

        private void btnbuscar_Click(object sender, RoutedEventArgs e)
        {
            c.Bus = txtbuscar1.Text;
            c.Cons = Convert.ToString(cobbus.Text);
            c.buscar(dgvclientes);
        }

        private void btnregresar2_Click(object sender, RoutedEventArgs e)
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
        private void txtnombre_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, "^[a-zA-Z]"))
            {
                e.Handled = true;
            }
        }

        public static bool Validartel(string tel)
        {
            string telfromato;

            telfromato = "[3289][0-9]*";
            if (Regex.IsMatch(tel, telfromato))
            {
                if (Regex.Replace(tel, telfromato, string.Empty).Length == 0)
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

        private void txtdireccion_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, "^[a-zA-Z0-9]"))
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

        private void txttel1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
                e.Handled = false;
        
            else
                e.Handled = true;
        }

        private void txttel1_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex re = new Regex(@"(8|9|2|3)[ -]*([0-9][ -]*){8}");
            if (re.IsMatch(e.Text))
            {
                e.Handled = true;
            }
          
        }


    }
}
