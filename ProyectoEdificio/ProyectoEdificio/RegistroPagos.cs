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
    public partial class RegistroPagos : Form
    {
        public RegistroPagos()
        {
            InitializeComponent();
        }

        private void botonEnviar_Click(object sender, EventArgs e)
        {
            MySqlConnection conexion = Login.conectarBD();

            if (String.IsNullOrEmpty(monto.Text) || String.IsNullOrEmpty(deptos.Text))
            {
                MessageBox.Show("Datos no válidos", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            conexion.Open();
            MySqlCommand cmd = conexion.CreateCommand();
            cmd.CommandText = "select pagado from pagoservicio, detallepago, servicio where servicio_idServicio = idServicio and nombre = '" + servicioc.Text + "' and departamento_nDepartamento = " + deptos.SelectedItem + " and detallePago_idDetalle = idDetalle and fechaLimite = '" + fechas.Text + "'";
            MySqlDataReader lector = cmd.ExecuteReader();
            lector.Read();
            bool pagado = lector.GetBoolean(0);
            lector.Close();

            if (pagado)
            {
                cmd.CommandText = "select fechaPago from pagoservicio, servicio where servicio_idServicio = idServicio and nombre = '" + servicioc.Text + "' and departamento_nDepartamento = " + deptos.SelectedItem;
                lector = cmd.ExecuteReader();
                lector.Read();
                MessageBox.Show("El pago ya ha sido realizado el " + lector.GetString(0), "Pago ya realizado");
                lector.Close();
                conexion.Close();
                return;
            }

            string fechaPago = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString();
            cmd.CommandText = "select idDetalle from detallepago, servicio, pagoservicio where idServicio = servicio_idServicio and nombre = '" + servicioc.Text + "' and idDetalle = detallepago_idDetalle and fechaLimite = '" + fechas.Text + "' group by idDetalle;";
            lector = cmd.ExecuteReader();
            lector.Read();
            int idDetalle = lector.GetInt32(0);
            lector.Close();
            cmd.CommandText = "update pagoservicio set pagado = true, fechaPago = '" + fechaPago +
                        "' where departamento_nDepartamento = " + deptos.SelectedItem + " and detallepago_idDetalle = " + idDetalle;
            if (cmd.ExecuteNonQuery() == 1)
                MessageBox.Show("Pago realizado con éxito\nServicio : " + servicioc.Text + "\nPago de :" + fechas.Text + "\nNoDepto : " + deptos.SelectedItem + "\nMonto : $" + monto.Text + "\nFecha : " + DateTime.Now.ToString(), "Éxito");
            else
                MessageBox.Show("No se pudo registrar el pago", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);

            conexion.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            InicioAdmin ia = new InicioAdmin();
            this.Hide();
            ia.ShowDialog();
        }

        private void RegistroPagos_Load(object sender, EventArgs e)
        {
            getDeptos(deptos);
            servicioc.SelectionStart = 0;
        }

        public static void getDeptos(ComboBox caja)
        {

            MySqlConnection conexion = Login.conectarBD();
            MySqlCommand cmd = conexion.CreateCommand();
            conexion.Open();
            cmd.CommandText = "select departamento_nDepartamento from inquilino group by departamento_nDepartamento;";
            MySqlDataReader lector = cmd.ExecuteReader();
            while (lector.Read())
            {
                caja.Items.Add(lector.GetInt16(0));
            }
            lector.Close();
            conexion.Close();
        }

        private void getFechas()
        {
            MySqlConnection conexion = Login.conectarBD();
            conexion.Open();
            MySqlCommand cmd = conexion.CreateCommand();
            cmd.CommandText = "select fechaLimite from detallePago, pagoServicio, servicio where idServicio = servicio_idServicio and detallePago_idDetalle = idDetalle and nombre = '" + servicioc.Text + "' group by fechaLimite;";
            MySqlDataReader lector = cmd.ExecuteReader();

            while (lector.Read())
            {
                DateTime fech = lector.GetDateTime(0);
                string fecha = fech.Year.ToString() + "-" + fech.Month.ToString() + "-" + fech.Day.ToString();
                fechas.Items.Add(fecha);
            }

            if (fechas.Items.Count == 0)
                MessageBox.Show("No hay pagos disponibles", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

            lector.Close();
            conexion.Close();
        }

        private void servicioc_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            getFechas();
        }

        private void fechas_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            MySqlConnection conexion = Login.conectarBD();
            conexion.Open();
            MySqlCommand cmd = conexion.CreateCommand();
            cmd.CommandText = "select monto from detallePago, pagoServicio, servicio where idServicio = servicio_idServicio "
                             + "and detallePago_idDetalle = idDetalle and nombre = \"" + servicioc.Text + "\" and fechaLimite = \"" + fechas.Text + "\";";
            MySqlDataReader lector = cmd.ExecuteReader();
            lector.Read();
            monto.Text = lector.GetInt32(0).ToString();

            lector.Close();
            conexion.Close();
        }

        private void deptos_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            MySqlConnection conexion = Login.conectarBD();
            MySqlCommand cmd = conexion.CreateCommand();
            conexion.Open();
            int depto = int.Parse(deptos.Text);
            cmd.CommandText = "select CONCAT(ii.nombre,\" \", ii.aPaterno,\" \",ii.aMaterno) from infoinquilino ii, inquilino i where ii.curp = i.infoInquilino_CURP and i.departamento_nDepartamento = " + depto + ";";
            MySqlDataReader lector = cmd.ExecuteReader();
            //ArrayList inquilinos = new ArrayList();
            while (lector.Read())
            {
                comboBox1.Items.Add(lector.GetString(0));
            }
            lector.Close();
            conexion.Close();

            comboBox1.SelectedIndex = 0;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
