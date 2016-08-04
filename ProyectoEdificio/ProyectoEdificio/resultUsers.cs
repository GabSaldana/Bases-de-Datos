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
    public partial class resultUsers : Form
    {
        public resultUsers()
        {
            InitializeComponent();
        }

        public resultUsers(int n)
        {
            InitializeComponent();
            MySqlConnection conexion = Login.conectarBD();
            conexion.Open();
            MySqlCommand cmd = conexion.CreateCommand();
            cmd.CommandText = "select nombre,aPaterno,aMaterno,curp from infoinquilino, inquilino where infoInquilino_curp = curp and departamento_nDepartamento = " + n + " and fechaBaja IS NULL";
            DataTable datos = new DataTable();
            MySqlDataAdapter da= new MySqlDataAdapter(cmd.CommandText, conexion);
            MySqlCommandBuilder cb = new MySqlCommandBuilder(da);
            da.Fill(datos);
            dataGridView1.DataSource = datos;
            conexion.Close();

        }

        private void resultUsers_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            AdminUsers au = new AdminUsers();
            this.Hide();
            au.ShowDialog();
        }
    }
}
