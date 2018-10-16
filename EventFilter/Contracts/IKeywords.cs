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

        //string GetIndexed();

        //string GetAllKeywords();

        //List<string> ToList();

        //void DeleteKeywords();

        //void AddKeyword(params string[] values);

        //void AddKeyword(CheckedListBox clb);

        void LoadKeywordsFromLocation(string path = "");

        List<string> Index();
    }
}