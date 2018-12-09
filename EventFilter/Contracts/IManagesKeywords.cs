using System.Collections.Generic;
using System.Windows.Forms;

namespace EventFilter.Contracts
{
    public interface IManagesKeywords
    {
        List<string> Items { get; }
        string GetAllKeywords();

        string GetIndexedKeywords();

        string[] ToArray();

        //List<string> GetKeywordsFromFile();

        void Delete();

        void Add(params string[] values);

        void Add(CheckedListBox clb);
    }
}