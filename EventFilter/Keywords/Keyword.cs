using EventFilter.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace EventFilter.Keywords
{
    public sealed partial class Keyword : IKeywords
    {
        /**
         * Property for the count: operator
         */
        public string KeywordToCount { get; set; }

        public bool KeywordsLoaded { get; set; }

        public static string FileLocation { get; set; }

        private static Keyword _keywords;
        private static readonly object _lock = new object();

        private Keyword()
        {
            //_operators = new List<string> { "-", "count:", "datestart:", "dateend:" };
            Refresh();
            SetLocation();
        }

        public static IKeywords GetInstance()
        {
            lock (_lock)
            {
                if (_keywords is null)
                    NewInstance();

                return _keywords;
            }
        }

        private static void NewInstance()
        {
            lock (_lock) { _keywords = new Keyword(); }
        }

        public static void SetLocation()
        {
            if (!FileLocation.IsEmpty())
                return;

            FileLocation = Bootstrap.CurrentLocation + @"\keywords.txt";
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

        public void Into(CheckedListBox clb)
        {
            List<string> arr = new List<string>();
            arr.AddRange(Items);
            arr.AddRangeWithPrefix(Ignorable, "-");
            arr.AddRangeWithPrefix(Piracy, "P: ");
            arr.AddRangeWithPrefix(IgnorablePiracy, "-P: ");

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

            Set(content[0], "Items");

            if (content.Length < 2)
                return;

            ProcessPiracyKeywords(content[1]);
        }

        private void ProcessPiracyKeywords(string keywords)
        {
            keywords = keywords.Replace("PIRACY: ", "");
            Set(keywords, "Piracy");
        }

        public void Map()
        {
            Refresh();
            AddFromClb();
            AddFromTextbox();
            AddOperators();
        }

        private void AddFromTextbox()
        {
            if (Helper.Form.tbKeywords.Text.IsEmpty())
                return;

            Add(Helper.Form.tbKeywords.Text.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries));
        }

        private void AddFromClb()
        {
            //Add(Actions.Form.clbKeywords);
            Set(Helper.Form.clbKeywords.CheckedItems);
        }

        public void SaveKeywords(string keywords, string piracy)
        {
            string keywords1 = keywords + "\nPIRACY: " + piracy;
            if (!SaveToFile(FileLocation, keywords1))
                return;

            Messages.KeywordsSaved();
        }

        public static bool SaveToFile(string fileName, string keywords)
        {
            try
            {
                File.WriteAllText(fileName, keywords);
                Helper.Report("Saving Keywords to file");

                return true;
            }
            catch(Exception error)
            {
                Helper.Report("An error occured when trying to save Keywords: " + error.Message);
                Messages.ProblemOccured("saving keywords");

                return false;
            }
        }
    }
}