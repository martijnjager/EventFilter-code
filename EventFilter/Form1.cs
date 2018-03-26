using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using EventFilter.Events;
using EventFilter.Events.Engine;
using EventFilter.Events.Engine.Contracts;
using EventFilter.Keywords;

namespace EventFilter
{
    public partial class Form1 : Form
    {
        private readonly Event _eventClass;
        private readonly Keyword _keywordClass;

        private ListViewItem _listview;

        internal Event EventClass => _eventClass;
        internal Keyword KeywordClass => _keywordClass;

        public Form1()
        {
            InitializeComponent();

            _listview = new ListViewItem();

            _eventClass = Event.Instance;
            _keywordClass = Keyword.Instance;
            
            try
            {
                // Instantiating
                var bootstrap = new Bootstrap(clbKeywords);

                IEventIndex eventIndex = _eventClass;

                bootstrap.LoadFiles();

                #region Load files into app

                if (string.IsNullOrEmpty(IndexEvent.EventLocation))
                {
                    Report("No eventlog.txt found");
                    lblSelectedFile.Text = "Selected file: no eventlog found";
                }
                else
                {
                    Report("Load event log from " + IndexEvent.EventLocation);
                    lblSelectedFile.Text = "Selected file: " + IndexEvent.EventLocation;
                }

                if (_keywordClass.GetAllKeywords() == "") Report("No Keywords.txt found");
                else Report("Load Keywords from " + _keywordClass.KeywordLocation);

                #endregion

                #region Set encoding of app
                Encodings.CurrentEncoding = System.Text.Encoding.Default;
                EncodingDefault.Text = Encodings.CurrentEncoding.BodyName;
                EncodingDefault.Checked = true;
                #endregion

                foreach (var encoding in (from object items in Utf8.Owner.Items let encoding = items as ToolStripMenuItem where encoding != null select encoding))
                {
                    Encodings.EncodingOptions.Add(encoding);
                }
            }
            catch (Exception error)
            {
                Report("ERROR LOADING FILES: " + error.Message);

                //Messages.AnErrorOccuredLoadingFiles();
            }

            #region btn design
            btnSearch.FlatStyle = FlatStyle.Popup;
            btnSearch.FlatAppearance.BorderColor = Color.Wheat;
            btnResultCleanup.FlatStyle = FlatStyle.Popup;
            btnResultCleanup.FlatAppearance.BorderColor = Color.Wheat;
            btnCopyClipboard.FlatStyle = FlatStyle.Popup;
            btnCopyClipboard.FlatAppearance.BorderColor = Color.Wheat;
            btnSaveReport.FlatStyle = FlatStyle.Popup;
            btnSaveReport.FlatAppearance.BorderColor = Color.Wheat;
            #endregion
            
            // Enables key events
            KeyPreview = true;

            Report("Initialization completed!");
        }

        #region buttons
        private void btnSearch_Click(object sender, EventArgs e)
        {
            Report("Start searching events");
//            searchContainer.ProcessCalls();

            lbEventResult.Items.Clear();

            if (IndexEvent.EventLocation != "openFileDialog1")
            {
                lblSelectedFile.Text = "Selected file: " + IndexEvent.EventLocation;

                Report("Selected log: " + lblSelectedFile.Text);

                Bootstrap.IsInputEmpty(SearchEventBGWorker, eventFilterBGWorker, clbKeywords, KeywordClass, EventClass, tbKeywords.Text);
            }
            else
            {
                Messages.SelectFileForSearching();
            }
        }

        private void BtnSaveBugReport(object sender, EventArgs e)
        {
            CreateReport();
        }

        private void btnResultCleanup_Click(object sender, EventArgs e)
        {
            if(eventFilterBGWorker.IsBusy == false)
            {
                Report("Cleaning up results");
                //MessageWrite("This may take some time", "Cleaning up", MessageBoxButtons.OK, MessageBoxIcon.None);
                foreach (ListViewItem item in lbEventResult.Items)
                {
                    item.Remove();
                }

                eventFilterBGWorker.RunWorkerAsync();
            }
            else
            {
                Messages.Filtering();
            }
        }
        #endregion

        #region BackgroundWorkers
        private void SearchEventBGWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if(e.UserState.ToString().Contains("Log: "))
            {
                Report(e.UserState.ToString().Replace("Log: ", ""));
            }

            if (e.UserState.ToString().Contains("Event: "))
            {
                AddListItem(Arr.Explode(e.UserState.ToString().Replace("Event: ", ""), " + "));
            }

            if (e.UserState.ToString().Contains("Time: "))
            {
                lblTime.Text = e.UserState.ToString().Replace("Time: ", "");
            }

            if (e.UserState.ToString().Contains("Counter: "))
            {
                lblResultCount.Text = e.UserState.ToString().Replace("Counter: ", "");
            }
        }

        private void SearchEventBGWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _eventClass.CheckCountOperator();

            if (KeywordClass.Counter != 0) Messages.CountKeywords(KeywordClass.KeywordCounted, KeywordClass.Counter);

            lbEventResult.Sort();
        }

        private void eventFilterBGWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var eventDescr = EventClass.Filter(EventClass.FoundEvents, EventClass.FoundIds, EventClass.FoundDates);

            var eventId = EventClass.FilteredEventId;
            var eventDate = EventClass.FilteredEventDate;

            var recorder = 0;

            for (var i = 0; i < eventDescr.Count; i++)
            {
                var data = new string[3];
                data[0] = "Data: " + eventDate[i];
                data[1] = "Data: " + eventDescr[i];
                data[2] = "Data: " + eventId[i];

                eventFilterBGWorker.ReportProgress(recorder, data);
                recorder++;
            }

            eventFilterBGWorker.ReportProgress(recorder, "Resultcount: Events found: " + lblResultCount.Text.Substring(lblResultCount.Text.Length - 1, 1) + "\t, After filtering: " + eventId.Count);
        }

        private void eventFilterBGWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.UserState.ToString().Contains("Resultcount: ") == false)
            {
                var items = ((IEnumerable)e.UserState).Cast<object>().Select(x => x.ToString()).ToArray();

                if (items.Length <= 1) return;
                for (var i = 0; i < items.Length; i++)
                {
                    items[i] = items[i].Replace("Data: ", "").Trim('{').Trim('}');
                }
                AddListItem(items);
            }
            else
                lblResultCount.Text = e.UserState.ToString().Replace("Resultcount: ", "").Trim('{').Trim('}');
        }
        #endregion

        #region Voids to append elements

        private void Report(string log = "") => rtbBugReport.AppendText(log + "\n");

        public static void Report(dynamic log) => Report(log);

        private void AddListItem(string[] item)
        {
            Report("Adding: " + Arr.Implode(item, "\t")+"\n");
            var addViewItem = new ListViewItem(item);
            lbEventResult.Items.Add(addViewItem);
        }

        public void Exception(Exception error)
        {
            Report(error.Message);
            Messages.ProblemOccured();
            CreateReport();
            Messages.ReportCreated();
        }

        private void CreateReport()
        {
            if (IndexEvent.EventLocation == "" && KeywordClass.GetAllKeywords() == "")
            {
                Messages.NoLogSaved();

                return;
            }

            Bug.CreateBugReport(KeywordClass, EventClass, rtbBugReport.Text);

            if (Bug.exception != null)
            {
                Messages.ErrorLogCollection();
                return;
            }

            Messages.LogSaved();
        }

        private void CopyToClipboard(dynamic dates)
        {
            List<dynamic> data = new List<dynamic>();

            for (int i = 0; i < dates.Count; i++)
            {
                data.Add(dates[i].Text.Trim().Replace("Date: ", "") + "\t\t" + dates[i].SubItems[1].Text.Trim());
            }

            Clipboard.SetText("[code]" + Arr.Implode(data, "\n") + "[/code]");
        }
        #endregion

        #region MenuItems
        private void miSaveKeywords_Click(object sender, EventArgs e)
        {
            Report("Start saving Keywords");
            saveFileDialog1.ShowDialog();
            string fileName = saveFileDialog1.FileName;
            try
            {
                StreamWriter streamWriter = new StreamWriter(fileName);
                streamWriter.WriteLine(tbKeywords.Text);
                Report("Saving Keywords " + tbKeywords.Text + " to file");
                streamWriter.Close();
            }
            catch(Exception error)
            {
                Report("An error occured when trying to save Keywords: " + error.Message);
            }
        }

        private void miSelectEventlog_Click(object sender, EventArgs e)
        {
            Report("Loading event logs");
            openFileDialog1.ShowDialog();

            if(openFileDialog1.FileName.Contains(".zip"))
            {
                string eventLocation = "";
                Zip.ExtractZip(openFileDialog1.FileName, ref eventLocation);
                IndexEvent.EventLocation = eventLocation;
            }
            else
            {
                IndexEvent.EventLocation = openFileDialog1.FileName;
            }

            if(string.IsNullOrEmpty(IndexEvent.EventLocation))
            {
                Messages.NoLogFound();
            }

            Report("Event log location: " + IndexEvent.EventLocation);

            lblSelectedFile.Text = "Selected file: " + IndexEvent.EventLocation;
        }

        private void miLoadKeywords_Click(object sender, EventArgs e)
        {
            Report("Loading Keywords to use");
            openFileDialog1.ShowDialog();

            string keyLoc = openFileDialog1.FileName;

            Report("Keywords to use location: " + keyLoc);

            KeywordClass.LoadKeywordsFromLocation(keyLoc);

            // Load keywords
            //tbKeywords.Text = Keyword.Keywords;
        }

        private void miAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Name: \t\t EventFilter\nDeveloper: \t Martijn (axe0)\nVersion: \t\t BETA\nDate: \t\t 2018-02-20", "About app", MessageBoxButtons.OK);
        }

        private void miEventFilter_Click(object sender, EventArgs e) => tabControl1.SelectedTab = tpEventFilter;

        private void miBugReport_Click(object sender, EventArgs e) => tabControl1.SelectedTab = tpBugReport;
        #endregion

        #region Listview actions
        private void lbEventResult_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            string text = EventClass.FindEvent(EventClass.Events, int.Parse(lbEventResult.SelectedItems[0].SubItems[2].Text));
            Message mes = new Message(text)
            {
                Id = int.Parse(lbEventResult.SelectedItems[0].SubItems[2].Text)
            };

            Report("\n\nCalling event id: " + EventClass.Id[int.Parse(lbEventResult.SelectedItems[0].SubItems[2].Text)]);
            Report("Output: \n" + text);
            mes.ShowDialog();
        }
        private void lbEventResult_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if(e.Column.ToString() == "Description")
            {
//                _eventClassSortOnDescription();
            }

            lbEventResult.Sorting = lbEventResult.Sorting == SortOrder.Descending ? SortOrder.Ascending : SortOrder.Descending;

            lbEventResult.Sort();
        }
        #endregion

        #region Form actions
        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            tpEventFilter.Size = new Size(Width - 51, Height - 88);
            lbEventResult.Size = new Size(Width - 66, Height - 219);
            tabControl1.Size = new Size(Width - 43, Height - 79);
            btnResultCleanup.Location = new Point(Width - 153, Height - 111);
            btnSearch.Location = new Point(Width - 132, 78);
            rtbBugReport.Size = new Size(Width - 58, Height - 271);
            btnSaveReport.Location = new Point(6, Height - 127);
            tbKeywords.Size = new Size(Width - 506, 20);
            rtbResults.Size = new Size(Width - 63, Height - 140);
            clbKeywords.Location = new Point(Width - 290, 6);
            btnCopyClipboard.Location = new Point(7, Height - 126);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.C && e.Modifiers == Keys.Control)
            {
                if(lbEventResult.SelectedItems.Count > 0)
                {
                    CopyToClipboard(lbEventResult.SelectedItems);
                }
                else
                {
                    CopyToClipboard(lbEventResult.Items);
                }
            }
        }
        #endregion

        private void Utf7_Click(object sender, EventArgs e)
        {
            Encodings.CheckState((ToolStripMenuItem) sender);
        }

        private void Utf8_Click(object sender, EventArgs e)
        {
            Encodings.CheckState((ToolStripMenuItem)sender);
        }

        private void Utf32_Click(object sender, EventArgs e)
        {
            Encodings.CheckState((ToolStripMenuItem)sender);
        }

        private void UtfUnicode_Click(object sender, EventArgs e)
        {
            Encodings.CheckState((ToolStripMenuItem)sender);
        }

        private void UtfAscii_Click(object sender, EventArgs e)
        {
            Encodings.CheckState((ToolStripMenuItem)sender);
        }

        private void UtfBigEndianUnicode_Click(object sender, EventArgs e)
        {
            Encodings.CheckState((ToolStripMenuItem)sender);
        }

        private void EncodingDefault_Click(object sender, EventArgs e)
        {
            Encodings.CheckState((ToolStripMenuItem)sender);
        }
    }
}