using EventFilter.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace EventFilter.Keywords
{
    public partial class Keyword : IManagesKeywords
    {
        // File and user input keywords

        public Dictionary<string, List<string>> Keywords { get; private set; }

        //private List<string> _fileKeywords;

        public string DateStart { get; private set; }

        public string DateEnd { get; private set; }

        public List<string> Piracy { get => Keywords["Piracy"]; }

        public List<string> Items { get => Keywords["Items"]; }

        public List<string> Ignorable { get => Keywords["Ignorable"]; }

        public List<string> IgnorablePiracy { get => Keywords["IgnorablePiracy"]; }

        /**
         * Indexes the used operators
         */
        //private readonly List<string> _operators;

        /**
         * Indexes all operators, both used and unused operators
         */
        //public List<string> AvailableOperators => _operators;

        //public string KeywordLocation { get; set; }

        //public string GetIndexedKeywords() => Arr.ToString(_fileKeywords, ", ");

        public string GetAllKeywords()
        {
            string items = Items.ToString(", ");
            string piracy = Piracy.ToString(", ");
            string ignorable = Ignorable.ToString(", ");
            string ignorablePiracy = IgnorablePiracy.ToString(", ");

            return items + "\n\tIgnorable:\t" + ignorable + "\n\tPiracy:\t" + piracy + "\n\tPiracy ignorable\t" + ignorablePiracy + "\n\n";
        }


        /// <summary>
        /// Add multiple values to the collection
        /// </summary>
        /// <param name="clb"></param>
        public void Set(CheckedListBox.CheckedItemCollection items)
        {
            /**
             * Used by the keywords loading function
             */
            foreach (string item in items)
            {
                AddToRelevantList(item);
            }
        }

        private void AddToRelevantList(string item)
        {
            if (item.StartsWith("P: "))
                AddPiracy(item.Trim("P: "));

            if (item.StartsWith("-P: "))
                AddIgnorablePiracy(item.Trim("-P: "));

            if (!item.StartsWith("-P: ") && item.StartsWith("-"))
                AddIgnorable(item.Trim("-"));

            if (item.StartsWith("-") || item.StartsWith("P: "))
                return;

            AddItem(item);
        }

        private void CheckWhereToAddAndAdd(string item)
        {
            if (item.StartsWith("-", System.StringComparison.Ordinal))
                AddIgnorable(item);
            else
                AddItem(item);
        }

        private void Set(string val, string target)
        {
            List<string> vals = val.Explode(", ").Trim().ToList<string>();

            if (target == "Items")
            {
                vals.ForEach(item =>
                {
                    if (item.StartsWith("-", System.StringComparison.Ordinal))
                        AddIgnorable(item.Trim("-"));
                    else
                        AddItem(item);
                });
            }

            if (target == "Piracy")
            {

                vals.ForEach(item =>
                {
                    if (item.StartsWith("-"))
                        AddIgnorablePiracy(item.Trim("-P: "));
                    else
                        AddPiracy(item.Trim("P: "));
                });
            }
        }

        public void Refresh()
        {
            Keywords = new Dictionary<string, List<string>>()
            {
                ["Items"] = new List<string>(),
                ["Ignorable"] = new List<string>(),
                ["Piracy"] = new List<string>(),
                ["IgnorablePiracy"] = new List<string>()
            };
        }

        private void AddItem(string item)
        {
            Items.Add(item);
        }

        private void AddIgnorable(string item)
        {
            Ignorable.Add(item);
        }

        private void AddIgnorablePiracy(string item)
        {
            IgnorablePiracy.Add(item);
        }

        private void AddPiracy(string item)
        {
            Piracy.Add(item);
        }

        public void Add(params string[] values)
        {
            foreach (string str in values)
            {
                CheckWhereToAddAndAdd(str);
            }
        }

        public bool NoItems() => Items.Count <= 0;

        public bool Has(string keyword)
        {
            foreach (string item in Items)
            {
                if (item.Contains(keyword))
                    return true;
            }

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