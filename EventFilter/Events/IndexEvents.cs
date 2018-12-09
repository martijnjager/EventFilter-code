using System.Collections.Generic;
using System.IO;
using System.Linq;
using EventFilter.Contracts;
using System.Diagnostics.Eventing.Reader;

namespace EventFilter.Events
{
    public partial class Event
    {
        public IKeywords Keywords { get; set; }

        public int EventIdentifier { get; set; }

        public FileInfo EventLocation { get; private set; }

        public List<string> Events { get; set; }
        private string[] EventArray { get; set; }
        public EventLogs[] Eventlogs { get; private set; }

        /// <summary>
        /// Index log so we know what it contains
        /// </summary>
        public void MapEvents()
        {
            if (EventLocation.Extension == ".evtx")
                CreateFromEventViewer();
            else
                CreateFromText();
        }

        /// <summary>
        /// Under development
        /// </summary>
        private void CreateFromEventViewer()
        {
            using (EventLogReader reader = new EventLogReader(EventLocation.FullName, PathType.FilePath))
            {
                EventRecord record;
                int counter = 0;
                Events = new List<string>();

                while ((record = reader.ReadEvent()) != null)
                {
                    string @event = CreateEventText(record, ref counter);

                    AddEvent(@event);
                }
            }

            InitiateIndex();
        }

        private static string CreateEventText(EventRecord record, ref int counter)
        {
            string task = record.TaskDisplayName != string.Empty ? record.TaskDisplayName : "N/A";
            string user = record.UserId != null ? record.UserId.ToString() : "N/A";
            string opcode = record.OpcodeDisplayName != string.Empty ? record.OpcodeDisplayName : "N/A";
            string desc = record.FormatDescription();

            string text = "Event[" + counter++ +
                "]:\n  Log Name: " + record.LogName +
                "\n  Source: " + record.ProviderName +
                "\n  Date: " + record.TimeCreated +
                "\n  Event ID: " + record.Id +
                "\n  Task: " + task +
                "\n  Level: " + record.LevelDisplayName +
                "\n  Opcode: " + opcode +
                "\n  Keyword: " + Arr.ToString(record.Keywords, ", ") +
                "\n  User: " + user +
                "\n  User Name: " + user +
                "\n  Computer: " + record.MachineName +
                "\n  Description: " + desc + "\n\n";
            return text;
        }

        private void InitProp()
        {
            Eventlogs = new EventLogs[Events.Count];
        }

        /// <summary>
        /// Create eventlog from location
        /// </summary>
        /// <returns></returns>
        private void CreateFromText()
        {
            string[] lines = File.ReadAllLines(EventLocation.FullName, Encodings.CurrentEncoding);

            EventArray = new string[lines.Length];

            EventArray = lines.ToArray();
            Events = new List<string>();
            Eventlogs = new EventLogs[0];

            MakeEvents();

            EventArray = new string[0];
        }

        private void MakeEvents()
        {
            for (int i = 0; i < EventArray.Length; i++)
            {
                if (EventArray[0].Contains("Event["))
                {
                    int count = 0;
                    string text = "";
                    while (i + count + 1 < EventArray.Length && EventArray[i + count + 1].Contains("Event[") != true)
                    {
                        text += EventArray[i + count] + "\n";
                        count++;
                    }

                    i += count;

                    AddEvent(text);
                }
            }

            InitiateIndex();
        }

        private void AddEvent(string text)
        {
            Events.Add(text);
        }

        private void InitiateIndex()
        {
            /**
             * The properties can be initialized now that Events array is ready
             */
            InitProp();

            SetIndex();
        }

        public string[] PrepareForMultipleLogs(List<string> files)
        {
            int eventCounter = 0;

            // stores content of all files
            List<string> eventlog = new List<string>();

            files.ForEach(file => 
            {
                string[] text = File.ReadAllLines(file, Encodings.CurrentEncoding);

                List<string[]> content = new List<string[]>() { text };

                CorrectEventLogCounter(ref content, ref eventCounter, ref eventlog);
            });

            return eventlog.ToArray();
        }

        private void CorrectEventLogCounter(ref List<string[]> logs, ref int eventCounter, ref List<string> eventlog)
        {
            for (int i = 0; i < logs[0].Count(); i++)
            {
                if (logs[0][i].Contains("Event["))
                {
                    logs[0][i] = "Event[" + eventCounter + "]:";
                    ++eventCounter;
                }

               eventlog.Add(logs[0][i]);
            }
        }
    }
}
