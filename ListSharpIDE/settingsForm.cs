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
    public partial class settingsForm : Form
    {
        public settingsForm()
        {
            InitializeComponent();
        }
        Button[] tabButtons;
        private void settingsForm_Load(object sender, EventArgs e)
        {

            tabButtons = new Button[] { tab1_button, tab2_button, tab3_button, tab4_button, tab5_button, tab6_button };
            tabChange(0);
            
        }



        private void settingsForm_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void tabChange(int index)
        {
            tabButtons.ToList().ForEach(n => n.BackColor = Color.FromArgb(44, 62, 80));
            tabButtons[index].BackColor = Color.FromArgb(36, 63, 86);
            tabControl1.SelectedIndex = index;
        }

        private void tab1_button_Click(object sender, EventArgs e) => tabChange(0);
        private void tab2_button_Click(object sender, EventArgs e) => tabChange(1);
        private void tab3_button_Click(object sender, EventArgs e) => tabChange(2);
        private void tab4_button_Click(object sender, EventArgs e) => tabChange(3);
        private void tab5_button_Click(object sender, EventArgs e) => tabChange(4);
        private void tab6_button_Click(object sender, EventArgs e) => tabChange(5);


    }
}
