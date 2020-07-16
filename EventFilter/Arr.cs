using System;
using System.Collections.Generic;

namespace EventFilter
{
    internal static class Arr
    {
        /// <summary>
        /// Glue an array together
        /// </summary>
        /// <param name="array"></param>
        /// <param name="delimater"></param>
        /// <returns>Array converted to string</returns>
        public static string ToString(dynamic array, string delimater = "") => CollectionToString(array, delimater);

        /// <summary>
        /// Convert string to array
        /// </summary>
        /// <param name="text" />
        /// <param name="delimater" />
        /// <returns></returns>
        public static string[] Explode(string text, string delimater) => text.Replace("\t", "").Split(new[] { delimater }, StringSplitOptions.RemoveEmptyEntries);

        /// <summary>
        /// Convert string array value to list array
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static List<string> Explode(string[] value) => new List<string>(value);

        /// <summary>
        /// Convert dynamic to list
        /// </summary>
        /// <param name="value"></param>
        /// <param name="delimater"></param>
        /// <returns></returns>
        public static List<string> ToList(dynamic value, string delimater = " ") => Explode(value is string ? Explode(value, delimater) : value);

        /// <summary>
        /// Convert collection to string by joining each element to each other
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
        public static string[] Trim(ref string[] array)
        {
            for (int i = 0; i < array.Length; i++)
                array[i] = array[i].Trim();

            return array;
        }
    }
}
