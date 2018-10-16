using System;
using System.Collections.Generic;
using System.Linq;
using EventFilter.Keywords;
using EventFilter.Contracts;

namespace EventFilter.Events
{
    public sealed partial class Event : IEvent
    {
        private static Event _event;

        public List<string[]> Entries { get; private set; }

        /**
         * Property ensures that the list items are unique
         * No duplicate entries can be added
         */
        public HashSet<string[]> ListItems { get; private set;}

        private Event()
        {
            Entries = new List<string[]>();
            ListItems = new HashSet<string[]>();
            FilteredEventDate = new List<string>();
            FilteredEventId = new List<string>();
            FilteredEventDesc = new List<string>();

            Eventlogs = new EventLogs[0];

            //Dates = new string[0];
            //Description = new string[0];
            //Id = new string[0];
            //EventArray = new string[0];
        }

        public static Event Instance
        {
            get
            {
                if (_event is null)
                    NewInstance();

                return _event;
            }
        }
        
        private static void InitProps()
        {
            _event.Eventlogs = new EventLogs[0];
            _event.FilteredEventDate = new List<string>();
            _event.FilteredEventId = new List<string>();
            _event.EventIdentifier = 0;
            _event.Entries = new List<string[]>();
            _event.ListItems = new HashSet<string[]>();
        }

        private static void NewInstance()
        {
            if (_event is null)
            {
                _event = new Event {Keywords = new Keyword()};

                return;
            }

            InitProps();
        }

        public dynamic Refresh()
        {
            NewInstance();

            return _event;
        }

        public void SetEventLocation(string location)
        {
            var fileInfo = new System.IO.FileInfo(location);
            if (fileInfo.Length == 0)
            {
                Messages.IncorrectLogSize();
                return;
            }

            EventLocation = fileInfo;
        }

        public Event SetKeywordObj(IKeywords keyword)
        {
            Keywords = keyword;

            return this;
        }

        /// <summary>
        /// Check Keywords on valid Operators
        /// </summary>
        public Event CheckCountOperator()
        {
            List<string> keywords = Keywords.Index();
            Keywords.Operators = new List<dynamic>();

            foreach(string key in keywords)
            {
                if(Keywords.AvailableOperators.Any(key.Contains))
                {
                    Keywords.Operators.Add(key);
                }
            }

            Count();

            return this;
        }

        /// <summary>
        /// Count how many times the given keyword is present in the log
        /// </summary>
        private void Count()
        {
            string keyword = Keywords.Operators.Find(s => s.Contains("count:"));

            if (!string.IsNullOrEmpty(keyword))
            {
                Keywords.Counter = Count(keyword.Replace("count:", ""));
                Keywords.KeywordCounted = keyword.Replace("count:", "");
            }
            else
            {
                Keywords.Counter = 0;
                Keywords.KeywordCounted = "";
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

            foreach (EventLogs e in Eventlogs)
            {
                count = CountValue(e.Log, value, count);
            }

            return count;
        }

        private static int CountValue(string line, string value, int count)
        {
            if (line.IndexOf(value, StringComparison.OrdinalIgnoreCase) != -1)
            {
                count++;
            }

            return count;
        }
    }
}