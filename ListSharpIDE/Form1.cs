using ScintillaNET;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
    public partial class Form1 : Form
    {
        Color listSharpC = Color.FromArgb(1, 0, 125, 125);
        Boolean saved = true;
        Regex _regex;
        Match match;
        public static string filename = "";
        public static string allVars; 
        string oldfilename;
        public static Dictionary<string, string> commandDictionary = new Dictionary<string, string>();
        [DllImport("User32.dll")]
        static extern IntPtr GetDC(IntPtr hwnd);

        [DllImport("User32.dll")]
        static extern int ReleaseDC(IntPtr hwnd, IntPtr dc);

        public Form1()
        {
            InitializeComponent();

        }

        public void loadFile()
        {
            scintilla1.Text = File.ReadAllText(filename);
            this.Text = "ListSharp IDE:" + filename;
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            allVars = returnAllCommandsString();
            commandDictionary = returnAllCommandsDictionary();
            scintilla1.Height = 700 - 220;
            label3.Height = 700 - 220;
            richTextBox1.Width = 1000;

            scintilla1.Width = 947;
            richTextBox1.Text = "interesting";
            richTextBox1.Location =new Point(0,40 + 700 - 220);
            pictureBox4.Location = new Point(1000-100-27, 7);
            
                /*
                string contents;
                using (var wc = new System.Net.WebClient())
                    contents = wc.DownloadString("https://raw.githubusercontent.com/timopomer/ListSharp/master/ListSharp/Testing/test_features.ls");
                scintilla1.Text = contents;

                */
                scintilla1.Margins[1].Width = 0;


            scintilla1.Styles[Style.Default].BackColor = listSharpC;
            scintilla1.Styles[Style.Default].ForeColor = Color.FromArgb(236, 240, 241);
            scintilla1.CaretForeColor = Color.FromArgb(1, 247, 247, 247);
            scintilla1.StyleClearAll();



        }

        private void scintilla1_TextChanged(object sender, EventArgs e)
        {
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

            if (oldfilename != filename)
                loadFile();
            oldfilename = filename;

  


            scintilla1.Lexer = Lexer.Null;
            scintilla1.Styles[Style.Cpp.Default].ForeColor = Color.FromArgb(204, 204, 255);
            scintilla1.Styles[Style.Cpp.Comment].ForeColor = Color.FromArgb(5, 30, 86);
            scintilla1.Styles[Style.Cpp.CommentLine].ForeColor = Color.FromArgb(5, 30, 86);
            scintilla1.Styles[Style.Cpp.CommentLineDoc].ForeColor = Color.FromArgb(5, 30, 86);
            scintilla1.Styles[Style.Cpp.Number].ForeColor = Color.Yellow;




            scintilla1.Styles[Style.Cpp.String].ForeColor = Color.FromArgb(241, 196, 15);
            scintilla1.Styles[Style.Cpp.Character].ForeColor = Color.FromArgb(241, 196, 15);
            scintilla1.Styles[Style.Cpp.Verbatim].ForeColor = Color.FromArgb(241, 196, 15);
            scintilla1.Styles[Style.Cpp.StringEol].BackColor = Color.Black;
            scintilla1.Styles[Style.Cpp.Operator].ForeColor = Color.FromArgb(102, 255, 51);
            scintilla1.Styles[Style.Cpp.Preprocessor].ForeColor = Color.Maroon;
            scintilla1.SetKeywords(0, "STRG ROWS TO SPLIT BY FROM IN WITH SHOW OUTP AS");
            scintilla1.SetKeywords(1, allVars);

            scintilla1.Styles[Style.Cpp.Word].ForeColor = Color.FromArgb(0, 61, 153);
            scintilla1.Styles[Style.Cpp.Word2].ForeColor = Color.FromArgb(153, 0, 61);

            scintilla1.Lexer = Lexer.Cpp;

            // Set the keywords



            string linesNumbs = "";
            for (int i = 0; i<100; i++)
                linesNumbs+=(scintilla1.FirstVisibleLine+i)+Environment.NewLine;

            label3.Text = linesNumbs;
        }

        private void scintilla1_CharAdded(object sender, CharAddedEventArgs e)
        {
            saved = false;
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
            File.WriteAllText(filename,scintilla1.Text);
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
            System.Diagnostics.Process.Start(filename);

        }

        private void scintilla1_AutoCCancelled(object sender, EventArgs e)
        {


        }

        public string returnAllCommandsString()
        {
            string r = "";
            WebClient wc = new WebClient();
            string fullhtml = wc.DownloadString("https://github.com/timopomer/ListSharp/wiki/List-of-functions-by-type");

            string[] allLines = fullhtml.Split('\n');
            foreach (string s in allLines)
            {
                if (s.Contains("<td><a href"))
                {
                    _regex = new Regex(@">([^>]*)</a></td>");
                    match = _regex.Match(s);
                    string invar = match.Groups[1].Value.Trim();

                    if (r != "")
                        r += " ";

                    r += invar;
                }
            }

            return r;
            
        }

        public Dictionary<string,string> returnAllCommandsDictionary()
        {
            Dictionary<string,string> temp = new Dictionary<string,string>();

            WebClient wc = new WebClient();
            

            string[] allCommands = returnAllCommandsString().Split(' ');
            foreach (string s in allCommands)
            {
                string fullhtml = wc.DownloadString("https://github.com/timopomer/ListSharp/wiki/" + s);


                _regex = new Regex(@"<code>([^>]*)</code>");
                    match = _regex.Match(fullhtml);
                    string invar = match.Groups[1].Value.Trim();
                    temp.Add(s, invar);

                
            }

            return temp;

        }





    }
}
