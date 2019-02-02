using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using EventFilter.Events;

namespace EventFilter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            try
            {
                Actions.Form = this;

                // Instantiating
                Bootstrap.Boot();

                Bootstrap.FilesFound();

                //Event.Instance.MapEvents();

                SetBackgroundWorkerProperties();
            }
            catch (Exception error)
            {
                Actions.Report("ERROR LOADING FILES: " + error.Message + error.StackTrace);
            }

            UpdateButtonStyles();

            // Enables key events
            KeyPreview = true;
            AllowDrop = true;

            openFileDialog1.InitialDirectory = Bootstrap.CurrentLocation;

            Actions.Report("Initialization completed!");
        }

        #region buttons
        private void btnSearch_Click(object sender, EventArgs e)
        {
            Actions.Report("Start searching events");

            if (Event.Instance.EventLocation is FileInfo && Event.Instance.EventLocation.FullName != "openFileDialog1")
            {
                lblSelectedFile.Text = @"Selected file: " + Event.Instance.EventLocation.FullName;

                Actions.Report("Selected log: " + lblSelectedFile.Text);

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

        private void btnResultCleanup_Click(object sender, EventArgs e)
        {
            if(eventFilterBGWorker.IsBusy == false)
            {
                Actions.Report("Cleaning up results");
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
            Actions.Report("Start saving Keywords");
            saveFileDialog1.ShowDialog();
            Keywords.Keyword.Instance.SaveToFile(saveFileDialog1.FileName, tbKeywords.Text);
        }

        private void miSelectEventlog_Click(object sender, EventArgs e)
        {
            Actions.Report("Loading event logs");
            openFileDialog1.ShowDialog();

            if(openFileDialog1.FileName.Contains(".zip"))
            {
                string eventLocation = "";
                Zip.ExtractZip(openFileDialog1.FileName, ref eventLocation);
                Event.Instance.SetLocation(new FileInfo(eventLocation));
            }
            else
            {
                if(openFileDialog1.FileName != "openFileDialog1")
                    Event.Instance.SetLocation(new FileInfo(openFileDialog1.FileName));
            }

            if(string.IsNullOrEmpty(Event.Instance.EventLocation.FullName))
            {
                Messages.NoLogFound();
            }

            Actions.Report("Event log location: " + Event.Instance.EventLocation.FullName);

            lblSelectedFile.Text = "Selected file: " + Event.Instance.EventLocation.FullName;
        }

        private void miLoadKeywords_Click(object sender, EventArgs e)
        {
            Actions.Report("Loading Keywords to use");
            openFileDialog1.ShowDialog();

            string keyLoc = openFileDialog1.FileName;

            Actions.Report("Keywords to use location: " + keyLoc);

            Keywords.Keyword.Instance.LoadFromLocation(keyLoc).LoadIntoCLB();
        }

        private void miAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Name: \t\t EventFilter\nDeveloper: \t Martijn (axe0)\nVersion: \t\t BETA\nDate: \t\t 12-02-2018", "About app", MessageBoxButtons.OK);
        }

        private void miEventFilter_Click(object sender, EventArgs e) => tabControl1.SelectedTab = tpEventFilter;

        private void miBugReport_Click(object sender, EventArgs e) => tabControl1.SelectedTab = tpBugReport;
        #endregion

        #region Listview actions
        private void lbEventResult_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            string text = Event.Instance.FindEvent(int.Parse(lbEventResult.SelectedItems[0].SubItems[2].Text));
            Message mes = new Message(text)
            {
                Id = int.Parse(lbEventResult.SelectedItems[0].SubItems[2].Text)
            };

            Actions.Report("\n\nCalling event id: " + Event.Instance.Eventlogs[int.Parse(lbEventResult.SelectedItems[0].SubItems[2].Text)].Id);
            Actions.Report("Output: \n" + text);
            mes.ShowDialog();
        }
        private void lbEventResult_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            lbEventResult.Sorting = lbEventResult.Sorting == SortOrder.Descending ? SortOrder.Ascending : SortOrder.Descending;

            lbEventResult.Sort();
        }
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
            lbEventResult.Size = new Size(Width - 66, Height - 219);
            tabControl1.Size = new Size(Width - 43, Height - 79);
            tbKeywords.Size = new Size(Width - 506, 20);
            clbKeywords.Location = new Point(Width - 290, 6);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.C && e.Modifiers == Keys.Control)
            {
                if(lbEventResult.SelectedItems.Count > 0)
                {
                    Actions.CopyToClipboard(lbEventResult.SelectedItems);
                }
                else
                {
                    Actions.CopyToClipboard(lbEventResult.Items);
                }
            }
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
                Event.Instance.SetLocation(new FileInfo(eventLocation));
            }
            else
                Event.Instance.SetLocation(new FileInfo(eventLocation));

            Actions.Report("Extracted eventlog from " + d);

            lblSelectedFile.Text = eventLocation;
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