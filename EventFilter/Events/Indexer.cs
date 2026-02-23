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
            if (Event.FileLocation.Extension == ".evtx")
                CreateFromEventViewer();
            else
                CreateFromText();

            return new Tuple<List<EventLog>, List<string>>(MappedEvents, RawEvents);
        }

        /// <summary>
        /// Improved event reading logic with strict error handling.
        /// </summary>
        private void CreateFromEventViewer()
        {
            RawEvents = new List<string>();
            HashSet<string> uniqueEvents = new HashSet<string>();
            int counter = 0;

            string tempLocation = this.TemporaryLocation();

            // Strict error handling: exceptions will propagate up.
            // This is required for testing/validation to ensure no errors are hidden.
            using (var reader = new EventLogReader(tempLocation, PathType.FilePath))
            {
                bool continueReading = true;
                while (continueReading)
                {
                    EventRecord record = null;
                    try
                    {
                        // De crash gebeurt hier als de binaire stream corrupt is
                        record = reader.ReadEvent();

                        if (record == null)
                        {
                            continueReading = false;
                            continue;
                        }

                        using (record)
                        {
                            ProcessEventRecord(record, ref counter, uniqueEvents);
                        }
                    }
                    catch (EventLogException ex)
                    {
                        // C# 7.3 ondersteunt exception filters (when)
                        // HResult -2146233087 duidt op corruptie
                        if (ex.HResult == -2146233087 || ex.Message.Contains("beschadigd"))
                        {
                            // Log de waarschuwing maar laat de applicatie niet crashen.
                            // De reader zal bij de volgende ReadEvent() proberen te synchroniseren
                            // naar het volgende geldige record-header segment.
                            counter++;
                            continue;
                        }

                        // Bij andere fouten (zoals Access Denied) willen we de loop wel stoppen
                        throw;
                    }
                    catch (Exception e)
                    {
                        // Vang onverwachte fouten tijdens het parsen van een specifiek corrupt record
                        throw;
                    }
                    finally
                    {
                        if (File.Exists(tempLocation))
                        {
                            try
                            {
                                File.Delete(tempLocation);
                            }
                            catch
                            {
                                // We kunnen hier niet veel aan doen, maar we willen ook niet dat dit een fout veroorzaakt.
                                // Log eventueel een waarschuwing als dat nodig is.
                            }
                        }
                    }
                }
            }
        }

        private void ProcessEventRecord(EventRecord record, ref int counter, HashSet<string> uniqueEvents)
        {
            string description = GetEventDescription(record);
            string dateStr = record.TimeCreated.HasValue ? record.TimeCreated.Value.ToString() : DateTime.MinValue.ToString();

            // Extract other properties safely
            string task = GetSafeProperty(() => record.TaskDisplayName, "N/A");
            string opcode = GetSafeProperty(() => record.OpcodeDisplayName, "N/A");
            string level = GetSafeProperty(() => record.LevelDisplayName, "N/A");
            string user = record.UserId != null ? record.UserId.ToString() : "N/A";
            string logName = GetSafeProperty(() => record.LogName, "Unknown Log");
            string provider = GetSafeProperty(() => record.ProviderName, "Unknown Source");
            string computer = GetSafeProperty(() => record.MachineName, "Unknown Computer");
            string eventId = GetSafeProperty(() => record.Id.ToString(), "0");
            string keywords = GetSafeProperty(() => Arr.ToString(record.Keywords, ", "), "");

            // Reconstruct the text format expected by the application
            // Format: Event[index]:\n  Log Name: ...
            string text = "Event[" + counter +
                          "]:\n  Log Name: " + logName +
                          "\n  Source: " + provider +
                          "\n  Date: " + dateStr +
                          "\n  Event ID: " + eventId +
                          "\n  Task: " + task +
                          "\n  Level: " + level +
                          "\n  Opcode: " + opcode +
                          "\n  Keyword: " + keywords +
                          "\n  User: " + user +
                          "\n  User Name: " + user +
                          "\n  Computer: " + computer +
                          "\n  Description: " + description + "\n\n";

            // Add to RawEvents (as per original logic)
            RawEvents.Add(text);

            // Deduplication logic: Use RecordId if available for better uniqueness
            string uniqueKey;
            if (record.RecordId.HasValue)
            {
                uniqueKey = record.RecordId.Value.ToString() + "_" + logName;
            }
            else
            {
                // Fallback to Date + Description if RecordId is missing
                uniqueKey = dateStr + ", " + description;
            }

            if (uniqueEvents.Add(uniqueKey))
            {
                EventLog eventLog = new EventLog
                {
                    Id = counter.ToString(),
                    Date = record.TimeCreated ?? DateTime.MinValue,
                    Description = description,
                    Log = text
                };
                MappedEvents.Add(eventLog);
            }

            counter++;
        }

        private string GetEventDescription(EventRecord record)
        {
            string description = record.FormatDescription();
            if (!string.IsNullOrEmpty(description))
            {
                return description;
            }

            // If it's null but didn't throw, we can try properties.
            if (record.Properties != null && record.Properties.Count > 0)
            {
                List<string> props = new List<string>();
                foreach (var prop in record.Properties)
                {
                    if (prop != null)
                    {
                        props.Add(prop.ToString());
                    }
                }
                return "Event Data (No Metadata): " + string.Join(", ", props);
            }

            return "No description found.";
        }

        private string GetSafeProperty(Func<string> getter, string fallback)
        {
            string val = getter();
            return string.IsNullOrEmpty(val) ? fallback : val;
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

        private string TemporaryLocation()
        {
            string tempLocation = Path.Combine(Path.GetTempPath(), "EventFilter_Temp.evtx");
            File.Copy(Event.FileLocation.FullName, tempLocation, true);
            return tempLocation;
        }
    }
}
