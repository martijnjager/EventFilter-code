using System.Windows.Forms;

namespace EventFilter.Contracts
{
    public interface IKeywords : IManagesKeywords
    {
        string KeywordToCount { get; set; }

        bool KeywordsLoaded { get; set; }

        IKeywords LoadFromLocation(string path = "");

        void Into(CheckedListBox clb);

        void Map();
    }
}