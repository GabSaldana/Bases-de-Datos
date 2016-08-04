using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ProyectoEdificio
{
    public partial class InicioAdmin : Form
    {
        public InicioAdmin()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AdminUsers au = new AdminUsers();
            this.Hide();
            au.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AltaPago ap = new AltaPago();
            this.Hide();
            ap.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            RegistroPagos rp = new RegistroPagos();
            this.Hide();
            rp.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            principalQuejas q = new principalQuejas();
            this.Hide();
            q.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
