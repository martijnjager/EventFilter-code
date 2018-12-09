using System.Collections.Generic;

namespace EventFilter.Contracts
{
    public interface IKeywords : IManagesKeywords, IRefresh
    {
        string DateStart { get; }
        string DateEnd { get; }

        bool KeywordsLoaded { get; set; }

        List<dynamic> Operators { get; set; }

        int Counter { get; set; }

        string KeywordCounted { get; set; }

        string KeywordLocation { get; set; }

        List<string> Ignorable { get; }
        
        List<string> AvailableOperators { get; set; }

        IKeywords Instance { get; }

        void LoadFromLocation(string path = "");

        void SaveToFile(string fileName, string keywords);

        IKeywords Map();
    }
}