using System;
using System.Drawing;
using System.Windows.Forms;

namespace Kuheylan
{
    public partial class FormAbout : Form
    {
        public FormAbout()
        {
            InitializeComponent();
            help1.SetHelpString(pictureBox1, "BiLAL  ARIKAN");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            
        }
    }
}
