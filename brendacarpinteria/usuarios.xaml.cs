using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private void btnagregar_Click(object sender, RoutedEventArgs e)
        {
    
                    endatos();
                    us.Insertar();
                    us.cargarDatausu(dgvusuarios);
                    limpiar();
                    btnactualizar.IsEnabled = true; 
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
            btnagregar.IsEnabled = true;
            btnactualizar.IsEnabled = false;
        }

        private void dgvusuarios_CellMouseClick(object sender, System.Windows.Forms.DataGridViewCellMouseEventArgs e)
        {
            seleccion();
            btnagregar.IsEnabled = false;
            btnactualizar.IsEnabled = true;
        }

        private void btnrefrescar_Click(object sender, RoutedEventArgs e)
        {
            us.cargarDatausu(dgvusuarios);
            btnagregar.IsEnabled = true;
            btnactualizar.IsEnabled = false;
           
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
    }
 }

