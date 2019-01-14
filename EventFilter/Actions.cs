using System.Collections.Generic;
using System.Windows.Forms;

namespace EventFilter
{
    public static class Actions
    {
        public static Form1 form { get; set; }

        public static void Report(string log = "") => form.rtbBugReport.AppendText(log + "\n");

        public static void AddListItem(string[] item)
        {
            Report("Adding: " + Arr.ToString(item, "\t"));
            ListViewItem addViewItem = new ListViewItem(item);
            form.lbEventResult.Items.Add(addViewItem);
        }

        public static void SetResultCount(string text)
        {
            form.lblResultCount.Text = "Events found: " + text;
        }


        /// <summary>
        /// Copies selected items to clipboard
        /// </summary>
        /// <param name="dates">Is dynamic to automatically convert to ListView.ListViewItemCollection or ListView.SelectedListViewItemCollection</param>
        public static void CopyToClipboard(dynamic dates)
        {
            List<dynamic> data = new List<dynamic>();

            for (int i = 0; i < dates.Count; i++)
            {
                data.Add(dates[i].Text.Trim().Replace("Date: ", "") + "\t\t" + dates[i].SubItems[1].Text.Trim());
            }

            Clipboard.SetText("[code]" + Arr.ToString(data, "\n") + "[/code]");
        }

        public static bool IsEmpty(string text)
        {
            return string.IsNullOrEmpty(text);
        }
    }
}
