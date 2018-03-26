using System;
using System.Collections.Generic;
using System.Linq;
using EventFilter.Events.Engine.Contracts;
using System.Windows.Forms;
using EventFilter.Events.Engine;
using EventFilter.Keywords;

namespace EventFilter.Events
{
    public class FilterEvents : SearchEvent, IFilterEvents
    {
        public List<dynamic> FilteredEventId { get; set; }

        public List<dynamic> FilteredEventDate { get; set; }
        
        protected FilterEvents()
        {
            FilteredEventDate = new List<dynamic>();
            FilteredEventId = new List<dynamic>();
        }

        /// <summary>
        /// Filter duplicate events 
        /// </summary>
        /// <param name="description">description of events to Filter</param>
        /// <param name="id">ID of first line of description</param>
        /// <param name="date">date of description</param>
        /// <returns>List of non-duplicate events</returns>
        public List<dynamic> Filter(List<dynamic> description, List<dynamic> id, List<dynamic> date)
        {
            var tags = new HashSet<dynamic>();

            var localId = new List<dynamic>();
            var localDate = new List<dynamic>();

            for (var i = 0; i < date.Count; i++)
            {
                if (!tags.Add(description[i])) continue;

                localId.Add(id[i]);
                localDate.Add(date[i]);
            }

            FilteredEventId = localId;
            FilteredEventDate = localDate;

            return tags.ToList();
        }

        /// <summary>
        /// Find event on first description line ID
        /// </summary>
        /// <param name="events"></param>
        /// <param name="id">ID of first line in description</param>
        /// <returns></returns>
        public string FindEvent(List<dynamic> events, int id) => events[id];

        public void SortOnDescription(ListView lbEvent)
        {
            var index = new HashSet<dynamic>();

            for(int i = 0; i < lbEvent.Columns.Count; i++)
            {

            }
        }
    }
}
