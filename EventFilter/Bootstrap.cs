using EventFilter.Contracts;
using EventFilter.Events;
using EventFilter.Keywords;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EventFilter
{
    public class Bootstrap
    {
        private const string EventFile = @"\eventlog.txt";

        private readonly List<string> _alternatives;

        public static readonly string CurrentLocation = Directory.GetCurrentDirectory() + "\\";

        private static readonly object Lock = new object();
        private static Bootstrap Instance;

        private static IEvent Events;
        private static IKeywords Keywords;

        public static bool AreFilesFound = false;

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

            InitProps();

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

                LogFilesFound();
            }
            catch (FileLoadException exception)
            {
                Helper.Report("FileLoadException: " + exception.Message);
            }
            catch (Exception exception)
            {
                Helper.Report("Exception: " + exception.Message);
            }
        }

        public static void LoadKeywordLocation()
        {
            if (!File.Exists(Keyword.FileLocation)) return;

            Keywords.LoadFromLocation().Into(Helper.Form.clbKeywords);
        }

        private void LoadEventlocation()
        {
            if (!File.Exists(CurrentLocation + EventFile))
            {
                string alternative = GetAlternativeLogs();

                if (!alternative.IsEmpty())
                    Events.SetLocation(alternative);
                else
                {
                    if (Directory.Exists(Zip.ExtractLocation) && Directory.GetFiles(Zip.ExtractLocation).Length > 0)
                        Events.SetLocation(Directory.GetFiles(Zip.ExtractLocation)[0]);
                }
            }
            else
                Events.SetLocation(CurrentLocation + EventFile);
        }

        private string GetAlternativeLogs()
        {
            foreach (string alternative in _alternatives)
            {
                string file = CurrentLocation + alternative;

                if (File.Exists(file))
                    return file;
            }

            return string.Empty;
        }

        private static void LogFilesFound()
        {
            SetDefaultEncoding();

            if (Keywords.GetAllKeywords().IsEmpty())
                Helper.Report("No Keywords.txt found");
            else
                Helper.Report("Load Keywords from " + Keyword.FileLocation);

            if (Event.GetInstance().FileLocation is FileInfo)
            {
                Helper.Report("Load event log from " + Events.FileLocation.FullName);
                Helper.Form.lblSelectedFile.Text = "Selected file: " + Events.FileLocation.FullName;

                AreFilesFound = true;
            }
            else
            {
                AreFilesFound = false;

                Helper.Report("No eventlog found");
                Helper.Form.lblSelectedFile.Text = Properties.Resources.NoLogFound;
            }
        }

        private static void SetDefaultEncoding()
        {
            foreach (ToolStripMenuItem encoding in (from object items in Helper.Form.Utf8.Owner.Items let encoding = items as ToolStripMenuItem where encoding != null select encoding))
                Encodings.EncodingOptions.Add(encoding);

            Encodings.CurrentEncoding = Encoding.Default;
            Helper.Form.EncodingDefault.Text = Encodings.CurrentEncoding.BodyName;
            Helper.Form.EncodingDefault.Checked = true;

            Helper.Report("Encoding set to" + Encoding.Default);
        }

        private static void InitProps()
        {
            Events = Event.GetInstance();
            Keywords = Keyword.GetInstance();
        }
    }
}
