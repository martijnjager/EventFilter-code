using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

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
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.miSelectEventLog = new System.Windows.Forms.ToolStripMenuItem();
            this.miLoadKeywords = new System.Windows.Forms.ToolStripMenuItem();
            this.saveKeywordsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.eventFilterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reportBugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.encodingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EncodingDefault = new System.Windows.Forms.ToolStripMenuItem();
            this.Utf7 = new System.Windows.Forms.ToolStripMenuItem();
            this.Utf8 = new System.Windows.Forms.ToolStripMenuItem();
            this.Utf32 = new System.Windows.Forms.ToolStripMenuItem();
            this.UtfUnicode = new System.Windows.Forms.ToolStripMenuItem();
            this.UtfAscii = new System.Windows.Forms.ToolStripMenuItem();
            this.UtfBigEndianUnicode = new System.Windows.Forms.ToolStripMenuItem();
            this.miManageKeywords = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpEventFilter = new System.Windows.Forms.TabPage();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            //this.columnDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            //this.columnDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            //this.columnId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.linklblPiracy = new System.Windows.Forms.LinkLabel();
            this.lblKMS = new System.Windows.Forms.Label();
            this.clbKeywords = new System.Windows.Forms.CheckedListBox();
            this.btnResultCleanup = new System.Windows.Forms.Button();
            this.lblTime = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbKeywords = new System.Windows.Forms.TextBox();
            this.lblResultCount = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.lblSelectedFile = new System.Windows.Forms.Label();
            this.tpBugReport = new System.Windows.Forms.TabPage();
            this.btnSaveReport = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.rtbBugReport = new System.Windows.Forms.RichTextBox();
            this.tpKeywords = new System.Windows.Forms.TabPage();
            this.btnSaveKeywords = new System.Windows.Forms.Button();
            this.rtbPiracyIgnorable = new System.Windows.Forms.RichTextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.rtbIgnorables = new System.Windows.Forms.RichTextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.rtbPiracyKeywords = new System.Windows.Forms.RichTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.rtbKeywordsToUse = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnCopyClipboard = new System.Windows.Forms.Button();
            this.rtbResults = new System.Windows.Forms.RichTextBox();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.eventFilterBGWorker = new System.ComponentModel.BackgroundWorker();
            this.SearchEventBGWorker = new System.ComponentModel.BackgroundWorker();
            this.menuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tpEventFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tpBugReport.SuspendLayout();
            this.tpKeywords.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem,
            this.eventFilterToolStripMenuItem,
            this.reportBugToolStripMenuItem,
            this.encodingToolStripMenuItem,
            this.miManageKeywords});
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
            this.miLoadKeywords.Text = "Load Keywords";
            this.miLoadKeywords.Click += new System.EventHandler(this.miLoadKeywords_Click);
            // 
            // saveKeywordsToolStripMenuItem
            // 
            this.saveKeywordsToolStripMenuItem.Name = "saveKeywordsToolStripMenuItem";
            this.saveKeywordsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.K)));
            this.saveKeywordsToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.saveKeywordsToolStripMenuItem.Text = "Save Keywords";
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
            this.eventFilterToolStripMenuItem.Size = new System.Drawing.Size(77, 20);
            this.eventFilterToolStripMenuItem.Text = "Event Filter";
            this.eventFilterToolStripMenuItem.Click += new System.EventHandler(this.miEventFilter_Click);
            // 
            // reportBugToolStripMenuItem
            // 
            this.reportBugToolStripMenuItem.Name = "reportBugToolStripMenuItem";
            this.reportBugToolStripMenuItem.Size = new System.Drawing.Size(78, 20);
            this.reportBugToolStripMenuItem.Text = "Report bug";
            this.reportBugToolStripMenuItem.Click += new System.EventHandler(this.miBugReport_Click);
            // 
            // encodingToolStripMenuItem
            // 
            this.encodingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.EncodingDefault,
            this.Utf7,
            this.Utf8,
            this.Utf32,
            this.UtfUnicode,
            this.UtfAscii,
            this.UtfBigEndianUnicode});
            this.encodingToolStripMenuItem.Name = "encodingToolStripMenuItem";
            this.encodingToolStripMenuItem.Size = new System.Drawing.Size(69, 20);
            this.encodingToolStripMenuItem.Text = "Encoding";
            // 
            // EncodingDefault
            // 
            this.EncodingDefault.Name = "EncodingDefault";
            this.EncodingDefault.Size = new System.Drawing.Size(171, 22);
            this.EncodingDefault.Text = "Default";
            this.EncodingDefault.Click += new System.EventHandler(this.EncodingDefault_Click);
            // 
            // Utf7
            // 
            this.Utf7.Name = "Utf7";
            this.Utf7.Size = new System.Drawing.Size(171, 22);
            this.Utf7.Text = "UTF-7";
            this.Utf7.Click += new System.EventHandler(this.Utf7_Click);
            // 
            // Utf8
            // 
            this.Utf8.Name = "Utf8";
            this.Utf8.Size = new System.Drawing.Size(171, 22);
            this.Utf8.Text = "UTF-8";
            this.Utf8.Click += new System.EventHandler(this.Utf8_Click);
            // 
            // Utf32
            // 
            this.Utf32.Name = "Utf32";
            this.Utf32.Size = new System.Drawing.Size(171, 22);
            this.Utf32.Text = "UTF-32";
            this.Utf32.Click += new System.EventHandler(this.Utf32_Click);
            // 
            // UtfUnicode
            // 
            this.UtfUnicode.Name = "UtfUnicode";
            this.UtfUnicode.Size = new System.Drawing.Size(171, 22);
            this.UtfUnicode.Text = "Unicode";
            this.UtfUnicode.Click += new System.EventHandler(this.UtfUnicode_Click);
            // 
            // UtfAscii
            // 
            this.UtfAscii.Name = "UtfAscii";
            this.UtfAscii.Size = new System.Drawing.Size(171, 22);
            this.UtfAscii.Text = "ASCII";
            this.UtfAscii.Click += new System.EventHandler(this.UtfAscii_Click);
            // 
            // UtfBigEndianUnicode
            // 
            this.UtfBigEndianUnicode.Name = "UtfBigEndianUnicode";
            this.UtfBigEndianUnicode.Size = new System.Drawing.Size(171, 22);
            this.UtfBigEndianUnicode.Text = "BigEndianUnicode";
            this.UtfBigEndianUnicode.Click += new System.EventHandler(this.UtfBigEndianUnicode_Click);
            // 
            // miManageKeywords
            // 
            this.miManageKeywords.Name = "miManageKeywords";
            this.miManageKeywords.Size = new System.Drawing.Size(115, 20);
            this.miManageKeywords.Text = "Manage keywords";
            this.miManageKeywords.Click += new System.EventHandler(this.MiManageKeywords_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tpEventFilter);
            this.tabControl1.Controls.Add(this.tpBugReport);
            this.tabControl1.Controls.Add(this.tpKeywords);
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
            this.tpEventFilter.Controls.Add(this.dataGridView1);
            this.tpEventFilter.Controls.Add(this.linklblPiracy);
            this.tpEventFilter.Controls.Add(this.lblKMS);
            this.tpEventFilter.Controls.Add(this.clbKeywords);
            this.tpEventFilter.Controls.Add(this.btnResultCleanup);
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
            // dataGridView1
            // 
            this.dataGridView1.BackgroundColor = this.BackColor;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(9, 108);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(814, 435);
            this.dataGridView1.TabIndex = 16;
            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            // 
            // linklblPiracy
            // 
            this.linklblPiracy.AutoSize = true;
            this.linklblPiracy.Location = new System.Drawing.Point(277, 50);
            this.linklblPiracy.Name = "linklblPiracy";
            this.linklblPiracy.Size = new System.Drawing.Size(175, 13);
            this.linklblPiracy.TabIndex = 15;
            this.linklblPiracy.TabStop = true;
            this.linklblPiracy.Text = "Click here to view the piracy events";
            this.linklblPiracy.Visible = false;
            this.linklblPiracy.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linklblPiracy_LinkClicked);
            // 
            // lblKMS
            // 
            this.lblKMS.AutoSize = true;
            this.lblKMS.Location = new System.Drawing.Point(6, 50);
            this.lblKMS.Name = "lblKMS";
            this.lblKMS.Size = new System.Drawing.Size(0, 13);
            this.lblKMS.TabIndex = 13;
            // 
            // clbKeywords
            // 
            this.clbKeywords.FormattingEnabled = true;
            this.clbKeywords.Location = new System.Drawing.Point(589, 6);
            this.clbKeywords.Name = "clbKeywords";
            this.clbKeywords.Size = new System.Drawing.Size(120, 94);
            this.clbKeywords.TabIndex = 12;
            // 
            // btnResultCleanup
            // 
            this.btnResultCleanup.BackColor = System.Drawing.Color.White;
            this.btnResultCleanup.Location = new System.Drawing.Point(727, 549);
            this.btnResultCleanup.Name = "btnResultCleanup";
            this.btnResultCleanup.Size = new System.Drawing.Size(96, 23);
            this.btnResultCleanup.TabIndex = 11;
            this.btnResultCleanup.Text = "Cleanup results";
            this.btnResultCleanup.UseVisualStyleBackColor = false;
            this.btnResultCleanup.Click += new System.EventHandler(this.btnResultCleanup_Click);
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
            // tbKeywords
            // 
            this.tbKeywords.Location = new System.Drawing.Point(9, 27);
            this.tbKeywords.Name = "tbKeywords";
            this.tbKeywords.Size = new System.Drawing.Size(373, 20);
            this.tbKeywords.TabIndex = 0;
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
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.White;
            this.btnSearch.Location = new System.Drawing.Point(748, 78);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 4;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = false;
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
            // tpBugReport
            // 
            this.tpBugReport.Controls.Add(this.btnSaveReport);
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
            // btnSaveReport
            // 
            this.btnSaveReport.Location = new System.Drawing.Point(6, 532);
            this.btnSaveReport.Name = "btnSaveReport";
            this.btnSaveReport.Size = new System.Drawing.Size(88, 23);
            this.btnSaveReport.TabIndex = 8;
            this.btnSaveReport.Text = "Save logs";
            this.btnSaveReport.UseVisualStyleBackColor = true;
            this.btnSaveReport.Click += new System.EventHandler(this.BtnSaveBugReport);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(7, 53);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(327, 13);
            this.label9.TabIndex = 7;
            this.label9.Text = "- If Available, provide the details of the error Exception that occured.";
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
            // tpKeywords
            // 
            this.tpKeywords.Controls.Add(this.btnSaveKeywords);
            this.tpKeywords.Controls.Add(this.rtbPiracyIgnorable);
            this.tpKeywords.Controls.Add(this.label11);
            this.tpKeywords.Controls.Add(this.rtbIgnorables);
            this.tpKeywords.Controls.Add(this.label10);
            this.tpKeywords.Controls.Add(this.rtbPiracyKeywords);
            this.tpKeywords.Controls.Add(this.label6);
            this.tpKeywords.Controls.Add(this.rtbKeywordsToUse);
            this.tpKeywords.Controls.Add(this.label2);
            this.tpKeywords.Location = new System.Drawing.Point(4, 5);
            this.tpKeywords.Name = "tpKeywords";
            this.tpKeywords.Size = new System.Drawing.Size(829, 572);
            this.tpKeywords.TabIndex = 3;
            this.tpKeywords.UseVisualStyleBackColor = true;
            // 
            // btnSaveKeywords
            // 
            this.btnSaveKeywords.Location = new System.Drawing.Point(364, 478);
            this.btnSaveKeywords.Name = "btnSaveKeywords";
            this.btnSaveKeywords.Size = new System.Drawing.Size(75, 23);
            this.btnSaveKeywords.TabIndex = 10;
            this.btnSaveKeywords.Text = "Save";
            this.btnSaveKeywords.UseVisualStyleBackColor = true;
            this.btnSaveKeywords.Click += new System.EventHandler(this.btnSaveKeywords_Click);
            // 
            // rtbPiracyIgnorable
            // 
            this.rtbPiracyIgnorable.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.rtbPiracyIgnorable.Location = new System.Drawing.Point(428, 58);
            this.rtbPiracyIgnorable.Name = "rtbPiracyIgnorable";
            this.rtbPiracyIgnorable.Size = new System.Drawing.Size(166, 376);
            this.rtbPiracyIgnorable.TabIndex = 9;
            this.rtbPiracyIgnorable.Text = "";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(425, 29);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(87, 13);
            this.label11.TabIndex = 8;
            this.label11.Text = "Piracy ignorables";
            // 
            // rtbIgnorables
            // 
            this.rtbIgnorables.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.rtbIgnorables.Location = new System.Drawing.Point(216, 58);
            this.rtbIgnorables.Name = "rtbIgnorables";
            this.rtbIgnorables.Size = new System.Drawing.Size(166, 376);
            this.rtbIgnorables.TabIndex = 7;
            this.rtbIgnorables.Text = "";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(213, 29);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(85, 13);
            this.label10.TabIndex = 6;
            this.label10.Text = "Ignore keywords";
            // 
            // rtbPiracyKeywords
            // 
            this.rtbPiracyKeywords.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.rtbPiracyKeywords.Location = new System.Drawing.Point(639, 58);
            this.rtbPiracyKeywords.Name = "rtbPiracyKeywords";
            this.rtbPiracyKeywords.Size = new System.Drawing.Size(166, 376);
            this.rtbPiracyKeywords.TabIndex = 3;
            this.rtbPiracyKeywords.Text = "";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(636, 29);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(84, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "Piracy keywords";
            // 
            // rtbKeywordsToUse
            // 
            this.rtbKeywordsToUse.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.rtbKeywordsToUse.Location = new System.Drawing.Point(4, 58);
            this.rtbKeywordsToUse.Name = "rtbKeywordsToUse";
            this.rtbKeywordsToUse.Size = new System.Drawing.Size(166, 376);
            this.rtbKeywordsToUse.TabIndex = 1;
            this.rtbKeywordsToUse.Text = "";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Keywords";
            // 
            // btnCopyClipboard
            // 
            this.btnCopyClipboard.Location = new System.Drawing.Point(0, 0);
            this.btnCopyClipboard.Name = "btnCopyClipboard";
            this.btnCopyClipboard.Size = new System.Drawing.Size(75, 23);
            this.btnCopyClipboard.TabIndex = 0;
            // 
            // rtbResults
            // 
            this.rtbResults.Location = new System.Drawing.Point(7, 7);
            this.rtbResults.Name = "rtbResults";
            this.rtbResults.Size = new System.Drawing.Size(816, 520);
            this.rtbResults.TabIndex = 0;
            this.rtbResults.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(863, 621);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(879, 660);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EventFilter";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Form1_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Form1_DragEnter);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tpEventFilter.ResumeLayout(false);
            this.tpEventFilter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tpBugReport.ResumeLayout(false);
            this.tpBugReport.PerformLayout();
            this.tpKeywords.ResumeLayout(false);
            this.tpKeywords.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem miSelectEventLog;
        private System.Windows.Forms.ToolStripMenuItem miLoadKeywords;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.ToolStripMenuItem eventFilterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveKeywordsToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem reportBugToolStripMenuItem;
        private System.Windows.Forms.TabPage tpBugReport;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnSaveReport;
        public System.ComponentModel.BackgroundWorker eventFilterBGWorker;
        public System.Windows.Forms.RichTextBox rtbResults;
        private System.Windows.Forms.Button btnCopyClipboard;
        public System.ComponentModel.BackgroundWorker SearchEventBGWorker;
        public System.Windows.Forms.RichTextBox rtbBugReport;
        private System.Windows.Forms.TabPage tpEventFilter;
        public System.Windows.Forms.CheckedListBox clbKeywords;
        private System.Windows.Forms.Button btnResultCleanup;
        //public System.Windows.Forms.ListView lbEventResult;
        //private System.Windows.Forms.ColumnHeader columnDate;
        //private System.Windows.Forms.ColumnHeader columnEvent;
        //private System.Windows.Forms.ColumnHeader columnId;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.TextBox tbKeywords;
        private System.Windows.Forms.Button btnSearch;
        public System.Windows.Forms.Label lblSelectedFile;
        public System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem encodingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem Utf7;
        public System.Windows.Forms.ToolStripMenuItem Utf8;
        private System.Windows.Forms.ToolStripMenuItem Utf32;
        private System.Windows.Forms.ToolStripMenuItem UtfUnicode;
        private System.Windows.Forms.ToolStripMenuItem UtfAscii;
        private System.Windows.Forms.ToolStripMenuItem UtfBigEndianUnicode;
        public System.Windows.Forms.ToolStripMenuItem EncodingDefault;
        public System.Windows.Forms.Label lblResultCount;
        public System.Windows.Forms.Label lblTime;
        public System.Windows.Forms.Label lblKMS;
        public System.Windows.Forms.LinkLabel linklblPiracy;
        private System.Windows.Forms.TabPage tpKeywords;
        private System.Windows.Forms.ToolStripMenuItem miManageKeywords;
        private System.Windows.Forms.Button btnSaveKeywords;
        private System.Windows.Forms.RichTextBox rtbPiracyIgnorable;
        private System.Windows.Forms.RichTextBox rtbIgnorables;
        private System.Windows.Forms.RichTextBox rtbPiracyKeywords;
        private System.Windows.Forms.RichTextBox rtbKeywordsToUse;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label2;
        public DataGridView dataGridView1;
        //private DataGridViewTextBoxColumn columnDate;
        //private DataGridViewTextBoxColumn columnDescription;
        //private DataGridViewTextBoxColumn columnId;
    }
}

