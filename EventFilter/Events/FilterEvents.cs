using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.ComponentModel;

namespace EventFilter.Events
{
    public partial class Event
    {
        public List<string> FilteredEventId { get; private set; }

        public List<string> FilteredEventDesc { get; private set; }

        public List<string> FilteredEventDate { get; private set; }
        
        /// <summary>
        /// Filter duplicate events 
        /// </summary>
        /// <returns>List of non-duplicate events</returns>
        public void Filter()
        {
            HashSet<string> tags = new HashSet<string>();

            List<string> localId = new List<string>();
            List<string> localDate = new List<string>();

            foreach (var entry in Entries)
            {
                if (!tags.Add(entry[1])) continue;

                localId.Add(entry[2]);
                localDate.Add(entry[0]);
            }

            FilteredEventId = localId;
            FilteredEventDate = localDate;
            FilteredEventDesc = tags.ToList();
        }



        /// <summary>
        /// If date keywords present, filter eventlog
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="tmpDescription"></param>
        /// <param name="tmpDate"></param>
        public void FilterDate(List<string> keyword, EventLogs[] eventlogs)
        {
            if (!Keywords.Items.Any(s => s.Contains("datestart")) && !keyword.Any(s => s.Contains("dateend"))) return;

            List<string> id = FilterOnDate();
            List<string> description = new List<string>();
            List<string> date = new List<string>();

            foreach(EventLogs eventlog in eventlogs)
            {
                if(id.Any(s => s == eventlog.Id))
                {
                    description.Add(eventlog.Description);
                    date.Add(eventlog.Date);
                }
            }

            //for (var i = 0; i < eventlogs.Length; i++)
            //{
            //    if(id.Any(s => s == i.ToString()))
            //    {
            //        description.Add(eventlogs[i].Description);
            //        date.Add(eventlogs[i].Date);
            //    }
            //}

            //tmpDescription = description.ToArray();
            //tmpDate = date.ToArray();
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
        private List<string> FilterOnDate()
        {
            List<string> results = new List<string>();

            foreach(EventLogs eventlog in Eventlogs)
            {
                if (string.IsNullOrEmpty(Keywords.DateEnd))
                    continue;

                if(!string.IsNullOrEmpty(Keywords.DateStart) && (eventlog.Date.Contains(Keywords.DateStart) 
                    || DateTime.Parse(eventlog.Date) > DateTime.Parse(Keywords.DateStart)))
                {
                    HasEndDateOrNotPassedStartDate(eventlog, results);

                    results.Add(eventlog.Id);
                }

                if(!eventlog.Date.Contains(Keywords.DateEnd) || DateTime.Parse(eventlog.Date) < DateTime.Parse(Keywords.DateEnd)){
                    results.Add(eventlog.Id);
                }
            }

            //for (var i = 0; i < Dates.Length; i++)
            //{
            //    if (Keywords.DateStart != null && (Dates[i].Contains(Keywords.DateStart) || DateTime.Parse(Dates[i]) > DateTime.Parse(Keywords.DateStart)))
            //    {
            //        HasEndDateOrNotPassedStartDate(i, results);

            //        results.Add(i.ToString());
            //    }

            //    if (Keywords.DateEnd == null) continue;

            //    if (Dates[i].Contains(Keywords.DateEnd) != true || DateTime.Parse(Dates[i]) < DateTime.Parse(Keywords.DateEnd))
            //    {
            //        results.Add(i.ToString());
            //    }
            //}

            return results;
        }

        private bool EndDateNotNull()
        {
            return Keywords.DateEnd != null;
        }

        private void HasEndDateOrNotPassedStartDate(EventLogs eventlogs, ICollection<string> results)
        {
            if (!EndDateNotNull())
                return;

            if (eventlogs.Date.Contains(Keywords.DateEnd) != true || DateTime.Parse(eventlogs.Date) < DateTime.Parse(Keywords.DateEnd))
            {
                results.Add(eventlogs.Id);
            }
        }

        public static void eventFilterBGWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            Instance.Filter();

            int progress;

            for (progress = 0; progress < Instance.FilteredEventDesc.Count; progress++)
            {
                string[] data =
                {
                    "Data: " + Instance.FilteredEventDate[progress],
                    "Data: " + Instance.FilteredEventDesc[progress],
                    "Data: " + Instance.FilteredEventId[progress]
                };

                worker.ReportProgress(progress, data);
            }

            worker.ReportProgress(++progress, "Resultcount: Events found: " + Actions.form.lblResultCount.Text.Substring(Actions.form.lblResultCount.Text.Length - 1, 1) + "\t, After filtering: " + Instance.FilteredEventId.Count);
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

        //        public void SortOnDescription(ListView lbEvent)
        //        {
        //            var index = new List<dynamic>();
        //
        //            for(int i = 0; i < lbEvent.Columns.Count; i++)
        //            {
        //
        //            }
        //        }
    }
}
