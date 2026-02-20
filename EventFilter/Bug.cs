using EventFilter.Events;
using EventFilter.Filesystem;
using EventFilter.Keywords;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;

namespace EventFilter
{
    public static class Bug
    {
        private static string Exception;

        public static string GetPath { get; } = Bootstrap.CurrentLocation + "\\bugs\\";

        public static string GetExceptionMessage() => Exception;

        public static void CreateReport(string bugText)
        {
            if (Event.FileLocation.Exists && Keyword.GetInstance().AllKeywordsToString().IsEmpty())
            {
                Messages.NoLogSaved();

                return;
            }

            CreateBugReport(bugText);

            if (Exception != null)
            {
                Messages.ErrorLogCollection();
                return;
            }

            Messages.LogSaved();
        }

        /// <summary>
        /// Create bug report
        /// </summary>
        /// <param name="bugreport" />  
        private static void CreateBugReport(string bugreport)
        {
            try
            {
                ClearDebugFolder();

                int createdFiles = 0;

                if (Event.GetInstance().Eventlogs is List<string> && Event.GetInstance().Eventlogs.Count > 0)
                {
                    List<string> log = new List<string>();

                    for (int i = 0; i < Event.GetInstance().Eventlogs.Count; i++)
                    {
                        log.Add(i + " " + Event.GetInstance().Eventlogs[i].Log.Replace("\n", "\r\n") + "\r\n");
                    }

                    File.WriteAllText(GetPath + "eventlog-debug.txt", Arr.ToString(log));
                    File.WriteAllText(GetPath + "eventlog.txt", Arr.ToString(Event.GetInstance().Eventlogs));
                    createdFiles++;
                }

                string bugReport = bugreport.Replace("\n", "\r\n");

                File.WriteAllText(GetPath + "problemReport.txt", bugReport);
                createdFiles++;

                if (!Keyword.GetInstance().AllKeywordsToString().IsEmpty())
                {
                    File.WriteAllText(GetPath + @"Keywords.txt", Keyword.GetInstance().AllKeywordsToString());
                    createdFiles++;
                }

                if (createdFiles == 0)
                    Messages.ErrorLogCollection();
            }
            catch (Exception e)
            {
                Exception = e.Message;
            }
        }

        /**
         * Check existence of debug folder: create if non-existence and clear if it has anything
         */
        private static void ClearDebugFolder()
        {
            if (Directory.Exists(GetPath))
                Remover.ClearFolder(new DirectoryInfo(GetPath));
            else
                Directory.CreateDirectory(GetPath);
        }
    }
}