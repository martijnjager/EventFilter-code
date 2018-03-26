using System.Collections.Generic;
using System.Windows.Forms;

namespace EventFilter.Keywords.Contracts
{
    public interface IKeywords : IManagesKeywords
    {
        string DateStart { get; }
        string DateEnd { get; }

        List<dynamic> Operators { get; set; }

        int Counter { get; set; }

        string KeywordCounted { get; set; }
        
        List<string> availableOperators { get; set; }

        //string GetIndexed();

        //string GetAllKeywords();

        //List<string> ToList();

        //void DeleteKeywords();

        //void AddKeyword(params string[] values);

        //void AddKeyword(CheckedListBox clb);

        void LoadKeywordsFromLocation(string path = "");

        List<dynamic> Index();
    }
}