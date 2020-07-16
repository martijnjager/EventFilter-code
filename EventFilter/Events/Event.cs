using EventFilter.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EventFilter.Events
{
    public sealed partial class Event : IEvent
    {
        /// <summary>
        /// Instance of the class
        /// </summary>
        private static Event _event;

        /// <summary>
        /// Stores all events filtered on duplicates
        /// </summary>
        public List<EventLog> FilteredEvents { get; private set; }

        public List<EventLog> PiracyEvents { get; private set; }

        /// <summary>
        /// Instance of the Keywords class
        /// </summary>
        private IKeywords Keyword;

        /// <summary>
        /// ID for message Form
        /// </summary>
        public int EventIdentifier { get; set; }

        /// <summary>
        /// Stores information about the file being used
        /// </summary>
        public FileInfo FileLocation { get; private set; }

        /// <summary>
        /// Stores all events
        /// </summary>
        public List<string> Events { get; set; }

        /// <summary>
        /// Stores all events, using the EventLogs struct allows the app to search/filter easier
        /// </summary>
        public List<EventLog> Eventlogs { get; private set; }

        /// <summary>
        /// Property ensures that the list items are unique 
        /// No duplicate entries can be added
        /// </summary>
        private HashSet<string[]> ListItem { get; }

        public int EventCounterForKeywords { get; set; }

        /// <summary>
        /// Private access point
        /// </summary>
        private Event(IKeywords keywords)
        {
            PiracyEvents = new List<EventLog>();
            ListItem = new HashSet<string[]>();
            Eventlogs = new List<EventLog>();
            this.Keyword = keywords;
        }

        /// <summary>
        /// Global access point
        /// </summary>
        public static IEvent GetInstance()
        {
            if (_event is null)
                NewInstance();

            return _event;
        }

        public List<EventLog> GetFilteredEvents() => FilteredEvents;


        /// <summary>
        /// Create new instance of the class
        /// </summary>
        private static void NewInstance()
        {
            _event = new Event(Keywords.Keyword.GetInstance());
        }

        /// <summary>
        /// Validates the file and assigns the property
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public void SetLocation(string location)
        {
            if (location.IsEmpty())
                Messages.IncorrectLogSize();
            else
            {
                if (File.Exists(location))
                    FileLocation = new FileInfo(location);
            }
        }

        public FileInfo GetLocation() => FileLocation;

        /// <summary>
        /// Check Keywords on valid Operators
        /// </summary>
        public IEvent IsCountOperatorUsed()
        {
            //string text = Arr.ToString(Keywords.AvailableOperators, "|");

            //foreach(string key in Keywords.Items)
            //{
            //    if(Regex.IsMatch(key, @"^(" + text +" )"))
            //    {
            //        Keywords.AddOperator(key);
            //    }
            //}

            Count();

            return this;
        }

        /// <summary>
        /// Count how many times the given keyword is present in the log
        /// </summary>
        private void Count()
        {
            string keyword = Keyword.KeywordToCount;

            if (!keyword.IsEmpty())
                EventCounterForKeywords = Count(keyword.Replace("count:", ""));
        }

        /// <summary>
        /// Count how many times a value is present
        /// </summary>
        /// <param name="value">value to Search for</param>
        /// <returns>string Count of value</returns>
        private int Count(string value)
        {
            int count = 0;

            foreach (EventLog e in Eventlogs)
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
        public dynamic GoToNext(int curId, EventLog[] logs, bool useFoundEvents = false)
        {
            if(useFoundEvents)
            {
                List<EventLog> e = GetFoundEvents();
                var eve = e.Where(x => x.GetId().Equals(curId)).FirstOrDefault();
                if (!(eve.Log is string))
                {
                    var result = this.FindClosestMatchingEventById(e, curId, false);

                    return result;
                }

                for (int i = 0; i < e.Count; i++)
                {
                    if (e[i].GetId() == curId)
                    {
                        if (i + 1 < e.Count)
                            i++;

                        return (i, e[i]);
                    }
                }
            }

            if (logs != null)
            {
                for (int index = 0; index < logs.Length; index++)
                {
                    if (logs[index].GetId() == curId)
                    {
                        if (index + 1 > logs.Length - 1)
                            return logs[index];

                        EventIdentifier = logs[index + 1].GetId();

                        return Eventlogs[EventIdentifier];
                    }
                }
            }

            if (curId >= Eventlogs.Count - 1)
                return Eventlogs[curId];

            EventIdentifier = curId + 1;

            return Eventlogs[EventIdentifier];
        }

        /// <summary>
        /// Skip current event and get Previous event
        /// </summary>
        /// <param name="curId">ID of first line of description of current event</param>
        /// <returns>string</returns>
        public dynamic GoToPrevious(int curId, EventLog[] logs = null, bool useFoundEvents = false)
         {
            if (useFoundEvents)
            {
                List<EventLog> e = GetFoundEvents();
                var eve = e.Where(x => x.GetId().Equals(curId)).FirstOrDefault();
                if (!(eve.Log is string))
                {
                    var result = this.FindClosestMatchingEventById(e, curId, true);

                    return result;
                }

                for (int i = 0; i < e.Count; i++)
                {
                    if (e[i].GetId() == curId)
                    {
                        if (i - 1 >= 0)
                            --i;

                        return (i, e[i]);
                    }
                }
            }

            if (logs != null)
            {
                if (curId == 0)
                    return logs[curId];

                EventIdentifier = curId - 1;
                return logs[EventIdentifier];
            }

            EventIdentifier = Eventlogs.GetByIdMinusOne(curId).GetId();

            return Eventlogs[EventIdentifier];
        }

        /// <summary>
        /// Checks if an item can be added to the property's list
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool CanAddListItem(string[] item)
        {
            return ListItem.Add(item);
        }

        public List<EventLog> GetFoundEvents()
        {
            List<EventLog> foundItems = new List<EventLog>();

            foreach(string[] x in ListItem)
            {
                foundItems.Add(new EventLog() 
                {
                    Date = x[0],
                    Description = x[1],
                    Id = x[2],
                    Log = this.Eventlogs[x[2].ToInt()].Log
                });
            }

            return foundItems;
        }
    }
}