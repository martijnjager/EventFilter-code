using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace EventFilter
{
    public static class Bug
    {
        public static string exception = "";

        static string path = Background.GetLocation() + "\\bugs\\";

        public static void CreateBugReport(string eventLog, string bugreport, string keywords)
        {
            try
            {
                Directory.CreateDirectory(Background.GetLocation() + "\\bugs");

                var eventLogs = File.ReadLines(eventLog);
                string[] events = new string[eventLogs.Count()];
                string bugReport = bugreport.Replace("\n", "\r\n");

                int i = -1;
                foreach (var line in eventLogs)
                {
                    i++;
                    events[i] = line;
                }

                string[] eventOriginal = events;

                for (i = 0; i < events.Length; i++)
                {
                    events[i] = i + " " + events[i] + "\r\n";
                }

                if (bugreport != "")
                {
                    File.WriteAllText(path + "problemReport.txt", bugReport);
                    //CreateFile(bugReport, path + "problemReport.txt");
                }

                string eventText = Array.ConcatArrayToString(events);

                File.WriteAllText(path + "eventlogBackup.txt", eventText);

                //if (File.Exists(eventLog))
                //{
                //    if(!File.Exists(path + "eventlog.txt"))
                //        File.Copy(eventLog, path + "eventlog.txt");
                //}
                //else
                //{
                //    //CreateFile(events, path + "eventlog.txt");
                //    File.WriteAllText(path + "eventlog.txt", eventLog);
                //}

                if (keywords != "")
                {
                    File.WriteAllText(path + "keywordsBackup.txt", keywords);
                    //CreateFile(new string[] { keywords }, path + "keywords.txt");
                }
            }
            catch(Exception e)
            {
                exception = e.Message;
            }
        }

        private static void CreateFile(string[] data, string filename)
        {
            StreamWriter bugData = new StreamWriter(filename);

            for(int i = 0; i < data.Length; i++)
            {
                bugData.WriteLine(data[i]);
            }
        }
    }
}
