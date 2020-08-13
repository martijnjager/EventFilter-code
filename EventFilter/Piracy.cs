using EventFilter.Contracts;
using EventFilter.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Windows.Forms;

namespace EventFilter
{
    public partial class Piracy : Form
    {
        private readonly DataTable EventTable;
        private readonly List<EventLog> logs;
        private DataGridView dataGridView1;
        private readonly IEvent events;

        public Piracy(IEvent events, List<EventLog> logs)
        {
            InitializeComponent();
            this.events = events;
            this.logs = logs;
            EventTable = new DataTable();
            EventTable.Columns.Add("Date");
            EventTable.Columns.Add("Description");
            EventTable.Columns.Add("ID");

            AddToTable(logs);
            
            dataGridView1.DataSource = EventTable;
            dataGridView1.ClipboardCopyMode = DataGridViewClipboardCopyMode.Disable;
        }

        private void AddToTable(List<EventLog> logs)
        {
            logs.ForEach(e => EventTable.Rows.Add(e.Date, e.Description, e.GetId()));
        }

        private void Piracy_SizeChanged(object sender, EventArgs e)
        {
            dataGridView1.Size = new Size(Width, Height);
        }

        private void Piracy_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.C || e.Modifiers != Keys.Control)
                return;
            if (dataGridView1.SelectedRows.Count > 0)
                Helper.CopyToClipboard(dataGridView1.SelectedRows);
            else
                Helper.CopyToClipboard(dataGridView1.Rows);
        }

        private void DataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            DataRow row = EventTable.Rows[e.RowIndex];
            Helper.Report(row.ToString());
            string str = row.ItemArray[2].ToString();
            EventLog log = events.FindEvent(str.ToInt());

            foreach (Form openForm in (ReadOnlyCollectionBase)Application.OpenForms)
            {
                if (openForm.Text == "Message")
                {
                    Message message = (Message)openForm;
                    message.Use(log);
                    message.Source(logs.ToArray());
                    Helper.Report("\n\nCalling event id: " + log.Id);
                    Helper.Report("Output: \n" + log.ToString());
                    message.Show();
                    return;
                }
            }

            Message message1 = new Message(events);
            message1.Source(logs.ToArray());
            message1.Use(log);
            Helper.Report("\n\nCalling event id: " + log.Id);
            Helper.Report("Output: \n" + log.ToString());
            message1.Show();
        }

        private void Piracy_FormClosing(object sender, FormClosingEventArgs e)
        {
            EventTable.Dispose();
            dataGridView1.Dispose();
        }
    }
}
