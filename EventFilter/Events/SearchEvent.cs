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
    public static class SearchEvent
    {
        private static BackgroundWorker worker;

        private static IEvent _event;

        private static IKeywords _keywords;

        public static DataTable EventTable;

        public static void SetupTable()
        {
            EventTable = new DataTable();
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
            int eventCounter = 0; // Counter for total found events
            int actionCounter = 0; // how many actions have been reported
            SetupTable();
            _keywords = Keyword.GetInstance();
            _event = Event.GetInstance();
            _event.MapEvents();
            _keywords.Map();

            if (_event.NoEvents()) return;

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
                Messages.NoEventLogHasKeyword();

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

        private static void PerformSearch(ref int eventCounter, ref int actionCounter, List<string> foundIds)
        {
            if (_keywords.Has("datestart") || _keywords.Has("dateend"))
                _event.FilterDate();

            LoopThroughEvents(ref eventCounter, ref actionCounter, foundIds);
        }

        private static void LoopThroughEvents(ref int eventCounter, ref int actionCounter, List<string> foundIds)
        {
            if (_event.Eventlogs.Count <= 0)
            {
                worker.ReportProgress(actionCounter++, "Log: There is no eventlog to search through");
                Messages.ProblemOccured("searching through the events, there appears to be no event present");
            }

            if (_keywords.NoItems())
            {
                foreach (EventLog eventlog in _event.Eventlogs)
                {
                    ++eventCounter;

                    worker.ReportProgress(actionCounter++, "Event: " + eventlog.Date + " ||| " + eventlog.Description + " ||| " + eventlog.Id);
                }
            }
            else
            {
                foreach (EventLog eventlog in _event.Eventlogs)
                {
                    if (!eventlog.Contains(_keywords.IgnorablePiracy) && eventlog.Contains(_keywords.Piracy))
                    {
                        _event.PiracyEvents.Add(eventlog);
                        worker.ReportProgress(actionCounter++, "Log: Piracy is detected in " + eventlog.Log + "\n\n");
                        worker.ReportProgress(actionCounter++, "Piracy: Piracy has been detected in one or more events.");
                    }

                    /**
                    * If description has ignorable keywords or no keywords at all
                    */
                    if (_event.With(eventlog.Description).HasNot(_keywords.Items) || _event.With(eventlog.Description).Has(_keywords.Ignorable))
                        continue;

                    ++eventCounter;

                    foundIds.Add(eventlog.Id);

                    worker.ReportProgress(actionCounter++, "Event: " + eventlog.Date + " ||| " + eventlog.Description + " ||| " + eventlog.Id);
                }
            }
        }

        private static string GetMessage(int index, dynamic data)
        {
            string[] events =
            {
                "Log: Parameters used: \t filepath: " + _event.FileLocation.FullName + "\n\t Keywords to use: ",
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
            if (e.ProgressPercentage == 0)
            {
                EventTable.Rows.Clear();
                Helper.Form.dataGridView1.DataSource = SearchEvent.EventTable;
            }

            string text = e.UserState.ToString();
            string state = text.Substring(0, e.UserState.ToString().IndexOf(": ", StringComparison.Ordinal));

            switch (state)
            {
                case "Log":
                    Helper.Report(text.Replace("Log: ", ""));
                    break;

                case "Event":
                    string[] t = text.Replace("Event:", "").Explode(" ||| ");
                    if (!_event.CanAddListItem(t))
                        break;

                    Helper.AddListItem(EventTable, t);
                    break;

                case "Time":
                    Helper.Form.lblTime.Text = text.Replace("Time: ", "") + "s";
                    break;

                case "Counter":
                    Helper.SetResultCount(text.Replace("Counter: ", ""));
                    break;

                case "Piracy":
                    Helper.Form.lblKMS.Text = text.Replace("Piracy:", "");
                    Helper.Form.lblKMS.ForeColor = Color.Red;
                    break;
            }
        }

        public static void SearchEventBGWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (_event.PiracyEvents.Count > 0)
            {
                Helper.Form.linklblPiracy.Visible = true;
                Helper.Form.lblKMS.Visible = true;
            }
            else
            {
                Helper.Form.lblKMS.Visible = false;
                Helper.Form.linklblPiracy.Visible = false;
            }

            _event.IsCountOperatorUsed();

            if (_event.EventCounterForKeywords == 0)
                return;

            Messages.KeywordCounted(_keywords.KeywordToCount.Trim("count:"), _event.EventCounterForKeywords);
        }
    }
}
