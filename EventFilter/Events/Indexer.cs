using EventFilter.Contracts;
using System;
using System.Collections.Generic;
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
        /// Improved event reading logic with robust error handling using custom binary parser.
        /// </summary>
        private void CreateFromEventViewer()
        {
            RawEvents = new List<string>();
            MappedEvents = new List<EventLog>();
            HashSet<string> uniqueEvents = new HashSet<string>();
            
            var processor = new EventViewerFileProcessor(Event.FileLocation.FullName);
            
            int counter = 0;
            
            bool success = processor.ProcessEvents((record) =>
            {
                ProcessEvtxEventRecord(record, ref counter, uniqueEvents);
            });

            if (!success || processor.SuccessfulReads == 0)
            {
                Messages.ProblemOccured("processing the event log file. No events could be read.");
            }
        }

        private void ProcessEvtxEventRecord(EvtxEventRecord record, ref int counter, HashSet<string> uniqueEvents)
        {
            string description = record.GetDescription();
            string dateStr = record.TimeCreated.ToString();
            string level = record.Level ?? "Information";
            string user = record.UserSid ?? "N/A";
            string logName = record.Channel ?? "Application";
            string provider = record.Provider ?? "Unknown Source";
            string computer = record.Computer ?? "Unknown Computer";
            string eventId = record.EventId ?? "0";

            // Reconstruct the text format expected by the application
            string text = "Event[" + counter +
                          "]:\n  Log Name: " + logName +
                          "\n  Source: " + provider +
                          "\n  Date: " + dateStr +
                          "\n  Event ID: " + eventId +
                          "\n  Task: N/A" +
                          "\n  Level: " + level +
                          "\n  Opcode: N/A" +
                          "\n  Keyword: " +
                          "\n  User: " + user +
                          "\n  User Name: " + user +
                          "\n  Computer: " + computer +
                          "\n  Description: " + description + "\n\n";

            // Add to RawEvents (as per original logic)
            RawEvents.Add(text);

            // Deduplication logic
            string uniqueKey = record.RecordId.ToString() + "_" + logName;

            if (uniqueEvents.Add(uniqueKey))
            {
                EventLog eventLog = new EventLog
                {
                    Id = counter.ToString(),
                    Date = record.TimeCreated,
                    Description = description,
                    Log = text
                };
                MappedEvents.Add(eventLog);
            }

            counter++;
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
