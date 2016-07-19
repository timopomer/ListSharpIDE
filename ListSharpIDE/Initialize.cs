using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ListSharpIDE
{
    public static class Initialize
    {
        public static Func<string, string[]> extractCommands = x => x.Split('\n').Where(n => n.Contains("<td><a href")).Select(n => new Regex(@">([^>]*)</a></td>").Match(n).Groups[1].Value).ToArray();
        public static void downloadWiki()
        {
            downloadConstants();
            downloadConditionals();
            downloadFunctions();
            downloadLaunchargs();
            saveWiki();
        }

        public static void readWiki()
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\wikiDocs";
            if (!Directory.Exists(path))
            {
                downloadWiki();
                return;
            }
            foreach (KeyValuePair<String, Dictionary<String, Tuple<String, String[]>>> differentWikis in Completion.wikiDictionary)
            {
                string singlefile = File.ReadAllText(path + "\\" + differentWikis.Key + ".txt");
                string[] wikiArr = Regex.Split(singlefile, "\r\n---\r\n", RegexOptions.Singleline);
                foreach (string singleWiki in wikiArr)
                {
                    string[] wikiLines = Regex.Split(singleWiki, "\r\n");
                    Completion.wikiDictionary[differentWikis.Key].Add(wikiLines[0], new Tuple<string, string[]>(wikiLines[1], wikiLines.Skip(2).Take(wikiLines.Length-2).ToArray()));
                }
            }
        }

        public static void saveWiki()
        {
            string path = Environment.CurrentDirectory + @"\wikiDocs";
            Directory.CreateDirectory(path);
            foreach (KeyValuePair<String, Dictionary<String, Tuple<String, String[]>>> differentWikis in Completion.wikiDictionary)
            {
                string toWrite = String.Join("\r\n---\r\n", differentWikis.Value.Select(n => n.Key + "\r\n" + n.Value.Item1 + "\r\n" + String.Join("\r\n", n.Value.Item2)));
                File.WriteAllText(path + "\\" + differentWikis.Key + ".txt", toWrite);
            }
        }

        public static void downloadConstants()
        {
            string rawHtml = new WebClient().DownloadString("https://github.com/timopomer/ListSharp/wiki/Special-constants");
            string[] wikiDefs = new Regex("<p>(?!<code>)(.*?)</p>", RegexOptions.Singleline).Matches(rawHtml).Cast<Match>().Select(n => n.Groups[1].Value).ToArray();
            string[] constants = new Regex("<p><code>(.*?)</code></p>", RegexOptions.Singleline).Matches(rawHtml).Cast<Match>().Select(n => n.Groups[1].Value.Replace("&lt;","<").Replace("&gt;", ">")).ToArray();

            for (int i = 0; i < wikiDefs.Length; i++)
            {
                Completion.wikiDictionary["constants"].Add(constants[i], new Tuple<string, string[]>(wikiDefs[i], new string[] { constants[i] }));
            }
        }
        public static void downloadConditionals()
        {
            string rawHtml = new WebClient().DownloadString("https://github.com/timopomer/ListSharp/wiki/List-of-conditionals-by-type");
            rawHtml = new Regex("<table>(.*)</table>", RegexOptions.Singleline).Match(rawHtml).Groups[1].Value;
            string[] conditionals = extractCommands.Invoke(rawHtml);

            foreach (string conditional in conditionals)
            {
                string fullHtml = new WebClient().DownloadString("https://github.com/timopomer/ListSharp/wiki/" + conditional);
                string wikiDef = new Regex("<p>(.*?)</p>", RegexOptions.Singleline).Match(fullHtml).Groups[1].Value.Trim();
                string[] uses = new Regex("<code>(.*?)</code>", RegexOptions.Singleline).Match(fullHtml).Groups[1].Value.Trim().Split('\n');
                Completion.wikiDictionary["conditionals"].Add(conditional, new Tuple<string, string[]>(wikiDef, uses));
            }
        }

        public static void downloadFunctions()
        {
            string fullHtml = new WebClient().DownloadString("https://github.com/timopomer/ListSharp/wiki/List-of-functions-by-type");
            fullHtml = new Regex("<table>(.*)</table>", RegexOptions.Singleline).Match(fullHtml).Groups[1].Value;
            string[] splitsections = Regex.Split(fullHtml, "<strong>").Where(n => n.Contains("FUNCTIONS")).ToArray();

            addFunctions(extractCommands.Invoke(splitsections[0]), "combinedFunctions");
            addFunctions(extractCommands.Invoke(splitsections[1]), "strgFunctions");
            addFunctions(extractCommands.Invoke(splitsections[2]), "rowsFunctions");
            addFunctions(extractCommands.Invoke(splitsections[3]), "specialFunctions");
        }

        public static void addFunctions(string[] functionNames, string dictionaryName)
        {
            foreach (string function in functionNames)
            {
                string fullHtml = new WebClient().DownloadString("https://github.com/timopomer/ListSharp/wiki/" + function);
                string wikiDef = new Regex("<p>(.*?)</p>", RegexOptions.Singleline).Match(fullHtml).Groups[1].Value.Trim();
                string[] uses = new Regex("<code>(.*?)</code>", RegexOptions.Singleline).Match(fullHtml).Groups[1].Value.Trim().Split('\n');
                Completion.wikiDictionary[dictionaryName].Add(function,new Tuple<string, string[]>(wikiDef,uses));
            }

        }

        public static void downloadLaunchargs()
        {
            string fullHtml = new WebClient().DownloadString("https://github.com/timopomer/ListSharp/wiki/Launching-arguments");
            string[] rawArgs = new Regex("<code>(.*?)</p>", RegexOptions.Singleline).Matches(fullHtml).Cast<Match>().Select(n => n.Groups[1].Value).ToArray();
            foreach (string arg in rawArgs)
            {
                string fullArg = new Regex("(.*?)\n").Match(arg).Groups[1].Value;
                string argName = fullArg.Split(':')[0];
                string wikiDef = new Regex("<p>(.*?)\\z").Match(arg).Groups[1].Value;
                Completion.wikiDictionary["launchargs"].Add(argName, new Tuple<string, string[]>(wikiDef, new string[] { fullArg }));
            }

        }


        }
}
