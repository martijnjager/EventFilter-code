using System;
using System.Collections.Generic;
using System.IO;
using EventFilter.Events;
using EventFilter.Keywords;

namespace EventFilter
{
    internal static class Bug
    {
        public static string exception;

        public static string GetPath { get; } = Bootstrap.CurrentLocation + "\\bugs\\";

        /// <summary>
        /// Create bug report
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="eClass" />
        /// <param name="bugreport" />
        public static void CreateBugReport(Keyword keyword, Event eClass, string bugreport)
        {
            try
            {
                List<dynamic> eventLog = eClass.EventArray;
                List<string> log = new List<string>();

                Filesystem.ClearFolder(new DirectoryInfo(GetPath));

                Directory.CreateDirectory(Bootstrap.CurrentLocation + @"\bugs");

                string bugReport = bugreport.Replace("\n", "\r\n");

                int i = -1;
                foreach (string line in eventLog)
                {
                    i++;
                    log.Add(i + " " + line.Replace("\n", "\r\n") + "\r\n");
                }

                File.WriteAllText(GetPath + "eventlog-debug.txt", Arr.Implode(log));
                File.WriteAllText(GetPath + "eventlog.txt", Arr.Implode(eventLog));

                if (bugreport != "")
                {
                    File.WriteAllText(GetPath + "problemReport.txt", bugReport);
                }

                if (keyword.GetAllKeywords() != "")
                {
                    File.WriteAllText(GetPath + @"Keywords.txt", keyword.GetIndexed());
                    File.WriteAllText(GetPath + @"allkeywords.txt", keyword.GetAllKeywords());
                    //File.WriteAllText(Path + "Keywords.txt", Keyword.keywordsAsString());
                }
            }
            catch (Exception e)
            {
                exception = e.Message;
            }
        }

//        /// <summary>
//        /// Add report to bugReport
//        /// </summary>
//        /// <param name="log">log</param>
//        public static void Report(string log = "")
//        {
//            Filesystem.form.Report(log);
//        }
//
//        public static void Exception(Exception error)
//        {
//            Filesystem.form.Exception(error);
//        }
    }
}