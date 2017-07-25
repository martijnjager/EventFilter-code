namespace EventFilter
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tbKeywords = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnSearch = new System.Windows.Forms.Button();
            this.lblSelectedFile = new System.Windows.Forms.Label();
            this.lblResultCount = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.miSelectEventLog = new System.Windows.Forms.ToolStripMenuItem();
            this.miLoadKeywords = new System.Windows.Forms.ToolStripMenuItem();
            this.saveKeywordsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.eventFilterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.miTemplate = new System.Windows.Forms.ToolStripMenuItem();
            this.reportBugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpEventFilter = new System.Windows.Forms.TabPage();
            this.btnResultCleanup = new System.Windows.Forms.Button();
            this.lbEventResult = new System.Windows.Forms.ListView();
            this.columnDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnEvent = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lblTime = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tpTemplate = new System.Windows.Forms.TabPage();
            this.rtbResults = new System.Windows.Forms.RichTextBox();
            this.tpBugReport = new System.Windows.Forms.TabPage();
            this.button1 = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.rtbBugReport = new System.Windows.Forms.RichTextBox();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.SearchEventBGWorker = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.operatorBGWorker = new System.ComponentModel.BackgroundWorker();
            this.menuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tpEventFilter.SuspendLayout();
            this.tpTemplate.SuspendLayout();
            this.tpBugReport.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbKeywords
            // 
            this.tbKeywords.Location = new System.Drawing.Point(9, 27);
            this.tbKeywords.Name = "tbKeywords";
            this.tbKeywords.Size = new System.Drawing.Size(373, 20);
            this.tbKeywords.TabIndex = 0;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(748, 78);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 4;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // lblSelectedFile
            // 
            this.lblSelectedFile.AutoSize = true;
            this.lblSelectedFile.Location = new System.Drawing.Point(6, 67);
            this.lblSelectedFile.Name = "lblSelectedFile";
            this.lblSelectedFile.Size = new System.Drawing.Size(71, 13);
            this.lblSelectedFile.TabIndex = 5;
            this.lblSelectedFile.Text = "Selected file: ";
            // 
            // lblResultCount
            // 
            this.lblResultCount.AutoSize = true;
            this.lblResultCount.Location = new System.Drawing.Point(6, 91);
            this.lblResultCount.Name = "lblResultCount";
            this.lblResultCount.Size = new System.Drawing.Size(73, 13);
            this.lblResultCount.TabIndex = 6;
            this.lblResultCount.Text = "Events found:";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem,
            this.eventFilterToolStripMenuItem,
            this.miTemplate,
            this.reportBugToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(863, 24);
            this.menuStrip1.TabIndex = 7;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miSelectEventLog,
            this.miLoadKeywords,
            this.saveKeywordsToolStripMenuItem,
            this.aboutToolStripMenuItem1});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // miSelectEventLog
            // 
            this.miSelectEventLog.Name = "miSelectEventLog";
            this.miSelectEventLog.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.miSelectEventLog.Size = new System.Drawing.Size(200, 22);
            this.miSelectEventLog.Text = "Select event log";
            this.miSelectEventLog.Click += new System.EventHandler(this.miSelectEventlog_Click);
            // 
            // miLoadKeywords
            // 
            this.miLoadKeywords.Name = "miLoadKeywords";
            this.miLoadKeywords.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
            this.miLoadKeywords.Size = new System.Drawing.Size(200, 22);
            this.miLoadKeywords.Text = "Load keywords";
            this.miLoadKeywords.Click += new System.EventHandler(this.miLoadKeywords_Click);
            // 
            // saveKeywordsToolStripMenuItem
            // 
            this.saveKeywordsToolStripMenuItem.Name = "saveKeywordsToolStripMenuItem";
            this.saveKeywordsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.K)));
            this.saveKeywordsToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.saveKeywordsToolStripMenuItem.Text = "Save keywords";
            this.saveKeywordsToolStripMenuItem.Click += new System.EventHandler(this.miSaveKeywords_Click);
            // 
            // aboutToolStripMenuItem1
            // 
            this.aboutToolStripMenuItem1.Name = "aboutToolStripMenuItem1";
            this.aboutToolStripMenuItem1.Size = new System.Drawing.Size(200, 22);
            this.aboutToolStripMenuItem1.Text = "About";
            this.aboutToolStripMenuItem1.Click += new System.EventHandler(this.miAbout_Click);
            // 
            // eventFilterToolStripMenuItem
            // 
            this.eventFilterToolStripMenuItem.Name = "eventFilterToolStripMenuItem";
            this.eventFilterToolStripMenuItem.Size = new System.Drawing.Size(75, 20);
            this.eventFilterToolStripMenuItem.Text = "Event filter";
            this.eventFilterToolStripMenuItem.Click += new System.EventHandler(this.miEventFilter_Click);
            // 
            // miTemplate
            // 
            this.miTemplate.Name = "miTemplate";
            this.miTemplate.Size = new System.Drawing.Size(68, 20);
            this.miTemplate.Text = "Template";
            this.miTemplate.Click += new System.EventHandler(this.miTemplate_Click);
            // 
            // reportBugToolStripMenuItem
            // 
            this.reportBugToolStripMenuItem.Name = "reportBugToolStripMenuItem";
            this.reportBugToolStripMenuItem.Size = new System.Drawing.Size(78, 20);
            this.reportBugToolStripMenuItem.Text = "Report bug";
            this.reportBugToolStripMenuItem.Click += new System.EventHandler(this.miBugReport_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tpEventFilter);
            this.tabControl1.Controls.Add(this.tpTemplate);
            this.tabControl1.Controls.Add(this.tpBugReport);
            this.tabControl1.ImeMode = System.Windows.Forms.ImeMode.On;
            this.tabControl1.ItemSize = new System.Drawing.Size(0, 1);
            this.tabControl1.Location = new System.Drawing.Point(13, 28);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(837, 581);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl1.TabIndex = 8;
            // 
            // tpEventFilter
            // 
            this.tpEventFilter.Controls.Add(this.btnResultCleanup);
            this.tpEventFilter.Controls.Add(this.lbEventResult);
            this.tpEventFilter.Controls.Add(this.lblTime);
            this.tpEventFilter.Controls.Add(this.label1);
            this.tpEventFilter.Controls.Add(this.tbKeywords);
            this.tpEventFilter.Controls.Add(this.lblResultCount);
            this.tpEventFilter.Controls.Add(this.btnSearch);
            this.tpEventFilter.Controls.Add(this.lblSelectedFile);
            this.tpEventFilter.Location = new System.Drawing.Point(4, 5);
            this.tpEventFilter.Name = "tpEventFilter";
            this.tpEventFilter.Padding = new System.Windows.Forms.Padding(3);
            this.tpEventFilter.Size = new System.Drawing.Size(829, 572);
            this.tpEventFilter.TabIndex = 0;
            this.tpEventFilter.Text = "tabPage1";
            this.tpEventFilter.UseVisualStyleBackColor = true;
            // 
            // btnResultCleanup
            // 
            this.btnResultCleanup.Location = new System.Drawing.Point(727, 549);
            this.btnResultCleanup.Name = "btnResultCleanup";
            this.btnResultCleanup.Size = new System.Drawing.Size(96, 23);
            this.btnResultCleanup.TabIndex = 11;
            this.btnResultCleanup.Text = "Cleanup results";
            this.btnResultCleanup.UseVisualStyleBackColor = true;
            this.btnResultCleanup.Click += new System.EventHandler(this.btnResultCleanup_Click);
            // 
            // lbEventResult
            // 
            this.lbEventResult.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnDate,
            this.columnEvent,
            this.columnId});
            this.lbEventResult.Location = new System.Drawing.Point(9, 108);
            this.lbEventResult.Name = "lbEventResult";
            this.lbEventResult.Size = new System.Drawing.Size(814, 441);
            this.lbEventResult.TabIndex = 10;
            this.lbEventResult.UseCompatibleStateImageBehavior = false;
            this.lbEventResult.View = System.Windows.Forms.View.Details;
            this.lbEventResult.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lbEventResult_ColumnClick);
            this.lbEventResult.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lbEventResult_MouseDoubleClick);
            // 
            // columnDate
            // 
            this.columnDate.Text = "Date";
            this.columnDate.Width = 200;
            // 
            // columnEvent
            // 
            this.columnEvent.Text = "Event description";
            this.columnEvent.Width = 1000;
            // 
            // columnId
            // 
            this.columnId.Text = "Id";
            this.columnId.Width = 0;
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Location = new System.Drawing.Point(322, 91);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(87, 13);
            this.lblTime.TabIndex = 9;
            this.lblTime.Text = "Found results in: ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(161, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Keywords to use (case sensitive)";
            // 
            // tpTemplate
            // 
            this.tpTemplate.Controls.Add(this.rtbResults);
            this.tpTemplate.Location = new System.Drawing.Point(4, 5);
            this.tpTemplate.Name = "tpTemplate";
            this.tpTemplate.Padding = new System.Windows.Forms.Padding(3);
            this.tpTemplate.Size = new System.Drawing.Size(829, 572);
            this.tpTemplate.TabIndex = 1;
            this.tpTemplate.Text = "tabPage2";
            this.tpTemplate.UseVisualStyleBackColor = true;
            // 
            // rtbResults
            // 
            this.rtbResults.Location = new System.Drawing.Point(7, 7);
            this.rtbResults.Name = "rtbResults";
            this.rtbResults.Size = new System.Drawing.Size(816, 520);
            this.rtbResults.TabIndex = 0;
            this.rtbResults.Text = "";
            // 
            // tpBugReport
            // 
            this.tpBugReport.Controls.Add(this.button1);
            this.tpBugReport.Controls.Add(this.label9);
            this.tpBugReport.Controls.Add(this.label8);
            this.tpBugReport.Controls.Add(this.label7);
            this.tpBugReport.Controls.Add(this.label5);
            this.tpBugReport.Controls.Add(this.label4);
            this.tpBugReport.Controls.Add(this.label3);
            this.tpBugReport.Controls.Add(this.rtbBugReport);
            this.tpBugReport.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tpBugReport.Location = new System.Drawing.Point(4, 5);
            this.tpBugReport.Name = "tpBugReport";
            this.tpBugReport.Size = new System.Drawing.Size(829, 572);
            this.tpBugReport.TabIndex = 2;
            this.tpBugReport.Text = "tabPage1";
            this.tpBugReport.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(6, 532);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(88, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "Save logs";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.btnSaveBugReport);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(7, 53);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(325, 13);
            this.label9.TabIndex = 7;
            this.label9.Text = "- If available, provide the details of the error exception that occured.";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(7, 40);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(361, 13);
            this.label8.TabIndex = 6;
            this.label8.Text = "- Provide a description of the problem, what did you do and what happened";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(101, 13);
            this.label7.TabIndex = 5;
            this.label7.Text = "How to report a bug";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 66);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(472, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "- Mail all files as attachment to martijnjager@live.nl with title \"Bugreport {you" +
    "r username} Eventfilter\"";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 111);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Program log:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(482, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "- Click on Save logs below, this will collect the program log (see below), keywor" +
    "ds.txt and eventlog.txt";
            // 
            // rtbBugReport
            // 
            this.rtbBugReport.Location = new System.Drawing.Point(4, 127);
            this.rtbBugReport.Name = "rtbBugReport";
            this.rtbBugReport.Size = new System.Drawing.Size(822, 389);
            this.rtbBugReport.TabIndex = 0;
            this.rtbBugReport.Text = "";
            // 
            // SearchEventBGWorker
            // 
            this.SearchEventBGWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.SearchEventBGWorker_DoWork);
            this.SearchEventBGWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.SearchEventBGWorker_ProgressChanged);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            // 
            // operatorBGWorker
            // 
            this.operatorBGWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.operatorBGWorker_DoWork);
            this.operatorBGWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.operatorBGWorker_ProgressChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(863, 621);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EventFilter";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tpEventFilter.ResumeLayout(false);
            this.tpEventFilter.PerformLayout();
            this.tpTemplate.ResumeLayout(false);
            this.tpBugReport.ResumeLayout(false);
            this.tpBugReport.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbKeywords;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label lblSelectedFile;
        private System.Windows.Forms.Label lblResultCount;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem miSelectEventLog;
        private System.Windows.Forms.ToolStripMenuItem miLoadKeywords;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpEventFilter;
        private System.Windows.Forms.TabPage tpTemplate;
        private System.Windows.Forms.ToolStripMenuItem eventFilterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem miTemplate;
        private System.Windows.Forms.ToolStripMenuItem saveKeywordsToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem reportBugToolStripMenuItem;
        private System.Windows.Forms.TabPage tpBugReport;
        private System.Windows.Forms.RichTextBox rtbBugReport;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.ListView lbEventResult;
        private System.Windows.Forms.ColumnHeader columnDate;
        private System.Windows.Forms.ColumnHeader columnEvent;
        private System.ComponentModel.BackgroundWorker SearchEventBGWorker;
        private System.Windows.Forms.RichTextBox rtbResults;
        private System.Windows.Forms.ColumnHeader columnId;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Button btnResultCleanup;
        private System.ComponentModel.BackgroundWorker operatorBGWorker;
    }
}

