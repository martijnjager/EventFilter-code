﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace EventFilter.Filesystem
{
    public static partial class Remover
    {
//        public static Form1 form = new Form1();

        private static readonly List<dynamic> Items = new List<dynamic>();

        // TODO: improve deleting

        /// <summary>
        /// DeleteKeywords files in folder
        /// </summary>
        public static void ClearFolder(DirectoryInfo dir, dynamic exceptions = null) => DeleteFiles(dir, exceptions);

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
                files.Add(FilePresent(Items, log));
            }

            string fileName = files.Find(s => s != null);
            string[] content = File.ReadAllLines(fileName, Encodings.CurrentEncoding);

            Move(ref fileName, content);

            //ClearFolder(new DirectoryInfo(path), new List<dynamic> { fileName });

            return fileName;
        }

        // Checked > scanDirectories
        private static string FilePresent(List<dynamic> files, string file)
        {
            return files.Find(s => s.Contains(file));
        }

        // Checked > scanDirectories
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

        // Checked > hasFile (2 params)
        /// <summary>
        /// Empty folder, keep selected files
        /// </summary>
        /// <param name="path">path to empty</param>
        /// <param name="files">list of files to keep</param>
//        private static void EmptyFolderKeepFiles(string path, List<dynamic> files)
//        {
//            if (Directory.Exists(path))
//            {
//                ClearFolders(new DirectoryInfo(path), files);
//            }
//        }

        // Checked > ClearFolder, 
        /// <summary>
        /// Clear a folder and keep exceptions
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="exceptions"></param>
        private static void DeleteFiles(DirectoryInfo dir, dynamic exceptions)
        {
            ClearFolderExcept(dir, exceptions);
        }

        // Checked > emptyFolderKeepFiles
        /// <summary>
        /// Clear folder of files
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileExceptions"></param>
        /*private static void ClearFolders(DirectoryInfo path, dynamic fileExceptions)
        {
            foreach (DirectoryInfo dir in path.GetDirectories())
            {
                DeleteFiles(dir, fileExceptions);
            }
        }*/

        // Checked
        /// <summary>
        /// DeleteKeywords file if not present in list
        /// </summary>
        /// <param name="file"></param>
        /// <param name="exceptions"></param>
        private static void DeleteAllButExceptions(FileSystemInfo file, List<dynamic> exceptions)
        {
            if(!exceptions.Any(s => file.FullName.Contains(s)))
            {
                file.Delete();
            }
        }

        // Checked
        /// <summary>
        /// Empty folder of files
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="exceptions"></param>
        private static void ClearFolderExcept(DirectoryInfo dir, List<dynamic> exceptions)
        {
            foreach(FileInfo file in dir.GetFiles("*", SearchOption.AllDirectories))
            {
                DeleteAllButExceptions(file, exceptions);
            }
        }

        /// <summary>
        /// Move files
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="destination"></param>
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