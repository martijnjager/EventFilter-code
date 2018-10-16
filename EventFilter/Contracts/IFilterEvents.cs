using System.Collections.Generic;
using EventFilter.Events;

namespace EventFilter.Contracts
{
    public interface IFilterEvents
    {
        IKeywords Keywords { get; }

        List<string> FilteredEventId { get; }
        
        List<string> FilteredEventDate { get; }

        List<string> FilteredEventDesc { get; }
        
        void Filter();

        string FindEvent(int id);

        void FilterDate(List<string> keyword, EventLogs[] eventlogs);
    }
}