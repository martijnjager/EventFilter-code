using System.Collections.Generic;
using System.Windows.Forms;

namespace EventFilter.Keywords
{
    public partial class Keyword
    {
        // File and user input keywords
        public List<string> Items { get; private set; }

        private List<string> _fileKeywords;

        public string[] ToArray() => Items.ToArray();

        public string GetIndexedKeywords() => Arr.ToString(_fileKeywords, ", ");

        public string GetAllKeywords() => Arr.ToString(Items, ",");
        
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
    }
}