using System.Collections.Generic;
using System.IO;
using System.Linq;
using EventFilter.Contracts;
using System.Diagnostics.Eventing.Reader;
using System;

namespace EventFilter.Events
{
    public partial class Event
    {
        /// <summary>
        /// Index log so we know what it contains
        /// </summary>
        public void MapEvents()
        {
            //if (NewFileUsed())
            //{
                if (EventLocation.Extension == ".evtx")
                    CreateFromEventViewer();
                else
                    CreateFromText();
            //}
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
                HashSet<string> array = new HashSet<string>();

                while ((record = reader.ReadEvent()) != null)
                {
                    string @event = CreateEventText(record, ref counter);

                    AddToIndex(array, @event);
                }
            }
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
            Eventlogs = new List<EventLogs>();
        }

        /// <summary>
        /// Create eventlog from location
        /// </summary>
        /// <returns></returns>
        private void CreateFromText()
        {
            string[] lines = File.ReadAllLines(EventLocation.FullName, Encodings.CurrentEncoding);

            Events = new List<string>();
            Eventlogs = new List<EventLogs>();

            MakeEvents(lines.ToArray());
        }

        private void MakeEvents(string[] EventArray)
        {
            InitProp();

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

        public bool NoEvents()
        {
            if (Eventlogs.Count > 0)
                return false;

            return true;
        }
    }
}
