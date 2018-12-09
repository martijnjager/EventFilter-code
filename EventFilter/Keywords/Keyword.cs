using System.Collections.Generic;
using System;
using System.Linq;
using System.IO;
using EventFilter.Contracts;

namespace EventFilter.Keywords
{
    public sealed partial class Keyword : IKeywords
    {
        /**
         * Indexes all operators, both used and unused operators
         */
        public List<string> AvailableOperators { get; set; }

        public bool KeywordsLoaded { get; set; }

        private Keyword _keywords;
        private readonly object _lock = new object();

        /**
         * Indexes the used operators
         */
        public List<dynamic> Operators { get; set; }

        public string KeywordLocation { get; set; }

        public string DateStart { get; private set; }

        public string DateEnd { get; private set; }

        public List<string> Ignorable { get; }

        public int Counter { get; set; }

        public string KeywordCounted { get; set; }

        public Keyword()
        {
            Operators = new List<dynamic>();
            Ignorable = new List<string>();
            Delete();
            _fileKeywords = new List<string>();

            AvailableOperators = new List<string> { "-", "count:", "datestart:", "dateend:" };

            if (Actions.IsEmpty(KeywordLocation))
            {
                KeywordLocation = Bootstrap.CurrentLocation + @"\Keywords.txt";
            }
        }

        public IKeywords Instance
        {
            get
            {
                NewInstance();

                return _keywords;
            }
        }

        private void NewInstance()
        {
            if (_keywords is null) lock(_lock){_keywords = new Keyword();}
        }

        public dynamic Refresh()
        {
            NewInstance();

            return _keywords;
        }

        /// <summary>
        /// Prepare app for Keywords
        /// - Check Keywords file existence
        /// - Load Keywords properly into the textbox
        /// - Make Keywords publicly visible
        /// </summary>
        /// <param name="path">Path of Keywords file</param>
        public void LoadFromLocation(string path = "")
        {
            string keywords = LoadFrom(!string.IsNullOrEmpty(path) ? path : KeywordLocation);

            Set(keywords);
            _fileKeywords = Arr.ToList(keywords, ", ");

            KeywordsLoaded = true;
        }

        /// <summary>
        /// Get Keywords from the provided location, default is the current location of the app
        /// </summary>
        /// <param name="path">Path of the file</param>
        /// <returns></returns>
        private static string LoadFrom(string path)
        {
            if (!File.Exists(path)) return "";
            StreamReader getKeywords = new StreamReader(path);
            string line = getKeywords.ReadLine();

            return line;
        }

        public IKeywords Map()
        {
            if (!KeywordsLoaded)
                LoadFromLocation();

            DateStart = null;
            DateEnd = null;

            foreach (string item in ToArray())
            {
                AddDate(item);
                AddIgnoreable(item);
            }

            return this;
        }


        /// <summary>
        /// Prepare Keywords for usage
        /// </summary>
        /// <returns></returns>
        //public List<string> Index()
        //{
        //    if (!KeywordsLoaded)
        //        LoadFromLocation();

        //    string[] key = ToArray();

        //    DateStart = null;
        //    DateEnd = null;

        //    for (int i = 0; i < key.Length; i++)
        //    {
        //        AddDate(key[i]);
        //        AddIgnoreable(key[i]);
        //    }

        //    // Convert keywords to usable string
        //    Set(key);

        //    return new List<string>(key);
        //}

        private void AddIgnoreable(string key)
        {
            if (key.Contains("-"))
                Ignorable.Add(key.Trim('-'));
        }

        private void AddDate(dynamic key)
        {
            if (key.Contains("dateend:"))
            {
                DateEnd = key.Substring(key.IndexOf(':') + 1);
            }

            if (key.Contains("datestart:"))
            {
                DateStart = key.Substring(key.IndexOf(':') + 1);
            }
        }

        public void SaveToFile(string fileName, string keywords)
        {
            try
            {
                StreamWriter streamWriter = new StreamWriter(fileName);
                streamWriter.WriteLine(keywords);
                Actions.Report("Saving Keywords " + keywords + " to file");
                streamWriter.Close();
            }
            catch(Exception error)
            {
                Actions.Report("An error occured when trying to save Keywords: " + error.Message);
                Messages.ProblemOccured("saving keywords");
            }
        }
    }
}