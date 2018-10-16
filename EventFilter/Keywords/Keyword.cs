using System.Collections.Generic;
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
            DeleteKeywords();
            KeywordsFromFile = new List<string>();

            AvailableOperators = new List<string> { "-", "count:", "datestart:", "dateend:" };

            if (string.IsNullOrEmpty(KeywordLocation))
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
        public void LoadKeywordsFromLocation(string path = "")
        {
            string keywords = LoadFrom(!string.IsNullOrEmpty(path) ? path : KeywordLocation);

            SetKeyword(keywords);
            KeywordsFromFile = Arr.ToList(keywords, ", ");

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

        /// <summary>
        /// Prepare Keywords for usage
        /// </summary>
        /// <returns></returns>
        public List<string> Index()
        {
            if (!KeywordsLoaded)
                LoadKeywordsFromLocation();

            string[] key = ToArray();

            DateStart = null;
            DateEnd = null;

            // ReSharper disable ForCanBeConvertedToForeach
            for (int i = 0; i < key.Length; i++)
            {
                AddDate(key[i]);
                AddIgnoreable(key[i]);
            }

            // Convert keywords to usable string
            SetKeyword(key);

            return new List<string>(key);
        }

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

//        private static void HasTextIgnoreOperator(string text, string key, ICollection<dynamic> events)
//        {
//            key = key.Trim('-');
//
//            if(text.Contains(key))
//            {
//                events.Add("operator");
//            }
//        }
    }
}