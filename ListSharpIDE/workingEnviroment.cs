using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListSharpIDE
{
    public static class workingEnviroment
    {
        public static string activeFilePath = "";
        public static string[] numbVars;
        public static string[] strgVars;
        public static string[] rowsVars;

        public static bool beforeInsertion = false;
        public static bool recursivelyInserting = false;
        public static string[] toInsertQuery;
    }
}
