﻿using System.Linq;
using System.IO;

namespace EventFilter.Filesystem
{
    public static partial class Remover
    {

        /// <summary>
        /// DeleteKeywords everything in dir
        /// </summary>
        /// <param name="dir">path to DeleteKeywords all files in</param>
        public static void Delete(DirectoryInfo dir)
        {
            DeleteFiles(dir);

            // Need to reverse the output to support deleting directory in directory
            foreach (DirectoryInfo d in dir.GetDirectories("*", SearchOption.AllDirectories).Reverse())
            {
                DeleteFolder(d);
            }
        }

        // Checked > DeleteKeywords
        /// <summary>
        /// DeleteKeywords all files in directory
        /// </summary>
        /// <param name="dir">Directory to empty</param>
        private static void DeleteFiles(DirectoryInfo dir)
        {
            foreach (string file in Directory.GetFiles(dir.ToString(), "*", SearchOption.AllDirectories))
            {
                FileInfo item = new FileInfo(file);
                item.Delete();
            }
        }

        // Checked > DeleteKeywords
        /// <summary>
        /// If directory contains files, DeleteKeywords them
        /// </summary>
        /// <param name="dir"></param>
        private static void DeleteFolder(DirectoryInfo dir)
        {
            if (dir.GetDirectories() != null)
            {
                DeleteFolders(dir);
            }

            dir.Delete();
        }

        private static void DeleteFolders(DirectoryInfo dir)
        {
            foreach (DirectoryInfo d in dir.GetDirectories("*", SearchOption.AllDirectories))
            {
                d.Delete();
            }
        }
    }
}