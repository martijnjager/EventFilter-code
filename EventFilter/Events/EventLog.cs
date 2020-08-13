using System.Collections.Generic;
using System.Linq;

namespace EventFilter.Events
{
    public struct EventLog
    {
        public string Id;
        public string Date;
        public string Description;
        public string Log;

        public int GetId() => Id.ToInt();

        public void SetId(int id)
        {
            Id = id.ToString();
        }

        public bool Contains(List<string> items) => items.Any(Description.Contains);

        public bool Contains(string items) => items.Any(Description.Contains);

        public override string ToString()
        {
            return Log;
        }
    }
}
