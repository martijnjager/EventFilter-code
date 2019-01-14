using System.Windows.Forms;

namespace EventFilter
{
    internal static class Messages
    {
        public static void SelectFileForSearching() =>
            MessageWrite("Please select a file to Search through.", "No file selected", MessageBoxButtons.OK, MessageBoxIcon.Information);

        public static void Filtering() =>
            MessageWrite("We're filtering...", "Busy");

        public static void KeywordCounted(string keywordCounted, int counter) =>
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

        //public static void ReportCreated() =>
            //MessageWrite("A report has been created and is saved in " + Bug.GetPath, "Bug report created", MessageBoxButtons.OK, MessageBoxIcon.Information);

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
            MessageWrite("Please provide keywords", "No input", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

        public static void IncorrectLogSize() =>
            MessageWrite("Please select a log that is not of size 0KB", "No valid log size", MessageBoxButtons.OK, MessageBoxIcon.Information);

        private static void MessageWrite(string text, string title = "", MessageBoxButtons button = MessageBoxButtons.OK, MessageBoxIcon icon = MessageBoxIcon.None) => MessageBox.Show(text, title, button, icon);
    }
}
