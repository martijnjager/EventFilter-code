using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using EventFilter.Keywords;
using EventFilter.Contracts;

namespace EventFilter.Events
{
    public sealed partial class Event : IEvent
    {
        /// <summary>
        /// Instance of the class
        /// </summary>
        private static Event _event;

        internal bool NewFile;

        /// <summary>
        /// Stores all events to display
        /// </summary>
        public List<string[]> Entries { get; private set; }

        /// <summary>
        /// Stores all events filtered on duplicates
        /// </summary>
        public List<EventLogs> FilteredEvents { get; private set; }

        /// <summary>
        /// Stores all events filtered on date 
        /// May still contain duplicates with description and is therefore used as some sort of a temp location to filter further
        /// </summary>
        private List<EventLogs> FilteredEventsOnDate { get; set; }

        /// <summary>
        /// Instance of the Keywords class
        /// </summary>
        public IKeywords Keywords { get; set; }
        
        /// <summary>
        /// ID for message form
        /// </summary>
        public int EventIdentifier { get; set; }

        /// <summary>
        /// Stores information about the file being used
        /// </summary>
        public FileInfo EventLocation { get; private set; }

        /// <summary>
        /// Stores all events
        /// </summary>
        public List<string> Events { get; set; }

        /// <summary>
        /// Stores all events, using the EventLogs struct allows the app to search/filter easier
        /// </summary>
        public List<EventLogs> Eventlogs { get; private set; }

        /// <summary>
        /// Property ensures that the list items are unique 
        /// No duplicate entries can be added
        /// </summary>
        private HashSet<string[]> _listItem { get; set;}

        /// <summary>
        /// Private access point
        /// </summary>
        private Event()
        {
            Entries = new List<string[]>();
            _listItem = new HashSet<string[]>();
            Eventlogs = new List<EventLogs>();
        }

        /// <summary>
        /// Global access point
        /// </summary>
        public static IEvent Instance
        {
            get
            {
                if (_event is null)
                    NewInstance();

                return _event;
            }
        }
        
        /// <summary>
        /// Create new instance of the class
        /// </summary>
        private static void NewInstance()
        {
            _event = new Event { Keywords = new Keyword() };

            return;
        }

        /// <summary>
        /// Validates the file and assigns the property
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public IEvent SetLocation(FileInfo location)
        {
            if (location.Length == 0)
            {
                Messages.IncorrectLogSize();
                return this;
            }

            EventLocation = location;
            NewFile = true;

            return this;
        }

        /// <summary>
        /// Assigns the keywords instance
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
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

            Keywords.Counter = 0;
            Keywords.KeywordCounted = "";

            if (!string.IsNullOrEmpty(keyword))
            {
                Keywords.Counter = Count(keyword.Replace("count:", ""));
                Keywords.KeywordCounted = keyword.Replace("count:", "");
            }
        }

        private bool NewFileUsed()
        {
            if (NewFile)
            {
                return true;
            }

            return false;
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
            if (!(curId < Eventlogs.Count)) return Eventlogs[curId].Log;

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

        /// <summary>
        /// Checks if an item can be added to the property's list
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool CanAddListItem(string[] item)
        {
            return _listItem.Add(item);
        }
    }
}