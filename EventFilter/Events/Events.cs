using System.Collections.Generic;

namespace EventFilter.Events
{
    public struct EventLogs
    {
        public string Id;
        public string Date;
        public string Description;
        public string Log;
    }

    public partial class Event
    {
        private void SetIndex()
        {
            for (int i = 0; i < Events.Count; i++)
            {
                AddEvent(Events[i], i);
            }
        }

        private void AddEvent(string eventlog, int index)
        {
            List<string> Event = SplitText(eventlog);
            HashSet<string> tmpDat = new HashSet<string>();

            string description = Event[13];
            string date = Event[3].Replace("Date", "");

            if(tmpDat.Add(date + ", " + description))
            {
                Eventlogs[index].Id = index.ToString();
                Eventlogs[index].Date = Event[3].Replace("Date:", "");
                Eventlogs[index].Description = Event[13];
                Eventlogs[index].Log = eventlog;
            }
        }

        private List<string> SplitText(string text)
        {
            return Arr.ToList(text, "\n");
        }
    }
}
