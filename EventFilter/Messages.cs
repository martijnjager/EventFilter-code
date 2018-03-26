using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EventFilter
{
    class Messages
    {
        //public static void AnErrorOccuredLoadingFiles() =>
        //    MessageWrite("An error occured loading the necessary files, try to load them manually.", "Error loading files", MessageBoxButtons.OK, MessageBoxIcon.Error);

        //public static void ProvideKeywords() =>
        //    MessageWrite("Please provide Keywords to Search for.", "No Keywords provided", MessageBoxButtons.OK, MessageBoxIcon.Information);

        public static void SelectFileForSearching() =>
            MessageWrite("Please select a file to Search through.", "No file selected", MessageBoxButtons.OK, MessageBoxIcon.Information);

        public static void Filtering() =>
            MessageWrite("We're filtering...", "Busy");

        public static void CountKeywords(string keywordCounted, int counter) =>
            MessageWrite(keywordCounted + " appears " + counter + " times", "Keyword Counter");

        public static void ProblemOccured(string action = "")
        {
            if(!string.IsNullOrEmpty(action))
            {
                MessageWrite("A problem has occured with " + action + ".\nPlease notify the developer of this issue!", "App crashed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageWrite("A problem has occured.\nPlease notify the developer of this issue!", "App crashed", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void ReportCreated() =>
            MessageWrite("A report has been created and is saved in " + Bug.GetPath, "Bug report created", MessageBoxButtons.OK, MessageBoxIcon.Information);

        public static void ErrorLogCollection() =>
            MessageWrite(Bug.exception, "Error collecting logs", MessageBoxButtons.OK, MessageBoxIcon.Error);

        public static void LogSaved() =>
            MessageWrite("Logs have been saved in " + Bug.GetPath, "Logs saved");

        public static void NoLogSaved() =>
            MessageWrite("No log could be saved! Check if the eventlog and Keywords are loaded", "No log");

        public static void NoLogFound() =>
            MessageWrite("No eventlog has been found", "No eventlog found");

        public static void NoEventLogHasKeyword() =>
            MessageWrite("No event log has the provided Keywords.", "No result", MessageBoxButtons.OK, MessageBoxIcon.Information);

        public static void NoInput() =>
            MessageWrite("Please provide keywords and/or select an eventlog", "No input", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);



        private static void MessageWrite(string text, string title = "", MessageBoxButtons button = MessageBoxButtons.OK, MessageBoxIcon icon = MessageBoxIcon.None) => MessageBox.Show(text, title, button, icon);
    }
}
