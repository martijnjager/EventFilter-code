using System.Collections.Generic;
using System.Windows.Forms;
using EventFilter.Contracts;
using System.Linq;

namespace EventFilter.Keywords
{
    public partial class Keyword
    {
        // File and user input keywords
        public List<string> Items { get; private set; }

        private List<string> _fileKeywords;

        public string DateStart { get; private set; }

        public string DateEnd { get; private set; }

        /**
         * Indexes the used operators
         */
        private List<string> _operators;

        /**
         * Indexes all operators, both used and unused operators
         */
        public List<string> AvailableOperators { get; set; }

        public string KeywordLocation { get; set; }

        public List<string> Ignorable { get; private set; }

        public string GetIndexedKeywords() => Arr.ToString(_fileKeywords, ", ");

        public string GetAllKeywords() => Arr.ToString(Items, ",");

        private void Set(string val)
        {
            string[] value = Arr.Explode(val, ", ");
            Arr.Trim(ref value);
            Items = Arr.ToList(value);
        }

        public void Delete()
        {
            Items = new List<string>();
            Ignorable = new List<string>();
            _operators = new List<string>();
            AvailableOperators = new List<string>();
        }

        /// <summary>
        /// Add multiple values to the collection
        /// </summary>
        /// <param name="values">values</param>
        public void Add(params string[] values)
        {
            /**
             * Used by the global app
             */
            foreach (string str in values)
            {
                Add(str);
            }
        }

        /// <summary>
        /// Add multiple values to the collection
        /// </summary>
        /// <param name="clb"></param>
        public void Add(CheckedListBox items)
        {
            /**
             * Used by the keywords loading function
             */
            foreach (string item in items.CheckedItems)
            {
                Add(item);
            }
        }

        private void Add(string keyword)
        {
            Items.Add(keyword);
        }

        public string FindOperator(string text) => _operators.Find(s => s.StartsWith(text));

        public void AddOperator(string o) => _operators.Add(o);

        public bool NoItems()
        {
            if (Items.Count > 0)
                return false;

            return true;
        }

        public bool Has(string keyword)
        {
            if (Items.Any(s => s.Contains(keyword)))
                return true;

            return false;
        }

        private void AddOperators()
        {
            foreach (string item in Items)
            {
                if (item.StartsWith("-"))
                {
                    Ignorable.Add(item.Trim('-'));
                }

                if (item.StartsWith("dateend:"))
                {
                    DateEnd = item.Substring(item.IndexOf(':') + 1);
                }

                if (item.StartsWith("datestart:"))
                {
                    DateStart = item.Substring(item.IndexOf(':') + 1);
                }
            }
        }
    }
}