using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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

        public static Dictionary<string, object> Autocomplete = new Dictionary<string, object>();
        public static Dictionary<string,Color> Highlighting = new Dictionary<string, Color>();
        public static string[] colorProperties = new string[] { "defaultColor",
                                                                "commentColor",
                                                                "commentLineColor",
                                                                "commentLineDocColor",
                                                                "numberColor",
                                                                "stringColor",
                                                                "characterColor",
                                                                "literalColor",
                                                                "brokenstringColor",
                                                                "operatorColor",
                                                                "launchargsColor",
                                                                "comparatorColor",
                                                                "commandColor",
                                                                "startingColor",
                                                                "backgroundColor",
                                                                "foregroundColor",
                                                                "caretColor",
                                                                "lineBgColor",
                                                                "lineColor" };
        public static string holdConfig;

        public static string toRaw(this Color input)
        {
            return input.R + "," + input.G + "," + input.B;
        }
        public static void Initialize()
        {
            Autocomplete.Add("isEnabled", true);
            Autocomplete.Add("onCharAdded", true);
            Autocomplete.Add("activationKey", Keys.F1);
            foreach (string colorProp in colorProperties)
            Highlighting.Add(colorProp, new Color());


        }

        public static void saveSettings()
        {
            List<string> toWrite = new List<string>();
            toWrite.Add("[General]");
            toWrite.Add("[Highlighting]");
            foreach (string colorProp in colorProperties)
            toWrite.Add(colorProp + "=" + Highlighting[colorProp].toRaw());
            toWrite.Add("[Autocomplete]");
            foreach (KeyValuePair<string, object> kvp in Autocomplete)
                toWrite.Add(kvp.Key + "=" + kvp.Value);
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
            foreach (string colorProp in colorProperties)
            Highlighting[colorProp] = loadColor(colorProp);

            Autocomplete["isEnabled"] = loadBoolean("isEnabled");
            Autocomplete["onCharAdded"] = loadBoolean("onCharAdded");
            Autocomplete["activationKey"] = loadKeycode("activationKey");
        }
        public static Keys loadKeycode(string keyName)
        {
            return (Keys)Enum.Parse(typeof(Keys),returnProperty(keyName));
        }

        public static bool loadBoolean(string boolName)
        {
            return returnProperty(boolName) == "True" ? true : false;
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
