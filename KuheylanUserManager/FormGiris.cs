using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UserManager
{
    public partial class FormGiris : Form
    {
        string pass = "6108";
        public FormGiris()
        {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (pass == textBox1.Text)
            {
                return;
            }
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void FormGiris_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (pass == textBox1.Text)
            {
                return;
            }
            Application.Exit();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return || e.KeyCode == Keys.Space)
            {
                button1_Click(null, null);
            }
        }
    }
}
