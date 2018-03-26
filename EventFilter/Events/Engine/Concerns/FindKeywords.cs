using System.Collections.Generic;
using System.Linq;
using EventFilter.Keywords;
using EventFilter.Keywords.Contracts;

namespace EventFilter.Events.Engine.Concerns
{
    class FindKeywords
    {
        /// <summary>
        /// Check if a keyword is present in an event
        /// </summary>
        /// <param name="keywords"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public bool HasKeyword(List<dynamic> keywords, string description)
        {
            if(keywords.Any(item => description.Contains(item)))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Check if event contains keyword with operator
        /// 
        /// Returns true if text contains -{keyword}
        /// </summary>
        /// <param name="keywords"></param>
        /// <param name="text">event</param>
        public bool HasIgnoreWord(List<dynamic> keywords, string text)
        {
            var eventToAdd = new List<dynamic>();

            foreach (var key in keywords.ToList())
            {
                HasIgnoreOperator(text, key, eventToAdd);
            }

            if (eventToAdd.Contains("operator"))
                return true;

            return false;
        }

        private static void HasIgnoreOperator(string text, string key, ICollection<dynamic> events)
        {
            if (!key.Contains('-'))
            {
                events.Add("");

                return;
            }

            HasTextIgnoreOperator(text, key, events);
        }

        private static void HasTextIgnoreOperator(string text, string key, ICollection<dynamic> events)
        {
            key = key.Trim('-');

            if (text.Contains(key))
            {
                events.Add("operator");
            }
        }
    }
}
