using ScintillaNET;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ListSharpIDE
{
    public partial class IDEform : Form
    {

        Boolean saved = true;

        [DllImport("User32.dll")]
        static extern IntPtr GetDC(IntPtr hwnd);

        [DllImport("User32.dll")]
        static extern int ReleaseDC(IntPtr hwnd, IntPtr dc);

        public IDEform()
        {
            InitializeComponent();
        }

        public void loadFile()
        {
            scintilla1.Text = File.ReadAllText(workingEnviroment.activeFilePath);
            this.Text = "ListSharp IDE:" + workingEnviroment.activeFilePath;
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            
            scintilla1.Height = 700 - 220;
            label3.Height = 700 - 220;
            richTextBox1.Width = 1000;
            scintilla1.Width = 947;
            richTextBox1.Text = "interesting";
            richTextBox1.Location = new Point(0,40 + 700 - 220);
            pictureBox4.Location = new Point(1000-100-27, 7);
            scintilla1.Margins[1].Width = 0;
            
            scintilla1.StyleClearAll();
            loadFile();
            

        }

        private void scintilla1_TextChanged(object sender, EventArgs e)
        {
            saved = false;
            updateInfo();
        }

        private void scintilla1_KeyDown(object sender, KeyEventArgs e)
        {
            updateInfo();
        }

        private void scintilla1_Click(object sender, EventArgs e)
        {
            updateInfo();
        }

        public void updateInfo()
        {
            return;
            /*
            string toshow = "";
            string currentLine = scintilla1.Lines[scintilla1.CurrentLine].Text;
            if (!currentLine.Contains('='))
            {
                richTextBox1.Text = "";
                return;
            }
            string side1 = currentLine.Substring(0,currentLine.IndexOf('=')+1);
            string side2 = currentLine.Substring(currentLine.IndexOf('='));


            foreach (KeyValuePair<string, string> pair in commandDictionary)
            {
                if (side2.Contains(pair.Key))
                    toshow = pair.Value;
            }

            if (toshow == "")
            {
                foreach (KeyValuePair<string, string> pair in commandDictionary)
                {
                    if (side1.Contains(pair.Key))
                        toshow = pair.Value;
                }
            }
            richTextBox1.Text = toshow;
            */
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            scintilla1.Width = this.Size.Width;
            scintilla1.Height = this.Size.Height - 220;
            label3.Height = this.Size.Height - 220;
            richTextBox1.Location = new Point(0, 40 + this.Size.Height - 220);
            richTextBox1.Width = this.Size.Width;
            pictureBox4.Location = new Point(this.Size.Width - 100 - 27, 7);
        }

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


            label3.BackColor = Settings.lineBgColor;
            label3.ForeColor = Settings.lineColor;

            updateLineNums();
        }
        public void updateLineNums()
        {
            string linesNumbs = "";
            for (int i = 0; i < 100; i++)
                linesNumbs += (scintilla1.FirstVisibleLine + i) + Environment.NewLine;

            label3.Text = linesNumbs;
        }
        private void scintilla1_CharAdded(object sender, CharAddedEventArgs e)
        {
            
        }



        private void scintilla1_AutoCCompleted(object sender, AutoCSelectionEventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            var myForm = new openfile();
            myForm.Show();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            saved = true;
            File.WriteAllText(workingEnviroment.activeFilePath,scintilla1.Text);
            var showr = new showtext();
            showr.Show();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            if (!saved)
            {
                MessageBox.Show("You have to save the script to run it");
                return;
            }
            Process.Start(workingEnviroment.activeFilePath);

        }

        private void scintilla1_AutoCCancelled(object sender, EventArgs e)
        {


        }

        private void IDEform_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
