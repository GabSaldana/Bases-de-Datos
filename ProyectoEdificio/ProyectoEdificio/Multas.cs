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
    public partial class Multas : Form
    {
        public Multas()
        {
            InitializeComponent();
            getDeptos(comboBox1);
            getMotivosmulta(comboBox2);

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

        public static void getMotivosmulta(ComboBox comboBox1)
        {

            MySqlConnection conexion = Login.conectarBD();
            MySqlCommand cmd = conexion.CreateCommand();
            conexion.Open();
            cmd.CommandText = "select idmotivoMulta from motivomulta;";
            MySqlDataReader lector = cmd.ExecuteReader();
            while (lector.Read())
            {
                comboBox1.Items.Add(lector.GetInt16("idmotivoMulta"));
            }

            lector.Close();
            conexion.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MySqlConnection conexion = Login.conectarBD();
            MySqlCommand cmd = conexion.CreateCommand();
            conexion.Open();
            int id = Convert.ToInt32(comboBox2.SelectedItem);
            cmd.CommandText = "select costo, descripcion from motivomulta where idmotivomulta = " + id + ";";
            MySqlDataReader lector = cmd.ExecuteReader();
            lector.Read();
            label8.Text = lector.GetString("costo");
            label9.Text = lector.GetString("descripcion");
            lector.Close();
            conexion.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PagoMulta p = new PagoMulta();
            p.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MySqlConnection conexion = Login.conectarBD();
            MySqlCommand cmd = conexion.CreateCommand();
            conexion.Open();
            String fechamulta = dateTimePicker1.Value.Year.ToString() + "-" + dateTimePicker1.Value.Month + "-" + dateTimePicker1.Value.Day;
            int id = Convert.ToInt32(comboBox2.SelectedItem);
            int dep = Convert.ToInt32(comboBox1.SelectedItem);
            cmd.CommandText = "insert into multa(pagado, fechaMulta, departamento_nDepartamento, motivoMulta_idmotivoMulta) values(0, '" + fechamulta + "', " + dep + ", " + id + ");";
            MySqlDataReader lector = cmd.ExecuteReader();
            lector.Read();
            lector.Close();
            MessageBox.Show("Multa registrada exitosamente", "Multa", MessageBoxButtons.OK);
            conexion.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            InicioAdmin ia = new InicioAdmin();
            this.Hide();
            ia.ShowDialog();
        }
    }
}
