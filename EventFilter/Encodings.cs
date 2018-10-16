﻿using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace EventFilter
{
    public static class Encodings
    {
        // Stores list of encoding options
        public static readonly List<ToolStripMenuItem> EncodingOptions = new List<ToolStripMenuItem>();

        // Stores currently used encoding
        public static Encoding CurrentEncoding = new UTF8Encoding();

        /// <summary>
        /// Updates the current encoding
        /// </summary>
        /// <param name="item"></param>
        public static void CheckState(ToolStripMenuItem item)
        {
            foreach (ToolStripMenuItem encoding in EncodingOptions)
            {
                if (encoding.Name == item.Name && encoding.Checked)
                    encoding.Checked = false;
                if (encoding.Name == item.Name && !encoding.Checked)
                    encoding.Checked = true;
                else
                    encoding.Checked = false;
            }

            UpdateCurrentEncoding();
        }

        private static void UpdateCurrentEncoding()
        {
            foreach (ToolStripMenuItem encoding in EncodingOptions)
            {
                if (encoding.Checked)
                {
                    CurrentEncoding = UpdateEncoding(encoding.Name);
                }
            }
        }

        private static Encoding UpdateEncoding(string encodeValue)
        {
            Encoding currentEncoding;

            switch (encodeValue.ToLower())
            {
                case "utf7":
                    currentEncoding = new UTF7Encoding();
                    break;
                case "utf8":
                    currentEncoding = new UTF8Encoding();
                    break;
                case "unicode":
                    currentEncoding = new UnicodeEncoding();
                    break;
                case "utf32":
                    currentEncoding = new UTF32Encoding();
                    break;
                case "ascii":
                    currentEncoding = new ASCIIEncoding();
                    break;
                default:
                    currentEncoding = Encoding.Default;
                    break;
            }

            return currentEncoding;
        }
    }
}