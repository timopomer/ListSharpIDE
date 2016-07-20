using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ListSharpIDE
{
    public partial class newfile : Form
    {
        public newfile()
        {
            InitializeComponent();
        }

        private void newfile_Paint(object sender, PaintEventArgs e)
        {

            //making a fake border for the textbox (aestetics only)
            System.Drawing.Rectangle rect = new Rectangle(textBox1.Location.X, textBox1.Location.Y, textBox1.ClientSize.Width, textBox1.ClientSize.Height);
            rect.Inflate(1, 1); // border thickness
            System.Drawing.Rectangle rect2 = new Rectangle(textBox2.Location.X, textBox2.Location.Y, textBox2.ClientSize.Width, textBox2.ClientSize.Height);
            rect2.Inflate(1, 1); // border thickness
            System.Windows.Forms.ControlPaint.DrawBorder(e.Graphics, rect, Color.FromArgb(57, 101, 142), ButtonBorderStyle.Solid);
            System.Windows.Forms.ControlPaint.DrawBorder(e.Graphics, rect2, Color.FromArgb(57, 101, 142), ButtonBorderStyle.Solid);

        }

        public static String pathname = "";
        private void button1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                
                pathname = folderBrowserDialog1.SelectedPath;
                pathname += "\\";
                textBox1.Text = pathname;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (pathname == "")
            {
                MessageBox.Show("You have to choose a path");
                return;
            }
            if (textBox2.Text == "")
            {
                MessageBox.Show("You have to choose a file name");
                return;
            }
            string fullpath = pathname + textBox2.Text + ".ls";
            File.WriteAllText(fullpath,"");
            workingEnviroment.activeFilePath = fullpath;
            this.Close();
        }
    }
}
