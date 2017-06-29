using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventFilter
{
    public class ArrayHandler
    {
        public ArrayHandler()
        {

        }

        public string ConvertArrayToString(string[] array, string delimater)
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

        public string[] ConvertStringToArray(string arr)
        {
            return new string[] { arr };
        }

        public string[] ConvertStringToArray(string arr, string delimater)
        {
            arr.Replace("\t", "");
            string[] array = arr.Split(new string[] { delimater }, StringSplitOptions.None);

            return array;
        }

        public string ConcatArrayToString(string[] array)
        {
            string returnString = "";

            if (array != null)
            {
                returnString = string.Join("", array);
            }

            return returnString;
        }

        public string ConcatNewLineArrayToString(string[] array)
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

        public string[] RecreateArray(string[] array)
        {
            if(array != null)
            {
                for(int i = 0; i < array.Length; i++)
                {
                    if(array[i] == null)
                    {
                        array.Where(w => w != array[i]).ToArray();
                    }
                }
            }

            return array;
        }

        public string ConvertListToString(List<string> array, string delimater)
        {
            return string.Join(delimater, array.ToArray());
        }

        public string[] ConvertListToArray(List<string> array)
        {
            return array.ToArray();
        }
    }
}
