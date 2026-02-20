using EventFilter.Contracts;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System;

namespace EventFilter.Events
{
    public partial class Event: IFilterEvents
    {
        /// <summary>
        /// Filter duplicate events 
        /// </summary>
        /// <returns>List of non-duplicate events</returns>
        public void Filter()
        {
            List<Tuple<int, EventLog>> e = new List<Tuple<int, EventLog>>();

            foreach (EventLog eventlog in GetFoundEvents())
            {
                if (!e.IncreaseCountIfDescriptionExists(eventlog))
                {
                    e.Add(new Tuple<int, EventLog>(1, new EventLog()
                    {
                        Id = eventlog.Id,
                        Date = eventlog.Date,
                        Description = eventlog.Description,
                        Log = eventlog.Log
                    }));
                }

            }

            FilteredEvents = e;
        }

        /// <summary>
        /// If date keywords present, filter event log
        /// </summary>
        public void FilterDate()
        {
            Eventlogs = FilterOnDate();
        }

        /// <summary>
        /// Find event on first description line ID
        /// </summary>
        /// <param name="id">ID of first line in description</param>
        /// <returns></returns>
        public EventLog FindEvent(int id) => Eventlogs[id];

        /// <summary>
        /// Filter events on date
        /// </summary>
        /// <returns>List of non-duplicate events</returns>
        private List<EventLog> FilterOnDate()
        {
            List<EventLog> results = new List<EventLog>();
            EventLog start = new EventLog();
            EventLog end = new EventLog();

            // Get the first match with DateStart
            if (Keyword.DateStart > DateTime.MinValue)
                start = FindClosestMatchingEvent(Keyword.DateStart);
            // Get the first match with DateEnd
            if (Keyword.DateEnd > DateTime.MinValue)
                end = FindClosestMatchingEvent(Keyword.DateEnd);

            if (start.Id == null && end.Id != null)
            {
                // Get the range
                var x = Eventlogs.ToList();
                results = x.GetRange(0, end.GetId());
            }

            if (start.Id != null && end.Id == null)
            {
                // Get the range
                results = Eventlogs.ToList().GetRange(start.GetId(), ((Eventlogs.Count - 1) - start.GetId()));
            }

            if (start.Id != null && end.Id != null)
            {
                // Get the range
                if (start.GetId() < end.GetId())
                    results = Eventlogs.ToList().GetRange(start.GetId(), ((end.GetId() - 1) - start.GetId()));
            }

            return results.Distinct().ToList();
        }

        public static void EventFilterBGWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            IEvent events = GetInstance();
            events.Filter();

            int progress;

            for (progress = 0; progress < events.GetFilteredEvents().Count - 1; progress++)
            {
                string[] data =
                {
                    events.GetFilteredEvents()[progress].Item2.Date.ToString(),
                    events.GetFilteredEvents()[progress].Item2.Description,
                    events.GetFilteredEvents()[progress].Item2.Id,
                    events.GetFilteredEvents()[progress].Item1.ToString()
                };

                worker.ReportProgress(progress, data);
            }

            worker.ReportProgress(progress++, "Resultcount: " + events.GetFoundEvents().Count + "\t, After filtering: " + (events.GetFilteredEvents().Count - 1));
        }

        public static void EventFilterBGWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 0)
            {
                Searcher.EventTable = new PaginationDataTable(1000, true);
                Searcher.EventTable.Columns.Add("Date");
                Searcher.EventTable.Columns.Add("Description");
                Searcher.EventTable.Columns.Add("ID", typeof(int));
                Searcher.EventTable.Columns.Add("Count");
            }

            if (!e.UserState.ToString().Contains("Resultcount:"))
            {
                // Cast the e.userstate as an IEnumerable to be able to cast it as an object
                // where we can select what we need and convert it to an array
                string[] items = ((IEnumerable)e.UserState).Cast<object>().Select(x => x.ToString()).ToArray();

                if (items.Length <= 1) return;

                for (int i = 0; i < items.Length; i++)
                {
                    items[i] = items[i].Trim('{').Trim('}');
                }

                Searcher.EventTable.Add(items);
            }
        }

        private EventLog FindClosestMatchingEvent(DateTime eventDate)
        {
            EventLog eventLog = Eventlogs.First();
            long minDiff = long.MaxValue;

            foreach (var e in Eventlogs)
            {
                if (e.Date.Equals(eventDate))
                {
                    eventLog = e;
                    break;
                }

                long diff = Math.Abs(eventDate.Ticks - e.Date.Ticks);

                if (diff < minDiff)
                {
                    minDiff = diff;
                    eventLog = e;
                }
            }

            return eventLog;
        }

        private dynamic FindClosestMatchingEventById(List<EventLog> foundEvents, int id, bool min = false)
        {
            EventLog closestEvent = Eventlogs.First();
            int closestIndex = -1;

            for (int i = 0; i < foundEvents.Count; i++)
            {
                var e = foundEvents[i];

                if ((min && e.GetId() < id) || (!min && e.GetId() > id))
                {
                    if ((min && e.GetId() > closestEvent.GetId()) || (!min && e.GetId() < closestEvent.GetId()))
                    {
                        closestEvent = e;
                        closestIndex = i;
                    }
                }
            }

            return new KeyValuePair<int, EventLog>(closestIndex, closestEvent);
        }
    }
}
