using EventFilter.Contracts;
using EventFilter.Keywords.Concerns;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace EventFilter.Keywords
{
    /// <summary>
    /// Class for all customized properties based on the keywords
    /// Accessor for event class
    /// </summary>
    public sealed class Keyword : IKeywords
    {
        /**
         * Property for the count: operator
         */
        const string COUNT_PREFIX = "count:";
        const string DATE_START = "datestart:";
        const string DATE_END = "dateend:";
        public string Countable { get; private set; }

        public DateTime DateStart { get; private set; }

        public DateTime DateEnd { get; private set; }

        private static IKeywords _keywords;
        private static readonly object _lock = new object();

        private IManagesKeywords manager;

        public static string FileLocation { get; set; }

        public bool KeywordsLoaded { get; private set; }

        public List<string> Items { get => this.manager.Items; }
        public List<string> Piracy { get => this.manager.Piracy; }
        public List<string> Ignorable { get => this.manager.Ignorable; }
        public List<string> IgnorablePiracy { get => this.manager.IgnorablePiracy; }


        public List<string> ItemsFile { get => this.manager.ItemsFile; }
        public List<string> PiracyFile { get => this.manager.PiracyFile; }
        public List<string> IgnorableFile { get => this.manager.IgnorableFile; }
        public List<string> IgnorablePiracyFile { get => this.manager.IgnorablePiracyFile; }

        private Keyword()
        {
            SetLocation();
            this.manager = new ManagesKeywords();
            this.manager.Clear();
        }

        public static IKeywords GetInstance()
        {
            lock (_lock)
            {
                if (_keywords is null)
                {
                    _keywords = new Keyword();
                }

                return (IKeywords)_keywords;
            }
        }

        /// <summary>
        /// Prepare app for Keywords
        /// - Check Keywords file existence
        /// - Load Keywords properly into the textbox
        /// - Make Keywords publicly visible
        /// </summary>
        /// <param name="path">Path of Keywords file</param>
        public IKeywords LoadFromLocation(string path = "")
        {
            try
            {
                LoadFrom(path.IsEmpty() ? FileLocation : path);
                KeywordsLoaded = true;
            }
            catch (IOException ex)
            {
                Helper.Report("An IO error occured loading keywords from file: " + ex.Message);
            }

            return this;
        }

        public void AddInto(CheckedListBox clb)
        {
            List<string> arr = new List<string>();
            arr.AddRange(this.manager.ItemsFile);
            arr.AddRangeWithPrefix(this.manager.IgnorableFile, KeywordPrefix.PREFIX_IGNORABLE);
            arr.AddRangeWithPrefix(this.manager.PiracyFile, KeywordPrefix.PREFIX_PIRACY);
            arr.AddRangeWithPrefix(this.manager.IgnorablePiracyFile, KeywordPrefix.PREFIX_IGNORABLE_PIRACY);

            arr.ForEach(item =>
            {
                clb.Items.Add(item.Trim(), true);
            });
        }

        /// <summary>
        /// Get Keywords from the provided location, default is the current location of the app
        /// </summary>
        /// <param name="path">Path of the file</param>
        /// <returns></returns>
        private void LoadFrom(string path)
        {
            if (!File.Exists(path)) return;

            string[] content = File.ReadAllLines(path);

            if (content.Length == 0)
                throw new IOException("There are no keywords in the file.");

            this.manager.AddKeywordsFromFile(content[0].Explode(", "));

            if (content.Length < 2)
                return;

            var keywords = content[1].Replace("PIRACY: ", "");
            this.manager.AddPiracyKeywordsFromFile(keywords.Explode(", "));
        }

        public void MapFromFile(CheckedListBox.CheckedItemCollection preloadedKeywords)
        {
            this.manager.Clear();
            Set(preloadedKeywords);
            AddFromTextbox();
        }

        private void AddFromTextbox()
        {
            if (Helper.Form.tbKeywords.Text.IsEmpty())
                return;

            this.Add(Helper.Form.tbKeywords.Text.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries));
        }

        //public void SaveKeywords(string keywords, string piracy)
        //{
        //    if (!SaveToFile(FileLocation, keywords1))
        //        return;
        //}

        private static bool SaveToFile(string keywords)
        {
            try
            {
                File.WriteAllText(FileLocation, keywords);
                Helper.Report("Saving Keywords to file");
                Messages.KeywordsSaved();

                return true;
            }
            catch(Exception error)
            {
                Helper.Report("An error occured when trying to save Keywords: " + error.Message);
                Messages.ProblemOccured("saving keywords");

                return false;
            }
        }

        public static void SaveKeywords(params string[] keywordsInput)
        {
            string piracy;
            string keywords = piracy = string.Empty;
            string keywordsToUse = keywordsInput[0];
            string ignorables = keywordsInput[1];
            string piracyKeywords = keywordsInput[2];
            string piracyIgnorables = keywordsInput[3];

            if (!keywordsToUse.Trim().IsEmpty())
                keywords = keywordsToUse.RemoveTrailingNewLine().Replace("\n", ", ");

            if (!ignorables.Trim().IsEmpty())
                keywords += ignorables.RemoveTrailingNewLine().Replace("\n", ", -").StartWith(", -");

            if (!piracyKeywords.Trim().IsEmpty())
                piracy = piracyKeywords.RemoveTrailingNewLine().Replace("\n", ", ");

            if (!piracyIgnorables.Trim().IsEmpty())
                piracy += piracyIgnorables.RemoveTrailingNewLine().Replace("\n", ", -").StartWith(", -");

            string allKeywordsToSave = keywords + "\nPIRACY: " + piracy;

            Keyword.SaveToFile(allKeywordsToSave);
        }

        /// <summary>
        /// Sets the location of the keyword file
        /// </summary>
        public static void SetLocation()
        {
            if (!FileLocation.IsEmpty())
                return;

            FileLocation = Bootstrap.CurrentLocation + @"\keywords.txt";
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
                this.AddToList(item);
            }
        }

        private void AddToList(string item)
        {
            if (item.StartsWith("P: "))
                this.manager.AddPiracyKeywords(item.Trim("P: "));

            if (item.StartsWith("-P: "))
                this.manager.AddIgnorablePiracyKeywords(item.Trim("-P: "));

            if (!item.StartsWith("-P: ") && item.StartsWith("-"))
                this.manager.AddIgnorableKeywords(item.Trim("-"));

            if (item.StartsWith("-") || item.StartsWith("P: "))
                return;

            this.manager.AddKeywords(item);
        }

        public void Add(params string[] values)
        {
            foreach (string str in values)
            {
                if (str.StartsWith("-", System.StringComparison.Ordinal))
                {
                    this.manager.AddIgnorableKeywords(str);
                }
                else
                {
                    this.manager.AddKeywords(str);
                }
            }
        }

        public bool HasItems()
        {
            return this.manager.HasItems();
        }

        public bool Has(string keyword)
        {
            return this.manager.Has(keyword);
        }

        public string AllKeywordsToString()
        {
            return this.manager.AllKeywordsToString();
        }

        public void Select(string keyword, bool state)
        {
            this.manager.Select(keyword, state);
        }

        public void Map(List<string> textboxItems)
        {
            this.manager.ClearNonFileKeywords();
            textboxItems.ForEach(item =>
            {
                if (item.StartsWith(KeywordPrefix.PREFIX_PIRACY))
                {
                    this.manager.AddPiracyKeywords(item.Trim(KeywordPrefix.PREFIX_PIRACY));
                }

                if (item.StartsWith(KeywordPrefix.PREFIX_IGNORABLE_PIRACY))
                {
                    this.manager.AddIgnorablePiracyKeywords(item.Trim(KeywordPrefix.PREFIX_IGNORABLE_PIRACY));
                }

                if (item.StartsWith(KeywordPrefix.PREFIX_IGNORABLE))
                {
                    this.manager.AddIgnorableKeywords(item.Trim(KeywordPrefix.PREFIX_IGNORABLE));
                }

                if (!item.StartsWith(KeywordPrefix.PREFIX_PIRACY) || 
                    !item.StartsWith(KeywordPrefix.PREFIX_IGNORABLE_PIRACY) || 
                    !item.StartsWith(KeywordPrefix.PREFIX_IGNORABLE))
                {
                    this.manager.AddKeywords(item);
                }

                if (item.StartsWith(COUNT_PREFIX))
                {
                    this.Countable = item.Trim(COUNT_PREFIX);
                }

                if (item.StartsWith(DATE_START))
                {
                    this.DateStart = DateTime.Parse(item.Trim(DATE_START));
                }

                if (item.StartsWith(DATE_END))
                {
                    this.DateEnd = DateTime.Parse(item.Trim(DATE_END));
                }
            });
        }
    }
}