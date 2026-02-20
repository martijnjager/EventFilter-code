using EventFilter.Contracts;
using EventFilter.Events;
using EventFilter.Keywords;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace EventFilter
{
    public partial class Form1 : Form
    {
        private delegate void StartSearch();

        private event StartSearch EventSearchEvent;

        private readonly IKeywords Keywords;

        private new readonly IEvent Events;

        private int currentCheckListState;

        private string originalSearchText;

        public Form1()
        {
            InitializeComponent();

            try
            {
                EventSearchEvent += Search;
                //saveKeywords += Helper.SaveKeywords;

                Helper.Form = this;
                dataGridView1.BackgroundColor = BackColor;
                dataGridView1.ClipboardCopyMode = DataGridViewClipboardCopyMode.Disable;

                Keywords = Keyword.GetInstance();
                Events = Event.GetInstance();

                // Instantiating
                Bootstrap.Boot(clbKeywords);

                SetBackgroundWorkerProperties();

                rtbKeywordsToUse.Text = Keywords.ItemsFile.ToString("\n");
                rtbIgnorables.Text = Keywords.IgnorableFile.ToString("\n");
                rtbPiracyKeywords.Text = Keywords.PiracyFile.ToString("\n");
                rtbPiracyIgnorable.Text = Keywords.IgnorablePiracyFile.ToString("\n");

                currentCheckListState = 1;
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

        private void BtnSaveKeywords_Click(object sender, EventArgs e)
        {
            if (ShouldSaveKeywords())
            {
                Helper.Report("Saving keywords");

                Keyword.SaveKeywords(new[] { rtbKeywordsToUse.Text, rtbIgnorables.Text, rtbPiracyKeywords.Text, rtbPiracyIgnorable.Text });

                //Helper.SaveKeywords(new[] { rtbKeywordsToUse.Text, rtbIgnorables.Text, rtbPiracyKeywords.Text, rtbPiracyIgnorable.Text });
            }
        }

        private void LinklblPiracy_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //new Piracy(Events, Events.PiracyEvents).Show();
        }

        #region buttons
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            EventSearchEvent();
        }

        private void Search()
        {
            Helper.Report("Start searching events");

            if (Event.FileLocation is FileInfo && Event.FileLocation.FullName != "openFileDialog1")
            {
                lblSelectedFile.Text = @"Selected file: " + Event.FileLocation.FullName;

                Helper.Report("Selected log: " + lblSelectedFile.Text);

                if (tbKeywords.Text.IsEmpty() && clbKeywords.CheckedItems.Count == 0)
                {
                    if (Messages.VerifyContinueNoInput() == DialogResult.Yes)
                    {
                        if (!SearchEventBGWorker.IsBusy)
                        {
                            originalSearchText = btnSearch.Text;
                            btnSearch.Text = "Searching...";
                            btnSearch.Enabled = false;
                            Cursor = Cursors.WaitCursor;
                            SearchEventBGWorker.RunWorkerAsync();
                        }
                    }
                }
                else
                {
                    if (!SearchEventBGWorker.IsBusy)
                    {
                        originalSearchText = btnSearch.Text;
                        btnSearch.Text = "Searching...";
                        btnSearch.Enabled = false;
                        Cursor = Cursors.WaitCursor;

                        List<string> items = tbKeywords.Text.Split(' ').ToList();

                        Tuple<List<string>> keywords = new Tuple<List<string>>(items);

                        SearchEventBGWorker.RunWorkerAsync(keywords);
                    }
                }
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

        private void DataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            ListSortDirection direction = GetListSortDirection();

            int eventIndex = 0;

            if (direction == ListSortDirection.Descending)
            {
                eventIndex = Searcher.EventTable.Rows.Count - e.RowIndex - 1;
            }
            else
            {
                eventIndex = e.RowIndex;
            }

            var data = Searcher.EventTable.Rows[eventIndex].ItemArray[2];
            Events.EventLog text = Event.GetInstance().FindEvent(data.ToString().ToInt());

            foreach (Form openForm in (ReadOnlyCollectionBase)System.Windows.Forms.Application.OpenForms)
            {
                if (openForm.Text == "Message")
                {
                    Helper.Message((Message)openForm, text);
                    return;
                }
            }

            Helper.Message(new Message(Event.GetInstance()), text);
        }

        private ListSortDirection GetListSortDirection()
        {
            return dataGridView1.SortOrder == SortOrder.Ascending ? ListSortDirection.Ascending : ListSortDirection.Descending;
        }

        private void BtnResultCleanup_Click(object sender, EventArgs e)
        {
            if (!eventFilterBGWorker.IsBusy)
            {
                Helper.Report("Cleaning up results");

                eventFilterBGWorker.RunWorkerAsync();

                return;
            }
            
            Messages.Filtering();
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
            SearchEventBGWorker.DoWork += Searcher.Search;
            SearchEventBGWorker.ProgressChanged += Searcher.SearchEventBGWorker_ProgressChanged;
            SearchEventBGWorker.ProgressChanged += UpdateResultsSearcher;

            SearchEventBGWorker.RunWorkerCompleted += Searcher.SearchEventBGWorker_RunWorkerCompleted;
            SearchEventBGWorker.RunWorkerCompleted += UpdateUIFinishSearch;

            eventFilterBGWorker.WorkerReportsProgress = true;
            eventFilterBGWorker.DoWork += Event.EventFilterBGWorker_DoWork;
            eventFilterBGWorker.ProgressChanged += Event.EventFilterBGWorker_ProgressChanged;
            eventFilterBGWorker.ProgressChanged += UpdateResultsEventFilter;
            eventFilterBGWorker.RunWorkerCompleted += UpdateUIFinishFilter;
        }

        #region MenuItems
        private void MiSelectEventlog_Click(object sender, EventArgs e)
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

            if (string.IsNullOrEmpty(Event.FileLocation.FullName))
            {
                Messages.NoLogFound();
            }

            Helper.Report("Event log location: " + Event.FileLocation.FullName);

            lblSelectedFile.Text = "Selected file: " + Event.FileLocation.FullName;
        }

        private void MiLoadKeywords_Click(object sender, EventArgs e)
        {
            Helper.Report("Loading Keywords to use");
            openFileDialog1.ShowDialog();

            string keyLoc = openFileDialog1.FileName;

            Helper.Report("Keywords to use location: " + keyLoc);

            Keywords.LoadFromLocation(keyLoc).AddInto(clbKeywords);
        }

        private void MiAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Name: \t\t EventFilter\nDeveloper: \t axe0\nVersion: \t\t 1.0.1\nDate: \t\t 13 Sept 2020", "About app", MessageBoxButtons.OK);
        }

        private void MiEventFilter_Click(object sender, EventArgs e)
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

        private void MiBugReport_Click(object sender, EventArgs e) => tabControl1.SelectedTab = tpBugReport;

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
            cbCheckAll.Location = new Point(clbKeywords.Location.X + clbKeywords.Width + 6, cbCheckAll.Location.Y);

            double resizeValue = 5.3;
            rtbKeywordsToUse.Size = new Size(Convert.ToInt32(Convert.ToDouble(Width) / resizeValue), Height - 285);

            rtbIgnorables.Location = new Point(rtbKeywordsToUse.Location.X + rtbKeywordsToUse.Size.Width + 46, rtbIgnorables.Location.Y);
            lblIgnoreKeywords.Location = new Point(rtbIgnorables.Location.X, lblIgnoreKeywords.Location.Y);
            rtbIgnorables.Size = new Size(Convert.ToInt32(Convert.ToDouble(Width) / resizeValue), Height - 285);

            rtbPiracyIgnorable.Location = new Point(rtbIgnorables.Location.X + rtbIgnorables.Size.Width + 46, rtbPiracyIgnorable.Location.Y);
            rtbPiracyIgnorable.Size = new Size(Convert.ToInt32(Convert.ToDouble(Width) / resizeValue), Height - 285);
            lblPiracyIgnorable.Location = new Point(rtbPiracyIgnorable.Location.X, lblPiracyIgnorable.Location.Y);

            rtbPiracyKeywords.Location = new Point(rtbPiracyIgnorable.Location.X + rtbPiracyIgnorable.Size.Width + 46, rtbPiracyKeywords.Location.Y);
            rtbPiracyKeywords.Size = new Size(Convert.ToInt32(Convert.ToDouble(Width) / resizeValue), Height - 285);
            lblPiracy.Location = new Point(rtbPiracyKeywords.Location.X, lblPiracy.Location.Y);

            btnSaveKeywords.Location = new Point(rtbIgnorables.Location.X + rtbIgnorables.Size.Width - 18, tpKeywords.Size.Height - 94);
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

            EventSearchEvent();
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

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ShouldSaveKeywords())
            {
                Helper.Report("Saving keywords");

                Keyword.SaveKeywords(new[] { rtbKeywordsToUse.Text, rtbIgnorables.Text, rtbPiracyKeywords.Text, rtbPiracyIgnorable.Text });
            }
        }
        #endregion

        private void CbCheckAll_CheckedChanged(object sender, EventArgs e)
        {
            UpdateCheckListItems();
        }

        public void UpdateCheckListItems()
        {
            currentCheckListState = currentCheckListState == 0 ? currentCheckListState = 1 : currentCheckListState = 0;

            for (int i = 0; i < clbKeywords.Items.Count; i++)
            {
                clbKeywords.SetItemCheckState(i, (CheckState)currentCheckListState);
            }
        }

        private bool ShouldSaveKeywords()
        {
            string input, fileKeywords;
            fileKeywords = input = string.Empty;

            if (File.Exists(Keyword.FileLocation))
                fileKeywords = string.Join("\n", File.ReadAllLines(Keyword.FileLocation));

            string keywordsToUse = rtbKeywordsToUse.Text.RemoveTrailingNewLine();
            string ignorable = rtbIgnorables.Text.RemoveTrailingNewLine();
            string piracy = rtbPiracyKeywords.Text.RemoveTrailingNewLine();
            string igPiracy = rtbPiracyIgnorable.Text.RemoveTrailingNewLine();
            string trace = (new StackTrace()).GetFrame(1).GetMethod().Name;

            if (!keywordsToUse.Trim().IsEmpty())
                input = keywordsToUse.Replace("\n", ", ");

            if (!ignorable.Trim().IsEmpty())
                input += ignorable.Replace("\n", ", -").StartWith(", -");

            if (!piracy.Trim().IsEmpty() || !igPiracy.Trim().IsEmpty())
                input = input.EndWith("\nPIRACY: ");

            if (!piracy.Trim().IsEmpty())
                input += piracy.Replace("\n", ", ");

            if (!igPiracy.Trim().IsEmpty())
            {
                if(!piracy.IsEmpty())
                    input += ", ";

                input += igPiracy.Replace("\n", ", -").StartWith("-");
            }

            if(trace != "BtnSaveKeywords_Click")
            {
                if (input.IsEmpty() || input == fileKeywords || MessageBox.Show("Unsaved changes in the keywords have been detected. \n Do you want to save the changes?",
                    "Keyword changes", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    return false;
                else
                    return true;
            }

            return true;
        }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

        }

        private void clbKeywords_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            string keyword = clbKeywords.Items[e.Index].ToString();

            Keywords.Select(keyword, e.NewValue == CheckState.Checked);
        }

        private void UpdateResultsSearcher(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 0)
            {
                dataGridView1.DataSource = Searcher.EventTable;
                Searcher.EventTable.UpdateText += EventTable_UpdateText;
            }

            string text = e.UserState.ToString();
            string state = text.Substring(0, e.UserState.ToString().IndexOf(": ", StringComparison.Ordinal));

            switch (state)
            {
                case "Time":
                    lblTime.Text = text.Replace("Time: ", "") + "s";
                    break;

                case "Piracy":
                    lblKMS.Text = text.Replace("Piracy:", "");
                    lblKMS.ForeColor = Color.Red;
                    break;

                case "Counter":
                    lblResultCount.Text = "Events found: " + Searcher.EventTable.GetPagingInfo();// text.Replace("Counter: ", "");
                    break;

                case "Log":
                    rtbBugReport.AppendText(text.Replace("Log: ", ""));
                    break;
            }
        }

        private void UpdateUIFinishSearch(object sender, RunWorkerCompletedEventArgs e)
        {
            btnSearch.Text = originalSearchText ?? "Search";
            btnSearch.Enabled = true;
            Cursor = Cursors.Default;

            if (Events.HasPiracyEvents())
            {
                linklblPiracy.Visible = true;
                lblKMS.Visible = true;
            }
            else
            {
                lblKMS.Visible = false;
                linklblPiracy.Visible = false;
            }

            if (Searcher.EventTable.HasNextPage())
            {
                linkLblNextPage.Visible = true;
            }
            else
            {
                linkLblNextPage.Visible = false;
            }

            if (Searcher.EventTable.HasPreviousPage())
            {
                linkLblPreviousPage.Visible = true;
            }
            else
            {
                linkLblPreviousPage.Visible = false;
            }
        }

        private void UpdateUIFinishFilter(object sender, RunWorkerCompletedEventArgs e)
        {
            UpdatePagingButtons();
        }

        private void UpdatePagingButtons()
        {
            if (Searcher.EventTable.HasNextPage())
            {
                linkLblNextPage.Visible = true;
            }
            else
            {
                linkLblNextPage.Visible = false;
            }

            if (Searcher.EventTable.HasPreviousPage())
            {
                linkLblPreviousPage.Visible = true;
            }
            else
            {
                linkLblPreviousPage.Visible = false;
            }
        }

        private void UpdateResultsEventFilter(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 0)
            {
                dataGridView1.DataSource = Searcher.EventTable;
            }

            if (e.UserState.ToString().Contains("Resultcount:"))
            {
                lblResultCount.Text = e.UserState.ToString().Replace("Resultcount: ", "").Trim('{').Trim('}');
            }
        }

        private void EventTable_UpdateText(string text)
        {
            lblResultCount.Text = "Events found: " + text;

            UpdatePagingButtons();
        }

        private void linkLblPreviousPage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (Searcher.EventTable.HasPreviousPage())
            {
                Searcher.EventTable.PreviousPage();
            }
        }

        private void linkLblNextPage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (Searcher.EventTable.HasNextPage())
            {
                Searcher.EventTable.NextPage();
            }
        }
    }
}