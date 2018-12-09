using System.Collections.Generic;   
using System.ComponentModel;
using EventFilter.Contracts;
using System.Linq;
using System;
using System.Diagnostics;

namespace EventFilter.Events
{
    public static class SearchEvent
    {
        private static BackgroundWorker worker;


        public static void Search(object sender, DoWorkEventArgs e)
        {
            worker = sender as BackgroundWorker;

            try
            {
                /**
                 * Preparations before searching
                 */
                List<string> foundIds = new List<string>();
                int eventCounter = 0; // Counter for total found events
                int actionCounter = 0; // how many actions have been reported

                Event.Instance.MapEvents();
                Event.Instance.Keywords.Map();

                if (Event.Instance.Events.Count == 0 || Event.Instance.Keywords.Items.Count == 0) return;

                /**
                * We're good to search
                */
                Stopwatch watch = Stopwatch.StartNew();

                Report(0, Arr.ToString(Event.Instance.Keywords.Items, ", "), ref actionCounter);
                Report(1, Event.Instance.Events.Count, ref actionCounter);

                PerformSearch(Event.Instance, ref eventCounter, ref actionCounter, foundIds);

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
            }
            catch (Exception error)
            {
                worker.ReportProgress(0, "Log: Error: " + error.Message);
                Messages.ProblemOccured("searching events for keywords");
            }
        }

        private static void PerformSearch(IEvent instance, ref int eventCounter, ref int actionCounter, List<string> foundIds)
        {
            if (instance.Keywords.Items.Any(s => s.Contains("datestart")) || instance.Keywords.Items.Any(s => s.Contains("dateend")))
                instance.FilterDate();

            LoopThroughEvents(instance, ref eventCounter, ref actionCounter, foundIds);
        }

        private static void LoopThroughEvents(IEvent events, ref int eventCounter, ref int actionCounter, List<string> foundIds)
        {
            foreach(EventLogs eventlog in events.Eventlogs)
            {
                /**
                 * If description has ignorable keywords or no keywords at all
                 */
                if (events.With(eventlog.Description).HasNot(events.Keywords.Items) || events.With(eventlog.Description).Has(events.Keywords.Ignorable))
                    continue;

                eventCounter++;

                foundIds.Add(eventlog.Id);

                worker.ReportProgress(actionCounter++, "Event: " + eventlog.Date + " + " + eventlog.Description + " + " + eventlog.Id);
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
        public static void SearchEventBGWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            string text = e.UserState.ToString();
            string state = text.Substring(0, e.UserState.ToString().IndexOf(": "));

            switch (state)
            {
                case "Log":
                    Actions.Report(text.Replace("Log: ", ""));
                    break;

                case "Event":
                    Event.Instance.Entries.Add(Arr.Explode(text.Replace("Event: ", ""), " + "));
                    break;

                case "Time":
                    Actions.form.lblTime.Text = text.Replace("Time: ", "") + "s";
                    break;

                case "Counter":
                    Actions.SetResultCount(text.Replace("Counter: ", ""));
                    break;
            }
        }

        public static void SearchEventBGWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Actions.form.lbEventResult.Items.Clear();

            IEvent events = Event.Instance;

            foreach (string[] item in Event.Instance.Entries)
                if (Event.Instance.CanAddListItem(item))
                    Actions.AddListItem(item);

            events.IsCountOperatorUsed();

            if (events.Keywords.Counter != 0) Messages.KeywordCounted(events.Keywords.KeywordCounted, events.Keywords.Counter);

            Actions.form.lbEventResult.Sort();
        }
    }
}
