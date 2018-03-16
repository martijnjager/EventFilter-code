using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using EventFilter.Events;
using EventFilter.Keywords.Concerns;
using EventFilter.Keywords.Contracts;

namespace EventFilter.Keywords
{
    public class Keyword : ManagesKeywords, IKeywords
    {
        private readonly List<string> _availableOperators;

        private static Keyword _instance;
        private static readonly object Lock = new object();

        private List<dynamic> Operators { get; set; }

        private Event _event;

        public string KeywordLocation { get; }

        public string DateStart { get; private set; }

        public string DateEnd { get; private set; }

        public int Counter { get; private set; }

        public string KeywordCounted { get; private set; }

        public Keyword()
        {
            Operators = new List<dynamic>();
            AllKeywords = new List<string>();
            KeywordsFromFile = new List<string>();

            _event = Event.Instance;

            _availableOperators = new List<string> { "-", "count:", "datestart:", "dateend:" };

            if (string.IsNullOrEmpty(KeywordLocation))
            {
                KeywordLocation = Bootstrap.CurrentLocation + @"\Keywords.txt";
            }

            LoadKeywordsFromLocation();
        }

        public static Keyword Instance
        {
            get
            {
                lock (Lock)
                {
                    return _instance ?? (_instance = new Keyword());
                }
            }
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
            var keywords = LoadFrom(!string.IsNullOrEmpty(path) ? path : KeywordLocation);

            SetKeyword(keywords);
            KeywordsFromFile = Arr.StringToList(keywords, ", ");
        }

        /// <summary>
        /// Get Keywords from the provided location, default is the current location of the app
        /// </summary>
        /// <param name="path">Path of the file</param>
        /// <returns></returns>
        private static string LoadFrom(string path)
        {
            if (!File.Exists(path)) return "";
            var getKeywords = new StreamReader(path);
            var line = getKeywords.ReadLine();

            return line;
        }

        /// <summary>
        /// Prepare Keywords for usage
        /// </summary>
        /// <returns></returns>
        public List<dynamic> Index()
        {
            var key = ToList();

            DateStart = null;
            DateEnd = null;

            for (var i = 0; i < key.Count; i++)
            {
                AddDate(key, i);
            }

            // Convert keywords to usable string
            SetKeyword(Arr.Implode(ToList()));

            return new List<dynamic>(key);
        }

        private void AddDate(dynamic key, int i)
        {
            if (key[i].Contains("dateend:"))
            {
                DateEnd = key[i].Substring(key[i].IndexOf(':') + 1);
            }

            if (key[i].Contains("datestart:"))
            {
                DateStart = key[i].Substring(key[i].IndexOf(':') + 1);
            }
        }

        /// <summary>
        /// Check Keywords on valid Operators
        /// </summary>
        public void CheckCountOperator()
       {
            IEnumerable<dynamic> keywords = Index();
            Operators = new List<dynamic>();

            foreach(string key in keywords)
            {
                if(_availableOperators.Any(key.Contains))
                {
                    Operators.Add(key);
                }
            }

            Count();
        }

        /// <summary>
        /// Check if event contains keyword with operator
        /// 
        /// Returns null if text contains -{keyword}
        /// </summary>
        /// <param name="text">event</param>
//        private string HasIgnoreWord(string text)
//        {
//            //string id = text.Substring(text.LastIndexOf('+') + 2);
//            var eventToAdd = new List<dynamic>();
//            //string full_text = Property.background.GetDescription(id);
//            
//            foreach (var key in ToList())
//            {
//                HasIgnoreOperator(text, key, eventToAdd);
//            }
//
//            return eventToAdd.Contains("operator") == false ? text : null;
//
//            // ignore parameter is present in the text, can't return text but must return something...
//        }

        private static void HasIgnoreOperator(string text, string key, ICollection<dynamic> events)
        {
            if(!key.Contains('-'))
            {
                events.Add("");

                return;
            }

            HasTextIgnoreOperator(text, key, events);
        }

        private static void HasTextIgnoreOperator(string text, string key, ICollection<dynamic> events)
        {
            key = key.Trim('-');

            if(text.Contains(key))
            {
                events.Add("operator");
            }
        }

        /// <summary>
        /// Count how many times the given keyword is present in the log
        /// </summary>
        private void Count()
        {
            string keyword = Operators.Find(s => s.Contains("count:"));

            if (!string.IsNullOrEmpty(keyword))
            {
                Counter = Count(keyword.Replace("count:", ""));
                KeywordCounted = keyword.Replace("count:", "");
            }
            else
            {
                Counter = 0;
                KeywordCounted = "";
            }
        }

        /// <summary>
        /// Count how many times a value is present
        /// </summary>
        /// <param name="value">value to Search for</param>
        /// <returns>string Count of value</returns>
        private int Count(string value)
        {
            int count = 0;

            foreach (string line in _event.Events)
            {
                count = CountValue(line, value, count);
            }

            return count;
        }

        private int CountValue(string line, string value, int count)
        {
            if (line.IndexOf(value, StringComparison.OrdinalIgnoreCase) != -1)
            {
                count++;
            }

            return count;
        }
    }
}