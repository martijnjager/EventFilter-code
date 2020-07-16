using EventFilter.Events;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace EventFilter
{
    public static class Helper
    {
        public static Form1 Form { get; set; }

        public static void Report(string log = "") => Form.rtbBugReport.AppendText(log + "\n");

        public static void AddListItem(DataTable table, string[] item)
        {
            string data = item.ToString("\n");
            Report("Adding: " + data);
            table.Rows.Add(item);
        }

        public static bool IsEmpty(this string text) => string.IsNullOrEmpty(text);

        public static string ToString(this string[] array, string delimater = "") => CollectionToString(array, delimater);

        public static string ToString(this List<string> array, string delimater = "") => CollectionToString(array, delimater);

        public static string ToString(this long? array, string delimater = "") => CollectionToString(array, delimater);

        public static string[] Explode(this string text, string delimater)
        {
            return text.Replace("\t", "").Split(new string[] { delimater }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static string ToString(Dictionary<string, List<string>> pairs, string delimater = "")
        {
            string empty = String.Empty;
            foreach (List<string> array in pairs.Values)
                empty += array.ToString(delimater);

            return empty;
        }

        private static List<string> Explode(string[] value)
        {
            return new List<string>(value);
        }

        private static string CollectionToString(dynamic array, string delimater = "")
        {
            string x = string.Empty;
            foreach(string y in array)
            {
                x += y;
                x += delimater;
            }

            return x;
        }

        public static string[] Trim(this string[] array)
        {
            for (int index = 0; index < array.Length; ++index)
                array[index] = array[index].Trim();

            return array;
        }

        public static string Trim(this string text, string trimChars) => text.Trim(trimChars.ToCharArray());

        public static void AddRangeWithPrefix(this List<string> array, List<string> toAdd, string prefix = "")
        {
            toAdd.ForEach(item =>
            {
                if (!item.IsEmpty())
                    array.Add(prefix + item);
            });
        }

        public static string StartWith(this string text, string start) => start + text;

        public static string EndWith(this string text, string end) => text + end;

        public static int ToInt(this string data)
        {
            if (int.TryParse(data, out _))
                return int.Parse(data);

            return 0;
        }

        public static EventLog GetByIdMinusOne(this List<EventLog> logs, int id)
        {
            for (int index = 0; index < logs.Count; ++index)
            {
                if (logs[index].GetId() == id)
                {
                    return index == 0 ? logs[index] : logs[index - 1];
                }
            }

            throw new Exception("No event found with id " + id);
        }

        public static void SetResultCount(string text)
        {
            Form.lblResultCount.Text = "Events found: " + text;
        }

        /// <summary>
        /// Copies selected items to clipboard
        /// </summary>
        /// <param name="dates">Is dynamic to automatically convert to ListView.ListViewItemCollection or ListView.SelectedListViewItemCollection</param>
        public static void CopyToClipboard(dynamic dates)
        {
            string text = "[code]";

            foreach (DataGridViewRow row in dates)
            {
                text += row.Cells[0].Value + "\t\t";
                text += row.Cells[1].Value + "\n";
            }

            text += "[/code]";

            Clipboard.SetText(text);
        }

        public static DateTime ToDate(this string date) => Convert.ToDateTime(date);
    }
}
