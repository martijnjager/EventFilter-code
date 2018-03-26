using System.ComponentModel;

namespace EventFilter.Events.Engine.Contracts
{
    interface ISearchEvent
    {
        void Search(object sender, DoWorkEventArgs e);
    }
}