using EventFilter.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;

namespace EventFilter.Events
{
    public sealed class Indexer : IIndexer
    {
        private List<EventLog> MappedEvents { get; set; }

        private List<string> RawEvents { get; set; }

        public Indexer()
        {
            RawEvents = new List<string>();
            MappedEvents = new List<EventLog>();
        }

        /// <summary>
        /// Index log so we know what it contains
        /// </summary>
        public Tuple<List<EventLog>, List<string>> Map()
        {
            if (Event.FileLocation.Extension== ".evtx")
                CreateFromEventViewer();
            else
                CreateFromText();

            return new Tuple<List<EventLog>, List<string>>(MappedEvents, RawEvents);
        }

        /// <summary>
        /// Under development
        /// </summary>
        private void CreateFromEventViewer()
        {
            using (EventLogReader reader = new EventLogReader(Event.FileLocation.FullName, PathType.FilePath))
            {
                EventRecord record;
                int counter = 0;
                RawEvents = new List<string>();
                HashSet<string> array = new HashSet<string>();

                while ((record = reader.ReadEvent()) != null)
                {
                    string @event = this.CreateEventText(record, ref counter);

                    this.AddToIndex(array, @event);
                }
            }
        }

        private static string GetDescription(List<string> Event)
        {
            string description;

            if (Event.Count - 1 > 12)
            {
                int range = Event.Count - 12;
                description = Arr.ToString(Event.GetRange(12, range), "\r").Replace("Description: ", "").Trim();
            }
            else
                description = Event[12].Replace("Description: ", "").Trim();

            return description;
        }

        private static List<string> SplitText(string text)
        {
            return Arr.ToList(text, "\n");
        }

        /// <summary>
        /// Done
        /// </summary>
        /// <param name="array"></param>
        /// <param name="text"></param>
        private void AddToIndex(HashSet<string> array, string text)
        {
            List<string> Event = SplitText(text);

            if (Event.Count < 13)
                return;

            int index = Event[0].Replace("Event[", "").Replace("]", "").Replace(":", "").ToInt();
            string description = GetDescription(Event);
            string date = Event[3].Replace("Date: ", "");

            if (array.Add(date + ", " + description))
            {
                EventLog @event = new EventLog
                {
                    Id = index.ToString(),
                    Date = DateTime.Parse(date),
                    Description = description,
                    Log = text
                };
                MappedEvents.Add(@event);
            }

            RawEvents.Add(text);
        }

        private string CreateEventText(EventRecord record, ref int counter)
        {
            string task = !string.IsNullOrEmpty(record.TaskDisplayName) ? record.TaskDisplayName : "N/A";
            string user = record.UserId != null ? record.UserId.ToString() : "N/A";
            string opcode = !string.IsNullOrEmpty(record.OpcodeDisplayName) ? record.OpcodeDisplayName : "N/A";
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

        /// <summary>
        /// Create eventlog from location
        /// </summary>
        /// <returns></returns>
        private void CreateFromText()
        {
            string[] lines = File.ReadAllLines(Event.FileLocation.FullName, Encodings.CurrentEncoding);

            RawEvents = new List<string>();
            MappedEvents = new List<EventLog>();

            MakeEvents(lines.ToArray());
        }

        private void MakeEvents(string[] EventArray)
        {
            for (int i = 0; i < EventArray.Length; i++)
            {
                if (EventArray[0].Contains("Event["))
                {
                    /**
                     * The event index needs to be set immediately to ensure the eventlogs are proper
                     * By setting the event index it's also possible to get rid of eventlogs that are for some reason unusuable
                     */
                    int count = 0;
                    string text = "";
                    HashSet<string> array = new HashSet<string>();

                    while (i + count + 1 < EventArray.Length && EventArray[i + count + 1].Contains("Event[") != true)
                    {
                        text += EventArray[i + count] + "\n";
                        count++;
                    }

                    i += count;

                    AddToIndex(array, text);
                }
            }

        }

        public string[] PrepareForMultipleLogs(List<string> files)
        {
            int eventCounter = 0;

            // stores content of all files
            List<string> eventlog = new List<string>();

            files.ForEach(file =>
            {
                List<string> text = File.ReadAllLines(file, Encodings.CurrentEncoding).ToList();

                AddContentToIndex(ref text, ref eventCounter, ref eventlog);
            });

            return eventlog.ToArray();
        }

        private void AddContentToIndex(ref List<string> logs, ref int eventCounter, ref List<string> eventlog)
        {
            for (int i = 0; i < logs.Count; i++)
            {
                if (logs[i].Contains("Event["))
                {
                    logs[i] = "Event[" + eventCounter + "]:";
                    ++eventCounter;
                }

                eventlog.Add(logs[i]);
            }
        }

        public bool NoEvents() => MappedEvents.Count == 0;
    }
}
