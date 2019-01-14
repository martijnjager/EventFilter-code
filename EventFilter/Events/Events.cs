using System.Collections.Generic;
using System.Linq;

namespace EventFilter.Events
{
    public partial class Event
    {
        private void AddToIndex(HashSet<string> array, string text)
        {
            List<string> Event = SplitText(text);

            if(Event.Count >= 13)
            {
                int index = int.Parse(Event[0].Replace("Event[", "").Replace("]:", ""));

                string description = GetDescription(Event);

                string date = Event[3].Replace("Date: ", "");

                if (array.Add(date + ", " + description))
                {
                    EventLogs @event = new EventLogs
                    {
                        Id = index.ToString(),
                        Date = date,
                        Description = description,
                        Log = text
                    };
                    Eventlogs.Add(@event);
                }

                Events.Add(text);
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
