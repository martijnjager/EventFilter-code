using System.Collections.Generic;

namespace EventFilter.Contracts
{
    public interface IFindKeywords
    {
        IEvent With(string action);

        bool Has(List<string> input);

        bool HasNot(List<string> input);
    }
}
