using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace EventFilter
{
    public static class Filesystem
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
            var files = new List<dynamic>();

            GetAllFiles(path);

            foreach (var log in Zip.Logs)
            {
                files.Add(FilePresent(Items, log));
            }

            var fileName = files.Find(s => s != null);

            Move(fileName, Zip.Location);

            ClearFolder(new DirectoryInfo(path), new List<dynamic> { fileName });

            return fileName;
        }

        /// <summary>
        /// DeleteKeywords everything in dir
        /// </summary>
        /// <param name="dir">path to DeleteKeywords all files in</param>
        public static void Delete(DirectoryInfo dir)
        {
            DeleteFiles(dir);

            foreach (var d in dir.GetDirectories("*", SearchOption.AllDirectories))
            {
                DeleteFolder(d);
            }
        }

        public static string Scan(dynamic files, string fileName, List<string> logs) => HasFile(files, fileName, logs);

        //private static void Scan(DirectoryInfo path)
        //{
        //    foreach(var item in path.GetDirectories("*", SearchOption.AllDirectories))
        //    {

        //    }
        //}

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
        private static void EmptyFolderKeepFiles(string path, List<dynamic> files)
        {
            if (Directory.Exists(path))
            {
                ClearFolders(new DirectoryInfo(path), files);
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

        // Checked > DeleteKeywords
        /// <summary>
        /// DeleteKeywords all files in directory
        /// </summary>
        /// <param name="dir">Directory to empty</param>
        private static void DeleteFiles(DirectoryInfo dir)
        {
            foreach (var file in Directory.GetFiles(dir.ToString(), "*", SearchOption.AllDirectories))
            {
                var item = new FileInfo(file);
                item.Delete();
            }
        }

        private static void DeleteFolders(DirectoryInfo dir)
        {
            foreach (var d in dir.GetDirectories("*", SearchOption.AllDirectories))
            {
                d.Delete();
            }
        }

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
        private static void ClearFolders(DirectoryInfo path, dynamic fileExceptions)
        {
            foreach (var dir in path.GetDirectories())
            {
                DeleteFiles(dir, fileExceptions);
            }
        }

        // Checked
        /// <summary>
        /// DeleteKeywords file if not present in list
        /// </summary>
        /// <param name="file"></param>
        /// <param name="exceptions"></param>
        private static void DeleteAllButExceptions(FileSystemInfo file, IEnumerable<dynamic> exceptions)
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
            foreach(var file in dir.GetFiles("*", SearchOption.AllDirectories))
            {
                DeleteAllButExceptions(file, exceptions);
            }
        }

        // Checked > scan (3 params)
        /// <summary>
        /// Check if file list has fileName
        /// </summary>
        /// <param name="files"></param>
        /// <param name="fileName"></param>
        /// <param name="logs"></param>
        /// <returns>string file</returns>
        private static string HasFile(dynamic files, string fileName, List<string> logs)
        {
            foreach (var file in files)
            {
                fileName = HasFile(file, logs);
            }

            return fileName;
        }

        // Checked > hasFile (3 params)
        /// <summary>
        /// Find file in log
        /// </summary>
        /// <param name="file"></param>
        /// <param name="logs"></param>
        /// <returns></returns>
        private static string HasFile(string file, List<string> logs)
        {
            if (!logs.Any(file.Contains)) return null;

            EmptyFolderKeepFiles(Zip.Location, Zip.Logs);

            return file;
        }

        /// <summary>
        /// Move files
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="destination"></param>
        private static void Move(string origin, string destination)
        {
            var originFile = origin.Substring(0, origin.LastIndexOf("\\", StringComparison.Ordinal));

            if(originFile != destination)
            {
                MoveFile(new FileInfo(originFile), destination);
            }
        }

        private static void MoveFile(FileInfo file, string destination)
        {
            file.MoveTo(destination);
        }
    }
}