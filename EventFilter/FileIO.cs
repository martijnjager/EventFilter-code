using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace EventFilter
{
    public static class file
    {
        
        public static bool CheckFileExistence(string file)
        {
            if (File.Exists(GetLocation() + file))
            {
                return true;
            }

            return false;
        }

        public static string GetKeywords(string path)
        {
            string line = "";

            if (File.Exists(path) == true)
            {
                StreamReader getKeywords = new StreamReader(path);
                line = getKeywords.ReadLine();
            }
            else
            {
                //bug.BugReportLog("No keywords found at path " + path);
            }

            return line;
        }

        public static string GetLocation()
        {
            return Directory.GetCurrentDirectory();
        }

    }
}
