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

namespace EventFilter
{
    public partial class Form1 : Form
    {
        /**
         * TODO:
         * File corruption:
         *  code:
         *      xxxx
         *      xxxxx
         *      xxxx
         *  
         * Registry corruption;
         *  code:
         *      xxxx
         *      xxxxx
         *      xxxx
         *      
         * save results in text file
         * new folder for results
         * 
         * Bug report - doing
         * 
         * Option: add keywords to ignore
         * Option: add parameters in keywords (datebefore:xxxx, datebefore:xxxx, datebetween:xxx:xxx)
         */


        public string _filePath = "";
        public string _keyword = "";
        public ListViewItem listview;
        public string[] _eventArray = new string[0];
        public string[] _eventId = new string[0];
        public string[] _keywords = new string[0];
        public string[,] _eventsPerKey = new string[0,100];
        public List<string> _eventStack = new List<string>();

        ArrayHandler array = new ArrayHandler();
        BackgroundWorker IOHandler = new BackgroundWorker();
        SearchEvents events = new SearchEvents();
        Bug bug = new Bug();

        private int sortColumn = -1;

        public Form1()
        {
            InitializeComponent();

            listview = new ListViewItem();

            BugReportLog("Developer: Martijn (axe0)");
            BugReportLog("App version: BETA");
            BugReportLog("");
            BugReportLog("");

            if (events.CheckFileExistence("\\eventlog.txt") == true)
            {
                _filePath = Directory.GetCurrentDirectory() + "\\eventlog.txt";
                lblSelectedFile.Text = "Selected file: " + _filePath;

                BugReportLog("Load event log from " + _filePath);
            }

            if(events.CheckFileExistence("\\keywords.txt") == true)
            {
                string keywords = Directory.GetCurrentDirectory() + "\\keywords.txt";
                tbKeywords.Text = events.GetKeywords(keywords);

                BugReportLog("Load keywords to use from " + keywords);
            }

            backgroundWorker1.WorkerReportsProgress = true;

            BugReportLog("Initialization completed!");
        }


        private void btnSearch_Click(object sender, EventArgs e)
        {
            BugReportLog("Start searching events");

            _keyword = tbKeywords.Text;
            _keyword = _keyword.Replace(" ", "");
            var keyWords = _keyword.Split(',');

            lbEventResult.Items.Clear();

            BugReportLog("Keywords to use: " + array.ConvertArrayToString(keyWords, ", "));

            if (_filePath != "openFileDialog1")
            {
                lblSelectedFile.Text = "Selected file: " + _filePath;

                BugReportLog("Selected log: " + lblSelectedFile.Text);

                if(_keyword != "")
                {
                    //Thread IOThread = new Thread(new ThreadStart(SearchEvents));

                    //IOThread.Start();

                    if (backgroundWorker1.IsBusy == false)
                    {
                        backgroundWorker1.RunWorkerAsync();
                    }

                    //SearchEvents();
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

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            //try
            //{
                // Initialization
                _keyword = tbKeywords.Text;
                var keywords = events.ValidateKeywords(_keyword);
                var watch = System.Diagnostics.Stopwatch.StartNew();

                var lines = File.ReadLines(_filePath);
                _eventArray = new string[lines.Count()];
                _eventId = new string[lines.Count()];

                int resultCount = 0;

                worker.ReportProgress(1, "Log: Parameters used: \t filepath: " + _filePath + "\n\t keywords to use: " + array.ConvertArrayToString(keywords, ", "));

                worker.ReportProgress(2, "Log: Lines in eventArray: " + lines.Count());

                int i = -1;
                foreach (var line in lines)
                {
                    i++;
                    _eventArray[i] = line;
                }

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
                            int counter = 0;

                            for (int a = -12; a < counter; a++)
                            {
                                /**
                                 * Scan the event Description part to find the keyword, if not found nothing is returned.
                                 */

                                string text = _eventArray[i + a];
                                // Count back from position of keyword to get the first line of description
                                if (_eventArray[i - 1].Contains("Description"))
                                {
                                    eventEntry[1] = _eventArray[i].ToString();
                                    resultCount++;
                                    localCounter++;

                                    break;
                                }
                                else
                                {
                                    text = _eventArray[i + a];
                                    if (text.Contains("Description"))
                                    {
                                        eventEntry[1] = _eventArray[(i + a) + 1].ToString();
                                        localCounter++;
                                        resultCount++;

                                        break;
                                    }
                                }
                            }

                            // Count back from position of keyword to get the date
                            for (int a = -13; a < counter; a++)
                            {
                                string text = _eventArray[i + a];
                                if (text.Contains("Date"))
                                {
                                    _eventId[i] = i.ToString();

                                    // Id
                                    eventEntry[2] = i.ToString();

                                    eventEntry[0] = _eventArray[i + a].ToString();

                                    worker.ReportProgress(logProgress + progress, "Log: \t Line nr: " + (i + a) + ": " + eventEntry[0] + ": " + eventEntry[1]);
                                    logProgress++;

                                    break;
                                }
                            }

                            worker.ReportProgress(logProgress + progress, "Event: " + eventEntry[0] + " + " + eventEntry[1] + " + " + eventEntry[2]);
                            logProgress++;
                            //AddListItem(eventEntry);
                    }
                    }

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
                //lblTime.Text = ;
            /*}
            catch (Exception exc)
            {
                worker.ReportProgress(0, "Error: " + exc.Message);
                MessageBox.Show("A problem has occured.\nPlease notify the developer of this issue!", "App crashed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }*/
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if(e.UserState.ToString().Contains("Log: "))
            {
                BugReportLog(e.UserState.ToString().Replace("Log: ", ""));
            }
            if (e.UserState.ToString().Contains("Event: "))
            {
                AddListItem(array.ConvertStringToArray(e.UserState.ToString().Replace("Event: ", ""), " + "));
            }
            if (e.UserState.ToString().Contains("Time: "))
            {
                lblTime.Text = e.UserState.ToString().Replace("Time: ", "");
            }
            if (e.UserState.ToString().Contains("Counter: "))
            {
                lblResultCount.Text = e.UserState.ToString().Replace("Counter: ", "");
            }


            //var addViewItem = new ListViewItem(item);
            //lbEventResult.Items.Add(addViewItem);
        }

        private void btnSaveBugReport(object sender, EventArgs e)
        {
            bug.CreateBugReport(_filePath, rtbBugReport.Text, tbKeywords.Text);

            string directory = Directory.GetCurrentDirectory() + "\\bug";

            MessageBox.Show("Logs have been saved in " + directory, "Logs saved", MessageBoxButtons.OK);
        }

        //private void SearchEvents(object sender, EventArgs e)
        //{
        //    BackgroundWorker 

            
        //}

        public void BugReportLog(string log = "")
        {
            rtbBugReport.AppendText(log + "\n");
        }

        public void AddListItem(string[] item)
        {
            var addViewItem = new ListViewItem(item);
            lbEventResult.Items.Add(addViewItem);
        }

        public void MessageWrite(string text, string title, MessageBoxButtons button, MessageBoxIcon icon)
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
        
        private void AddKeywordRtbResult(string keyword)
        {
            rtbResults.AppendText("[b]" + keyword + "[/b]\n");
        }

        private void AddEventRTBResults(string[,] data)
        {
            rtbResults.AppendText("[code]");
            foreach(string d in data)
            {
                rtbResults.AppendText(d + "\n");
            }
            rtbResults.AppendText("[/code]\n\n");
        }

        private void AddCounterRtbResults(int counter)
        {
            rtbResults.AppendText("Events: " + counter.ToString() + "\n");
        }

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
            catch
            {
                BugReportLog("An error occured when trying to save keywords to use");
            }
        }

        private void miSelectEventlog_Click(object sender, EventArgs e)
        {
            BugReportLog("Loading event logs");
            openFileDialog1.ShowDialog();

            _filePath = openFileDialog1.FileName;

            BugReportLog("Event log location: " + _filePath);

            lblSelectedFile.Text = "Selected file: " + _filePath;
        }

        private void miLoadKeywords_Click(object sender, EventArgs e)
        {
            BugReportLog("Loading keywords to use");
            openFileDialog1.ShowDialog();

            string keyLoc = openFileDialog1.FileName;

            BugReportLog("Keywords to use location: " + keyLoc);

            tbKeywords.Text = events.GetKeywords(keyLoc);
        }

        private void miAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Name: \t EventFilter\nDeveloper: Martijn (axe0)\nVersion: \t BETA\nDate: \t22-06-2017\\"+ System.DateTime.Now, "About app", MessageBoxButtons.OK);
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


        private void lbEventResult_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //string eventData = events.SearchEvent(_eventArray, lbEventResult.SelectedItems[0].SubItems[0].Text, lbEventResult.SelectedItems[0].SubItems[1].Text, rtbBugReport.Text);
            string eventData = events.SearchEvent(_eventArray, lbEventResult.SelectedItems[0].SubItems[2].Text);

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


        private void btnAnalyzeResults_Click(object sender, EventArgs e)
        {
            //events.AnalyzeResults(array.ConvertStringToArray(rtbResults.Text), _keywords);
        }
    }
}