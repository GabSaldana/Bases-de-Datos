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
    public partial class AdminUsers : Form
    {
        public AdminUsers()
        {
            InitializeComponent();
        }

        private void buscar_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(Ndepto.Text))
                MessageBox.Show("Un valor es necesario", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                resultUsers buscar = new resultUsers(Convert.ToInt32(Ndepto.Text));
                this.Hide();
                buscar.ShowDialog();
            }
        }

        private void AdminUsers_Load(object sender, EventArgs e)
        {
            getDeptos(Ndepto);
        }

        private void getDeptos(ComboBox caja)
        {
            MySqlConnection conexion = Login.conectarBD();
            conexion.Open();
            MySqlCommand cmd = conexion.CreateCommand();
            cmd.CommandText = "select nDepartamento from departamento";
            MySqlDataReader lector = cmd.ExecuteReader();

            while (lector.Read())
            {
                Ndepto.Items.Add(lector.GetInt16(0));
            }

            lector.Close();
            conexion.Close();
        }

        private bool validacion()
        {
            if (String.IsNullOrEmpty(Ndepto.Text) || String.IsNullOrEmpty(nombret.Text) || String.IsNullOrEmpty(aPaternot.Text) || String.IsNullOrEmpty(aMaternot.Text) || String.IsNullOrEmpty(curpt.Text))
            {
                MessageBox.Show("Un valor es necesario", "Dato faltante", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (curpt.Text.Length != 18)
            {
                MessageBox.Show("CURP no válido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (pwd1.Text.CompareTo(pwd2.Text) != 0 || String.IsNullOrEmpty(pwd1.Text) || String.IsNullOrEmpty(pwd2.Text))
            {
                MessageBox.Show("Contraseña incorrecta", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            try
            {
                int num = Convert.ToInt32(Ndepto.Text);
            }
            catch (FormatException)
            {
                MessageBox.Show("Dato no válido", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void guardar_Click(object sender, EventArgs e)
        {

            MySqlConnection conexion = Login.conectarBD();
            conexion.Open();
            MySqlCommand cmd = conexion.CreateCommand();
            if (!validacion())
                return;
            int n = 0;
            cmd.CommandText = "insert into infoinquilino values('" + curpt.Text + "','" + nombret.Text + "','" + aPaternot.Text + "','" + aMaternot.Text + "','" + pwd1.Text + "',2)";
            try
            {
                cmd.ExecuteNonQuery();
                string fecha = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString();
                cmd.CommandText = "insert into inquilino(fechaAlta, infoInquilino_curp, departamento_nDepartamento) values('" + fecha + "','" + curpt.Text + "'," + Ndepto.Text + ")";
                n = cmd.ExecuteNonQuery();
            }

            catch (MySqlException)
            {
                cmd.CommandText = "select fechaBaja from inquilino where infoinquilino_curp = '" + curpt.Text + "'";
                MySqlDataReader lector = cmd.ExecuteReader();
                lector.Read();
                if (!lector.IsDBNull(0))
                {
                    lector.Close();
                    cmd.CommandText = "update inquilino set fechaBaja = NULL where infoinquilino_curp = '" + curpt.Text + "'";
                    n = cmd.ExecuteNonQuery();
                }

                else
                {
                    MessageBox.Show("Inquilino ya registrado", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


            }

            if (n == 1)
                MessageBox.Show("Inquilino registrado exitosamente", "Registrado", MessageBoxButtons.OK);
            conexion.Close();
        }

        private void eliminar_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(curpt.Text) || curpt.Text.Length != 18)
            {
                MessageBox.Show("CURP no válido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MySqlConnection conexion = Login.conectarBD();
            conexion.Open();
            MySqlCommand cmd = conexion.CreateCommand();
            cmd.CommandText = "select infoinquilino_curp from inquilino where infoinquilino_curp ='" + curpt.Text + "'";
            MySqlDataReader lector = cmd.ExecuteReader();

            if (!lector.Read())
            {
                MessageBox.Show("Inquilino no registrado", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string fecha = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString();

            lector.Close();
            cmd.CommandText = "update inquilino set fechaBaja = '" + fecha + "' where infoinquilino_curp = '" + curpt.Text + "'";
            if (cmd.ExecuteNonQuery() == 1)
                MessageBox.Show("Inquilino dado de baja", "Éxito", MessageBoxButtons.OK);

            conexion.Close();
        }

        private void actualizar_Click(object sender, EventArgs e)
        {
            InicioAdmin ia = new InicioAdmin();
            this.Hide();
            ia.ShowDialog();
        }

    }
}