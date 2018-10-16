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

        //public string[] Dates { get; set; }
        //public string[] Description { get; set; }
        //public string[] Id { get; set; }
        public List<string> Events { get; set; }
        private string[] EventArray { get; set; }
        public EventLogs[] Eventlogs { get; private set; }

        /// <summary>
        /// Skip current event and get Next event
        /// </summary>
        /// <param name="curId">ID of first line of description of current event</param>
        /// <returns></returns>
        public string Next(int curId)
        {
            if (!(curId < Eventlogs.Length)) return Eventlogs[curId].Log;
            
            EventIdentifier = curId + 1;
            return Eventlogs[curId + 1].Log;
        }

        /// <summary>
        /// Skip current event and get Previous event
        /// </summary>
        /// <param name="curId">ID of first line of description of current event</param>
        /// <returns>string</returns>
        public string Previous(int curId)
        {
            if (!(curId > 0)) return Eventlogs[curId].Log;

            EventIdentifier = curId - 1;
            return Eventlogs[curId - 1].Log;
        }

        /// <summary>
        /// Index log so we know what it contains
        /// </summary>
        public void IndexEvents()
        {
            if (EventLocation.Extension == ".evtx")
                CreateEvents();
            else
                CreateEventList();
        }

        /// <summary>
        /// Under development
        /// </summary>
        private void CreateEvents()
        {
            using (EventLogReader reader = new EventLogReader(EventLocation.FullName, PathType.FilePath))
            {
                EventRecord record;
                int counter = 0;

                while ((record = reader.ReadEvent()) != null)
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

                    Events.Add(text);
                }
            }
        }

        private void InitProp()
        {
            Eventlogs = new EventLogs[Events.Count];
        }

        /// <summary>
        /// Create eventlog from location
        /// </summary>
        /// <returns></returns>
        private void CreateEventList()
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
                int count = 0;
                string text = "";
                while (i + count + 1 < EventArray.Length && EventArray[i + count + 1].Contains("Event[") != true)
                {
                    text += EventArray[i + count] + "\n";
                    count++;
                }

                i += count;

                Events.Add(text);
            }

            /**
             * The properties can be initialized now that Events array is ready
             */
            InitProp();

            SetIndex();
        }

        //private void Index(string text, int idCounter)
        //{
        //    var data = new HashSet<string>();

        //    var date = GetDate(text);
        //    var desc = GetDescription(text);
        //    var dat = date + ", " + desc;

        //    // Check if the date and description can be added to ensure the events are unique
        //    if (!data.Add(dat))
        //        return;

        //    Eventlogs[idCounter].Date = date;
        //    Eventlogs[idCounter].Description = desc;
        //    Eventlogs[idCounter].Id = (idCounter + 13).ToString();
        //    Eventlogs[idCounter].Log = text;
        //}
    }
}
