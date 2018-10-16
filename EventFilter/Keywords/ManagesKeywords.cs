using System.Collections.Generic;
using System.Windows.Forms;

namespace EventFilter.Keywords
{
    public partial class Keyword
    {
        // File and user input keywords
        public List<string> Items { get; private set; }

        // keywords loaded from file
        private List<string> KeywordsFromFile;

        public string[] ToArray() => Items.ToArray();

//        protected List<string> ToList(string value = "") => string.IsNullOrEmpty(value) ? Arr.DynamicToList(items, ",") : Arr.DynamicToList(value, ",");

        public string GetIndexedKeywords() => Arr.ToString(KeywordsFromFile, ", ");

        public string GetAllKeywords() => Arr.ToString(Items, ",");
        
        public List<string> GetKeywordsFromFile() => KeywordsFromFile;

        public void SetKeyword(string val)
        {
            // Assume the argument is a string
            string[] value = Arr.Explode(val, ", ");
            Arr.Trim(ref value);
            Items = Arr.ToList(value);
        }

        public void SetKeyword(string[] val)
        {
            Items = Arr.ToList(val);
        }

        public void DeleteKeywords() => Items = new List<string>();

        /// <summary>
        /// Add multiple values to the collection
        /// </summary>
        /// <param name="values">values</param>
        public void AddKeyword(params string[] values)
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
        public void AddKeyword(CheckedListBox clb)
        {
            /**
             * Used by the keywords loading function
             */

            foreach(string clbItem in clb.CheckedItems)
            {
                Add(clbItem);
            }

            //for (var i = 0; i < clb.Items.Count; i++)
            //{
            //    if (clb.GetItemCheckState(i) == CheckState.Checked)
            //    {
            //        Add(clb.Items[i].ToString());
            //    }
            //}
        }
        
        private void Add(string keyword)
        {
            Items.Add(keyword);
        }
    }
}