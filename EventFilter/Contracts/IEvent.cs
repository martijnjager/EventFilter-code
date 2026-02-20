using EventFilter.Events;
using System.Collections.Generic;
using System.IO;

namespace EventFilter.Contracts
{
    public interface IEvent : IFilterEvents, IFindKeywords
    {
        int EventIdentifier { get; set; }

        List<EventLog> Eventlogs { get; }

        void SetLocation(string location);

        int CountableCounted { get; set; }

        bool CanAddListItem(string[] item);

        List<EventLog> GetFoundEvents();

        dynamic GoToNext(int curId, EventLog[] logs = null, bool useFoundEvents = false);

        dynamic GoToPrevious(int curId, EventLog[] logs = null, bool useFoundEvents = false);

        bool HasEvents();

        bool HasPiracyEvents();

        void Index();
    }
}
