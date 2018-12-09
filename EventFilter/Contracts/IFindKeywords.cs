using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventFilter.Contracts
{
    public interface IFindKeywords
    {
        IEvent With(string action);

        bool Has(List<string> input);

        bool HasNot(List<string> input);
    }
}
