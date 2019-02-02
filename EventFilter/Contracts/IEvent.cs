using System.Collections.Generic;
using EventFilter.Events;
using System.IO;

namespace EventFilter.Contracts
{
    public interface IEvent : IFilterEvents, IEventIndex, IFindKeywords
    {
        List<EventLogs> Eventlogs { get; }

        List<string[]> Entries { get; }

        FileInfo EventLocation { get; }

        int EventCounterForKeywords { get; set; }

        IEvent IsCountOperatorUsed();

        bool CanAddListItem(string[] item);

        //IEvent SetKeywordInstance(IKeywords keyword);

        IEvent SetLocation(FileInfo location);
    }
}
