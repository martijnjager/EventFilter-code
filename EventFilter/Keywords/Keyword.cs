using System.Collections.Generic;
using System;
using System.Linq;
using System.IO;
using EventFilter.Contracts;

namespace EventFilter.Keywords
{
    public sealed partial class Keyword : IKeywords, IRefresh
    {

        public bool KeywordsLoaded { get; set; }

        private Keyword _keywords;
        private readonly object _lock = new object();

        public int Counter { get; set; }

        public string KeywordCounted { get; set; }

        public Keyword()
        {
            Operators = new List<dynamic>();
            Ignorable = new List<string>();
            Delete();
            _fileKeywords = new List<string>();

            AvailableOperators = new List<string> { "-", "count:", "datestart:", "dateend:" };

            SetLocation();
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
        public IKeywords LoadFromLocation(string path = "")
        {
            string keywords = LoadFrom(!string.IsNullOrEmpty(path) ? path : KeywordLocation);

            Set(keywords);
            _fileKeywords = Arr.ToList(keywords, ", ");

            KeywordsLoaded = true;

            return this;
        }

        public void LoadIntoCLB()
        {
            foreach (string str in Items)
            {
                Actions.form.clbKeywords.Items.Add(str.Trim(), true);
            }
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

        private void AddIgnoreable(string key)
        {
            if (key.Contains("-"))
                Ignorable.Add(key.Trim('-'));
        }

        private void AddDate(string key)
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