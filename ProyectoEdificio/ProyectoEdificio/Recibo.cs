using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;

namespace ProyectoEdificio
{
    public partial class Recibo : Form
    {
        public Recibo()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LblInfo.Text = "";
            try
            {
                pictureBox1.Image = CargarImagen(comboBox1.SelectedItem.ToString(), Login.curp);
                pictureBox1.BackgroundImageLayout = ImageLayout.Center;
            }
            catch (Exception) {
                LblInfo.Text = "No hay recibo pendiente";
                pictureBox1.Image = null;
            }
        }

        Image CargarImagen(string Nombre, string curp)
        {
            using (MySqlConnection conn = Login.conectarBD())
            {
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT d.recibo FROM detallepago d, pagoservicio p, servicio s WHERE s.nombre = @nombre and s.idServicio=p.servicio_idServicio and p.detallePago_idDetalle=d.idDetalle and p.fechaPago is NULL and p.departamento_nDepartamento=(select departamento_nDepartamento from inquilino where infoinquilino_CURP=@curp)";
                    cmd.Parameters.AddWithValue("@nombre", Nombre);
                    cmd.Parameters.AddWithValue("@curp", curp);
                    byte[] imgArr = (byte[])cmd.ExecuteScalar();
                    imgArr = (byte[])cmd.ExecuteScalar();
                    using (var stream = new MemoryStream(imgArr))
                    {
                        Image img = Image.FromStream(stream);
                        conn.Close();
                        return img;
                    }
                   
            }
        }

        private void Recibo_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Intro i = new Intro();
            this.Hide();
            i.ShowDialog();
        }

    }
}
