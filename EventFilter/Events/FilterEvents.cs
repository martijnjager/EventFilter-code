using EventFilter.Contracts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;

namespace EventFilter.Events
{
    public partial class Event : IFilterEvents
    {
        /// <summary>
        /// Filter duplicate events 
        /// </summary>
        /// <returns>List of non-duplicate events</returns>
        public void Filter()
        {
            HashSet<string> tags = new HashSet<string>();
            List<EventLog> e = new List<EventLog>();

            GetFoundEvents().ForEach(x =>
            {
                if (!tags.Add(x.Description))
                    return;

                e.Add(new EventLog() { Id = x.Id, Date = x.Date, Description = x.Description, Log = x.Log });
            });

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
            if (!Keyword.DateStart.IsEmpty())
                start = FindClosestMatchingEvent(Keyword.DateStart);
            // Get the first match with DateEnd
            if (!Keyword.DateEnd.IsEmpty())
                end = FindClosestMatchingEvent(Keyword.DateEnd);

            if (start.Id == null && end.Id != null)
            {
                // Get the range
                var x = Eventlogs.ToList();
                results = x.GetRange(0, int.Parse(end.Id));
            }

            if (start.Id != null && end.Id == null)
            {
                // Get the range
                results = Eventlogs.ToList().GetRange(int.Parse(start.Id), ((Eventlogs.Count - 1) - int.Parse(start.Id)));
            }

            if (start.Id != null && end.Id != null)
            {
                // Get the range
                if (int.Parse(start.Id) < int.Parse(end.Id))
                    results = Eventlogs.ToList().GetRange(int.Parse(start.Id), ((int.Parse(end.Id) - 1) - int.Parse(start.Id)));
            }

            return results.Distinct().ToList();
        }

        public static void eventFilterBGWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            IEvent events = GetInstance();
            events.Filter();

            int progress;

            for (progress = 0; progress < events.FilteredEvents.Count - 1; progress++)
            {
                string[] data =
                {
                    GetInstance().FilteredEvents[progress].Date,
                    events.FilteredEvents[progress].Description,
                    events.FilteredEvents[progress].Id
                };

                worker.ReportProgress(progress, data);
            }

            //string s = Helper.Form.lblResultCount.Text.Substring(14, Helper.Form.lblResultCount.Text.Length - 1);

            worker.ReportProgress(progress++, "Resultcount: " + events.GetFoundEvents().Count + "\t, After filtering: " + (events.FilteredEvents.Count - 1));
        }

        public static void eventFilterBGWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if(e.ProgressPercentage == 0)
                SearchEvent.EventTable.Rows.Clear();

            if (!e.UserState.ToString().Contains("Resultcount:"))
            {
                // Cast the e.userstate as an IEnumerable to be able to cast it as an object where we can select what we need and convert it to an array
                string[] items = ((IEnumerable)e.UserState).Cast<object>().Select(x => x.ToString()).ToArray();

                if (items.Length <= 1) return;

                for (int i = 0; i < items.Length; i++)
                    items[i] = items[i].Trim('{').Trim('}');

                SearchEvent.EventTable.Rows.Add(items);
            }
            else
                Helper.SetResultCount(e.UserState.ToString().Replace("Resultcount: ", "").Trim('{').Trim('}'));
        }

        private EventLog FindClosestMatchingEvent(string eventDate)
        {
            SortedList<long, EventLog> data = new SortedList<long, EventLog>();

            Eventlogs.ForEach(e => data.Add((eventDate.ToDate().Ticks - e.Date.ToDate().Ticks), e));

            return data.First().Value;
        }

        private dynamic FindClosestMatchingEventById(List<EventLog> foundEvents, int id, bool min = false)
        {
            SortedList<int, EventLog> data = new SortedList<int, EventLog>();

            for (int i = 0; i < foundEvents.Count; i++)
            {
                var e = foundEvents[i];

                if (min)
                    if (e.GetId() < id)
                        data.Add(i, e);
                if (!min)
                    if (e.GetId() > id)
                        data.Add(i, e);
            }

            if (min)
                return data.Last();

            return data.First();
        }
    }
}
