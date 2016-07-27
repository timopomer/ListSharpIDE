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
        public static string[] logicWords = new string[] { "TO", "SPLIT", "BY", "FROM", "IN", "WITH", "AS" , "WHERE" , "AND", "ANY", "EVERY", "LENGTH", "IN" };
        public static string[] comparators = new string[] { "IS", "ISNOT", "ISUNDER", "ISOVER", "ISEQUAL", "CONTAINS", "CONTAINSNOT" };
        //public static string[] comparators = new string[] { "ANY", "EVERY", "LENGTH", "IN", "STRG", "ROWS", "NUMB", "IS", "ISNOT", "ISUNDER", "ISOVER", "ISEQUAL", "CONTAINS", "CONTAINSNOT" };
        public static Func<Dictionary<String, Tuple<String, String[]>>,String> getDictionaryNameEntries = input => String.Join(" ", input.Select(n => n.Key));


        public static string connectorsString = "";
        public static string commandString = "";
        public static string startingString = "";
        public static void setConnectorsString()
        {
            connectorsString = String.Join(" ", logicWords) + " " + String.Join(" ", comparators);
        }
        public static Dictionary<String, Tuple<String, String[]>> combineDictionaries(params string[] dictionaryNames)
        {
            return dictionaryNames.Select(n=> wikiDictionary[n]).SelectMany(dict => dict)
                         .ToDictionary(pair => pair.Key, pair => pair.Value);
        }
        public static void setCommandString()
        {
            commandString = getDictionaryNameEntries.Invoke(wikiDictionary["combinedFunctions"]) + " " +
                            getDictionaryNameEntries.Invoke(wikiDictionary["strgFunctions"]) + " " +
                            getDictionaryNameEntries.Invoke(wikiDictionary["rowsFunctions"]) + " " +
                            getDictionaryNameEntries.Invoke(wikiDictionary["conditionals"]);
        }
        public static void setStartingString()
        {
            startingString = getDictionaryNameEntries.Invoke(wikiDictionary["specialFunctions"]) + " " + String.Join(" ",new string[] { "STRG", "ROWS", "NUMB" });
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
