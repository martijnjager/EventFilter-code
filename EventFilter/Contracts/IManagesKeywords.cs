using System.Collections.Generic;

namespace EventFilter.Contracts
{
    public interface IManagesKeywords : IRefresh
    {
        Dictionary<string, List<string>> Keywords { get; }

        List<string> Items { get; }

        List<string> Piracy { get; }

        List<string> Ignorable { get; }

        List<string> IgnorablePiracy { get; }

        string DateStart { get; }

        string DateEnd { get; }

        string GetAllKeywords();

        bool NoItems();

        bool Has(string keyword);

        void SaveKeywords(string keywords, string piracy);
    }
}