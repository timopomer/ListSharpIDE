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
    public partial class welcomeForm : Form
    {
        public welcomeForm()
        {      
            InitializeComponent();
            
            Completion.createDictionaries();
            Initialize.readWiki();
            launchedDirectly();
        }
        IDEform IDEform = new IDEform();
        private void welcomeForm_Load(object sender, EventArgs e)
        {
            if (workingEnviroment.activeFilePath!="")
            {
                this.Opacity = 0;
                showIDE();
            }
        }

        public bool launchedDirectly()
        {
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length <= 1)
                return false;

            workingEnviroment.activeFilePath = args.Last();
            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            openfile openfile = new openfile();
            openfile.ShowDialog();
            if (workingEnviroment.activeFilePath != "")
                showIDE();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            settingsForm settingsForm = new settingsForm();
            settingsForm.ShowDialog();
        }
        public void showIDE()
        {
            this.Hide();
            IDEform.Show();
        }


    }
}
