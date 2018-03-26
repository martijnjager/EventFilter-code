using System;
using System.Collections.Generic;
using EventFilter.Events.Engine;
using EventFilter.Events.Engine.Contracts;
using EventFilter.Keywords.Contracts;
using EventFilter.Keywords;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;

namespace EventFilter.Events
{
    public class IndexEvent : IEventIndex
    {
        public IKeywords _keywords { get; }

        public int _eventIdentifier { get; set; }

        public static string EventLocation { get; set; }

        public List<dynamic> Dates { get; set; }
        public List<dynamic> Description { get; set; }
        public List<dynamic> Id { get; set; }
        public List<dynamic> Events { get; set; }
        public List<dynamic> EventArray { get; set; }

        protected internal IndexEvent()
        {
            Dates = new List<dynamic>();
            Description = new List<dynamic>();
            Id = new List<dynamic>();
            Events = new List<dynamic>();
            EventArray = new List<dynamic>();

            _keywords = Keyword.Instance;
        }

        /// <summary>
        /// Skip current event and get Next event
        /// </summary>
        /// <param name="curId">ID of first line of description of current event</param>
        /// <returns></returns>
        public string Next(int curId)
        {
            if (!(curId < Events.Count)) return Events[curId];
            
            _eventIdentifier = curId + 1;
            return Events[curId + 1];
        }

        /// <summary>
        /// Skip current event and get Previous event
        /// </summary>
        /// <param name="curId">ID of first line of description of current event</param>
        /// <returns>string</returns>
        public string Previous(int curId)
        {
            if (!(curId > 0)) return Events[curId];

            _eventIdentifier = curId - 1;             
            return Events[curId - 1];
        }

        /// <summary>
        /// Index log so we know what it contains
        /// </summary>
        public void IndexEvents()
        {
            Event.ResetProperties();

            if(EventLocation.Contains(".evtx"))
            {
                CreateFromEvtx(EventLocation);
            }
            else
            {
                CreateEventList(EventLocation);
            }
        }

        /// <summary>
        /// Create eventlog from location
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        private void CreateEventList(string location)
        {
            var lines = File.ReadAllLines(location, Encodings.CurrentEncoding);

            EventArray = lines.Cast<dynamic>().ToList();

            for (int i = 0; i < EventArray.Count; i++)
            {
                IndexDates(i);

                IndexDescription(i);
            }

            MakeEvents();

            EventArray = new List<dynamic>();
        }

        private void CreateFromEvtx(string location)
        {
            using (var reader = new EventLogReader(location, PathType.FilePath))
            {
                EventRecord record;

                int counter = 0;
                while ((record = reader.ReadEvent()) != null)
                {
                    counter++;
                    using (record)
                    {
                        Id.Add(counter);
                        Dates.Add(DateTime.Parse(record.TimeCreated.ToString()).ToString("yyyy-MM-ddTHH:mm:ss"));
                        Description.Add(record.FormatDescription());

                        if (counter == 500)
                        {
                            
                        }
                    }
                }
            }
        }

        private void IndexDates(int i)
        {
            if (EventArray[i].Contains("Date:"))
            {
                Dates.Add(EventArray[i].Substring(EventArray[i].IndexOf(':') + 1));
            }
        }

        private void IndexDescription(int i)
        {
            if (EventArray[i].Contains("Description:"))
            {
                int counter = 1;
                Id.Add((i + counter).ToString());

                Description.Add(IndexDescription(i, counter));
            }
        }

        private string IndexDescription(int count, int counter)
        {
            string text = "";

            while ((count + counter) < EventArray.Count && EventArray[count + counter].Contains("Event[") != true)
            {
                text += EventArray[count + counter];
                counter++;
            }

            return text;
        }

        private void MakeEvents()
        {
            for (int i = 0; i < EventArray.Count; i++)
            {
                int count = 0;
                string text = "";
                while ((i + count + 1) < EventArray.Count && EventArray[i + count + 1].Contains("Event[") != true)
                {
                    text += EventArray[i + count] + "\n";
                    count++;
                }

                i += count;

                Events.Add(text);
            }
        }
    }
}
