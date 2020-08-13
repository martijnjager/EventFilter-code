using EventFilter.Contracts;
using EventFilter.Events;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace EventFilter
{
    public partial class Message : Form
    {
        public int Id { get; private set; }
        private readonly IEvent _events;
        private EventLog[] source;

        public EventLog SelectedEvent { get; set; }
        private dynamic selectedEvent;

        public Message(IEvent @event)
        {
            InitializeComponent();
            _events = @event;
            Height = 339;
        }

        public void Use(EventLog log)
        {
            UpdateText(log.Log);
            SelectedEvent = log;
        }

        public void Source(EventLog[] events = null)
        {
            source = events;

            if (events is EventLog[])
            {
                btnNextFound.Visible = false;
                btnPreviousFound.Visible = false;
            }
        }

        private void BtnPrevious_Click(object sender, EventArgs e)
        {
            SelectedEvent = _events.GoToPrevious(SelectedEvent.GetId(), source);

            UpdateText(SelectedEvent.Log);

            UpdateButtons();
        }

        private void BtnNext_Click(object sender, EventArgs e)
        {
            SelectedEvent = _events.GoToNext(SelectedEvent.GetId(), source);

            UpdateText(SelectedEvent.Log);

            UpdateButtons();
        }

        private void UpdateButtons()
        {
            if (SelectedEvent.GetId() <= _events.GetFoundEvents().First().GetId())
                btnPreviousFound.Enabled = false;
            else
                btnPreviousFound.Enabled = true;

            if (SelectedEvent.GetId() >= _events.GetFoundEvents().Last().GetId())
                btnNextFound.Enabled = false;
            else
                btnNextFound.Enabled = true;

            if (SelectedEvent.GetId() >= _events.Eventlogs.Count - 1)
                btnNext.Enabled = false;
            else
                btnNext.Enabled = true;

            if (SelectedEvent.GetId() <= 0)
                btnPrevious.Enabled = false;
            else
                btnPrevious.Enabled = true;
        }

        private void UpdateText(string text)
        {
            lblEvent.Text = text;
            ChangeSize(text);
        }

        private void ChangeSize(string text)
        {
            Size size = TextRenderer.MeasureText(text, SystemFonts.CaptionFont);

            SetButtonLocations(size);

            int width = size.Width + 5;

            Height = btnNextFound.Location.Y + 75;

            if (width >= 300)
                Width = width;
        }

        private void SetButtonLocations(Size size)
        {
            btnNext.Location = new Point(173, size.Height);
            btnPrevious.Location = new Point(13, size.Height);
            btnNextFound.Location = new Point(173, btnNext.Location.Y + 50);
            btnPreviousFound.Location = new Point(13, btnNext.Location.Y + 50);
        }

        private void Message_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Application.Exit();
        }

        private void BtnPreviousFound_Click(object sender, EventArgs e)
        {
            selectedEvent = _events.GoToPrevious(SelectedEvent.GetId(), null, true);

            if (selectedEvent is KeyValuePair<int, EventLog>)
            {
                UpdateText(selectedEvent.Value.Log);

                SelectedEvent = selectedEvent.Value;

                UpdateButtons();
            }
            else
            {
                UpdateText(selectedEvent.Item2.Log);

                SelectedEvent = selectedEvent.Item2;

                UpdateButtons();
            }
        }

        private void BtnNextFound_Click(object sender, EventArgs e)
        {
            selectedEvent = _events.GoToNext(SelectedEvent.GetId(), null, true);

            if (selectedEvent is KeyValuePair<int, EventLog>)
            {
                UpdateText(selectedEvent.Value.Log);

                SelectedEvent = selectedEvent.Value;

                UpdateButtons();
            }
            else
            {
                UpdateText(selectedEvent.Item2.Log);

                SelectedEvent = selectedEvent.Item2;

                UpdateButtons();
            }
        }
    }
}
