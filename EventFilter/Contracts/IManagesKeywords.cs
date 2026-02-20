using System.Collections.Generic;
using System.Windows.Forms;

namespace EventFilter.Contracts
{
    public interface IManagesKeywords
    {
        List<string> Items { get; }
        List<string> Ignorable { get; }
        List<string> Piracy { get; }
        List<string> IgnorablePiracy { get; }
        List<string> ItemsFile { get; }
        List<string> IgnorableFile { get; }
        List<string> PiracyFile { get; }
        List<string> IgnorablePiracyFile { get; }


        bool HasItems();

        bool Has(string keyword);

        void Select(string keyword, bool state);

        string AllKeywordsToString();

        void Clear();

        void AddKeywordsFromFile(params string[] values);

        void AddPiracyKeywordsFromFile(params string[] values);

        void AddPiracyKeywords(params string[] values);

        void AddIgnorablePiracyKeywords(params string[] values);

        void AddIgnorableKeywords(params string[] values);

        void AddKeywords(params string[] values);

        void ClearNonFileKeywords();
    }
}