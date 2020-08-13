using System.Windows.Forms;
using Resource = EventFilter.Properties.Resources;

namespace EventFilter
{
    internal static class Messages
    {
        public static void SelectFileForSearching() =>
            MessageWrite(Resource.SelectFile, "No file selected", MessageBoxButtons.OK, MessageBoxIcon.Information);

        public static void Filtering() =>
            MessageWrite(Resource.Filtering, "Busy");

        public static void KeywordCounted(string KeywordToCount, int counter) =>
            MessageWrite(KeywordToCount + " appears " + counter + " times", "Keyword Counter");

        public static void KeywordsSaved() => MessageWrite("Keywords have been successfully saved in " + Keywords.Keyword.FileLocation, "", MessageBoxButtons.OK, MessageBoxIcon.None);

        public static void ProblemOccured(string action = "")
        {
            if (!action.IsEmpty())
            {
                MessageWrite("A problem has occured with " + action + ".\nPlease notify the developer of this issue!", "App crashed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageWrite(Resource.ProblemNotifyDeveloper, "App crashed", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void ErrorLogCollection() =>
            MessageWrite(Bug.GetExceptionMessage(), "Error collecting logs", MessageBoxButtons.OK, MessageBoxIcon.Error);

        public static void LogSaved() =>
            MessageWrite("Logs have been saved in " + Bug.GetPath, "Logs saved");

        public static void NoLogSaved() =>
            MessageWrite(Resource.NoLogCouldBeSaved, "No log");

        public static void NoLogFound() =>
            MessageWrite(Resource.NoLogFound, "No eventlog found");

        public static void NoEventLogHasKeyword() =>
            MessageWrite(Resource.NoEventWithKeywords, "No result", MessageBoxButtons.OK, MessageBoxIcon.Information);

        public static void NoInput() =>
            MessageWrite(Resource.ProvideKeywords, "No input", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

        public static void IncorrectLogSize() =>
            MessageWrite(Resource.ZeroSizeFile, "No valid log size", MessageBoxButtons.OK, MessageBoxIcon.Information);

        public static DialogResult VerifyContinueNoInput() =>
            MessageBox.Show("If you continue all events will be displayed which may take some time. \nAre you sure you want to continue?", "No keyword input", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

        private static void MessageWrite(string text, string title = "", MessageBoxButtons button = MessageBoxButtons.OK, MessageBoxIcon icon = MessageBoxIcon.None) => MessageBox.Show(text, title, button, icon);
    }
}
