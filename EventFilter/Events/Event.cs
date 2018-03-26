using System;
using System.Collections.Generic;
using System.Linq;

namespace EventFilter.Events
{
    public class Event : FilterEvents
    {
        private static Event _instance;
        private static readonly object Lock = new object();

        private Event()
        {
            //IndexEvents();
        }

        public static Event Instance
        {
            get
            {
                lock (Lock)
                {
                    return _instance ?? (_instance = new Event());
                }
            }
        }

        public static void ResetProperties()
        {
            //            FilteredEventDate = new List<dynamic>();
            //            FilteredEventDescription = new List<dynamic>();
            //            FilteredEventId = new List<dynamic>();

            var _event = new Event
            {
                FoundDates = new List<dynamic>(),
                FoundEvents = new List<dynamic>(),
                FoundIds = new List<dynamic>(),
                Dates = new List<dynamic>(),
                Description = new List<dynamic>(),
                Id = new List<dynamic>(),
                EventArray = new List<dynamic>(),
                Events = new List<dynamic>()
            };

        }

        /// <summary>
        /// Check Keywords on valid Operators
        /// </summary>
        public void CheckCountOperator()
        {
            IEnumerable<dynamic> keywords = _keywords.Index();
            _keywords.Operators = new List<dynamic>();

            foreach(string key in keywords)
            {
                if(_keywords.availableOperators.Any(key.Contains))
                {
                    _keywords.Operators.Add(key);
                }
            }

            Count();
        }

        /// <summary>
        /// Count how many times the given keyword is present in the log
        /// </summary>
        private void Count()
        {
            string keyword = _keywords.Operators.Find(s => s.Contains("count:"));

            if (!string.IsNullOrEmpty(keyword))
            {
                _keywords.Counter = Count(keyword.Replace("count:", ""));
                _keywords.KeywordCounted = keyword.Replace("count:", "");
            }
            else
            {
                _keywords.Counter = 0;
                _keywords.KeywordCounted = "";
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

            foreach (string line in Events)
            {
                count = CountValue(line, value, count);
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