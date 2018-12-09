using System.Collections.Generic;
using System.Linq;

namespace EventFilter.Events
{
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

            string description = GetDescription(Event);
            
            string date = Event[3].Replace("Date: ", "");

            if(tmpDat.Add(date + ", " + description))
            {
                Eventlogs[index].Id = index.ToString();
                Eventlogs[index].Date = date;
                Eventlogs[index].Description = description;
                Eventlogs[index].Log = eventlog;
            }
        }

        private string GetDescription(List<string> Event)
        {
            string description = "";

            if (Event.Count - 1 > 12)
            {
                int range = Event.Count - 12;
                description = Arr.ToString(Event.GetRange(12, range), "\r").Replace("Description: ", "").Trim();
            }
            else
                description = Event[12].Replace("Description: ", "").Trim();

            return description;
        }

        private List<string> SplitText(string text)
        {
            return Arr.ToList(text, "\n");
        }
    }
}
