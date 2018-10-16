using System.Collections.Generic;

namespace EventFilter.Events.Contracts
{
    public interface IFilterEvents : ISearchEvent
    {
        List<dynamic> FilteredEventId { get; }
        
        List<dynamic> FilteredEventDate { get; }
        
        List<dynamic> Filter();

        string FindEvent(int id);
    }
}