using EventFilter.Contracts;
using EventFilter.Keywords;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;

namespace EventFilter.Events
{
    public static class Searcher
    {
        private static BackgroundWorker worker;

        private static IEvent _event;

        private static IKeywords _keywords;

        public static PaginationDataTable EventTable;

        public static void SetupTable()
        {
            EventTable = new PaginationDataTable(1000);
            EventTable.Columns.Add("Date");
            EventTable.Columns.Add("Description");
            EventTable.Columns.Add("ID", typeof(int));
        }

        public static void Search(object sender, DoWorkEventArgs e)
        {
            worker = sender as BackgroundWorker;

            try
            {
                /**
                 * Preparations before searching
                 */

                List<string> foundIds = new List<string>();
                var inputArguments = e.Argument as Tuple<List<string>>;
                int actionCounter = 0; // how many actions have been reported
                int eventsCounted = 0;
                SetupTable();

                _keywords = Keyword.GetInstance();
                _event = Event.GetInstance();
                _keywords.Map(inputArguments.Item1);
                _event.Index();

                if (_event.Eventlogs.Count < 1) return;

                /**
                * We're good to search
                */
                Stopwatch watch = Stopwatch.StartNew();

                Report(0, Arr.ToString(_keywords.Items, ", "), ref actionCounter);
                Report(1, _event.Eventlogs.Count, ref actionCounter);

                PerformSearch(ref eventsCounted, ref actionCounter, foundIds);

                Report(2, eventsCounted, ref actionCounter);
                Report(3, eventsCounted, ref actionCounter);

                if (eventsCounted == 0)
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
                // Ensure exceptions (including those re-thrown from Indexer) are caught and reported
                worker.ReportProgress(0, "Log: Error: " + error.Message);
                // We cannot use UI method Messages.ProblemOccured here safely if it uses MessageBox on non-UI thread,
                // but Messages implementation suggests it uses MessageBox which should be on UI thread.
                // However, worker.ReportProgress is safe.
                // Let's assume Messages handles UI thread marshalling or is called after completion?
                // Actually Messages.ProblemOccured shows MessageBox. Showing MessageBox from BackgroundWorker is bad practice but works (blocks worker).
                // But better to let the worker fail gracefully?
                // The original code had it here.
                Messages.ProblemOccured("searching events for keywords: " + error.Message);
            }
        }

        private static void PerformSearch(ref int eventsCounted, ref int actionCounter, List<string> foundIds)
        {
            if (_keywords.Has("datestart") || _keywords.Has("dateend"))
                _event.FilterDate();

            LoopThroughEvents(ref eventsCounted, ref actionCounter, foundIds);
        }

        private static void LoopThroughEvents(ref int eventsCounted, ref int actionCounter, List<string> foundIds)
        {
            if (_event.Eventlogs.Count <= 0)
            {
                worker.ReportProgress(actionCounter++, "Log: There is no eventlog to search through");
                Messages.ProblemOccured("searching through the events, there appears to be no event present");
            }

            if (!_keywords.HasItems())
            {
                foreach (EventLog eventlog in _event.Eventlogs)
                {
                    worker.ReportProgress(actionCounter++, "Event: " + eventlog.Date + " ||| " + eventlog.Description + " ||| " + eventlog.Id);
                }

                return;
            }

            foreach (EventLog eventlog in _event.Eventlogs)
            {
                if (!eventlog.Contains(_keywords.IgnorablePiracy) && eventlog.Contains(_keywords.Piracy))
                {
                    eventlog.MarkPirateEvent();
                    worker.ReportProgress(actionCounter++, "Log: Piracy is detected in " + eventlog.Log + "\n\n");
                    worker.ReportProgress(actionCounter++, "Piracy: Piracy has been detected in one or more events.");
                }

                if (!string.IsNullOrEmpty(_keywords.Countable) && eventlog.Log.Contains(_keywords.Countable))
                {
                    _event.CountableCounted++;
                }

                /**
                * If description has ignorable keywords or no keywords at all
                */
                if (_event.With(eventlog.Description).HasNot(_keywords.Items) || _event.With(eventlog.Description).Has(_keywords.Ignorable))
                    continue;

                foundIds.Add(eventlog.Id);

                eventsCounted++;

                worker.ReportProgress(actionCounter++, "Event: " + eventlog.Date + " ||| " + eventlog.Description + " ||| " + eventlog.Id);
            }
        }

        private static string GetMessage(int index, dynamic data)
        {
            string[] events =
            {
                "Log: Parameters used: \t filepath: " + Event.FileLocation.FullName + "\n\t Keywords to use: ",
                "Log: Lines in eventArray: " + _event.Eventlogs.Count,
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
            if (e.ProgressPercentage == 0)
            {
                EventTable.Rows.Clear();
            }

            string text = e.UserState.ToString();
            string state = text.Substring(0, e.UserState.ToString().IndexOf(": ", StringComparison.Ordinal));

            switch (state)
            {
                case "Event":
                    string[] t = text.Replace("Event:", "").Explode(" ||| ");
                    if (!_event.CanAddListItem(t))
                        break;

                    EventTable.Add(t);
                    break;
            }
        }

        public static void SearchEventBGWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!string.IsNullOrEmpty(_keywords.Countable))
            {
                Messages.KeywordCounted(_keywords.Countable, _event.CountableCounted);
            }
        }
    }
}
