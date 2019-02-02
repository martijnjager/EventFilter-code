using System.Collections.Generic;
using System.Windows.Forms;

namespace EventFilter.Contracts
{
    public interface IManagesKeywords : IRefresh
    {
        List<string> Items { get; }

        //List<string> Operators { get; set; }

        string DateStart { get; }

        string DateEnd { get; }

        //void Set();

        void AddOperator(string o);

        void SetLocation();
        string GetAllKeywords();

        string GetIndexedKeywords();

        string FindOperator(string text);

        void Delete();

        void Add(params string[] values);

        void Add(CheckedListBox items);

        bool NoItems();

        bool Has(string keyword);
    }
}