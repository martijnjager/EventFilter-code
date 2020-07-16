using EventFilter.Events;
using System.Collections.Generic;
using System.IO;

namespace EventFilter.Contracts
{
    public interface IEvent : IFilterEvents, IEventIndex, IFindKeywords
    {
        void SetLocation(string location);

        List<EventLog> PiracyEvents { get; }

        //IKeywords Keyword { get; }

        List<EventLog> Eventlogs { get; }

        //List<string[]> Entries { get; }

        FileInfo FileLocation { get; }

        int EventCounterForKeywords { get; set; }

        IEvent IsCountOperatorUsed();

        bool CanAddListItem(string[] item);

        //IEvent SetKeywordInstance(IKeywords keyword);

        List<EventLog> GetFoundEvents();
    }
}
