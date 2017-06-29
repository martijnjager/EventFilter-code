using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace EventFilter
{
    public class Bug
    {
        ArrayHandler array = new ArrayHandler();

        public void CreateBugReport(string eventLog, string bugreport, string keywords)
        {
            var eventLogs = File.ReadLines(eventLog);
            string[] events = new string[eventLogs.Count()];
            string bugReport = bugreport.Replace("\n", "\r\n");

            int i = -1;
            foreach (var line in eventLogs)
            {
                i++;
                events[i] = line;
            }

            for (i = 0; i < events.Length; i++)
            {
                events[i] = i + " " + events[i] + "\r\n";
            }

            string eventText = array.ConcatArrayToString(events);

            if (!Directory.Exists(Directory.GetCurrentDirectory() + "\\bug"))
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\bug");
            }

            File.WriteAllText(Directory.GetCurrentDirectory() + "\\bug\\bugReport.txt", bugReport);

            File.WriteAllText(Directory.GetCurrentDirectory() + "\\bug\\eventLog.txt", eventText);

            if (!File.Exists(Directory.GetCurrentDirectory() + "\\bug\\keywords.txt"))
            {
                if(!File.Exists(Directory.GetCurrentDirectory() + "\\keywords.txt"))
                {
                    File.WriteAllText(Directory.GetCurrentDirectory() + "\\keywords.txt", keywords);
                }
                File.Copy(Directory.GetCurrentDirectory() + "\\keywords.txt", Directory.GetCurrentDirectory() + "\\bug" + "\\keywords.txt");
            }
        }

        public static void Delete(string delete = "")
        {
            delete = null;
        }
        
        public static void Delete(object delObj)
        {
            delObj = null;
        }
    }
}
