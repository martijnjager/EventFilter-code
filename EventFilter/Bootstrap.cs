using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.ComponentModel;
using EventFilter.Contracts;
using EventFilter.Events;
using System.Text;
using EventFilter.Keywords;

namespace EventFilter
{
    public class Bootstrap
    {
        private const string EventLocation = @"\eventlog.txt";

        private readonly List<string> _alternatives;

        public static readonly string CurrentLocation = Directory.GetCurrentDirectory();

        private static readonly object Lock = new object();
        private static Bootstrap Instance;

        private static IEvent Events = Event.Instance;
        private static IKeywords Keywords = Keyword.Instance;

        private Bootstrap()
        {
            _alternatives = new List<string>
            {
                "eventlog.txt",
                "EvtxSysDump.txt",
                "system-events.txt",
                "application-events.txt",
                "pnp-events.txt"
            };

            LoadFiles();
        }

        public static Bootstrap Boot()
        {
            lock (Lock)
            {
                return Instance ?? (Instance = new Bootstrap());
            }
        }

        private void LoadFiles()
        {
            try
            {
                LoadKeywordLocation();

                LoadEventlocation();
            }
            catch (FileLoadException exception)
            {
                Actions.Report("FileLoadException: " + exception.Message);
            }
            
        }

        private void LoadKeywordLocation()
        {
            if (!File.Exists(Keyword.FileLocation)) return;
            
            Keywords.LoadFromLocation().LoadIntoCLB();
        }

        private void LoadEventlocation()
        {
            if (!File.Exists(CurrentLocation + EventLocation))
            {
                if (!string.IsNullOrEmpty(CheckEventLogAlternatives()))
                    Events.SetLocation(new FileInfo(CheckEventLogAlternatives()));
                else
                {
                    if (Directory.Exists(Zip.ExtractLocation))
                    {
                        if (Directory.GetFiles(Zip.ExtractLocation).Length > 0)
                            Events.SetLocation(new FileInfo(Directory.GetFiles(Zip.ExtractLocation)[0]));
                    }
                }
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

        /// <summary>
        /// If input is empty return a message
        /// </summary>
        public static void IsInputEmpty(BackgroundWorker searchEventBgWorker, CheckedListBox clbKeywords, string tbKeywords)
        {
            //Events.Keywords.Refresh();

            if (Actions.IsEmpty(tbKeywords) && clbKeywords.CheckedItems.Count == 0)
            {
                Messages.NoInput();

                return;
            }

            if (searchEventBgWorker.IsBusy)
                return;

            /**
             * Clear the keywords and add new onces
             */
            //Events.Keywords.Delete();
            //Events.Keywords.Add(clbKeywords);

            //if (!string.IsNullOrEmpty(tbKeywords))
            //{
            //    Events.Keywords.Add(tbKeywords.Split(','));
            //}

            //Events.SetKeywordInstance(Events.Keywords);
            searchEventBgWorker.RunWorkerAsync();
        }

        public static bool FilesFound()
        {
            SetDefaultEncoding();

            if (Keywords.GetAllKeywords() == "") Actions.Report("No Keywords.txt found");
            else Actions.Report("Load Keywords from " + Keyword.FileLocation);

            if (Events.EventLocation is FileInfo)
            {
                Actions.Report("Load event log from " + Event.Instance.EventLocation.FullName);
                Actions.Form.lblSelectedFile.Text = "Selected file: " + Event.Instance.EventLocation.FullName;

                return true;
            }

            Actions.Report("No eventlog.txt found");
            Actions.Form.lblSelectedFile.Text = "Selected file: no eventlog found";

            return false;
        }

        private static void SetDefaultEncoding()
        {
            foreach (ToolStripMenuItem encoding in from object items in Actions.Form.Utf8.Owner.Items let encoding = items as ToolStripMenuItem where encoding != null select encoding)
            {
                Encodings.EncodingOptions.Add(encoding);
            }

            Encodings.CurrentEncoding = Encoding.Default;
            Actions.Form.EncodingDefault.Text = Encodings.CurrentEncoding.BodyName;
            Actions.Form.EncodingDefault.Checked = true;

            Actions.Report("Encoding set to" + Encoding.Default);
        }
    }
}
