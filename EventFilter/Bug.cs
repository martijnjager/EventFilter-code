using System;
using System.Collections.Generic;
using System.IO;
using EventFilter.Events;
using EventFilter.Filesystem;

namespace EventFilter
{
    internal static class Bug
    {
        public static string exception;

        public static string GetPath { get; } = Bootstrap.CurrentLocation + "\\bugs\\";

        /// <summary>
        /// Create bug report
        /// </summary>
        /// <param name="bugreport" />  
        private static void CreateBugReport(string bugreport)
        {
            try
            {
                string[] eventLog = Event.Instance.Events.ToArray();
                List<string> log = new List<string>();

                Remover.ClearFolder(new DirectoryInfo(GetPath));

                Directory.CreateDirectory(Bootstrap.CurrentLocation + @"\bugs");

                string bugReport = bugreport.Replace("\n", "\r\n");

                int i = -1;
                foreach (string line in eventLog)
                {
                    i++;
                    log.Add(i + " " + line.Replace("\n", "\r\n") + "\r\n");
                }

                File.WriteAllText(GetPath + "eventlog-debug.txt", Arr.ToString(log));
                File.WriteAllText(GetPath + "eventlog.txt", Arr.ToString(eventLog));

                if (bugreport != "")
                {
                    File.WriteAllText(GetPath + "problemReport.txt", bugReport);
                }

                if (string.IsNullOrEmpty(Event.Instance.Keywords.GetAllKeywords())) return;
                
                File.WriteAllText(GetPath + @"Keywords.txt", Event.Instance.Keywords.GetIndexedKeywords());
                File.WriteAllText(GetPath + @"allkeywords.txt", Event.Instance.Keywords.GetAllKeywords());
                //File.WriteAllText(Path + "Keywords.txt", Keyword.keywordsAsString());
            }
            catch (Exception e)
            {
                exception = e.Message;
            }
        }

        public static void CreateReport(string bugText)
        {
            if (Event.Instance.EventLocation.Exists && Event.Instance.Keywords.GetAllKeywords() == "")
            {
                Messages.NoLogSaved();

                return;
            }

            CreateBugReport(bugText);

            if (exception != null)
            {
                Messages.ErrorLogCollection();
                return;
            }

            Messages.LogSaved();
        }

//        /// <summary>
//        /// Add report to bugReport
//        /// </summary>
//        /// <param name="log">log</param>
//        public static void Actions.Report(this, (string log = "")
//        {
//            Filesystem.form.Actions.Report(this, (log);
//        }
//
//        public static void Exception(Exception error)
//        {
//            Filesystem.form.Exception(error);
//        }
    }
}