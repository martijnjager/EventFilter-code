using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Collections;

namespace EventFilter
{
    public partial class Form1 : Form
    {
        //private static string Keyword.EventLocation;
        //private static string Keyword;
        private ListViewItem listview;
        string[] _eventArray = new string[0];
        string[] _keywords = new string[0];
        
        public List<string> _eventId = new List<string>();
        public List<string> _events = new List<string>();
        public List<string> _eventDate = new List<string>();

        private int sortColumn = -1;

        SearchEvents _event = new SearchEvents();
        Background _background = new Background();
        Keyword Keyword = new Keyword();

        public Form1()
        {
            InitializeComponent();

            //Keyword = keywords.KeyLocation;
            //Keyword.EventLocation = keywords.EventLocation;

            listview = new ListViewItem();

            #region Load files into app
            //Keyword.EventLocation = Background.CheckFileExistence(keywords.EventLocation) ? keywords.EventLocation = Background.GetLocation() + keywords.EventLocation : "";

            lblSelectedFile.Text = "Selected file: " + (Background.CheckFileExistence(Keyword.EventLocation) ? Keyword.EventLocation : "");

            if((Background.CheckFileExistence(Keyword.EventLocation) ? Keyword.EventLocation : "") == "")
                BugReportLog("No eventlog.txt found");
            else
                BugReportLog("Load event log from " + Keyword.EventLocation);

            Keyword.GetKeywords(Keyword.KeyLocation);
            tbKeywords.Text = Keyword.Keywords;

            if(Keyword.Keywords == "")
                BugReportLog("No keywords.txt found");
            else
                BugReportLog("Load keywords from " + Background.GetLocation() + Keyword.KeyLocation);
            #endregion

            SearchEventBGWorker.WorkerReportsProgress = true;
            backgroundWorker1.WorkerReportsProgress = true;
            operatorBGWorker.WorkerReportsProgress = true;

            BugReportLog("Initialization completed!");
        }

        #region buttons
        private void btnSearch_Click(object sender, EventArgs e)
        {
            BugReportLog("Start searching events");

            _eventArray = null;

            string keyword = tbKeywords.Text;
            var keyWords = keyword.Split(',');
            keyWords = Array.TrimArray(keyWords);

            Keyword.CheckKeywordsOnOperator(keyWords);

            lbEventResult.Items.Clear();

            BugReportLog("Keywords to use: " + Array.ConvertArrayToString(keyWords, ", "));

            if (Keyword.EventLocation != "openFileDialog1")
            {
                lblSelectedFile.Text = "Selected file: " + Keyword.EventLocation;

                BugReportLog("Selected log: " + lblSelectedFile.Text);

                if (keyword != "")
                {
                    if (_background.operators != null)
                    {
                        if (operatorBGWorker.IsBusy == false)
                        {
                            operatorBGWorker.RunWorkerAsync();
                        }
                    }
                    if (_background.operators == null)
                    {
                        if (SearchEventBGWorker.IsBusy == false)
                        {
                            SearchEventBGWorker.RunWorkerAsync();
                        }
                    }
                }
                else
                {
                    MessageWrite("Please provide keywords to search for.", "No keywords provided", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageWrite("Please select a file to search through.", "No file selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnSaveBugReport(object sender, EventArgs e)
        {
            if (Keyword.EventLocation != "" && tbKeywords.Text != "")
            {
                Bug.CreateBugReport(Keyword.EventLocation, rtbBugReport.Text, tbKeywords.Text);

                if(Bug.exception != "")
                {
                    MessageWrite(Bug.exception, "Error collecting logs", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string directory = Background.GetLocation() + "\\bugs";

                MessageBox.Show("Logs have been saved in " + directory, "Logs saved", MessageBoxButtons.OK);
            }
                
            else
                MessageBox.Show("No log could be saved! Check if the eventlog and keywords are loaded", "No log", MessageBoxButtons.OK);
        }

        private void btnResultCleanup_Click(object sender, EventArgs e)
        {
            if(backgroundWorker1.IsBusy == false)
            {
                //MessageWrite("This may take some time", "Cleaning up", MessageBoxButtons.OK, MessageBoxIcon.None);
                foreach (ListViewItem item in lbEventResult.Items)
                {
                    item.Remove();
                }

                backgroundWorker1.RunWorkerAsync();
            }
        }
        #endregion

        #region BackgroundWorkers
        private void SearchEventBGWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            try
            {
                string keyword = tbKeywords.Text;
                var keywords = Keyword.ValidateKeywords(keyword);
                var watch = System.Diagnostics.Stopwatch.StartNew();
                _eventArray = Array.ConstructEventArray(Keyword.EventLocation);
                int resultCount = 0;
                worker.ReportProgress(1, "Log: Parameters used: \t filepath: " + Keyword.EventLocation + "\n\t keywords to use: " + Array.ConvertArrayToString(keywords, ", "));
                worker.ReportProgress(2, "Log: Lines in eventArray: " + _eventArray.Length);
                int i = -1;
                var lastKeyword = keywords[0];
                worker.ReportProgress(3, "Log: First lastKeyword: " + lastKeyword);
                int progress = 3;
                int keyProgress = 0;
                int logProgress = keyProgress;

                foreach (var key in keywords)
                {
                    int localCounter = 0;

                    keyProgress++;
                    worker.ReportProgress(keyProgress + progress, "Log: Overwriting lastKeyword " + lastKeyword + " with " + key + "\n\n");
                    keyProgress++;
                    lastKeyword = key;

                    worker.ReportProgress(keyProgress + progress, "Log: Following results have been found using keyword: " + key);

                    for (i = 0; i < _eventArray.Length; i++)
                    {
                        string[] eventEntry = new string[3];

                        if (_eventArray[i].Contains(key))
                        {
                            int a = 0;

                            while (!_eventArray[i + a].Contains("Event["))
                            {
                                if (_eventArray[i + a].Contains("Description"))
                                {
                                    eventEntry[1] = _eventArray[(i + a) + 1].ToString();
                                    _events.Add(_eventArray[i]);

                                    // Add the first line of description into the list.
                                    _eventId.Add((i + a + 1).ToString());

                                    localCounter++;
                                    resultCount++;
                                }

                                if (_eventArray[i + a].Contains("Date"))
                                {

                                    _eventDate.Add(_eventArray[i + a]);

                                    // Id
                                    eventEntry[2] = i.ToString();

                                    eventEntry[0] = _eventArray[i + a].ToString();

                                    worker.ReportProgress(logProgress + progress, "Log: \t Line nr: " + (i + a) + ": " + eventEntry[0] + ": " + eventEntry[1]);
                                    logProgress++;

                                    break;
                                }
                                a--;
                            }

                            if(resultCount >= 10000)
                            {
                                MessageWrite("There are over 5000 events matching the keywords", "Over 5000 events", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                break;
                            }

                            worker.ReportProgress(logProgress + progress, "Event: " + eventEntry[0] + " + " + eventEntry[1] + " + " + eventEntry[2]);
                            logProgress++;
                        }
                    }

                    if (resultCount >= 10000) break;

                    worker.ReportProgress(logProgress + progress, "Log: \nFound " + localCounter.ToString() + " with keyword " + key);
                    logProgress++;
                    worker.ReportProgress(logProgress + progress, "Log: ===========================================\n\n\n\n\n");
                    logProgress++;
                }

                worker.ReportProgress(logProgress + progress, "Log: \n\nEvents found: " + resultCount.ToString());
                logProgress++;
                worker.ReportProgress(logProgress + progress, "Counter: Events found: " + resultCount.ToString());
                logProgress++;

                if (resultCount == 0)
                {
                    MessageBox.Show("No event log has the provided keywords.", "No result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                watch.Stop();
                var elapsedTime = watch.Elapsed.TotalSeconds;

                worker.ReportProgress(logProgress + progress, "Time: Found results in: " + elapsedTime.ToString());

                e.Result = _eventId;
            }
            catch (Exception exc)
            {
                worker.ReportProgress(0, "Log: Error: " + exc.Message);
                MessageBox.Show("A problem has occured.\nPlease notify the developer of this issue!", "App crashed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SearchEventBGWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if(e.UserState.ToString().Contains("Log: "))
                BugReportLog(e.UserState.ToString().Replace("Log: ", ""));

            if (e.UserState.ToString().Contains("Event: ") && e.UserState.ToString().Contains("Date: "))
                AddListItem(Array.ConvertStringToArray(e.UserState.ToString().Replace("Event: ", ""), " + "));

            if (e.UserState.ToString().Contains("Time: "))
                lblTime.Text = e.UserState.ToString().Replace("Time: ", "");

            if (e.UserState.ToString().Contains("Counter: "))
                lblResultCount.Text = e.UserState.ToString().Replace("Counter: ", "");
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            //string[] eventArray = _events.ToArray();
            string[] eventArr = _event.FilterDuplicates(_events, _eventId, _eventDate);

            string[] eventId = _event.eventId;
            string[] eventDate = _event.eventDate;

            int recorder = 0;

            for (int i = 0; i < eventId.Length; i++)
            {
                string[] data = new string[3];
                data[0] = "Data: " + eventDate[i];
                data[1] = "Data: " + eventArr[i];
                data[2] = "Data: " + eventId[i];

                backgroundWorker1.ReportProgress(recorder, data);
                recorder++;
            }

            backgroundWorker1.ReportProgress(recorder + 1, "Resultcount: " +lblResultCount.Text + "\t, After filtering: " + eventArr.Length);
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.UserState.ToString().Contains("Resultcount: ") == false)
            {
                string[] items = ((IEnumerable)e.UserState).Cast<object>().Select(x => x.ToString()).ToArray();

                if (items.Length > 1)
                {
                    for (int i = 0; i < items.Length; i++)
                    {
                        items[i] = items[i].Replace("Data: ", "").Trim('{').Trim('}');
                    }
                    AddListItem(items);

                    //BugReportLog("Filtering results to: \n\t" + array.ConvertArrayToString(items, "\n\t"));
                }
            }
            else
                lblResultCount.Text = e.UserState.ToString().Replace("Resultcount: ", "").Trim('{').Trim('}');
        }
        #endregion

        #region Voids to append elements

        public void BugReportLog(string log = "")
        {
            rtbBugReport.AppendText(log + "\n");
        }

        public void AddListItem(string[] item)
        {
            BugReportLog("Adding: " + Array.ConvertArrayToString(item, "\t")+"\n");
            var addViewItem = new ListViewItem(item);
            lbEventResult.Items.Add(addViewItem);
        }

        public static void MessageWrite(string text, string title, MessageBoxButtons button, MessageBoxIcon icon)
        {
            MessageBox.Show(text, title, button, icon);
        }

        public void ResultCount(string text)
        {
            lblResultCount.Text = text;
        }

        public void Time(string text)
        {
            lblTime.Text = text;
        }
        #endregion
        
        #region MenuItems
        private void miSaveKeywords_Click(object sender, EventArgs e)
        {
            BugReportLog("Start saving keywords");
            saveFileDialog1.ShowDialog();

            string keyword = saveFileDialog1.FileName;

            try
            {
                StreamWriter saveKeywords = new StreamWriter(keyword);
                saveKeywords.WriteLine(tbKeywords.Text);
                BugReportLog("Saving keywords " + tbKeywords.Text + " to file");
                saveKeywords.Close();
            }
            catch(Exception ex)
            {
                BugReportLog("An error occured when trying to save keywords to use: " + ex.Message);
            }
        }

        private void miSelectEventlog_Click(object sender, EventArgs e)
        {
            BugReportLog("Loading event logs");
            openFileDialog1.ShowDialog();

            Keyword.EventLocation = openFileDialog1.FileName;

            BugReportLog("Event log location: " + Keyword.EventLocation);

            lblSelectedFile.Text = "Selected file: " + Keyword.EventLocation;
        }

        private void miLoadKeywords_Click(object sender, EventArgs e)
        {
            BugReportLog("Loading keywords to use");
            openFileDialog1.ShowDialog();

            string keyLoc = openFileDialog1.FileName;

            BugReportLog("Keywords to use location: " + keyLoc);

            Keyword.GetKeywords(keyLoc);

            tbKeywords.Text = Keyword.Keywords;
        }

        private void miAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Name: \t EventFilter\nDeveloper: Martijn (axe0)\nVersion: \t BETA\nDate: \t01-07-2017\\"+ System.DateTime.Now, "About app", MessageBoxButtons.OK);
        }

        private void miTemplate_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tpTemplate;
        }

        private void miEventFilter_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tpEventFilter;
        }

        private void miBugReport_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tpBugReport;
        }
        #endregion

        #region Listview actions
        private void lbEventResult_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            BugReportLog("\n\nCalling event with id: " + lbEventResult.SelectedItems[0].SubItems[2].Text);
            string eventData = _event.SearchEvent(lbEventResult.SelectedItems[0].SubItems[2].Text);

            BugReportLog("Output: \n" + eventData);

            MessageBox.Show(eventData, "Event log");
            eventData = "";
        }

        private void lbEventResult_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if(e.Column != sortColumn)
            {
                sortColumn = e.Column;
                lbEventResult.Sorting = SortOrder.Ascending;
            }
            else
            {
                if(lbEventResult.Sorting == SortOrder.Ascending)
                {
                    lbEventResult.Sorting = SortOrder.Descending;
                }
                else
                {
                    lbEventResult.Sorting = SortOrder.Ascending;
                }
            }

            lbEventResult.Sort();
        }

        private void operatorBGWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            worker.ReportProgress(0, _background.GetCount(_background.operators));
        }
        #endregion

        private void operatorBGWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            MessageWrite(e.UserState.ToString(), "Result", MessageBoxButtons.OK, MessageBoxIcon.None);
        }
    }
}