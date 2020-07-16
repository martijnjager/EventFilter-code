using EventFilter.Events;
using System.Collections.Generic;

namespace EventFilter.Contracts
{
    public interface IFilterEvents
    {
        //IKeywords Keyword { get; }

        List<EventLog> FilteredEvents { get; }

        void Filter();

        EventLog FindEvent(int id);

        void FilterDate();
    }
}