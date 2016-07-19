using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ListSharpIDE
{
    public partial class openfile : Form
    {

        public openfile()
        {    
            InitializeComponent();
        }

        public static String filename = "";
        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                filename = openFileDialog1.FileName;
                textBox1.Text = filename;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (filename == "")
            {
                MessageBox.Show("You have to choose a file");
                return;
            }
            workingEnviroment.activeFilePath = filename;
            this.Close();
        }





    }
}
