using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ListSharpIDE
{
    public partial class showtext : Form
    {
        public showtext()
        {
            InitializeComponent();
        }

        private void showtext_Load(object sender, EventArgs e)
        {
            FadeOut(this, 80);
            
            //this.Close();

                
        }

        private async void FadeOut(Form o, int interval = 80)
        {
            //Object is fully visible. Fade it out
            while (o.Opacity > 0.0)
            {
                await Task.Delay(interval);
                o.Opacity -= 0.05;
            }
            o.Opacity = 0; //make fully invisible    
            this.Close();
        }
    }
}
