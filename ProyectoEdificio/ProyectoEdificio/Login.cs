using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;

namespace ProyectoEdificio
{
    public partial class Login : Form
    {
        public static string curp;

        public Login()
        {
            InitializeComponent();
        }

        public string getCurp() {
            return curp;
        }

        private void BtnEntrar_Click(object sender, EventArgs e)
        {
            LblInfo.Text = "";
            try {
                MySqlConnection con = conectarBD();
                con.Open();
                MySqlCommand cmd = con.CreateCommand();
                cmd.CommandText = "select * from infoInquilino where curp='" + TxtUsuario.Text.Trim() + "' and pass='"+ TxtPass.Text.Trim() + "'";
                MySqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows == false)
                {
                    LblInfo.Text = "No exite el usuario, VERIFICAR los datos.";
                    con.Close();
                    return;
                }
                else {
                    dr.Read();
                    int rol = Convert.ToInt32(dr["rol_idrol"]);
                    string nombre = dr["nombre"].ToString();
                    dr.Close();
                    con.Close();
                    LblInfo.Text = "BIENVENIDO " + nombre;
                    curp = TxtUsuario.Text;
                    if (rol == 1) {
                        InicioAdmin ia = new InicioAdmin();
                        this.Hide();
                        ia.ShowDialog();
                    }
                    if (rol == 2)
                    {
                        Intro ia = new Intro();
                        this.Hide();
                        ia.ShowDialog();
                    }

                }
                
            } catch(MySqlException ex){
                LblInfo.Text = "Se produjo un problema por : " + ex.Message;
            }
        }

        public static MySqlConnection conectarBD()
        {
            MySqlConnection conexion = new MySqlConnection("server=localhost;database=edificio;UID=root;pwd=root;");
            return conexion;
        }
    }
}
