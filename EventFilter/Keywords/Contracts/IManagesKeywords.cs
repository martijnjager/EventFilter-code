using System.Collections.Generic;
using System.Windows.Forms;

namespace EventFilter.Keywords.Contracts
{
    public interface IManagesKeywords
    {
        string GetAllKeywords();

        string GetIndexed();

        List<string> ToList();

        IEnumerable<string> GetKeywordsFromFile();

        void DeleteKeywords();

        void AddKeyword(params string[] values);

        void AddKeyword(CheckedListBox clb);
    }
}