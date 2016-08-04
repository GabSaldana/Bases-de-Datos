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
    public partial class principalQuejas : Form
    {
        public principalQuejas()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Quejas q = new Quejas();
            this.Hide();
            q.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LeerQuejas q1 = new LeerQuejas();
            this.Hide();
            q1.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            InicioAdmin ia = new InicioAdmin();
            this.Hide();
            ia.ShowDialog();
        }
    }
}
