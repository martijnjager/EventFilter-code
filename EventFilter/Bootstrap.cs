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

        public static List<string> FoundLogs = new List<string>();

        private static readonly object Lock = new object();
        private static Bootstrap Instance;

        private readonly CheckedListBox _clbKeywords;

        private static IEvent Events;

        private Bootstrap(IEvent EventClass, CheckedListBox clbkeywords)
        {
            _alternatives = new List<dynamic>
            {
                "eventlog.txt",
                "EvtxSysDump.txt",
                "system-events.txt",
                "application-events.txt",
                "pnp-events.txt"
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
            
            Events.Keywords.LoadFromLocation();

            LoadKeywordsInClb();
        }

        private void LoadEventlocation()
        {
            if (!File.Exists(CurrentLocation + EventLocation))
            {
                if (!string.IsNullOrEmpty(CheckEventLogAlternatives()))
                    Events.SetLocation(new FileInfo(CheckEventLogAlternatives()));
                else
                    if (Directory.Exists(Zip.ExtractLocation))
                        if (Directory.GetFiles(Zip.ExtractLocation).Length > 0)
                            Events.SetLocation(new FileInfo(Directory.GetFiles(Zip.ExtractLocation)[0]));
            }
                
            if(File.Exists(CurrentLocation + EventLocation))
                Events.SetLocation(new FileInfo(CurrentLocation + EventLocation));
        }

        private string CheckEventLogAlternatives()
        {
            foreach(string alternative in _alternatives)
                if (File.Exists(CurrentLocation + alternative)) return alternative;

            return string.Empty;
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
            Events.Keywords.Refresh();

            if (Actions.IsEmpty(tbKeywords) && clbKeywords.Items.Count == 0)
            {
                Messages.NoInput();

                return;
            }

            if (searchEventBgWorker.IsBusy)
                return;

            Events.Keywords.Delete();
            Events.Keywords.Add(clbKeywords);

            if (!string.IsNullOrEmpty(tbKeywords))
            {
                Events.Keywords.Add(tbKeywords.Split(','));
            }

            Events.SetKeywordInstance(Events.Keywords);
            searchEventBgWorker.RunWorkerAsync();
        }

        public static bool FilesFound()
        {
            SetDefaultEncoding();

            if (Events.Keywords.GetAllKeywords() == "") Actions.Report("No Keywords.txt found");
            else Actions.Report("Load Keywords from " + Event.Instance.Keywords.KeywordLocation);

            if (Events.EventLocation is FileInfo)
            {
                Actions.Report("Load event log from " + Event.Instance.EventLocation.FullName);
                Actions.form.lblSelectedFile.Text = "Selected file: " + Event.Instance.EventLocation.FullName;

                return true;
            }

            Actions.Report("No eventlog.txt found");
            Actions.form.lblSelectedFile.Text = "Selected file: no eventlog found";

            return false;
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
