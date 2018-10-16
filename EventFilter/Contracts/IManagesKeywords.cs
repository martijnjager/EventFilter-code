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

        List<string> GetKeywordsFromFile();

        void DeleteKeywords();

        void AddKeyword(params string[] values);

        void AddKeyword(CheckedListBox clb);
    }
}