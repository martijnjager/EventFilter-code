using System.Collections.Generic;
using System.Windows.Forms;

namespace EventFilter.Contracts
{
    public interface IManagesKeywords : IRefresh
    {
        List<string> Items { get; }
        string GetAllKeywords();

        string GetIndexedKeywords();

        string[] ToArray();

        void Delete();

        void Add(params string[] values);

        void Add(CheckedListBox clb);

        bool NoItems();

        bool IsPresent(string keyword);
    }
}