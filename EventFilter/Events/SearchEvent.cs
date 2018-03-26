using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using EventFilter.Events.Engine;
using EventFilter.Events.Engine.Contracts;
using EventFilter.Keywords.Contracts;
using EventFilter.Keywords;

namespace EventFilter.Events
{
    public class SearchEvent : IndexEvent, ISearchEvent
    {
        public List<dynamic> FoundIds { get; protected set; }

        public List<dynamic> FoundEvents { get; protected set; }

        public List<dynamic> FoundDates { get; protected set; }

        public SearchEvent()
        {
            FoundDates = new List<dynamic>();
            FoundEvents = new List<dynamic>();
            FoundIds = new List<dynamic>();
        }
        
        public void Search(object sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorker;

            var eventCounter = 0; // Counter for total found events
            var actionCounter = 0;

            IndexEvents();

            //try
            //{
                
                var keyword = _keywords.Index(); // Keywords into array

                if (Description.Count == 0 || Id.Count == 0 || Dates.Count == 0 || keyword.Count == 0) return;
                
                worker.ReportProgress(actionCounter, "Log: Parameters used: \t filepath: " + Event.EventLocation + "\n\t Keywords to use: " + Arr.Implode(_keywords.GetAllKeywords(), ", "));
                worker.ReportProgress(actionCounter++, "Log: Lines in eventArray: " + Description.Count);

                var lastKeyword = keyword[0]; // lastKeyword logging, starting with first keyword

                worker.ReportProgress(actionCounter++, "Log: First lastKeyword: " + lastKeyword);

                var watch = System.Diagnostics.Stopwatch.StartNew(); // Time counters
                
                var tmpDescription = Description;
                var tmpDate = Dates;

                if (keyword.Any(s => s.Contains("datestart")) && keyword.Any(s => s.Contains("dateend")))
                {
                    FilterDate(keyword, ref tmpDescription, ref tmpDate);
                }

                ForEachDescription(worker, ref eventCounter, ref actionCounter, tmpDescription, tmpDate, keyword);

                worker.ReportProgress(actionCounter++, "Log: \n\nEvents found: " + eventCounter);
                worker.ReportProgress(actionCounter++, "Counter: Events found: " + eventCounter);

                if (eventCounter == 0)
                {
                    Messages.NoEventLogHasKeyword();
                }

                // Register time
                watch.Stop();
                var elapsedTime = watch.Elapsed.TotalSeconds;
                worker.ReportProgress(actionCounter, "Time: Found results in: " + elapsedTime);

                e.Result = FoundIds;
            //}
            //catch (Exception error)
            //{
            //    worker.ReportProgress(0, "Log: Error: " + error.Message);
            //    Messages.ProblemOccured("searching events for keywords");
            //}
        }

        /// <summary>
        /// For each event description, check if it has any given keywords
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="eventCounter"></param>
        /// <param name="actionCounter"></param>
        /// <param name="tmpDescription"></param>
        /// <param name="tmpDate"></param>
        private void ForEachDescription(BackgroundWorker worker, ref int eventCounter, ref int actionCounter, List<dynamic> tmpDescription, List<dynamic> tmpDate, dynamic keywords)
        {
            for (var i = 0; i < tmpDescription.Count; i++)
            {
                HasKeyword(worker, ref eventCounter, ref actionCounter, tmpDescription, tmpDate, keywords, i);
            }
        }

        /// <summary>
        /// Check if description has keyword
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="eventCounter"></param>
        /// <param name="actionCounter"></param>
        /// <param name="tmpDescription"></param>
        /// <param name="tmpDate"></param>
        /// <param name="i"></param>
        private void HasKeyword(BackgroundWorker worker, ref int eventCounter, ref int actionCounter, IReadOnlyList<dynamic> tmpDescription, IReadOnlyList<dynamic> tmpDate, dynamic keywords, int i)
        {
            var findKeyword = new Engine.Concerns.FindKeywords();

            var eventEntry = new string[3];

            /**
             * If description has keyword but NOT ignore keywords
             */
            if (findKeyword.HasKeyword(keywords, tmpDescription[i]) && findKeyword.HasIgnoreWord(keywords, tmpDescription[i]) == false)
            {
                eventCounter++;

                eventEntry[0] = tmpDate[i];
                eventEntry[1] = tmpDescription[i];
                eventEntry[2] = i.ToString();

                FoundDates.Add(eventEntry[0]);
                FoundEvents.Add(eventEntry[1]);
                FoundIds.Add(eventEntry[2]);

                worker.ReportProgress(actionCounter++, "Event: " + eventEntry[0] + " + " + eventEntry[1] + " + " + eventEntry[2]);
            }
        }

        /// <summary>
        /// If date keywords present, filter eventlog
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="tmpDescription"></param>
        /// <param name="tmpDate"></param>
        private void FilterDate(List<dynamic> keyword, ref List<dynamic> tmpDescription, ref List<dynamic> tmpDate)
        {
            if (!keyword.Any(s => s.Contains("datestart")) && !keyword.Any(s => s.Contains("dateend"))) return;

            var id = FilterOnDate();
            var description = new List<dynamic>();
            var date = new List<dynamic>();

            for (var i = 0; i < description.Count; i++)
            {
                if (id.All(s => s != i.ToString())) continue;
                description.Add(description[i]);
                date.Add(Dates[i]);
            }

            tmpDescription = description;
            tmpDate = date;
        }

        /// <summary>
        /// Filter events on date
        /// </summary>
        /// <returns>List of non-duplicate events</returns>
        private List<dynamic> FilterOnDate()
        {
            var results = new List<dynamic>();

            for (var i = 0; i < Dates.Count; i++)
            {
                if (_keywords.DateStart != null && (Dates[i].Contains(_keywords.DateStart) || DateTime.Parse(Dates[i]) > DateTime.Parse(_keywords.DateStart)))
                {
                    if (_keywords.DateEnd != null)
                    {
                        if (Dates[i].Contains(_keywords.DateEnd) != true || DateTime.Parse(Dates[i]) < DateTime.Parse(_keywords.DateEnd))
                        {
                            results.Add(i.ToString());
                        }
                    }

                    results.Add(i.ToString());
                }

                if (_keywords.DateEnd == null) continue;
                if (Dates[i].Contains(_keywords.DateEnd) != true || DateTime.Parse(Dates[i]) < DateTime.Parse(_keywords.DateEnd))
                {
                    results.Add(i.ToString());
                }
            }

            return results;
        }
    }
}
