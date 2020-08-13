using EventFilter.Events;
using System.Collections.Generic;
using System;

namespace EventFilter.Contracts
{
    public interface IFilterEvents
    {
        //IKeywords Keyword { get; }

        List<Tuple<int, EventLog>> GetFilteredEvents();

        void Filter();

        EventLog FindEvent(int id);

        void FilterDate();
    }
}