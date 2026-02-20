using EventFilter.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace EventFilter.Events
{
    public partial class Event : IFindKeywords
    {
        private string @event;

        /// <summary>
        /// Select event to validate keywords on
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public IEvent With(string action)
        {
            @event = action;

            return this;
        }

        /// <summary>
        /// When event has keywords
        /// </summary>
        /// <param name="input"></param>
        /// <returns>True if the event has the keyword, else false</returns>
        public bool Has(List<string> input)
        {
            return input.Any(@event.Contains);
        }

        /// <summary>
        /// When event does not have the keyword
        /// </summary>
        /// <param name="input"></param>
        /// <returns>True if the event doesn't have the keyword, else false</returns>
        public bool HasNot(List<string> input)
        {
            return !input.Any(@event.Contains);
        }
    }
}
