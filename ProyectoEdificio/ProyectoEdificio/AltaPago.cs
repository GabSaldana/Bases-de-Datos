using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;
using MySql.Data.MySqlClient;
using System.Collections;

namespace ProyectoEdificio
{
    public partial class AltaPago : Form
    {
        public AltaPago()
        {
            InitializeComponent();
        }

        public void AltaPago_Load(){
        
        }

        void GuardarImagen(Image imagen, int idDetalle)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                imagen.Save(ms, ImageFormat.Jpeg);
                byte[] imgArr = ms.ToArray();
                using (MySqlConnection conn = Login.conectarBD())
                {
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                        
                        cmd.CommandText = "update detallepago set recibo=@imgArr where idDetalle=" + idDetalle;
                        cmd.Parameters.AddWithValue("@imgArr", imgArr);
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    
                }
            }
        }

        private void BtnCargar_Click(object sender, EventArgs e)
        {
            try
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string imagen = openFileDialog1.FileName;
                    pictureBox1.Image = Image.FromFile(imagen);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("El archivo seleccionado no es un tipo de imagen válido");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(comboBox1.SelectedItem.ToString()) || String.IsNullOrEmpty(richTextBox1.Text))
            {
                MessageBox.Show("Un valor es necesario", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Se requiere cargar la imagen del recibo primero", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            string servicio = comboBox1.SelectedItem.ToString();
            Double monto = 0;
            try
            {
                monto = Convert.ToDouble(richTextBox1.Text);
            }
            catch (FormatException)
            {
                MessageBox.Show("Dato no válido", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (monto <= 0)
            {
                MessageBox.Show("Valor no válido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string limite = dateTimePicker1.Value.Year.ToString() + "-" + dateTimePicker1.Value.Month.ToString() + "-" + dateTimePicker1.Value.Day.ToString();
            MySqlConnection conexion = Login.conectarBD();
            conexion.Open();
            MySqlCommand cmd = conexion.CreateCommand();
            cmd.CommandText = "select idServicio from servicio where nombre = \"" + servicio + "\";";
            MySqlDataReader lector = cmd.ExecuteReader();
            lector.Read();
            int id = lector.GetInt16(0);
            lector.Close();
            cmd.CommandText = "select departamento_nDepartamento from inquilino group by departamento_nDepartamento;";
            lector = cmd.ExecuteReader();
            ArrayList deptos = new ArrayList();

            while (lector.Read())
                deptos.Add(lector.GetInt16("departamento_nDepartamento"));

            lector.Close();
            int tot = deptos.Count;
            int n = 0;

            cmd.CommandText = "select fechaLimite from detallePago, pagoServicio where servicio_idServicio = " + id +
                                " and fechaLimite = \"" + limite + "\";";
            lector = cmd.ExecuteReader();

            if (lector.Read())
            {
                MessageBox.Show("Pago ya dado de alta", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                lector.Close();
                return;
            }
            lector.Close();


            cmd.CommandText = " select idDetalle from detallePago where costo = " + monto + " and fechaLimite = \"" + limite + "\";";
            lector = cmd.ExecuteReader();
            int idDetalle;

            if (lector.Read())
            {
                idDetalle = lector.GetInt16(0);
                lector.Close();
            }

            else
            {
                MySqlCommand cmd2 = conexion.CreateCommand();
                cmd2.CommandText = "insert into detallePago(costo, fechaLimite) values(" + monto + ",\"" + limite + "\");";
                lector.Close();
                cmd2.ExecuteNonQuery();
                lector = cmd.ExecuteReader();
                lector.Read();
                idDetalle = lector.GetInt32(0);
                lector.Close();
            }
            try
            {
                for (int i = 0; i < tot; i++)
                {
                    cmd.CommandText = "insert into pagoServicio(servicio_idServicio, departamento_nDepartamento,detallepago_idDetalle, monto, pagado) values(" + id + "," + deptos[i] + "," + idDetalle + "," + monto / tot + ",false);";
                    n = cmd.ExecuteNonQuery();
                }
            }

            catch (MySqlException)
            {
                MessageBox.Show("No se pudo dar de alta el pago", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("El pago se ha dado de alta", "Exito", MessageBoxButtons.OK);
            cmd.CommandText = "select max(idDetalle) from detallepago";
            int registro = Convert.ToInt32(cmd.ExecuteScalar());
            conexion.Close();
            GuardarImagen(pictureBox1.Image, registro);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            InicioAdmin ia = new InicioAdmin();
            this.Hide();
            ia.ShowDialog();
        }
    }
}
