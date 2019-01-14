using System.Collections.Generic;

namespace EventFilter.Contracts
{
    public interface IEventIndex
    {
        int EventIdentifier { get; set; }

        List<string> Events { get; set; }

        void MapEvents();
        
        string Next(int curId);

        string Previous(int curId);

        string[] PrepareForMultipleLogs(List<string> files);

        bool NoEvents();
    }
}