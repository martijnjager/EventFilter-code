using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace EventFilter.Contracts
{
    public interface IKeywords
    {
        string Countable { get; }

        bool KeywordsLoaded { get; }

        List<string> Items { get; }
        List<string> Piracy { get; }
        List<string> Ignorable { get; }
        List<string> IgnorablePiracy { get; }
        List<string> ItemsFile { get; }
        List<string> IgnorableFile { get; }
        List<string> PiracyFile { get; }
        List<string> IgnorablePiracyFile { get; }

        DateTime DateStart { get; }
        DateTime DateEnd { get; }
        void Select(string keyword, bool state);

        IKeywords LoadFromLocation(string path = "");

        void AddInto(CheckedListBox clb);

        bool HasItems();

        bool Has(string keyword);

        void Add(params string[] values);

        void MapFromFile(CheckedListBox.CheckedItemCollection clb);

        string AllKeywordsToString();

        void Map(List<string> textboxItems);
    }
}