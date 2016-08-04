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
    public partial class LeerQuejas : Form
    {
        public LeerQuejas()
        {
            InitializeComponent();
        }

        private void LeerQuejas_Load(object sender, EventArgs e)
        {
            MySqlConnection conexion = Login.conectarBD();
            MySqlCommand cmd = conexion.CreateCommand();
            conexion.Open();
            DataTable dtDatos = new DataTable();
            MySqlDataAdapter mdaDatos = new MySqlDataAdapter("SELECT * FROM queja", conexion);
            mdaDatos.Fill(dtDatos);
            dataGridView1.DataSource = dtDatos;
            conexion.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            principalQuejas q = new principalQuejas();
            this.Hide();
            q.ShowDialog();
        }
    }
}
