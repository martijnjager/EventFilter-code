using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventFilter.Events.Contracts
{
    public interface IEvent : IFilterEvents, ISearchEvent, IEventIndex
    {
        void CheckCountOperator();

        void Reset();

        bool IsIndexed();
    }
}
