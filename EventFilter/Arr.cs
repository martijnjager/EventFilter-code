using System;
using System.Collections.Generic;

namespace EventFilter
{
    static class Arr
    {
        /// <summary>
        /// Glue an array together
        /// </summary>
        /// <param name="array"></param>
        /// <param name="delimater"></param>
        /// <returns></returns>
        public static string Implode(dynamic array, string delimater = "") => CollectionToString(array, delimater);

        /// <summary>
        /// Convert string to array
        /// </summary>
        /// <param name="text" />
        /// <param name="delimater" />
        /// <returns></returns>
        public static dynamic Explode(string text, string delimater) => text.Replace("\t", "").Split(new [] { delimater }, StringSplitOptions.RemoveEmptyEntries);

        /// <summary>
        /// Convert dynamic value to array
        /// </summary>
        /// <param name="value"></param>
        /// <param name="uselessOptional"></param>
        /// <returns></returns>
        public static dynamic Explode(string[] value, string uselessOptional = "") => new List<string>(value);

        /// <summary>
        /// Convert a string to list
        /// </summary>
        /// <param name="text">text to convert</param>
        /// <param name="delimater">character to split with, optional</param>
        /// <returns></returns>
        public static List<string> StringToList(string text, string delimater = " ") => Explode(Explode(text, delimater));

        /// <summary>
        /// Convert dynamic to list
        /// </summary>
        /// <param name="value"></param>
        /// <param name="delimater"></param>
        /// <returns></returns>
        public static List<string> DynamicToList(dynamic value, string delimater = " ") => Explode(value, delimater);

        /// <summary>
        /// Convert colection to string by joining each element to each other
        /// </summary>
        /// <param name="array" />
        /// <param name="delimater" />
        /// <returns></returns>
        private static string CollectionToString(dynamic array, string delimater = "") => string.Join(delimater, array);

        /// <summary>
        /// Trim items from array
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static string[] Trim(string[] array)
        {
            string[] newArray = new string[array.Length];
            int c = 0;
            foreach (string str in array)
            {
                newArray[c] = str.Trim();
                c++;
            }
            
            return newArray;
        }

        /// <summary>
        /// Check if collection has anything
        /// </summary>
        /// <param name="dynamicList"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(List<dynamic> dynamicList)
        {
            return dynamicList == null || dynamicList.Count < 1;
        }
    }
}
