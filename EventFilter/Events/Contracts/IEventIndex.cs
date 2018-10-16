using System.Collections.Generic;

namespace EventFilter.Events.Contracts
{
    public interface IEventIndex
    {
        int _eventIdentifier { get; set; }
        
        List<dynamic> Dates { get; set; }
        
        List<dynamic> Description { get; set; }
        
        List<dynamic> Id { get; set; }
        
        List<dynamic> Events { get; set; }
        
        List<dynamic> EventArray { get; set; }
        
        void IndexEvents();
        
        string Next(int curId);

        string Previous(int curId);
    }
}