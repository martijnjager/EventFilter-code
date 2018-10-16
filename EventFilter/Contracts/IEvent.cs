using System.Collections.Generic;
using EventFilter.Events;
using System.IO;

namespace EventFilter.Contracts
{
    public interface IEvent : IFilterEvents, IEventIndex, IRefresh
    {
        EventLogs[] Eventlogs { get; }

        Event CheckCountOperator();

        Event with(string @event);

        List<string[]> Entries { get; }
        HashSet<string[]> ListItems { get; }

        FileInfo EventLocation { get; }

        Event SetKeywordObj(IKeywords keyword);

        void SetEventLocation(string location);
    }
}
