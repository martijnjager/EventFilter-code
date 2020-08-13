using EventFilter.Events;
using System.Collections.Generic;

namespace EventFilter.Contracts
{
    public interface IEventIndex
    {
        int EventIdentifier { get; set; }

        List<string> Events { get; set; }

        void MapEvents();

        dynamic GoToNext(int curId, EventLog[] logs = null, bool useFoundEvents = false);

        dynamic GoToPrevious(int curId, EventLog[] logs = null, bool useFoundEvents = false);

        string[] PrepareForMultipleLogs(List<string> files);

        bool NoEvents();
    }
}