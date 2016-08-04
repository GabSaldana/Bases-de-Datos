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
    public partial class PagoMulta : Form
    {
        public PagoMulta()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MySqlConnection conexion = Login.conectarBD();
            MySqlCommand cmd = conexion.CreateCommand();
            conexion.Open();
            String fechapago = dateTimePicker2.Value.Year.ToString() + "-" + dateTimePicker2.Value.Month + "-" + dateTimePicker2.Value.Day;
            Boolean pagado = false;
            if (checkBox1.Checked)
            {
                pagado = true;
            }
            cmd.CommandText = "update multa set pagado = " + pagado + ", fechapago = '" + fechapago + "' where idmulta = LAST_INSERT_ID();";
            MySqlDataReader lector = cmd.ExecuteReader();
            lector.Read();
            lector.Close();
            MessageBox.Show("Pago de multa registrado exitosamente", "Pago de multa", MessageBoxButtons.OK);
            conexion.Close();
        }

        private void PagoMulta_Load(object sender, EventArgs e)
        {

        }
    }
}
