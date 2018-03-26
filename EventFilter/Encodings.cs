using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EventFilter.Events;

namespace EventFilter
{
    class Encodings
    {
        // Stores list of encoding options
        public static List<ToolStripMenuItem> EncodingOptions = new List<ToolStripMenuItem>();

        // Stores currently used encoding
        public static Encoding CurrentEncoding;

        private static readonly Event _eventClass = Event.Instance;

        /// <summary>
        /// Updates the current encoding
        /// </summary>
        /// <param name="item"></param>
        public static void CheckState(ToolStripMenuItem item)
        {
            foreach (var encoding in EncodingOptions)
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

        public static void UpdateCurrentEncoding()
        {
            foreach (var encoding in EncodingOptions)
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

        //private static byte[] Converter(dynamic encoding, dynamic text) => encoding.GetBytes(text);

        //private static dynamic UpdatePropertyEncoding(ref List<dynamic> propertyList)
        //{
        //    int counter = 0;
        //    foreach (string date in propertyList)
        //    {
        //        propertyList[counter++] = Converter(CurrentEncoding, date);
        //    }

        //    return propertyList;
        //}
    }
}
