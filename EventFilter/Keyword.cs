using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace EventFilter
{
    class Keyword
    {
        Background background = new Background();

        private static string _keywords;

        private static string _keyLocation = Background.GetLocation() + "\\keywords.txt";
        private static string _eventLocation = Background.GetLocation() + "\\eventlog.txt";

        public static string Keywords
        {
            get { return _keywords; }
            set { _keywords = value; }
        }
        public static string EventLocation
        {
            get { return _eventLocation; }
            set { _eventLocation = value; }
        }
        public static string KeyLocation
        {
            get { return _keyLocation; }
            set { _keyLocation = value; }
        }

        public Keyword()
        {

        }

        /// <summary>
        /// Prepare app for keywords
        /// - Check keywords file existence
        /// - Load keywords properly into the textbox
        /// - Make keywords publicly visible
        /// </summary>
        /// <param name="path">Path of keywords file</param>
        public static void GetKeywords(string path)
        {
            if (Background.CheckFileExistence(path))
            {
                string keywords = GetKeyWordsFromLocation(path);
                _keywords = keywords;
            }
        }
        
        /// <summary>
        /// Get keywords from the provided location, default is the current location of the app
        /// </summary>
        /// <param name="path">Path of the file</param>
        /// <returns></returns>
        private static string GetKeyWordsFromLocation(string path)
        {
            string line = "";

            if (File.Exists(path) == true)
            {
                StreamReader getKeywords = new StreamReader(path);
                line = getKeywords.ReadLine();
            }

            return line;
        }

        /// <summary>
        /// Prepare keywords for usage
        /// </summary>
        /// <param name="keywords">Keyword to prepare</param>
        /// <returns></returns>
        public static string[] ValidateKeywords(string keywords)
        {
            string[] keys = keywords.Split(',');

            for (int i = 0; i < keys.Length; i++)
            {
                keys[i] = keys[i].Trim();
            }

            return keys;
        }

        /// <summary>
        /// Validate keywords on operators
        /// </summary>
        /// <param name="keyword">Keyword to validate</param>
        public static void CheckKeywordsOnOperator(string[] keyword)
        {
            foreach (string key in keyword)
            {
                if (key.Contains(":"))
                {
                    if (key.Contains("count:"))
                    {
                        _keywords = key.Substring(6);
                        break;
                    }
                }
            }
        }
    }
}
