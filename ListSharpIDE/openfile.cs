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
        public static int opened = 0;
        public openfile()
        {
            InitializeComponent();
            

            string[] args = Environment.GetCommandLineArgs();

            if (args.Length > 1)
            {
                string scriptfile = "";
                foreach (string s in args)
                {
                    scriptfile = s;
                }
                Form1.filename = scriptfile;

            }
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
            Form1.filename = filename;

            this.Close();

        }

        private void openfile_Load(object sender, EventArgs e)
        {
            opened++;
            filename = "";
        }

        private void openfile_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (Form1.filename == "")
                Application.Exit();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (Form1.filename!="" && opened==1)
            {

                this.Close();
            }
            
        }
    }
}
