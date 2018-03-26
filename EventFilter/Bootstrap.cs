using System.Collections.Generic;
using System.IO;
using EventFilter.Events;
using System.Windows.Forms;
using EventFilter.Keywords;
using System.ComponentModel;

namespace EventFilter
{
    public class Bootstrap
    {
        private const string EventLocation = @"\eventlog.txt";

        private readonly List<dynamic> _alternatives;

        public static readonly string CurrentLocation = Directory.GetCurrentDirectory();

        private readonly CheckedListBox _clbKeywords;

        private readonly Keyword _keywordClass;
        private readonly Event _eventClass;

        public Bootstrap(CheckedListBox clbkeywords)
        {
            _alternatives = new List<dynamic>
            {
                "eventlog.txt",
                "EvtxSysDump.txt",
                "system-events.txt"
            };

            _clbKeywords = clbkeywords;
            
            _eventClass = Event.Instance;
            _keywordClass = Keyword.Instance;
        }

        public void LoadFiles()
        {
            LoadKeywordLocation();

            LoadEventlocation();
        }

        private void LoadKeywordLocation()
        {
            if (!File.Exists(_keywordClass.KeywordLocation)) return;
            
            _keywordClass.LoadKeywordsFromLocation();

            LoadKeywordsInClb();
        }

        private void LoadEventlocation()
        {
            if (!File.Exists(CurrentLocation + EventLocation))
            {
                IndexEvent.EventLocation = CheckEventLogAlternatives();
            }

            IndexEvent.EventLocation = CurrentLocation + EventLocation;
        }

        private string CheckEventLogAlternatives()
        {
            foreach(string alternative in _alternatives)
            {
                if (File.Exists(CurrentLocation + alternative)) return alternative;
            }

            return null;
        }

        private void LoadKeywordsInClb()
        {
            foreach (var str in _keywordClass.ToList())
            {
               _clbKeywords.Items.Add(str.Trim(), true);
            }
        }

        /// <summary>
        /// If input is empty return a message
        /// </summary>
        public static void IsInputEmpty(BackgroundWorker SearchEventBGWorker, BackgroundWorker eventFilterBGWorker, CheckedListBox clbKeywords, Keyword keywordClass, Event eventClass, string tbKeywords)
        {
            if ((string.IsNullOrEmpty(tbKeywords) && clbKeywords.Items.Count == 0) || string.IsNullOrEmpty(IndexEvent.EventLocation))
            {
                Messages.NoInput();

                return;
            }

            if (SearchEventBGWorker.IsBusy == false)
            {
                keywordClass.DeleteKeywords();

                keywordClass.AddKeyword(clbKeywords);

                if (tbKeywords != string.Empty)
                {
                    keywordClass.AddKeyword(tbKeywords.Split(','));
                }

                Form1.Report("Keywords to use: " + Arr.Implode(keywordClass.GetAllKeywords(), ", "));

                #region Set worker reports of bgw to true
                SearchEventBGWorker.WorkerReportsProgress = true;
                SearchEventBGWorker.DoWork += eventClass.Search;

                eventFilterBGWorker.WorkerReportsProgress = true;
                #endregion

                SearchEventBGWorker.RunWorkerAsync();
            }
        }
    }
}
