using System.Collections.Generic;
using System.Linq;
using System;

namespace EventFilter.Events
{
    public struct EventLog
    {
        public string Id;
        public DateTime Date;
        public string Description;
        public string Log;
        public bool IsPiracyEvent;
        public string RawEvent;

        public int GetId() => Id.ToInt();

        public bool Contains(List<string> items) => items.Any(Description.Contains);

        public bool Contains(string items) => items.Any(Description.Contains);

        public override string ToString()
        {
            return Log;
        }

        public void MarkPirateEvent()
        {
            IsPiracyEvent = true;
        }
    }
}
