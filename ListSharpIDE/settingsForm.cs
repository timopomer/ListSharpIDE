using ScintillaNET;
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
        public bool changed = false;
        private void settingsForm_Load(object sender, EventArgs e)
        {
            changeButtonStates(false);
            label19.Text = @"1
2
3
4
5
6
7
8
9";
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
            
            pictureBox1.BackColor = Settings.defaultColor;
            pictureBox2.BackColor = Settings.commentColor;
            pictureBox3.BackColor = Settings.commentLineColor;
            pictureBox4.BackColor = Settings.commentLineDocColor;
            pictureBox5.BackColor = Settings.numberColor;
            pictureBox6.BackColor = Settings.stringColor;
            pictureBox7.BackColor = Settings.characterColor;
            pictureBox8.BackColor = Settings.literalColor;
            pictureBox9.BackColor = Settings.brokenstringColor;
            pictureBox10.BackColor = Settings.operatorColor;
            pictureBox11.BackColor = Settings.launchargsColor;
            pictureBox12.BackColor = Settings.comparatorColor;
            pictureBox13.BackColor = Settings.commandColor;
            pictureBox14.BackColor = Settings.startingColor;
            pictureBox15.BackColor = Settings.backgroundColor;
            pictureBox16.BackColor = Settings.foregroundColor;
            pictureBox17.BackColor = Settings.caretColor;
            pictureBox18.BackColor = Settings.lineBgColor;
            pictureBox19.BackColor = Settings.lineColor;
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
            scintilla1.StyleClearAll();
            scintilla1.Styles[Style.Cpp.Default].ForeColor = Settings.defaultColor;
            scintilla1.Styles[Style.Cpp.Comment].ForeColor = Settings.commentColor;
            scintilla1.Styles[Style.Cpp.CommentLine].ForeColor = Settings.commentLineColor;
            scintilla1.Styles[Style.Cpp.CommentLineDoc].ForeColor = Settings.commentLineDocColor;
            scintilla1.Styles[Style.Cpp.Number].ForeColor = Settings.numberColor;
            scintilla1.Styles[Style.Cpp.String].ForeColor = Settings.stringColor;
            scintilla1.Styles[Style.Cpp.Character].ForeColor = Settings.characterColor;
            scintilla1.Styles[Style.Cpp.Verbatim].ForeColor = Settings.literalColor;
            scintilla1.Styles[Style.Cpp.StringEol].BackColor = Settings.brokenstringColor;
            scintilla1.Styles[Style.Cpp.Operator].ForeColor = Settings.operatorColor;
            scintilla1.Styles[Style.Cpp.Preprocessor].ForeColor = Settings.launchargsColor;
            scintilla1.Styles[Style.Default].BackColor = Settings.backgroundColor;
            scintilla1.Styles[Style.Default].ForeColor = Settings.foregroundColor;
            scintilla1.CaretForeColor = Settings.caretColor;

            //scintilla1.Styles[Style.Default].BackColor = Color.Pink;
            #region 1st keywords
            scintilla1.SetKeywords(0, Completion.connectorsString);
            scintilla1.Styles[Style.Cpp.Word].ForeColor = Settings.comparatorColor;
            #endregion

            #region 2nd keywords
            scintilla1.SetKeywords(1, Completion.commandString);
            scintilla1.Styles[Style.Cpp.Word2].ForeColor = Settings.commandColor;
            #endregion

            #region 3rd keywords
            scintilla1.SetKeywords(3, Completion.startingString);
            scintilla1.Styles[Style.Cpp.GlobalClass].ForeColor = Settings.startingColor;
            #endregion

            scintilla1.Lexer = Lexer.Cpp;

            label19.BackColor = Settings.lineBgColor;
            label19.ForeColor = Settings.lineColor;
        }

        private void colorChange(int index)
        {

            DialogResult result = colorDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                changedSettings();
                switch (index)
                {
                    case 0:
                        pictureBox1.BackColor = colorDialog1.Color;
                        Settings.defaultColor = colorDialog1.Color;
                        break;
                    case 1:
                        pictureBox2.BackColor = colorDialog1.Color;
                        Settings.commentColor = colorDialog1.Color;
                        break;
                    case 2:
                        pictureBox3.BackColor = colorDialog1.Color;
                        Settings.commentLineColor = colorDialog1.Color;
                        break;
                    case 3:
                        pictureBox4.BackColor = colorDialog1.Color;
                        Settings.commentLineDocColor = colorDialog1.Color;
                        break;
                    case 4:
                        pictureBox5.BackColor = colorDialog1.Color;
                        Settings.numberColor = colorDialog1.Color;
                        break;
                    case 5:
                        pictureBox6.BackColor = colorDialog1.Color;
                        Settings.stringColor = colorDialog1.Color;
                        break;
                    case 6:
                        pictureBox7.BackColor = colorDialog1.Color;
                        Settings.characterColor = colorDialog1.Color;
                        break;
                    case 7:
                        pictureBox8.BackColor = colorDialog1.Color;
                        Settings.literalColor = colorDialog1.Color;
                        break;
                    case 8:
                        pictureBox9.BackColor = colorDialog1.Color;
                        Settings.brokenstringColor = colorDialog1.Color;
                        break;
                    case 9:
                        pictureBox10.BackColor = colorDialog1.Color;
                        Settings.operatorColor = colorDialog1.Color;
                        break;
                    case 10:
                        pictureBox11.BackColor = colorDialog1.Color;
                        Settings.launchargsColor = colorDialog1.Color;
                        break;
                    case 11:
                        pictureBox12.BackColor = colorDialog1.Color;
                        Settings.comparatorColor = colorDialog1.Color;
                        break;
                    case 12:
                        pictureBox13.BackColor = colorDialog1.Color;
                        Settings.commandColor = colorDialog1.Color;
                        break;
                    case 13:
                        pictureBox14.BackColor = colorDialog1.Color;
                        Settings.startingColor = colorDialog1.Color;
                        break;
                    case 14:
                        pictureBox15.BackColor = colorDialog1.Color;
                        Settings.backgroundColor = colorDialog1.Color;
                        break;
                    case 15:
                        pictureBox16.BackColor = colorDialog1.Color;
                        Settings.foregroundColor = colorDialog1.Color;
                        break;
                    case 16:
                        pictureBox17.BackColor = colorDialog1.Color;
                        Settings.caretColor = colorDialog1.Color;
                        break;
                    case 17:
                        pictureBox18.BackColor = colorDialog1.Color;
                        Settings.lineBgColor = colorDialog1.Color;
                        break;
                    case 18:
                        pictureBox19.BackColor = colorDialog1.Color;
                        Settings.lineColor = colorDialog1.Color;
                        break;
                }

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
    }
}
