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
    public partial class Perfil : Form
    {
        public Perfil()
        {
            InitializeComponent();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView4_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Perfil_Load(object sender, EventArgs e)
        {
            MySqlConnection conexion = Login.conectarBD();
            DataTable dtDatos = new DataTable();
            MySqlDataAdapter mdaDatos = new MySqlDataAdapter("SELECT m.pagado, m.fechaMulta, m.fechaPago FROM  multa m, inquilino i, departamento d  where i.departamento_nDepartamento=d.nDepartamento and m.departamento_nDepartamento=d.nDepartamento and i.infoinquilino_curp='" + Login.curp + "'", conexion);
            mdaDatos.Fill(dtDatos);
            dataGridView1.DataSource = dtDatos;

            DataTable dtDatos2 = new DataTable();
            MySqlDataAdapter mdaDatos2 = new MySqlDataAdapter("SELECT monto,pagado,fechaPago FROM  pagoServicio p, inquilino i, departamento d where i.departamento_nDepartamento=d.nDepartamento and d.nDepartamento=p.departamento_nDepartamento and i.infoinquilino_curp='" + Login.curp + "'", conexion);
            mdaDatos2.Fill(dtDatos2);
            dataGridView2.DataSource = dtDatos2;

            DataTable dtDatos3 = new DataTable();
            MySqlDataAdapter mdaDatos3 = new MySqlDataAdapter("SELECT nombre ,aPaterno ,aMaterno ,pass FROM infoInquilino where curp='" + Login.curp + "'", conexion);
            mdaDatos3.Fill(dtDatos3);
            dataGridView3.DataSource = dtDatos3;

            DataTable dtDatos4 = new DataTable();
            MySqlDataAdapter mdaDatos4 = new MySqlDataAdapter("SELECT e.descripcion FROM estatusDep e, departamento d, inquilino i where i.departamento_nDepartamento=d.nDepartamento and d.estatusDep_idEstatusDep=e.idEstatusDep and i.infoinquilino_curp='" + Login.curp + "'", conexion);
            mdaDatos4.Fill(dtDatos4);
            dataGridView4.DataSource = dtDatos4;
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Intro i = new Intro();
            this.Hide();
            i.ShowDialog();
        }

        
    }
}
