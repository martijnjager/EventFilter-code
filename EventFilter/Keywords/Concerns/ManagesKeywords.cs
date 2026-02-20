using EventFilter.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Windows.Forms;

namespace EventFilter.Keywords.Concerns
{
    public class ManagesKeywords : IManagesKeywords
    {
        // File and user input keywords
        public KeywordType[] AllKeywords { get; private set; }

        public List<string> Items => AllKeywords[ItemsIndex].Keywords;

        public List<string> Ignorable => AllKeywords[IgnorableIndex].Keywords;

        public List<string> Piracy => AllKeywords[PiracyIndex].Keywords;

        public List<string> IgnorablePiracy => AllKeywords[IgnorablePiracyIndex].Keywords;

        public List<string> ItemsFile => AllKeywords[ItemsIndexFile].Keywords;

        public List<string> IgnorableFile => AllKeywords[IgnorableIndexFile].Keywords;

        public List<string> PiracyFile => AllKeywords[PiracyIndexFile].Keywords;
        
        public List<string> IgnorablePiracyFile => AllKeywords[IgnorablePiracyIndexFile].Keywords;

        private int PiracyIndex = 0;
        private int ItemsIndex = 1;
        private int IgnorableIndex = 2;
        private int IgnorablePiracyIndex = 3;
        private int PiracyIndexFile = 4;
        private int ItemsIndexFile = 5;
        private int IgnorableIndexFile = 6;
        private int IgnorablePiracyIndexFile = 7;

        public ManagesKeywords()
        {
            // Initialize the array and preset the types
            this.AllKeywords = new KeywordType[8];
            this.AllKeywords[PiracyIndex] = new KeywordType(Type.Piracy, false);
            this.AllKeywords[ItemsIndex] = new KeywordType(Type.Items, false);
            this.AllKeywords[IgnorableIndex] = new KeywordType(Type.Ignorable, false);
            this.AllKeywords[IgnorablePiracyIndex] = new KeywordType(Type.IgnorablePiracy, false);
            this.AllKeywords[PiracyIndexFile] = new KeywordType(Type.Piracy, true);
            this.AllKeywords[ItemsIndexFile] = new KeywordType(Type.Items, true);
            this.AllKeywords[IgnorableIndexFile] = new KeywordType(Type.Ignorable, true);
            this.AllKeywords[IgnorablePiracyIndexFile] = new KeywordType(Type.IgnorablePiracy, true);
        }

        /// <summary>
        /// Add multiple values to the collection
        /// </summary>
        /// <param name="values">values</param>
        public void AddKeywords(params string[] values)
        {
            foreach (var str in values)
            {
                AllKeywords[ItemsIndex].Add(str);
            }
        }

        public void AddKeywordsFromFile(params string[] values)
        {
            foreach (var str in values)
            {
                if (str[0].Equals('-'))
                {
                    AllKeywords[IgnorableIndexFile].Add(str.Substring(1));
                }
                else
                {
                    AllKeywords[ItemsIndexFile].Add(str);
                }
            }
        }

        public void AddIgnorableKeywords(params string[] values)
        {
            foreach (var str in values)
            {
                AllKeywords[IgnorableIndex].Add(str);
            }
        }

        public void AddPiracyKeywords(params string[] values)
        {
            foreach (var str in values)
            {
                AllKeywords[PiracyIndex].Add(str);
            }
        }

        public void AddPiracyKeywordsFromFile(params string[] keywords)
        {
            foreach (var keyword in keywords)
            {
                if (keyword[0].Equals('-'))
                {
                    AllKeywords[IgnorablePiracyIndexFile].Add(keyword.Substring(1));
                }
                else
                {
                    AllKeywords[PiracyIndexFile].Add(keyword);
                }
            }
        }

        public void AddIgnorablePiracyKeywords(params string[] values)
        {
            foreach (var str in values)
            {
                AllKeywords[IgnorablePiracyIndex].Add(str);
            }
        }

        public void AddByType(string keyword, Type type)
        {
            switch (type)
            {
                case Type.Piracy:
                    AllKeywords[PiracyIndexFile].Add(keyword);
                    break;
                case Type.Ignorable:
                    AllKeywords[IgnorableIndexFile].Add(keyword);
                    break;
                case Type.IgnorablePiracy:
                    AllKeywords[IgnorablePiracyIndexFile].Add(keyword);
                    break;
                case Type.Items:
                    AllKeywords[ItemsIndexFile].Add(keyword);
                    break;
            }
        }

        public void Clear()
        {
            foreach (var keywordType in AllKeywords)
            {
                keywordType.Clear();
            }
        }

        public bool Has(string keyword)
        {
            foreach (var keywordType in AllKeywords)
            {
                if (keywordType.Keywords.Contains(keyword))
                {
                    return true;
                }
            }

            return false;
        }

        public bool HasItems()
        {
            foreach (var keywordType in AllKeywords)
            {
                if (keywordType.Keywords.Count > 0)
                {
                    return true;
                }
            }

            return false;
        }
        public string AllKeywordsToString()
        {
            string items = Items.ToString(", ");
            string piracy = Piracy.ToString(", ");
            string ignorable = Ignorable.ToString(", ");
            string ignorablePiracy = IgnorablePiracy.ToString(", ");

            return items + "\n\tIgnorable:\t" + ignorable + "\n\tPiracy:\t" + piracy + "\n\tPiracy ignorable\t" + ignorablePiracy + "\n\n";
        }

        public void Select(string keyword, bool state)
        {
            for (int i = PiracyIndexFile; i <= IgnorablePiracyIndexFile; i++)
            {
                if (AllKeywords[i].Keywords.Contains(keyword))
                {
                    AllKeywords[i].Select(state);
                }
            }
        }

        public void ClearNonFileKeywords()
        {
            AllKeywords[PiracyIndex].Clear();
            AllKeywords[ItemsIndex].Clear();
            AllKeywords[IgnorableIndex].Clear();
            AllKeywords[IgnorablePiracyIndex].Clear();
        }
    }
}