using System.Collections.Generic;
using System.ComponentModel;
using EventFilter.Contracts;
using System.Linq;

namespace EventFilter.Events
{
    public static class SearchEvent
    {
        private static BackgroundWorker worker;

        public static void Search(object sender, DoWorkEventArgs e)
        {
            worker = sender as BackgroundWorker;

            //try
            //{
                Event.Instance.IndexEvents();

            List<string> keyword = Event.Instance.Keywords.Index(); // Keywords into array

                if (Event.Instance.Events.Count == 0 || keyword.Count == 0) return;

                /**
                * We're good to search
                */
                System.Diagnostics.Stopwatch watch = System.Diagnostics.Stopwatch.StartNew(); // Time

                int eventCounter = 0; // Counter for total found events
                int actionCounter = 0; // how many actions have been reported

                List<string> foundIds = new List<string>();

                Report(0, Arr.ToString(keyword, ", "), ref actionCounter);
                Report(1, Event.Instance.Events.Count, ref actionCounter);

                PerformSearch(ref eventCounter, ref actionCounter, foundIds);

                Report(2, eventCounter, ref actionCounter);
                Report(3, eventCounter, ref actionCounter);

                if (eventCounter == 0)
                {
                    Messages.NoEventLogHasKeyword();
                }

                watch.Stop();
                double elapsedTime = watch.Elapsed.TotalSeconds;

                Report(4, elapsedTime, ref actionCounter);

                e.Result = foundIds;
            //}
            //catch (Exception error)
            //{
            //    worker.ReportProgress(0, "Log: Error: " + error.Message);
            //    Messages.ProblemOccured("searching events for keywords");
            //}
        }
        public static void SearchEventBGWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.UserState.ToString().Contains("Log: "))
                Actions.Report(e.UserState.ToString().Replace("Log: ", ""));

            if (e.UserState.ToString().Contains("Event: "))
                Event.Instance.Entries.Add(Arr.Explode(e.UserState.ToString().Replace("Event: ", ""), " + "));

            if (e.UserState.ToString().Contains("Time: "))
                Actions.form.lblTime.Text = e.UserState.ToString().Replace("Time: ", "");

            if (e.UserState.ToString().Contains("Counter: "))
                Actions.SetResultCount(e.UserState.ToString().Replace("Counter: ", ""));
        }

        public static void SearchEventBGWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Actions.form.lbEventResult.Items.Clear();

            foreach (string[] item in Event.Instance.Entries)
                if (Event.Instance.ListItems.Add(item))
                    Actions.AddListItem(item);

            Event events = Event.Instance;

            events.CheckCountOperator();

            if (events.Keywords.Counter != 0) Messages.CountKeywords(events.Keywords.KeywordCounted, events.Keywords.Counter);

            Actions.form.lbEventResult.Sort();
        }

        private static void PerformSearch(ref int eventCounter, ref int actionCounter, List<string> foundIds)
        {
            FilterEventsOnDateKeywords(Event.Instance, Event.Instance.Eventlogs);

            LoopThroughEvents(Event.Instance, ref eventCounter, ref actionCounter, foundIds, Event.Instance.Eventlogs);
        }

        private static void LoopThroughEvents(IEvent events, ref int eventCounter, ref int actionCounter, List<string> foundIds, EventLogs[] eventlogs)
        {
            IKeywords keywords = events.Keywords;

            foreach(EventLogs eventlog in eventlogs)
            {
                string[] eventEntry = new string[3];

                /**
                 * If description has ignorable keywords or no keywords at all
                 */
                if (events.with(eventlog.Description).HasNot(keywords.Items) || events.with(eventlog.Description).Has(keywords.Ignorable))
                    continue;

                eventCounter++;

                eventEntry[0] = eventlog.Date;
                eventEntry[1] = eventlog.Description;
                eventEntry[2] = eventlog.Id;

                foundIds.Add(eventlog.Id);

                worker.ReportProgress(actionCounter++, "Event: " + eventlog.Date + " + " + eventlog.Description + " + " + eventlog.Id);
            }
        }

        private static void FilterEventsOnDateKeywords(IEvent events, EventLogs[] eventlogs)
        {
            List<string> keywords = events.Keywords.Items;

            if (keywords.Any(s => s.Contains("datestart")) || keywords.Any(s => s.Contains("dateend")))
            {
                events.FilterDate(keywords, eventlogs);
            }
        }

        private static string GetMessage(int index, dynamic data)
        {
            string[] events = new string[]
           {
            "Log: Parameters used: \t filepath: " + Event.Instance.EventLocation.FullName + "\n\t Keywords to use: ",
            "Log: Lines in eventArray: " + Event.Instance.Events.Count,
            "Log: \n\nEvents found: ",
            "Counter: ",
            "Time: Found results in: ",
            "Log: Error: "
           };

            return events[index] + data;
        }

        private static void Report(int index, dynamic data, ref int ActionCounter)
        {
            worker.ReportProgress(ActionCounter++, GetMessage(index, data));
        }
    }
}
