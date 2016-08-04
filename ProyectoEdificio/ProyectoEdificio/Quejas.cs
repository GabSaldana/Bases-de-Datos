using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace ProyectoEdificio
{
    public partial class Quejas : Form
    {
        public Quejas()
        {
            InitializeComponent();
            getDeptos(comboBox1);
        }

        public static void getDeptos(ComboBox comboBox1)
        {

            MySqlConnection conexion = Login.conectarBD();
            MySqlCommand cmd = conexion.CreateCommand();
            conexion.Open();
            cmd.CommandText = "select nDepartamento from departamento;";
            MySqlDataReader lector = cmd.ExecuteReader();
            while (lector.Read())
            {
                comboBox1.Items.Add(lector.GetInt16("nDepartamento"));
            }
            lector.Close();
            conexion.Close();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            MySqlConnection conexion = Login.conectarBD();
            conexion.Open();
            MySqlCommand cmd = conexion.CreateCommand();
            String motivo = textBox1.Text;
            String queja = richTextBox1.Text;
            String fecha = dateTimePicker1.Value.Year.ToString() + "-" + dateTimePicker1.Value.Month + "-" + dateTimePicker1.Value.Day;
            int dep = Convert.ToInt32(comboBox1.SelectedValue);
            cmd.CommandText = "insert into queja(motivo, queja, fechaQueja, departamento_nDepartamento) values (\"" + motivo + "\", \"" + queja + "\", \"" + fecha + "\", " + dep + ");";
            MySqlDataReader lector = cmd.ExecuteReader();
            lector.Read();
            lector.Close();
            MessageBox.Show("Queja registrada exitosamente", "Multa", MessageBoxButtons.OK);
            conexion.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            principalQuejas q = new principalQuejas();
            this.Hide();
            q.ShowDialog();
        }
    }
}
