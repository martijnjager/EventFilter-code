using EventFilter.Contracts;
using EventFilter.Events;
using EventFilter.Keywords;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace EventFilter
{
    public partial class Form1 : Form
    {
        private event StartSearch _startSearch;

        private IKeywords Keywords;

        private IEvent Events;

        private delegate void StartSearch();

        public Form1()
        {
            InitializeComponent();

            try
            {
                this._startSearch += Search;

                Helper.Form = this;
                dataGridView1.BackgroundColor = BackColor;
                dataGridView1.ClipboardCopyMode = DataGridViewClipboardCopyMode.Disable;

                // Instantiating
                Bootstrap.Boot();

                SetBackgroundWorkerProperties();

                Keywords = Keyword.GetInstance();
                Events = Event.GetInstance();

                this.rtbKeywordsToUse.Text = Keywords.Items.ToString("\n");
                this.rtbIgnorables.Text = Keywords.Ignorable.ToString("\n");
                this.rtbPiracyKeywords.Text = Keywords.Piracy.ToString("\n");
                this.rtbPiracyIgnorable.Text = Keywords.IgnorablePiracy.ToString("\n");
            }
            catch (Exception error)
            {
                Helper.Report("ERROR LOADING FILES: " + error.Message + error.StackTrace);
            }

            UpdateButtonStyles();

            // Enables key events
            KeyPreview = true;
            AllowDrop = true;

            openFileDialog1.InitialDirectory = Bootstrap.CurrentLocation;

            Helper.Report("Initialization completed!");
        }

        private void MiManageKeywords_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tpKeywords;
        }

        private void btnSaveKeywords_Click(object sender, EventArgs e)
        {
            Helper.Report("Saving keywords");
            SaveKeywords();
        }

        private void SaveKeywords()
        {
            string piracy;
            string keywords = piracy = string.Empty;

            if (!rtbKeywordsToUse.Text.Trim().IsEmpty())
                keywords = rtbKeywordsToUse.Text.Replace("\n", "");

            if (!rtbIgnorables.Text.Trim().IsEmpty())
                keywords += rtbIgnorables.Text.Replace("\n", ", -").StartWith(", -");

            if (!rtbPiracyKeywords.Text.Trim().IsEmpty())
                piracy = rtbPiracyKeywords.Text.Replace("\n", ", ");

            if (!rtbPiracyIgnorable.Text.Trim().IsEmpty())
                piracy += rtbPiracyIgnorable.Text.Replace("\n", ", -").StartWith(", -");

            Keywords.SaveKeywords(keywords, piracy);
        }

        private void linklblPiracy_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new Piracy(Events, Events.PiracyEvents).Show();
        }

        #region buttons
        private void btnSearch_Click(object sender, EventArgs e)
        {
            _startSearch();
        }

        private void Search()
        {
            Helper.Report("Start searching events");

            if (Event.GetInstance().FileLocation is FileInfo && Event.GetInstance().FileLocation.FullName != "openFileDialog1")
            {
                lblSelectedFile.Text = @"Selected file: " + Event.GetInstance().FileLocation.FullName;

                Helper.Report("Selected log: " + lblSelectedFile.Text);

                Bootstrap.IsInputEmpty(SearchEventBGWorker, clbKeywords, tbKeywords.Text);
            }
            else
            {
                Messages.SelectFileForSearching();
            }
        }

        private void BtnSaveBugReport(object sender, EventArgs e)
        {
            Bug.CreateReport(rtbBugReport.Text);
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            EventLog text = Event.GetInstance().FindEvent(SearchEvent.EventTable.Rows[e.RowIndex].ItemArray[2].ToString().ToInt());

            foreach(Form openForm in (ReadOnlyCollectionBase) Application.OpenForms)
            {
                if(openForm.Text == "Message")
                {
                    Message((Message)openForm, text);
                    return;
                }
            }

            Message(new Message(Event.GetInstance()), text);
        }

        private void Message(Message message, EventLog text)
        {
            message.Use(text);
            message.Source(null);
            Helper.Report("\n\nCalling event id: " + text.Id);
            Helper.Report("Output: \n" + text);
            message.Show();
        }

        private void btnResultCleanup_Click(object sender, EventArgs e)
        {
            if (eventFilterBGWorker.IsBusy == false)
            {
                Helper.Report("Cleaning up results");
                //foreach (ListViewItem item in lbEventResult.Items)
                //{
                //    item.Remove();
                //}

                eventFilterBGWorker.RunWorkerAsync();
            }
            else
            {
                Messages.Filtering();
            }
        }
        #endregion

        private void UpdateButtonStyles()
        {
            btnSearch.FlatStyle = FlatStyle.Popup;
            btnSearch.FlatAppearance.BorderColor = Color.Wheat;
            btnResultCleanup.FlatStyle = FlatStyle.Popup;
            btnResultCleanup.FlatAppearance.BorderColor = Color.Wheat;
            btnCopyClipboard.FlatStyle = FlatStyle.Popup;
            btnCopyClipboard.FlatAppearance.BorderColor = Color.Wheat;
            btnSaveReport.FlatStyle = FlatStyle.Popup;
            btnSaveReport.FlatAppearance.BorderColor = Color.Wheat;
        }

        private void SetBackgroundWorkerProperties()
        {
            SearchEventBGWorker.WorkerReportsProgress = true;
            SearchEventBGWorker.DoWork += SearchEvent.Search;
            SearchEventBGWorker.ProgressChanged += SearchEvent.SearchEventBGWorker_ProgressChanged;
            SearchEventBGWorker.RunWorkerCompleted += SearchEvent.SearchEventBGWorker_RunWorkerCompleted;

            eventFilterBGWorker.WorkerReportsProgress = true;
            eventFilterBGWorker.DoWork += Event.eventFilterBGWorker_DoWork;
            eventFilterBGWorker.ProgressChanged += Event.eventFilterBGWorker_ProgressChanged;
        }

        #region MenuItems
        private void miSaveKeywords_Click(object sender, EventArgs e)
        {
            Helper.Report("Start saving Keywords");
            saveFileDialog1.ShowDialog();
            Keywords.SaveKeywords(saveFileDialog1.FileName, tbKeywords.Text);
        }

        private void miSelectEventlog_Click(object sender, EventArgs e)
        {
            Helper.Report("Loading event logs");
            openFileDialog1.ShowDialog();

            if (openFileDialog1.FileName.Contains(".zip"))
            {
                string eventLocation = "";
                Zip.ExtractZip(openFileDialog1.FileName, ref eventLocation);
                Events.SetLocation(eventLocation);
            }
            else
            {
                if (openFileDialog1.FileName != "openFileDialog1")
                    Events.SetLocation(openFileDialog1.FileName);
            }

            if (string.IsNullOrEmpty(Events.FileLocation.FullName))
            {
                Messages.NoLogFound();
            }

            Helper.Report("Event log location: " + Events.FileLocation.FullName);

            lblSelectedFile.Text = "Selected file: " + Events.FileLocation.FullName;
        }

        private void miLoadKeywords_Click(object sender, EventArgs e)
        {
            Helper.Report("Loading Keywords to use");
            openFileDialog1.ShowDialog();

            string keyLoc = openFileDialog1.FileName;

            Helper.Report("Keywords to use location: " + keyLoc);

            Keywords.LoadFromLocation(keyLoc).Into(this.clbKeywords);
        }

        private void miAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Name: \t\t EventFilter\nDeveloper: \t Martijn (axe0)\nVersion: \t\t BETA\nDate: \t\t 12-02-2018", "About app", MessageBoxButtons.OK);
        }

        private void miEventFilter_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tpEventFilter;

            clbKeywords.Items.Clear();

            List<string> arr = new List<string>();

            if (!rtbKeywordsToUse.Text.Trim().IsEmpty())
                arr.AddRangeWithPrefix(rtbKeywordsToUse.Text.Split('\n').Trim().ToList());

            if (!rtbIgnorables.Text.Trim().IsEmpty())
                arr.AddRangeWithPrefix(rtbIgnorables.Text.Split('\n').Trim().ToList(), "-");

            if (!rtbPiracyKeywords.Text.Trim().IsEmpty())
                arr.AddRangeWithPrefix(rtbPiracyKeywords.Text.Split('\n').Trim().ToList(), "P: ");

            if (!rtbPiracyIgnorable.Text.Trim().IsEmpty())
                arr.AddRangeWithPrefix(rtbPiracyIgnorable.Text.Split('\n').Trim().ToList(), "-P: ");

            arr.ForEach(i => clbKeywords.Items.Add(i, true));
        }

        private void miBugReport_Click(object sender, EventArgs e) => tabControl1.SelectedTab = tpBugReport;
        #endregion

        #region Listview actions
        //private void lbEventResult_MouseDoubleClick(object sender, MouseEventArgs e)
        //{
        //    EventLog text = Event.Instance.FindEvent(int.Parse(lbEventResult.SelectedItems[0].SubItems[2].Text));
        //    Message mes = new Message()
        //    {
        //        Id = int.Parse(lbEventResult.SelectedItems[0].SubItems[2].Text)
        //    };

        //    Helper.Report("\n\nCalling event id: " + Event.Instance.Eventlogs[int.Parse(lbEventResult.SelectedItems[0].SubItems[2].Text)].Id);
        //    Helper.Report("Output: \n" + text);
        //    mes.ShowDialog();
        //}
        //private void lbEventResult_ColumnClick(object sender, ColumnClickEventArgs e)
        //{
        //    lbEventResult.Sorting = lbEventResult.Sorting == SortOrder.Descending ? SortOrder.Ascending : SortOrder.Descending;

        //    lbEventResult.Sort();
        //}
        #endregion

        #region Form actions
        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            btnResultCleanup.Location = new Point(Width - 153, Height - 111);
            btnSearch.Location = new Point(Width - 132, 78);
            btnSaveReport.Location = new Point(6, Height - 127);
            btnCopyClipboard.Location = new Point(7, Height - 126);
            tpEventFilter.Size = new Size(Width - 51, Height - 88);
            rtbBugReport.Size = new Size(Width - 58, Height - 271);
            rtbResults.Size = new Size(Width - 63, Height - 140);
            //lbEventResult.Size = new Size(Width - 66, Height - 219);
            tabControl1.Size = new Size(Width - 43, Height - 79);
            tbKeywords.Size = new Size(Width - 506, 20);
            clbKeywords.Location = new Point(Width - 290, 6);
            dataGridView1.Size = new Size(Width - 66, Height - 219);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.C && e.Modifiers == Keys.Control)
            {
                if (dataGridView1.SelectedRows.Count > 0)
                    Helper.CopyToClipboard(dataGridView1.SelectedRows);
                else
                    Helper.CopyToClipboard(dataGridView1.Rows);
            }

            if (e.KeyCode != Keys.Return || tabControl1.SelectedTab != tpEventFilter)
                return;

            _startSearch();
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            Array data = e.Data.GetData(DataFormats.FileDrop) as Array;
            string d = data.GetValue(0).ToString();
            string eventLocation = d;

            if (d.Contains(".zip"))
            {
                Zip.ExtractZip(d, ref eventLocation);
                Events.SetLocation(eventLocation);
            }
            else
                Events.SetLocation(eventLocation);

            Helper.Report("Extracted eventlog from " + d);

            lblSelectedFile.Text = eventLocation;
        }
        #endregion

        private void Utf7_Click(object sender, EventArgs e)
        {
            Encodings.CheckState((ToolStripMenuItem)sender);
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

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            string fileKeywords = string.Empty;

            if (File.Exists(Keyword.FileLocation))
                fileKeywords = string.Join("\n", File.ReadAllLines(Keyword.FileLocation));

            string input = string.Empty;

            if (!rtbKeywordsToUse.Text.Trim().IsEmpty())
                input = rtbKeywordsToUse.Text.Replace("\n", ", ");

            if (!rtbIgnorables.Text.Trim().IsEmpty())
                input += rtbIgnorables.Text.Replace("\n", ", -").StartWith("-");

            if(!rtbPiracyKeywords.Text.Trim().IsEmpty() || !rtbPiracyIgnorable.Text.Trim().IsEmpty())
            {
                input = input.Substring(0, input.Length - 3);
                input = input.EndWith("\nPIRACY: ");
            }

            if (!rtbPiracyKeywords.Text.Trim().IsEmpty())
                input += rtbPiracyKeywords.Text.Replace("\n", ", ");

            if (!rtbPiracyIgnorable.Text.Trim().IsEmpty())
                input += rtbPiracyIgnorable.Text.Replace("\n", ", -").StartWith("-");

            input = input.Substring(0, input.Length - 3);

            if (input.IsEmpty() || input == fileKeywords || MessageBox.Show("Unsaved changes in the keywords have been detected. \n Do you want to save the changes?", 
                "Keyword changes", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            SaveKeywords();
        }
    }
}