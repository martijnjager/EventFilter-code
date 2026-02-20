using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventFilter.Keywords.Concerns
{
    public enum Type
    {
        Piracy,
        Items,
        Ignorable,
        IgnorablePiracy,
    }

    public class KeywordPrefix
    {
        public const string PREFIX_PIRACY = "P: ";
        public const string PREFIX_IGNORABLE = "-";
        public const string PREFIX_IGNORABLE_PIRACY = "-P: ";
    }

    public class KeywordType
    {
        public bool IsFile { get; private set; }

        public Type Type { get; private set; }

        public List<string> Keywords { get; private set; }

        public bool IsSelected { get; private set; }

        public KeywordType(Type type, bool isFile = true)
        {
            this.Type = type;
            this.IsFile = isFile;
            this.Keywords = new List<string>();
            this.IsSelected = isFile;
        }

        public void Add(string keyword)
        {
            this.Keywords.Add(keyword);
        }

        public void Clear()
        {
            this.Keywords.Clear();
        }

        public void Select(bool state)
        {
            if (this.IsFile)
            {
                this.IsSelected = state;
            }
        }
    }
}
