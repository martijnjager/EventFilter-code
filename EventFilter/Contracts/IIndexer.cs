using EventFilter.Events;
using System;
using System.Collections.Generic;

namespace EventFilter.Contracts
{
    public interface IIndexer
    {
        string[] PrepareForMultipleLogs(List<string> files);

        bool NoEvents();

        Tuple<List<EventLog>, List<string>> Map();
    }
}