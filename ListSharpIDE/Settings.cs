using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ListSharpIDE
{
    public static class Settings
    {
        public static string configPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\configuration.ini";
        public static Color defaultColor,
                            commentColor,
                            commentLineColor,
                            commentLineDocColor,
                            numberColor,
                            stringColor,
                            characterColor,
                            literalColor,
                            brokenstringColor,
                            operatorColor,
                            launchargsColor,
                            comparatorColor,
                            commandColor,
                            startingColor,
                            backgroundColor,
                            foregroundColor,
                            caretColor,
                            lineBgColor,
                            lineColor;

        public static string holdConfig;

        public static string toRaw(this Color input)
        {
            return input.R + "," + input.G + "," + input.B;
        }

        public static void saveSettings()
        {
            List<string> toWrite = new List<string>();
            toWrite.Add("[General]");
            toWrite.Add("[Highlighting]");
            toWrite.Add("defaultColor=" + defaultColor.toRaw());
            toWrite.Add("commentColor=" + commentColor.toRaw());
            toWrite.Add("commentLineColor=" + commentLineColor.toRaw());
            toWrite.Add("commentLineDocColor=" + commentLineDocColor.toRaw());
            toWrite.Add("numberColor=" + numberColor.toRaw());
            toWrite.Add("stringColor=" + stringColor.toRaw());
            toWrite.Add("characterColor=" + characterColor.toRaw());
            toWrite.Add("literalColor=" + literalColor.toRaw());
            toWrite.Add("brokenstringColor=" + brokenstringColor.toRaw());
            toWrite.Add("operatorColor=" + operatorColor.toRaw());
            toWrite.Add("launchargsColor=" + launchargsColor.toRaw());
            toWrite.Add("comparatorColor=" + comparatorColor.toRaw());
            toWrite.Add("commandColor=" + commandColor.toRaw());
            toWrite.Add("startingColor=" + startingColor.toRaw());
            toWrite.Add("backgroundColor=" + backgroundColor.toRaw());
            toWrite.Add("foregroundColor=" + foregroundColor.toRaw());
            toWrite.Add("caretColor=" + caretColor.toRaw());
            toWrite.Add("lineBgColor=" + lineBgColor.toRaw());
            toWrite.Add("lineColor=" + lineColor.toRaw());
            toWrite.Add("[Autocomplete]");
            toWrite.Add("[Versions]");
            toWrite.Add("[Wiki]");
            toWrite.Add("[About]");
            File.WriteAllLines(configPath, toWrite.ToArray());
        }

        public static void loadDefaults()
        {
            //needs to be inplemented
        }

        public static void loadSettings()
        {
            if (!File.Exists(configPath))
            {
                MessageBox.Show("have to load defaults");
                loadDefaults();
                return;
            }
            holdConfig = File.ReadAllText(configPath);
            defaultColor =           loadColor("defaultColor");
            commentColor =           loadColor("commentColor");
            commentLineColor =       loadColor("commentLineColor");
            commentLineDocColor =    loadColor("commentLineDocColor");
            numberColor =            loadColor("numberColor");
            stringColor =            loadColor("stringColor");
            characterColor =         loadColor("characterColor");
            literalColor =           loadColor("literalColor");
            brokenstringColor =      loadColor("brokenstringColor");
            operatorColor =          loadColor("operatorColor");
            launchargsColor =        loadColor("launchargsColor");
            comparatorColor =        loadColor("comparatorColor");
            commandColor =           loadColor("commandColor");
            startingColor =          loadColor("startingColor");
            backgroundColor =        loadColor("backgroundColor");
            foregroundColor =        loadColor("foregroundColor");
            caretColor =             loadColor("caretColor");
            lineBgColor =            loadColor("lineBgColor");
            lineColor =              loadColor("lineColor");

        }
        public static Color loadColor(string colorName)
        {
            string colorString = returnProperty(colorName);
            string[] bytes = colorString.Split(',');
            return Color.FromArgb(byte.Parse(bytes[0]), byte.Parse(bytes[1]), byte.Parse(bytes[2]));
            

        }
        public static string returnProperty(string propertyName)
        {
            return new Regex(propertyName + "=(.*?)\r\n").Match(holdConfig).Groups[1].Value;
        }
    }
}
