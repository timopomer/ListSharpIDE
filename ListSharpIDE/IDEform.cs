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

            updateElementLocations();
            scintilla1.Margins[1].Width = 0;
            
            scintilla1.StyleClearAll();
            scintilla1.AutoCSeparator = ',';
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
            


        }

        private void scintilla1_KeyDown(object sender, KeyEventArgs e)
        {


            if (e.KeyCode == (Keys)Settings.Autocomplete["activationKey"])
            {
                workingEnviroment.recursivelyInserting = false;
                showAutoComplete();
            }
                
        }

        private void scintilla1_Click(object sender, EventArgs e)
        {

        }

        private void scintilla1_CharAdded(object sender, CharAddedEventArgs e)
        {
            if ((bool)Settings.Autocomplete["onCharAdded"])
            if (!workingEnviroment.recursivelyInserting)
            {
                string currentLine = scintilla1.Lines[scintilla1.CurrentLine].Text;
                currentLine = Regex.Replace(currentLine, "\r\n", "");
                if (currentLine!="")
                showAutoComplete();
            }
                
        }

        public void updateWorkingVariables()
        {
            workingEnviroment.numbVars = scintilla1.Lines.Where(n => n.Text.StartsWith("NUMB")).Select(m => new Regex("NUMB (.*?)=").Match(m.Text).Groups[1].Value.Trim()).Where(k => k != "" && !k.StartsWith("(")).Distinct().ToArray();
            workingEnviroment.strgVars = scintilla1.Lines.Where(n => n.Text.StartsWith("STRG")).Select(m => new Regex("STRG (.*?)=").Match(m.Text).Groups[1].Value.Trim()).Where(k => k != "" && !k.StartsWith("(")).Distinct().ToArray();
            workingEnviroment.rowsVars = scintilla1.Lines.Where(n => n.Text.StartsWith("ROWS")).Select(m => new Regex("ROWS (.*?)=").Match(m.Text).Groups[1].Value.Trim()).Where(k => k != "" && !k.StartsWith("(")).Distinct().ToArray();
        }

        public void processWiki()
        {

            KeyValuePair<String, Tuple<String, String[]>> kvp = getRightEntry();
            if (kvp.Key == "empty")
                return;
            showWiki(kvp);
        }

        public void showWiki(KeyValuePair<String, Tuple<String, String[]>> kvp)
        {
            if (richTextBox1.Text != kvp.Key)
                richTextBox1.Text = kvp.Key;

            if (richTextBox2.Text != kvp.Value.Item1)
                richTextBox2.Text = kvp.Value.Item1;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //label2.Text = workingEnviroment.recursivelyInserting.ToString();
            label2.Text = scintilla1.Lines[scintilla1.CurrentLine].Text;
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

        private void timer2_Tick(object sender, EventArgs e)
        {
            //ToolTip toolTip1 = new ToolTip();

            //toolTip1.Show(scintilla1.Lines[scintilla1.CurrentLine].Text, scintilla1); // Message of the toolTip and to what control to appear.

            processWiki();
        }

        public bool isRightKey(string line, KeyValuePair<String, Tuple<String, String[]>> kvp)
        {
            return line.StartsWith(kvp.Key);
        }

        private void scintilla1_AutoCCompleted(object sender, AutoCSelectionEventArgs e)
        {
            if (workingEnviroment.recursivelyInserting)
            {
                recursionChecks();
            }
            /*
            if (!workingEnviroment.recursivelyInserting)
                showAutoComplete();
                */
        }


        private void scintilla1_AutoCSelection(object sender, AutoCSelectionEventArgs e)
        {
            workingEnviroment.beforeInsertion = true;
        }

        public KeyValuePair<String, Tuple<String, String[]>> getRightEntry()
        {

            string currentLine = scintilla1.Lines[scintilla1.CurrentLine].Text;
            currentLine = Regex.Replace(currentLine, "\r\n", "");
            if (currentLine == "")
            {
                    return new KeyValuePair<string, Tuple<string, string[]>>("emptyline", new Tuple<string, string[]>("emptyline", Completion.combineDictionaries("variableInitializers", "specialFunctions", "launchargs").SelectMany(n => n.Value.Item2).ToArray()));


            }


            
            if (!currentLine.Contains("="))
                foreach (KeyValuePair<String, Tuple<String, String[]>> kvp in Completion.combineDictionaries("variableInitializers","specialFunctions"))
                {
                    if (kvp.Key == currentLine)
                        return kvp;
                }
            
            if (currentLine.StartsWith("#"))
            {
                foreach (KeyValuePair<String, Tuple<String, String[]>> kvp in Completion.wikiDictionary["launchargs"])
                {
                    if (isRightKey(currentLine, kvp))
                        return kvp;
                }
                return new KeyValuePair<string, Tuple<string, string[]>>("launchargs", new Tuple<string, string[]>("launchargs", Completion.wikiDictionary["launchargs"].SelectMany(n => n.Value.Item2.Select(m=>m.Substring(1))).ToArray()));
            }


            if (currentLine.StartsWith("["))
                foreach (KeyValuePair<String, Tuple<String, String[]>> kvp in Completion.wikiDictionary["conditionals"])
                {
                    if (isRightKey(currentLine.Substring(1), kvp))
                        return kvp;
                }

            if (currentLine.Contains("="))
            {
                string[] currentLineSplit = currentLine.Split('=').Select(n=>n.Trim()).ToArray();
                foreach (KeyValuePair<String, Tuple<String, String[]>> kvp in Completion.combineDictionaries("combinedFunctions", "strgFunctions", "rowsFunctions"))
                {
                    if (currentLineSplit[1] == kvp.Key)
                        return kvp;
                }
                
                if (currentLineSplit[1]=="")
                {

                    if (currentLineSplit[0].StartsWith("STRG"))
                        return new KeyValuePair<string, Tuple<string, string[]>>("strgcmds", new Tuple<string, string[]>("strgcmds", Completion.wikiDictionary["strgFunctions"].Select(n => n.Key).ToArray()));

                    if (currentLineSplit[0].StartsWith("ROWS"))
                        return new KeyValuePair<string, Tuple<string, string[]>>("rowscmds", new Tuple<string, string[]>("rowscmds", Completion.wikiDictionary["rowsFunctions"].Select(n => n.Key).ToArray()));
                    /*
                    if (currentLineSplit[0].StartsWith("NUM"))
                        return new KeyValuePair<string, Tuple<string, string[]>>("numbcmds", new Tuple<string, string[]>("numbcmds", Completion.wikiDictionary["strgFunctions"].Select(n => n.Key).ToArray()));
                    
                    number functions dont exist
                    */
                }
                

            }
            return new KeyValuePair<string, Tuple<string, string[]>>("empty", new Tuple<string, string[]>("empty", new string[] { "empty" }));
        }

        public void showAutoComplete()
        {
            if (!(bool)Settings.Autocomplete["isEnabled"])
                return;

            KeyValuePair<String, Tuple<String, String[]>> kvp = getRightEntry();

            if (kvp.Key == "empty")
                return;
            displaySuggestions(kvp.Value.Item2);

        }

        public void displaySuggestions(string[] suggestions)
        {
            var currentPos = scintilla1.CurrentPosition;
            var wordStartPos = scintilla1.WordStartPosition(currentPos, true);

            // Display the autocompletion list
            var lenEntered = currentPos - wordStartPos;

            scintilla1.AutoCShow(lenEntered, String.Join(",", suggestions));
            
        }

        private void scintilla1_InsertCheck(object sender, InsertCheckEventArgs e)
        {

            if (workingEnviroment.beforeInsertion&&!workingEnviroment.recursivelyInserting)
            {
                setAutoCQuery(e.Text);
                e.Text = "";
                
            }
            workingEnviroment.beforeInsertion = false;
        }

        public static IEnumerable<String[]> CombineBy(string[] values, Func<string, bool> predicate)
        {
            var result = new List<string[]>();
            while (values.Any())
            {
                if (predicate(values.First()))
                {
                    yield return values.Take(1).ToArray();
                    values = values.Skip(1).ToArray();
                }
                else
                {
                    var toAdd = values.TakeWhile(n => !predicate(n)).ToArray();
                    yield return toAdd.ToArray();
                    values = values.Skip(toAdd.Length).ToArray();
                }
            }

        }

        public void setAutoCQuery(string query)
        {
            String[] mc = Regex.Split(query, " ");

            mc = mc.Select(n => n + (query.Contains(n + " ") && !n.StartsWith("(") ? " " : "")).ToArray();
            mc = mc.Select((n, i) => (query.Contains(" " + n) && !n.StartsWith("(") && i != 0 && mc[i - 1].StartsWith("(") ? " " : "") + n).ToArray();

            mc = mc.SelectMany((n, i) => (n.StartsWith("(") && i != 0 && mc[i - 1].StartsWith("(")) ? new string[] { " ", n } : new string[] { n }).ToArray();

            mc = mc.SelectMany(m => m.Contains("[") ? new string[] { m.Split('[')[0], "[", m.Split('[')[1] } : new string[] { m }).ToArray();
            mc = mc.SelectMany(m => m.Contains("]") ? new string[] { m.Split(']')[0], "]", m.Split(']')[1] } : new string[] { m }).ToArray();

            mc = CombineBy(mc, m => m.Contains("(") || m.Contains(")")).Select(n => String.Join("", n)).ToArray();
            workingEnviroment.toInsertQuery = mc;
        }

        public void recursiveAutoC()
        {
            if (workingEnviroment.toInsertQuery[0].StartsWith("("))
            {   
                string[] items = new Regex("\\((.*?)\\)").Match(workingEnviroment.toInsertQuery[0]).Groups[1].Value.Split('|');
                items = items.SelectMany(n => n == "ROWS" ? workingEnviroment.rowsVars : n == "STRG" ? workingEnviroment.strgVars : n == "NUMB" ? workingEnviroment.numbVars : n == "VAR" ? workingEnviroment.strgVars.Concat(workingEnviroment.rowsVars.Concat(workingEnviroment.numbVars)) : new string[] { n }).ToArray();
                displaySuggestions(items);
            }
            else
            {
                scintilla1.InsertText(scintilla1.CurrentPosition,workingEnviroment.toInsertQuery[0]);
                scintilla1.CurrentPosition += workingEnviroment.toInsertQuery[0].Length;
                scintilla1.SelectionStart = scintilla1.SelectionEnd;
                recursionChecks();
            }
        }

        private void checkQueryToBeInserted_Tick(object sender, EventArgs e)
        {
            //if ()
            if (workingEnviroment.toInsertQuery?.Length > 0 && !workingEnviroment.recursivelyInserting)
            {
                workingEnviroment.recursivelyInserting = true;
                recursiveAutoC();
            }
        }

        public void recursionChecks()
        {
            workingEnviroment.toInsertQuery = workingEnviroment.toInsertQuery.Skip(1).ToArray();

            if (workingEnviroment.toInsertQuery.Length > 0)
                recursiveAutoC();
            else
                workingEnviroment.recursivelyInserting = false;
        }

        private void autoCompleteTimer_Tick(object sender, EventArgs e)
        {
            
        }
    }
}
