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
        public List<dynamic> Operators { get; set; }
        
        /**
         * Indexes all operators, both used and unused operators
         */
        public List<string> AvailableOperators { get; set; }

        public string KeywordLocation { get; set; }

        public List<string> Ignorable { get; }

        public string[] ToArray() => Items.ToArray();

        public string GetIndexedKeywords() => Arr.ToString(_fileKeywords, ", ");

        public string GetAllKeywords() => Arr.ToString(Items, ",");

        public void SetLocation()
        {
            if (Actions.IsEmpty(KeywordLocation))
            {
                KeywordLocation = Bootstrap.CurrentLocation + @"\keywords.txt";
            }
        }
        
        public void Set(string val)
        {
            string[] value = Arr.Explode(val, ", ");
            Arr.Trim(ref value);
            Items = Arr.ToList(value);
        }

        public void Delete() => Items = new List<string>();

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
        public void Add(CheckedListBox clb)
        {
            /**
             * Used by the keywords loading function
             */

            foreach(string clbItem in clb.CheckedItems)
            {
                Add(clbItem);
            }
        }
        
        private void Add(string keyword)
        {
            Items.Add(keyword);
        }

        public bool NoItems()
        {
            if (Items.Count > 0)
                return false;

            return true;
        }

        public bool IsPresent(string keyword)
        {
            if (Items.Any(s => s.Contains(keyword)))
                return true;

            return false;
        }
    }
}