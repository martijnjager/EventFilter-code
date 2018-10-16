using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.ComponentModel;
using EventFilter.Contracts;
using EventFilter.Events;
using System.Text;

namespace EventFilter
{
    public class Bootstrap
    {
        private const string EventLocation = @"\eventlog.txt";

        private readonly List<dynamic> _alternatives;

        public static readonly string CurrentLocation = Directory.GetCurrentDirectory();

        public bool IsBooted;

        private static readonly object Lock = new object();
        private static Bootstrap Instance;

        private readonly CheckedListBox _clbKeywords;

        private readonly IEvent Events;

        public Bootstrap(IEvent EventClass, CheckedListBox clbkeywords)
        {
            _alternatives = new List<dynamic>
            {
                "eventlog.txt",
                "EvtxSysDump.txt",
                "system-events.txt",
                "eventlogSystem.txt"
            };

            _clbKeywords = clbkeywords;
            
            Events = EventClass;

            LoadFiles();
        }

        public static Bootstrap Boot(IEvent EventClass, CheckedListBox clbkeywords)
        {
            lock (Lock)
            {
                return Instance ?? (Instance = new Bootstrap(EventClass, clbkeywords));
            }
        }

        private void LoadFiles()
        {
            try
            {
                LoadKeywordLocation();

                LoadEventlocation();

                IsBooted = true;
            }
            catch (FileLoadException exception)
            {
                IsBooted = false;

                Actions.Report("FileLoadException: " + exception.Message);
            }
            
        }

        private void LoadKeywordLocation()
        {
            if (!File.Exists(Events.Keywords.KeywordLocation)) return;
            
            Events.Keywords.LoadKeywordsFromLocation();

            LoadKeywordsInClb();
        }

        private void LoadEventlocation()
        {
            if (!File.Exists(CurrentLocation + EventLocation))
                Events.SetEventLocation(CheckEventLogAlternatives());

            Events.SetEventLocation(CurrentLocation + EventLocation);
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
            foreach (string str in Events.Keywords.Items)
            {
               _clbKeywords.Items.Add(str.Trim(), true);
            }
        }

        /// <summary>
        /// If input is empty return a message
        /// </summary>
        public static void IsInputEmpty(BackgroundWorker searchEventBgWorker, CheckedListBox clbKeywords, string tbKeywords)
        {
            Event.Instance.Keywords.Refresh();

            if ((string.IsNullOrEmpty(tbKeywords) && clbKeywords.Items.Count == 0) || string.IsNullOrEmpty(Event.Instance.EventLocation.FullName))
            {
                Messages.NoInput();

                return;
            }

            if (searchEventBgWorker.IsBusy)
                return;

            Event.Instance.Keywords.DeleteKeywords();
            Event.Instance.Keywords.AddKeyword(clbKeywords);

            if (!string.IsNullOrEmpty(tbKeywords))
            {
                Event.Instance.Keywords.AddKeyword(tbKeywords.Split(','));
            }

            Event.Instance.SetKeywordObj(Event.Instance.Keywords);
            searchEventBgWorker.RunWorkerAsync();
        }

        public static bool FilesFound()
        {
            bool status = false;
            if (string.IsNullOrEmpty(Event.Instance.EventLocation.FullName))
            {
                Actions.Report("No eventlog.txt found");
                Actions.form.lblSelectedFile.Text = "Selected file: no eventlog found";

                status = false;
            }
            else
            {
                Actions.Report("Load event log from " + Event.Instance.EventLocation.FullName);
                Actions.form.lblSelectedFile.Text = "Selected file: " + Event.Instance.EventLocation.FullName;

                status = true;
            }

            if (Event.Instance.Keywords.GetAllKeywords() == "") Actions.Report("No Keywords.txt found");
            else Actions.Report("Load Keywords from " + Event.Instance.Keywords.KeywordLocation);

            SetDefaultEncoding();

            return status;
        }

        private static void SetDefaultEncoding()
        {
            foreach (ToolStripMenuItem encoding in from object items in Actions.form.Utf8.Owner.Items let encoding = items as ToolStripMenuItem where encoding != null select encoding)
            {
                Encodings.EncodingOptions.Add(encoding);
            }

            Encodings.CurrentEncoding = Encoding.Default;
            Actions.form.EncodingDefault.Text = Encodings.CurrentEncoding.BodyName;
            Actions.form.EncodingDefault.Checked = true;

            Actions.Report("Encoding set to" + Encoding.Default);
        }
    }
}
