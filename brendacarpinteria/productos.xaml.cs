﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MessageBox = System.Windows.Forms.MessageBox;

namespace brendacarpinteria
{
    /// <summary>
    /// Lógica de interacción para productos.xaml
    /// </summary>
   

    public partial class productos : Window

    {
        private string cadenaconexion = "Data Source =MENDEZ\\SQLEXPRESS; Initial Catalog = Carpinteria_BD; Integrated Security=True";

        Clsproducto1 pro = new Clsproducto1();
        int i;
        public productos()
        {
            InitializeComponent();


        }
       

        public void envdatos()
        {
            pro.Name = txtnombre.Text;
            pro.Desc = txtprecio1.Text;
            pro.Precio = Convert.ToDecimal(txtprecio.Text);
            pro.Cantidad = Convert.ToInt64(txtcantidad1.Text);
           
            
        }
   
    
        private void btnagregar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtnombre.Text == String.Empty || txtprecio.Text == String.Empty)
                {
                    MessageBox.Show("Por favor llene todos los campos");
                }
                else
                {
                    envdatos();
                    pro.Insertar();
                    pro.cargarDatapro(dgvproductos);
                    txtbuscar1.Clear();
                    txtnombre.Clear();
                    txtprecio.Clear();
                    txtprecio1.Clear();
                    txtcantidad1.Clear();


                    
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error ejecutando proceso");

            }
           
        }

        private void WindowsFormsHost_ChildChanged(object sender, System.Windows.Forms.Integration.ChildChangedEventArgs e)
        {
            pro.cargarDatapro(dgvproductos);
         


        }

        private void seleccionar()
        {
            txtid.Text = dgvproductos.CurrentRow.Cells[0].Value.ToString();
            txtnombre.Text = dgvproductos.CurrentRow.Cells[1].Value.ToString();
            txtprecio1.Text = dgvproductos.CurrentRow.Cells[2].Value.ToString();
            txtcantidad1.Text = dgvproductos.CurrentRow.Cells[4].Value.ToString();
            txtprecio.Text = dgvproductos.CurrentRow.Cells[3].Value.ToString();
        }

        private void txtnombre_Copy_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btnbuscar_Click_1(object sender, RoutedEventArgs e)
        {
            pro.Bus = txtbuscar1.Text;
            pro.Cons = Convert.ToString(cobbus.Text);
            pro.buscar(dgvproductos);
        }

        private void combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnrefrescar_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection conexion = new SqlConnection(cadenaconexion);
            conexion.Open();
            pro.cargarDatapro(dgvproductos);
            txtbuscar1.Clear();
            conexion.Close();
            txtbuscar1.Clear();
            txtnombre.Clear();
            txtprecio.Clear();
            txtprecio1.Clear();
            txtcantidad1.Clear();
            txtid.Clear();
            btnagregar.IsEnabled = true;
            btnActualizar.IsEnabled = false;
        }

        private void btnActualizar_Click(object sender, RoutedEventArgs e)
        {
            
            pro.Id = Convert.ToInt32(txtid.Text);
            pro.Name = txtnombre.Text;
            pro.Desc = txtprecio1.Text;
            pro.Precio = Convert.ToDecimal(txtprecio.Text);
            pro.Cantidad = Convert.ToInt32(txtcantidad1.Text);
            pro.Actualizar(dgvproductos);
            pro.cargarDatapro(dgvproductos);
            btnagregar.IsEnabled = true;
            btnActualizar.IsEnabled = false;
        }

        private void dgvproductos_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            seleccionar();
            btnagregar.IsEnabled = false;
            btnActualizar.IsEnabled = true;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

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