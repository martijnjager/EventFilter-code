using System.Collections.Generic;
using EventFilter.Events;
using System.IO;

namespace EventFilter.Contracts
{
    public interface IEvent : IFilterEvents, IEventIndex, IFindKeywords
    {
        EventLogs[] Eventlogs { get; }

        IEvent IsCountOperatorUsed();

        List<string[]> Entries { get; }
        bool CanAddListItem(string[] item);

        FileInfo EventLocation { get; }

        IEvent SetKeywordInstance(IKeywords keyword);

        IEvent SetLocation(FileInfo location);
    }
}
