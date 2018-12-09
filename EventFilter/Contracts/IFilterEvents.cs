using System.Collections.Generic;
using EventFilter.Events;

namespace EventFilter.Contracts
{
    public interface IFilterEvents
    {
        IKeywords Keywords { get; }

        List<EventLogs> FilteredEvents { get; }

        //List<string> FilteredEventId { get; }

        //List<string> FilteredEventDate { get; }

        //List<string> FilteredEventDesc { get; }

        IEvent Filter();

        string FindEvent(int id);

        IEvent FilterDate();
    }
}