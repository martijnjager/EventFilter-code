using System;
using System.Drawing;
using System.Windows.Forms;
using EventFilter.Events;

namespace EventFilter
{
    public partial class Message : Form
    {
        public int Id;
        private readonly Event _eventClass;

        /// <inheritdoc />
        public Message(string text)
        {
            InitializeComponent();
            Height = 300;
            ChangeSize(text);
            lblEvent.Text = text;

            _eventClass = Event.Instance;
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            string text = _eventClass.Previous(Id);
            if(text == "")
            {
                Close();
            }

            Id = _eventClass._eventIdentifier;
            lblEvent.Text = text;
            ChangeSize(text);

            if (Id == 1)
                btnPrevious.Visible = false;

            if (Id == _eventClass.Events.Count)
                btnNext.Visible = false;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            string text = _eventClass.Next(Id);

            if (text == "")
            {
                Close();
            }

            Id = _eventClass._eventIdentifier;
            lblEvent.Text = text;
            ChangeSize(text);

            if (Id == _eventClass.Events.Count)
                btnNext.Visible = false;

            if (Id == 1)
                btnPrevious.Visible = false;
        }
        
        private void ChangeSize(string text)
        {
            int height = TextRenderer.MeasureText(text, SystemFonts.CaptionFont).Height + 75;
            int width = TextRenderer.MeasureText(text, SystemFonts.CaptionFont).Width + 5;

            if(height >= 240)
                Height = height;

            if (width >= 300)
                Width = width;

            btnNext.Location = new Point(173, TextRenderer.MeasureText(text, SystemFonts.CaptionFont).Height);
            btnPrevious.Location = new Point(13, TextRenderer.MeasureText(text, SystemFonts.CaptionFont).Height);
        }

        private void Message_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Escape)
                Application.Exit();
        }
    }
}
