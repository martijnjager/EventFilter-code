using System.ComponentModel;
using System.Collections.Generic;
using EventFilter.Keywords.Contracts;

namespace EventFilter.Events.Contracts
{
    public interface ISearchEvent : IEventIndex
    {
        IKeywords Keywords { get; }
        
        List<dynamic> FoundIds { get; }

        List<dynamic> FoundEvents { get; }

        List<dynamic> FoundDates { get; }

        //void Search(object sender, DoWorkEventArgs e);

        void FilterDate(List<dynamic> keyword, ref List<dynamic> tmpDescription, ref List<dynamic> tmpDate);
    }
}