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
        private const string DefaultEventFile = @"\eventlog.txt";

        private readonly List<string> _alternatives;

        public static readonly string CurrentLocation = Directory.GetCurrentDirectory() + "\\";

        private static readonly object Lock = new object();
        private static Bootstrap Instance;

        private static IEvent Events;
        private static IKeywords Keywords;

        //public static bool FilesFound = false;

        private Bootstrap(CheckedListBox checkedListBox)
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

            LoadFiles(checkedListBox);
        }

        public static Bootstrap Boot(CheckedListBox checkedListBox)
        {
            lock (Lock)
            {
                return Instance ?? (Instance = new Bootstrap(checkedListBox));
            }
        }

        private void LoadFiles(CheckedListBox checkedListBox)
        {
            try
            {
                LoadKeywordLocation(checkedListBox);

                SetEventlocation();

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

        public static void LoadKeywordLocation(CheckedListBox checkedListBox)
        {
            if (!File.Exists(Keyword.FileLocation)) return;

            Keywords.LoadFromLocation().AddInto(checkedListBox);
        }

        private void SetEventlocation()
        {
            if (!File.Exists(CurrentLocation + DefaultEventFile))
            {
                string alternative = GetAlternativeLogs();

                if (!alternative.IsEmpty())
                {
                    Events.SetLocation(alternative);
                }
                else
                {
                    if (Directory.Exists(Zip.ExtractLocation) && Directory.GetFiles(Zip.ExtractLocation).Length > 0)
                    {
                        Events.SetLocation(Directory.GetFiles(Zip.ExtractLocation)[0]);
                    }
                }
            }
            else
            {
                Events.SetLocation(CurrentLocation + DefaultEventFile);
            }
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

            if (!Keywords.KeywordsLoaded)
                Helper.Report("No Keywords.txt found");
            else
                Helper.Report("Loaded Keywords from " + Keyword.FileLocation);

            if (Event.FileLocation is FileInfo)
            {
                Helper.Report("Load event log from " + Event.FileLocation.FullName);
                Helper.Form.lblSelectedFile.Text = "Selected file: " + Event.FileLocation.FullName;

                //FilesFound = true;
            }
            else
            {
                //FilesFound = false;

                Helper.Report("No eventlog found");
                Helper.Form.lblSelectedFile.Text = Properties.Resources.NoLogFound;
            }
        }

        private static void SetDefaultEncoding()
        {
            foreach (ToolStripMenuItem encoding in (
                from object items 
                in Helper.Form.Utf8.Owner.Items 
                let encoding = items as ToolStripMenuItem 
                where encoding != null 
                select encoding)
            )
            {
                Encodings.EncodingOptions.Add(encoding);
            }

            Encodings.CurrentEncoding = Encoding.Default;
            Helper.Form.EncodingDefault.Text = Encodings.CurrentEncoding.BodyName;
            Helper.Form.EncodingDefault.Checked = true;

            Helper.Report("Encoding set to" + Encoding.Default.EncodingName);
        }

        private static void InitProps()
        {
            Events = Event.GetInstance();
            Keywords = Keyword.GetInstance();
        }
    }
}
