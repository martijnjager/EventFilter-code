﻿namespace EventFilter
{
    partial class Message
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
            this.btnPrevious = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.lblEvent = new System.Windows.Forms.Label();
            this.btnPreviousFound = new System.Windows.Forms.Button();
            this.btnNextFound = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnPrevious
            // 
            this.btnPrevious.Location = new System.Drawing.Point(13, 220);
            this.btnPrevious.Name = "btnPrevious";
            this.btnPrevious.Size = new System.Drawing.Size(88, 23);
            this.btnPrevious.TabIndex = 0;
            this.btnPrevious.Text = "Previous";
            this.btnPrevious.UseVisualStyleBackColor = true;
            this.btnPrevious.Click += new System.EventHandler(this.BtnPrevious_Click);
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(173, 220);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(89, 23);
            this.btnNext.TabIndex = 1;
            this.btnNext.Text = "Next";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.BtnNext_Click);
            // 
            // lblEvent
            // 
            this.lblEvent.AutoSize = true;
            this.lblEvent.Location = new System.Drawing.Point(13, 13);
            this.lblEvent.Name = "lblEvent";
            this.lblEvent.Size = new System.Drawing.Size(16, 13);
            this.lblEvent.TabIndex = 2;
            this.lblEvent.Text = "...";
            // 
            // btnPreviousFound
            // 
            this.btnPreviousFound.Location = new System.Drawing.Point(13, 265);
            this.btnPreviousFound.Name = "btnPreviousFound";
            this.btnPreviousFound.Size = new System.Drawing.Size(89, 23);
            this.btnPreviousFound.TabIndex = 3;
            this.btnPreviousFound.Text = "Previous found";
            this.btnPreviousFound.UseVisualStyleBackColor = true;
            this.btnPreviousFound.Click += new System.EventHandler(this.BtnPreviousFound_Click);
            // 
            // btnNextFound
            // 
            this.btnNextFound.Location = new System.Drawing.Point(173, 265);
            this.btnNextFound.Name = "btnNextFound";
            this.btnNextFound.Size = new System.Drawing.Size(89, 23);
            this.btnNextFound.TabIndex = 4;
            this.btnNextFound.Text = "Next found";
            this.btnNextFound.UseVisualStyleBackColor = true;
            this.btnNextFound.Click += new System.EventHandler(this.BtnNextFound_Click);
            // 
            // Message
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(284, 300);
            this.Controls.Add(this.btnNextFound);
            this.Controls.Add(this.btnPreviousFound);
            this.Controls.Add(this.lblEvent);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnPrevious);
            this.Name = "Message";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Message";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Message_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnPrevious;
        private System.Windows.Forms.Button btnNext;
        public System.Windows.Forms.Label lblEvent;
        private System.Windows.Forms.Button btnPreviousFound;
        private System.Windows.Forms.Button btnNextFound;
    }
}