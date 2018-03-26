using System.Collections.Generic;

namespace EventFilter.Events.Engine.Contracts
{
    internal interface IFilterEvents
    {
        List<dynamic> FilteredEventId { get; set; }
        
        List<dynamic> FilteredEventDate { get; set; }
        
        List<dynamic> Filter(List<dynamic> description, List<dynamic> id, List<dynamic> date);

        string FindEvent(List<dynamic> events, int id);
    }
}