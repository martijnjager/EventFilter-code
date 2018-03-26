using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using EventFilter.Keywords.Contracts;

namespace EventFilter.Keywords.Concerns
{
    public class ManagesKeywords
    {
        // File and user input keywords
        protected List<string> AllKeywords;

        // keywords loaded from file
        protected List<string> KeywordsFromFile;

        /// <summary>
        /// GetAllKeywords() returns a List<char> array, need to convert to string explicitly.
        /// </summary>
        /// <returns>Keywords in List<string></returns>
        public List<string> ToList() => Arr.StringToList(GetAllKeywords(), ",");
        
        private static List<string> ToList(dynamic value) => Arr.DynamicToList(value, ",");

        public string GetIndexed() => Arr.Implode(KeywordsFromFile, ", ");

        public string GetAllKeywords() => Arr.Implode(AllKeywords, ",");
        
        public IEnumerable<string> GetKeywordsFromFile() => KeywordsFromFile;

        protected internal void SetKeyword(string val) => AllKeywords = ToList(Arr.Trim(Arr.Explode(val, ",")));

        public void DeleteKeywords() => AllKeywords = new List<string>();

        private void SetKeyword(dynamic key) => SetKeyword((string) Arr.Implode(key, ", "));

        /// <summary>
        /// Add multiple values to the collection
        /// </summary>
        /// <param name="values">values</param>
        public void AddKeyword(params string[] values)
        {
            foreach (var str in values)
            {
                AllKeywords.Add(str);
            }
        }

        /// <summary>
        /// Add multiple values to the collection
        /// </summary>
        /// <param name="clb"></param>
        public void AddKeyword(CheckedListBox clb)
        {
            for (var i = 0; i < clb.Items.Count; i++)
            {
                if (clb.GetItemCheckState(i) == CheckState.Checked)
                {
                    AllKeywords.Add(clb.Items[i].ToString());
                }
            }
        }
        
    }
}