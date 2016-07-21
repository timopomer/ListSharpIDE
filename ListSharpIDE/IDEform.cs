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
            /*
            scintilla1.Height = 700 - 220;
            label3.Height = 700 - 220;
            richTextBox1.Width = 1000;
            richTextBox2.Width = 1000;
            scintilla1.Width = 947;

            richTextBox1.Location = new Point(0, 40 + 700 - 220);
            richTextBox2.Location = new Point(0, 95 + 700 - 220);
            pictureBox4.Location = new Point(1000-100-27, 7);
            */
            updateElementLocations();
            scintilla1.Margins[1].Width = 0;
            
            scintilla1.StyleClearAll();
            loadFile();
            

        }
        private void Form1_Resize(object sender, EventArgs e)
        {
            updateElementLocations();
        }
        public void updateElementLocations()
        {
            scintilla1.Width = this.Size.Width;
            scintilla1.Height = this.Size.Height - 210;
            label3.Height = this.Size.Height - 210;
            richTextBox1.Location = new Point(0, 50 + this.Size.Height - 220);
            richTextBox1.Width = this.Size.Width;
            label1.Location = new Point(0, 80 + this.Size.Height - 220);
            richTextBox2.Location = new Point(0, 104 + this.Size.Height - 220);
            richTextBox2.Width = this.Size.Width;
            pictureBox4.Location = new Point(this.Size.Width - 100 - 27, 7);
        }
        private void scintilla1_TextChanged(object sender, EventArgs e)
        {
            saved = false;
            updateWorkingVariables();
            updateInfo();
        }

        private void scintilla1_KeyDown(object sender, KeyEventArgs e)
        {
            processWiki();

        }

        private void scintilla1_Click(object sender, EventArgs e) => processWiki();


        private void scintilla1_CharAdded(object sender, CharAddedEventArgs e)
        {

        }

        public void updateWorkingVariables()
        {
            workingEnviroment.numbVars = scintilla1.Lines.Where(n => n.Text.StartsWith("NUMB")).Select(m => new Regex("NUMB (.*?)=").Match(m.Text).Groups[1].Value.Trim()).Where(k => k != "").Distinct().ToArray();
            workingEnviroment.strgVars = scintilla1.Lines.Where(n => n.Text.StartsWith("STRG")).Select(m => new Regex("STRG (.*?)=").Match(m.Text).Groups[1].Value.Trim()).Where(k => k != "").Distinct().ToArray();
            workingEnviroment.rowsVars = scintilla1.Lines.Where(n => n.Text.StartsWith("ROWS")).Select(m => new Regex("ROWS (.*?)=").Match(m.Text).Groups[1].Value.Trim()).Where(k => k != "").Distinct().ToArray();
        }
        public void processWiki()
        {
            string currentLine = scintilla1.Lines[scintilla1.CurrentLine].Text;
            foreach (KeyValuePair<String, Tuple<String, String[]>> kvp in Completion.wikiDictionary["specialFunctions"])
            {
                if (isRightKey(currentLine, kvp))
                    return;
            }
            foreach (KeyValuePair<String, Tuple<String, String[]>> kvp in Completion.wikiDictionary["launchargs"])
            {
                if (isRightKey(currentLine, kvp))
                    return;
            }
            if (currentLine.StartsWith("["))
            foreach (KeyValuePair<String, Tuple<String, String[]>> kvp in Completion.wikiDictionary["conditionals"])
            {
                if (isRightKey(currentLine.Substring(1), kvp))
                    return;
            }

            if (!currentLine.Contains("="))
                return;

            currentLine = currentLine.Split('=')[1].Trim();
            foreach (KeyValuePair<String, Tuple<String, String[]>> kvp in Completion.wikiDictionary["combinedFunctions"])
            {
                if (isRightKey(currentLine, kvp))
                    return;
            }
            foreach (KeyValuePair<String, Tuple<String, String[]>> kvp in Completion.wikiDictionary["strgFunctions"])
            {
                if (isRightKey(currentLine, kvp))
                    return;
            }
            foreach (KeyValuePair<String, Tuple<String, String[]>> kvp in Completion.wikiDictionary["rowsFunctions"])
            {
                if (isRightKey(currentLine, kvp))
                    return;
            }

        }
        public bool isRightKey(string line,KeyValuePair<String, Tuple<String, String[]>> kvp)
        {
            if (line.StartsWith(kvp.Key))
            {
                showWiki(kvp);
                return true;
            }
            return false;
        }
        public void showWiki(KeyValuePair<String, Tuple<String, String[]>> kvp)
        {
            richTextBox1.Text = kvp.Key;
            richTextBox2.Text = kvp.Value.Item1;
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



        private void timer1_Tick(object sender, EventArgs e)
        {
            if (scintilla1.Zoom > 6)
                scintilla1.Zoom = 6;
            if (scintilla1.Zoom < -6)
                scintilla1.Zoom = -6;
            label3.Font = new Font(scintilla1.Styles[Style.Default].Font, scintilla1.Styles[Style.Default].Size+ scintilla1.Zoom);

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

            label3.BackColor = Settings.Highlighting["lineBgColor"];
            label3.ForeColor = Settings.Highlighting["lineColor"];
            updateLineNums();
        }
        public void updateLineNums()
        {
            label3.Text = String.Join(Environment.NewLine, Enumerable.Range(scintilla1.FirstVisibleLine, scintilla1.FirstVisibleLine + 500));
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
