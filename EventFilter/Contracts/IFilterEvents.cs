using System.Collections.Generic;
using EventFilter.Events;

namespace EventFilter.Contracts
{
    public interface IFilterEvents
    {
        //IKeywords Keywords { get; }

        List<EventLogs> FilteredEvents { get; }

        IEvent Filter();

        string FindEvent(int id);

        IEvent FilterDate();
    }
}