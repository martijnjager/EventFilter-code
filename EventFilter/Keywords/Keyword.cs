using System.Collections.Generic;
using System;
using System.Linq;
using System.IO;
using EventFilter.Contracts;
using EventFilter.Keywords;
using System.Windows.Forms;

namespace EventFilter.Keywords
{
    public sealed partial class Keyword : IKeywords
    {
        /**
         * Property for the count: operator
         */
        public string KeywordToCount { get; set; }

        public bool KeywordsLoaded { get; set; }

        public static string FileLocation { get; set; }

        private static Keyword _keywords;
        private static readonly object _lock = new object();

        private Keyword()
        {
            AvailableOperators = new List<string> { "-", "count:", "datestart:", "dateend:" };
            Items = new List<string>();
            _operators = new List<string>();
            Ignorable = new List<string>();

            SetLocation();
        }

        public static IKeywords Instance
        {
            get
            {
                if(_keywords is null)
                    NewInstance();

                return _keywords;
            }
        }

        private static void NewInstance()
        {
            lock (_lock) { _keywords = new Keyword(); }
        }

        public IKeywords Refresh()
        {
            NewInstance();

            return _keywords;
        }

        public void SetLocation()
        {
            if (Actions.IsEmpty(FileLocation))
            {
                FileLocation = Bootstrap.CurrentLocation + @"\keywords.txt";
            }
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
            string keywords = LoadFrom(!string.IsNullOrEmpty(path) ? path : FileLocation);

            Set(keywords);
            _fileKeywords = Arr.ToList(keywords, ", ");

            KeywordsLoaded = true;

            return this;
        }

        public void LoadIntoCLB()
        {
            foreach (string str in Items)
            {
                Actions.Form.clbKeywords.Items.Add(str.Trim(), true);
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

            /**
             * Clear the keywords and add new onces
             */
            Delete();
            AddFromClb();
            AddFromTextbox();

            AddOperators();

            return this;
        }

        private void AddFromTextbox()
        {
            if (!string.IsNullOrEmpty(Actions.Form.tbKeywords.Text))
            {
                Add(Actions.Form.tbKeywords.Text.Split(','));
            }
        }

        private void AddFromClb()
        {
            Add(Actions.Form.clbKeywords);
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