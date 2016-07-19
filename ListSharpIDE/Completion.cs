using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListSharpIDE
{
    public static class Completion
    {
        //functionType => (functionName,(Wiki,Uses))
        public static Dictionary<String, Dictionary<String, Tuple<String, String[]>>> wikiDictionary = new Dictionary<String, Dictionary<String, Tuple<String, String[]>>>();
        public static string[] logicWords = new string[] { "TO", "SPLIT", "BY", "FROM", "IN", "WITH", "AS" };
        public static string[] comparators = new string[] { "ANY", "EVERY", "LENGTH", "IN", "STRG", "IS", "ISNOT", "ISUNDER", "ISOVER", "ISEQUAL", "CONTAINS", "CONTAINSNOT" };
        public static Func<Dictionary<String, Tuple<String, String[]>>,String> getDictionaryNameEntries = input => String.Join(" ", input.Select(n => n.Key));

        public static string connectorsString()
        {
            return String.Join(" ", logicWords) + " " + String.Join(" ", comparators);
        }
        public static string commandString()
        {
            return getDictionaryNameEntries.Invoke(wikiDictionary["combinedFunctions"]) + " " + 
                   getDictionaryNameEntries.Invoke(wikiDictionary["strgFunctions"]) + " " + 
                   getDictionaryNameEntries.Invoke(wikiDictionary["rowsFunctions"]) + " " +
                   getDictionaryNameEntries.Invoke(wikiDictionary["specialFunctions"]);
        }
        public static void createDictionaries()
        {
            new List<String>() { "combinedFunctions", "strgFunctions", "rowsFunctions", "specialFunctions", "launchargs", "conditionals", "constants" }
            .ForEach(n => wikiDictionary.Add(n, new Dictionary<String, Tuple<String, String[]>>()));
        }
        public static void resetDictionaries()
        {
            wikiDictionary = new Dictionary<String, Dictionary<String, Tuple<String, String[]>>>();
            createDictionaries();
        }






    }
}
