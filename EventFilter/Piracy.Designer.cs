using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace EventFilter
{
    partial class Piracy
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
            //this.components = new System.ComponentModel.Container();
            //this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            //this.ClientSize = new System.Drawing.Size(800, 450);
            //this.Text = "Piracy";

            this.dataGridView1 = new DataGridView();
            ((ISupportInitialize)this.dataGridView1).BeginInit();
            this.SuspendLayout();
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new Point(-7, 8);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new Size(814, 435);
            this.dataGridView1.TabIndex = 15;
            this.dataGridView1.CellDoubleClick += new DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            this.AutoScaleDimensions = new SizeF(6f, 13f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(800, 450);
            this.Controls.Add((Control)this.dataGridView1);
            this.Name = "Piracy";
            this.Text = "Piracy";
            this.SizeChanged += new EventHandler(this.Piracy_SizeChanged);
            this.KeyDown += new KeyEventHandler(this.Piracy_KeyDown);
            ((ISupportInitialize)this.dataGridView1).EndInit();
            this.ResumeLayout(false);
        }

        #endregion
    }
}