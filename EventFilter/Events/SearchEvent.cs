using System.Collections.Generic;   
using System.ComponentModel;
using EventFilter.Contracts;
using System;
using System.Diagnostics;
using EventFilter.Keywords;

namespace EventFilter.Events
{
    public static class SearchEvent
    {
        private static BackgroundWorker worker;

        private static readonly IEvent _event = Event.Instance;

        private static readonly IKeywords _keywords = Keyword.Instance;

        public static void Search(object sender, DoWorkEventArgs e)
        {
            worker = sender as BackgroundWorker;

            //try
            //{
                /**
                 * Preparations before searching
                 */
                List<string> foundIds = new List<string>();
                int eventCounter = 0; // Counter for total found events
                int actionCounter = 0; // how many actions have been reported

                _event.MapEvents();
                _keywords.Map();

                if (_event.NoEvents() || _keywords.NoItems()) return;

                /**
                * We're good to search
                */
                Stopwatch watch = Stopwatch.StartNew();

                Report(0, Arr.ToString(_keywords.Items, ", "), ref actionCounter);
                Report(1, _event.Events.Count, ref actionCounter);

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

        private static void PerformSearch(ref int eventCounter, ref int actionCounter, List<string> foundIds)
        {
            if (_keywords.Has("datestart") || _keywords.Has("dateend") )
                _event.FilterDate();

            LoopThroughEvents(ref eventCounter, ref actionCounter, foundIds);
        }

        private static void LoopThroughEvents(ref int eventCounter, ref int actionCounter, List<string> foundIds)
        {
            if(_event.Eventlogs.Count > 0)
            {
                foreach (EventLogs eventlog in _event.Eventlogs)
                {
                    /**
                     * If description has ignorable keywords or no keywords at all
                     */
                    if (_event.With(eventlog.Description).HasNot(_keywords.Items) || _event.With(eventlog.Description).Has(_keywords.Ignorable))
                        continue;

                    eventCounter++;

                    foundIds.Add(eventlog.Id);

                    worker.ReportProgress(actionCounter++, "Event: " + eventlog.Date + " + " + eventlog.Description + " + " + eventlog.Id);
                }
            }
        }

        private static string GetMessage(int index, dynamic data)
        {
            string[] events =
            {
                "Log: Parameters used: \t filepath: " + _event.EventLocation.FullName + "\n\t Keywords to use: ",
                "Log: Lines in eventArray: " + _event.Events.Count,
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
            string state = text.Substring(0, e.UserState.ToString().IndexOf(": ", StringComparison.Ordinal));

            switch (state)
            {
                case "Log":
                    Actions.Report(text.Replace("Log: ", ""));
                    break;

                case "Event":
                    _event.Entries.Add(Arr.Explode(text.Replace("Event: ", ""), " + "));
                    break;

                case "Time":
                    Actions.Form.lblTime.Text = text.Replace("Time: ", "") + "s";
                    break;

                case "Counter":
                    Actions.SetResultCount(text.Replace("Counter: ", ""));
                    break;
            }
        }

        public static void SearchEventBGWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Actions.Form.lbEventResult.Items.Clear();

            foreach (string[] item in _event.Entries)
                if (_event.CanAddListItem(item))
                    Actions.AddListItem(item);

            _event.IsCountOperatorUsed();

            if (_event.EventCounterForKeywords != 0) Messages.KeywordCounted(_keywords.KeywordToCount, _event.EventCounterForKeywords);

            Actions.Form.lbEventResult.Sort();
        }
    }
}
