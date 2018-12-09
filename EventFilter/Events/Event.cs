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
        private HashSet<string[]> _listItem { get; set;}

        private Event()
        {
            Entries = new List<string[]>();
            _listItem = new HashSet<string[]>();
            Eventlogs = new EventLogs[0];
        }

        public static IEvent Instance
        {
            get
            {
                if (_event is null)
                    NewInstance();

                return _event;
            }
        }
        
        private static void NewInstance()
        {
            _event = new Event { Keywords = new Keyword() };

            return;
        }

        public IEvent SetLocation(System.IO.FileInfo location)
        {
            if (location.Length == 0)
            {
                Messages.IncorrectLogSize();
                return this;
            }

            EventLocation = location;

            return this;
        }

        public IEvent SetKeywordInstance(IKeywords keyword)
        {
            Keywords = keyword;

            return this;
        }

        /// <summary>
        /// Check Keywords on valid Operators
        /// </summary>
        public IEvent IsCountOperatorUsed()
        {
            List<string> keywords = Keywords.Map().Items;
            Keywords.Operators = new List<dynamic>();

            foreach(string key in Keywords.Map().Items)
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
                if (e.Log.IndexOf(value, StringComparison.OrdinalIgnoreCase) != -1)
                {
                    count++;
                }
            }

            return count;
        }

        /// <summary>
        /// Skip current event and get Next event
        /// </summary>
        /// <param name="curId">ID of first line of description of current event</param>
        /// <returns></returns>
        public string Next(int curId)
        {
            if (!(curId < Eventlogs.Length)) return Eventlogs[curId].Log;

            EventIdentifier = curId + 1;
            return Eventlogs[EventIdentifier].Log;
        }

        /// <summary>
        /// Skip current event and get Previous event
        /// </summary>
        /// <param name="curId">ID of first line of description of current event</param>
        /// <returns>string</returns>
        public string Previous(int curId)
        {
            if (!(curId > 0)) return Eventlogs[curId].Log;

            EventIdentifier = curId - 1;
            return Eventlogs[EventIdentifier].Log;
        }

        public bool CanAddListItem(string[] item)
        {
            return _listItem.Add(item);
        }
    }
}