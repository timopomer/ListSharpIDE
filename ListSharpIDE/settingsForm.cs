using ScintillaNET;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
        public bool changed = false;
        PictureBox[] indexedPixtures;


        private void settingsForm_Load(object sender, EventArgs e)
        {
            label23.Text = Directory.GetCreationTime(Initialize.wikiPath).ToString();
            indexedPixtures = new PictureBox[] {    pictureBox1,
                                                    pictureBox2,
                                                    pictureBox3,
                                                    pictureBox4,
                                                    pictureBox5,
                                                    pictureBox6,
                                                    pictureBox7,
                                                    pictureBox8,
                                                    pictureBox9,
                                                    pictureBox10,
                                                    pictureBox11,
                                                    pictureBox12,
                                                    pictureBox13,
                                                    pictureBox14,
                                                    pictureBox15,
                                                    pictureBox16,
                                                    pictureBox17,
                                                    pictureBox18,
                                                    pictureBox19 };
            changeButtonStates(false);
            label19.Text = @"1
2
3
4
5
6
7
8
9
10
11
12
13
14
15
16
17
18
19
20";
            scintilla1.Text = @"#DownloadMaxTries: 3
//comment
/*commented out*/
ROWS a = {""arr1"",""arr2""} + ""string""
ROWS a = EXTRACT COLLUM[1] FROM a SPLIT BY [""""]
[IF a LENGTH ISOVER 1]
DEBG = a";
            scintilla1.ReadOnly = true;
            tabButtons = new Button[] { tab1_button, tab2_button, tab3_button, tab4_button, tab5_button, tab6_button };
            tabChange(0);
            scintilla1.Margins[1].Width = 0;

            for (int i = 0; i<indexedPixtures.Length; i++)
            indexedPixtures[i].BackColor = Settings.Highlighting[Settings.colorProperties[i]];

        }



        private void settingsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            timer1.Enabled = false;
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (scintilla1.Zoom > 6)
                scintilla1.Zoom = 6;
            if (scintilla1.Zoom < -6)
                scintilla1.Zoom = -6;
            label19.Font = new Font(scintilla1.Styles[Style.Default].Font, scintilla1.Styles[Style.Default].Size + scintilla1.Zoom);

            scintilla1.StyleClearAll();
            scintilla1.Styles[Style.Cpp.Default].ForeColor = Settings.Highlighting["defaultColor"];
            scintilla1.Styles[Style.Cpp.Comment].ForeColor = Settings.Highlighting["commentColor"];
            scintilla1.Styles[Style.Cpp.CommentLine].ForeColor = Settings.Highlighting["commentLineColor"];
            scintilla1.Styles[Style.Cpp.CommentLineDoc].ForeColor = Settings.Highlighting["commentLineDocColor"];
            scintilla1.Styles[Style.Cpp.Number].ForeColor = Settings.Highlighting["numberColor"];
            scintilla1.Styles[Style.Cpp.String].ForeColor = Settings.Highlighting["stringColor"];
            scintilla1.Styles[Style.Cpp.Character].ForeColor = Settings.Highlighting["characterColor"];
            scintilla1.Styles[Style.Cpp.Verbatim].ForeColor = Settings.Highlighting["literalColor"];
            scintilla1.Styles[Style.Cpp.StringEol].BackColor = Settings.Highlighting["brokenstringColor"];
            scintilla1.Styles[Style.Cpp.Operator].ForeColor = Settings.Highlighting["operatorColor"];
            scintilla1.Styles[Style.Cpp.Preprocessor].ForeColor = Settings.Highlighting["launchargsColor"];
            scintilla1.Styles[Style.Default].BackColor = Settings.Highlighting["backgroundColor"];
            scintilla1.Styles[Style.Default].ForeColor = Settings.Highlighting["foregroundColor"];
            scintilla1.CaretForeColor = Settings.Highlighting["caretColor"];

            #region 1st keywords
            scintilla1.SetKeywords(0, Completion.connectorsString);
            scintilla1.Styles[Style.Cpp.Word].ForeColor = Settings.Highlighting["comparatorColor"];
            #endregion

            #region 2nd keywords
            scintilla1.SetKeywords(1, Completion.commandString);
            scintilla1.Styles[Style.Cpp.Word2].ForeColor = Settings.Highlighting["commandColor"];
            #endregion

            #region 3rd keywords
            scintilla1.SetKeywords(3, Completion.startingString);
            scintilla1.Styles[Style.Cpp.GlobalClass].ForeColor = Settings.Highlighting["startingColor"];
            #endregion

            scintilla1.Lexer = Lexer.Cpp;

            label19.BackColor = Settings.Highlighting["lineBgColor"];
            label19.ForeColor = Settings.Highlighting["lineColor"];
        }

        private void colorChange(int index)
        {

            DialogResult result = colorDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                changedSettings();

                        indexedPixtures[index].BackColor = colorDialog1.Color;
                        Settings.Highlighting[Settings.colorProperties[index]] = colorDialog1.Color;

            }
        }

        private void pictureBox1_Click(object sender, EventArgs e) => colorChange(0);
        private void pictureBox2_Click(object sender, EventArgs e) => colorChange(1);
        private void pictureBox3_Click(object sender, EventArgs e) => colorChange(2);
        private void pictureBox4_Click(object sender, EventArgs e) => colorChange(3);
        private void pictureBox5_Click(object sender, EventArgs e) => colorChange(4);
        private void pictureBox6_Click(object sender, EventArgs e) => colorChange(5);
        private void pictureBox7_Click(object sender, EventArgs e) => colorChange(6);
        private void pictureBox8_Click(object sender, EventArgs e) => colorChange(7);
        private void pictureBox9_Click(object sender, EventArgs e) => colorChange(8);
        private void pictureBox10_Click(object sender, EventArgs e) => colorChange(9);
        private void pictureBox11_Click(object sender, EventArgs e) => colorChange(10);
        private void pictureBox12_Click(object sender, EventArgs e) => colorChange(11);
        private void pictureBox13_Click(object sender, EventArgs e) => colorChange(12);
        private void pictureBox14_Click(object sender, EventArgs e) => colorChange(13);
        private void pictureBox15_Click(object sender, EventArgs e) => colorChange(14);
        private void pictureBox16_Click(object sender, EventArgs e) => colorChange(15);
        private void pictureBox17_Click(object sender, EventArgs e) => colorChange(16);
        private void pictureBox18_Click(object sender, EventArgs e) => colorChange(17);
        private void pictureBox19_Click(object sender, EventArgs e) => colorChange(18);

        private void button1_Click(object sender, EventArgs e) => saveProcess();

        public void saveProcess()
        {
            changed = false;
            changeButtonStates(false);
            Settings.saveSettings();
        }
        public void changedSettings()
        {
            changed = true;
            changeButtonStates(true);
        }
        public void changeButtonStates(bool state)
        {
            new List<Button>() { button1 }.ForEach(n => n.Enabled = state);
        }

        private void button2_Click(object sender, EventArgs e)
        {

            Color Guiblue = Color.FromArgb(44, 62, 80);
            pictureBox20.BackColor = Guiblue;
            pictureBox21.BackColor = Guiblue;
            pictureBox22.BackColor = Guiblue;
            pictureBox23.BackColor = Guiblue;
            pictureBox24.BackColor = Guiblue;
            button2.Enabled = false;
            System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();
            t.Interval = 1000;
            PictureBox[] tempPic = new PictureBox[] { pictureBox20, pictureBox21, pictureBox22, pictureBox23, pictureBox24 };
            int index = 0;
            
            t.Tick += (j, k) => {
                tempPic[index].BackColor = Color.Teal;
                index++;
                if (index == 5)
                    t.Enabled = false;
            };
            t.Enabled = true;
            Thread tr = new Thread(Initialize.reDownloadWiki);
            tr.Start();
            button2.Enabled = true;
            label23.Text = Directory.GetCreationTime(Initialize.wikiPath).ToString();
        }
    }
}
