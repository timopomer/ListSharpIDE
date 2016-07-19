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
        public static void createDictionaries()
        {
            new List<String>() { "combinedFunctions", "strgFunctions", "rowsFunctions", "specialFunctions", "launchargs", "conditionals", "constants" }
            .ForEach(n => wikiDictionary.Add(n, new Dictionary<String, Tuple<String, String[]>>()));
        }


    }
}
