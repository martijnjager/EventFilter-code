using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace EventFilter
{
    public static class Array
    {
        public static string ConvertArrayToString(string[] array, string delimater)
        {
            string returnString = "";

            if (delimater != "")
            {
                if (array != null)
                {
                    foreach (string a in array)
                    {
                        returnString += a + delimater;
                    }
                }
            }

            return returnString;
        }

        public static string[] ConvertStringToArray(string arr)
        {
            return new string[] { arr };
        }

        public static string[] ConvertStringToArray(string arr, string delimater)
        {
            arr.Replace("\t", "");
            string[] array = arr.Split(new string[] { delimater }, StringSplitOptions.None);

            return array;
        }

        public static string ConcatArrayToString(string[] array)
        {
            string returnString = "";

            if (array != null)
            {
                returnString = string.Join("", array);
            }

            return returnString;
        }

        public static string ConcatNewLineArrayToString(string[] array)
        {
            string returnString = "";

            if (array != null)
            {
                foreach (string a in array)
                {
                    returnString += a + "\n";
                }
            }

            return returnString;
        }

        public static string ConvertListToString(List<string> array, string delimater)
        {
            return string.Join(delimater, array.ToArray());
        }

        public static string[] ConvertListToArray(List<string> array)
        {
            return array.ToArray();
        }

        public static string[] ConstructArray(List<string> list)
        {
            string[] arr = new string[list.Count];
            int z = 0;
            // Construct global eventDate
            foreach (string str in list)
            {
                arr[z] = str;
                z++;
            }

            return arr;
        }

        public static string[] ConstructEventArray(string location)
        {
            var lines = File.ReadLines(location);
            string[] array = new string[lines.Count()];
            int counter = 0;
            foreach(var line in lines)
            {
                array[counter] = line;
                ++counter;
            }

            return array;
        }
    }
}
