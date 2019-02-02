using System.Collections.Generic;
using System.Windows.Forms;

namespace EventFilter.Contracts
{
    public interface IKeywords : IManagesKeywords, IRefresh
    {
        string KeywordToCount { get; set; }

        bool KeywordsLoaded { get; set; }

        string KeywordLocation { get; set; }

        List<string> Ignorable { get; }

        List<string> AvailableOperators { get; set; }

        IKeywords LoadFromLocation(string path = "");

        void LoadIntoCLB();

        void SaveToFile(string fileName, string keywords);

        IKeywords Map();


        //string DateStart { get; set; }

        //string DateEnd { get; set; }

        //List<string> Operators { get; }

        //string KeywordCounted { get; set; }

        //List<string> Ignorable { get; }

        //List<string> AvailableOperators { get; }

        //List<string> Items { get; }

        //string AllKeywords();

        //bool KeywordsLoaded { get; set; }

        //IKeywords LoadFromLocation(string path = "");

        //void LoadIntoCLB();

        //void SaveToFile(string fileName, string keywords);

        //IKeywords Map();

        //void SetLocation();

        //string IndexedKeywords();

        //void Add(params string[] values);

        //void Add(CheckedListBox clb);

        //bool NoItems();

        //bool Has(string keyword);
    }
}