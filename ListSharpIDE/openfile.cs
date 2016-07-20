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

        private void openfile_Paint(object sender, PaintEventArgs e)
        {
            //making a fake border for the textbox (aestetics only)
            System.Drawing.Rectangle rect = new Rectangle(textBox1.Location.X, textBox1.Location.Y, textBox1.ClientSize.Width, textBox1.ClientSize.Height);

            rect.Inflate(1, 1); // border thickness
            System.Windows.Forms.ControlPaint.DrawBorder(e.Graphics, rect, Color.FromArgb(57, 101, 142), ButtonBorderStyle.Solid);

        }
    
    }
}
