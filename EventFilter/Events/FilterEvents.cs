using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.ComponentModel;
using EventFilter.Contracts;

namespace EventFilter.Events
{
    public partial class Event
    {
        /// <summary>
        /// Filter duplicate events 
        /// </summary>
        /// <returns>List of non-duplicate events</returns>
        public IEvent Filter()
        {
            HashSet<string> tags = new HashSet<string>();
            List<EventLogs> e = new List<EventLogs>();

            Eventlogs.ForEach(x =>
            {
                if(tags.Add(x.Date + " " + x.Description))
                {
                    e.Add(new EventLogs() { Id = x.Id, Date = x.Date, Description = x.Description, Log = x.Log });
                }
            });

            FilteredEvents = e;

            return this;
        }

        /// <summary>
        /// If date keywords present, filter event log
        /// </summary>
        public IEvent FilterDate()
        {
            Eventlogs = FilterOnDate();

            return this;
        }

        /// <summary>
        /// Find event on first description line ID
        /// </summary>
        /// <param name="id">ID of first line in description</param>
        /// <returns></returns>
        public string FindEvent(int id) => Eventlogs[id].Log;

        /// <summary>
        /// Filter events on date
        /// </summary>
        /// <returns>List of non-duplicate events</returns>
        private List<EventLogs> FilterOnDate()
        {
            List<EventLogs> results = new List<EventLogs>();
            EventLogs start = new EventLogs();
            EventLogs end = new EventLogs();

            // Get the first match with DateStart
            if (Keywords.DateStart != null)
                start = Eventlogs.FirstOrDefault(s => s.Date.Contains(Keywords.DateStart));
            // Get the first match with DateEnd
            if (Keywords.DateEnd != null)
                end = Eventlogs.FirstOrDefault(s => s.Date.Contains(Keywords.DateEnd));

            if(start.Id == null && end.Id != null)
            {
                // Get the range
                var x = Eventlogs.ToList();
                results = x.GetRange(0, int.Parse(end.Id));
            }

            if(start.Id != null && end.Id == null)
            {
                // Get the range
                results = Eventlogs.ToList().GetRange(int.Parse(start.Id), ((Eventlogs.Count - 1) - int.Parse(start.Id)));
            }

            if(start.Id != null && end.Id != null)
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

            Instance.Filter();

            int progress;

            for (progress = 0; progress < Instance.FilteredEvents.Count; progress++)
            {
                string[] data =
                {
                    "Data: " + Instance.FilteredEvents[progress].Date,
                    "Data: " + Instance.FilteredEvents[progress].Description,
                    "Data: " + Instance.FilteredEvents[progress].Id
                };

                worker.ReportProgress(progress, data);
            }

            worker.ReportProgress(++progress, "Resultcount: Events found: " + Actions.Form.lblResultCount.Text.Substring(Actions.Form.lblResultCount.Text.Length - 1, 1) + "\t, After filtering: " + Instance.FilteredEvents.Count);
        }

        public static void eventFilterBGWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.UserState.ToString().Contains("Resultcount: ") == false)
            {
                // Cast the e.userstate as an IEnumerable to be able to cast it as an object where we can select what we need and convert it to an array
                string[] items = ((IEnumerable)e.UserState).Cast<object>().Select(x => x.ToString()).ToArray();

                if (items.Length <= 1) return;

                for (int i = 0; i < items.Length; i++)
                {
                    items[i] = items[i].Replace("Data: ", "").Trim('{').Trim('}');
                }

                Actions.AddListItem(items);
            }
            else
                Actions.SetResultCount(e.UserState.ToString().Replace("Resultcount: ", "").Trim('{').Trim('}'));
        }
    }
}
