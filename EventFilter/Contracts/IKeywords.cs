using System.Collections.Generic;

namespace EventFilter.Contracts
{
    public interface IKeywords : IManagesKeywords
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

        IKeywords LoadFromLocation(string path = "");

        void LoadIntoCLB();

        void SaveToFile(string fileName, string keywords);

        IKeywords Map();
    }
}