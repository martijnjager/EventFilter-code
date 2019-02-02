using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace EventFilter.Filesystem
{
    public static partial class Remover
    {
        private static readonly List<string> Items = new List<string>();

        // TODO: improve deleting

        /// <summary>
        /// Scan all directories relative to the path for a file
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ScanDirectories(string path)
        {
            List<string> files = new List<string>();

            GetAllFiles(path);

            foreach (string log in Zip.Logs)
            {
                FilePresent(ref files, Items, log);
            }

            if (files.Count >= 2)
            {
                string filename = files.First();
                string[] fileContent = Events.Event.Instance.PrepareForMultipleLogs(files);

                Move(ref filename, fileContent);

                return filename;
            }

            string fileName = files.First();
            string[] content = File.ReadAllLines(fileName, Encodings.CurrentEncoding);

            Move(ref fileName, content);

            return fileName;
        }

        /// <summary>
        /// Checks if the given file is present in the items list, if so it adds the file to list
        /// </summary>
        /// <param name="list"></param>
        /// <param name="items"></param>
        /// <param name="file"></param>
        private static void FilePresent(ref List<string> list, List<string> items, string file)
        {
            string filePath = items.Find(s => s.Contains(file));

            AddToList(ref list, filePath);
        }

        /// <summary>
        /// Adds a file present in the items list to the list.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="filePath"></param>
        private static void AddToList(ref List<string> list, string filePath)
        {
            if (!string.IsNullOrEmpty(filePath))
                list.Add(filePath);
        }

        /// <summary>
        /// Scan everything relatively from a directory
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        private static void GetAllFiles(dynamic dir)
        {
            foreach (string item in Directory.GetFiles(dir, "*", SearchOption.AllDirectories))
            {
                Items.Add(item);
            }
        }

        //private static void DeleteFiles(DirectoryInfo dir, dynamic exceptions)
        //{
        //    ClearFolderExcept(dir, exceptions);
        //}

        //private static void ClearFolderExcept(DirectoryInfo dir, List<dynamic> exceptions)
        //{
        //    foreach (FileInfo file in dir.GetFiles("*", SearchOption.AllDirectories))
        //    {
        //        DeleteAllButExceptions(file, exceptions);
        //    }
        //}

        //private static void DeleteAllButExceptions(FileSystemInfo file, List<dynamic> exceptions)
        //{
        //    if(!exceptions.Any(s => file.FullName.Contains(s)))
        //    {
        //        file.Delete();
        //    }
        //}

        /// <summary>
        /// Move files
        /// </summary>
        private static void Move(ref string file, dynamic content)
        {
            /**
             * To fix an issue with moving the eventlog file to the right directory, the extract folder need to be deleted first and then the eventlog file needs to be moved
             * and the extract folder recreated at the same time
             */
            Delete(new DirectoryInfo(Zip.ExtractLocation));

            // Set the file location
            file = Zip.ExtractLocation + "\\" + file.Substring(file.LastIndexOf("\\", StringComparison.Ordinal) + 1);

            File.WriteAllLines(file, content, Encodings.CurrentEncoding);
        }
    }
}