using EventFilter.Filesystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace EventFilter
{
    public static class Zip
    {
        /**
         * Done
         */


        public static List<string> Logs { get; } = new List<string> { "eventlog.txt", "EvtxSysDump.txt", "EvtAppDump.txt", "system-events.txt", ".evtx", "application-events.txt", "pnp-events.txt" };
        public static string ExtractLocation { get; } = Bootstrap.CurrentLocation + "\\extract";

        public static void ExtractZip(string zipfile, ref string eventLocation)
        {
            try
            {
                Extract(zipfile);

                if (Directory.GetDirectories(ExtractLocation) == null)
                    return;

                eventLocation = Remover.ScanDirectories(ExtractLocation);
            }
            catch (IOException exception)
            {
                Messages.NoLogFound();
                Helper.Report("An error occured during zip extraction: " + exception.Message + "\n" + exception.StackTrace);
            }
            catch (Exception exception)
            {
                Helper.Report("An error occured during zip extraction: " + exception.Message + "\n" + exception.StackTrace);
            }
        }

        private static void Extract(string zipfile)
        {
            EmptyExtractLocation();

            DirectoryInfo di = Directory.CreateDirectory(ExtractLocation);
            string destinationDirectoryFullPath = di.FullName;
            if (!destinationDirectoryFullPath.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                destinationDirectoryFullPath += Path.DirectorySeparatorChar;
            }

            using (ZipArchive archive = ZipFile.OpenRead(zipfile))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    string destinationPath = Path.GetFullPath(Path.Combine(destinationDirectoryFullPath, entry.FullName));

                    if (!destinationPath.StartsWith(destinationDirectoryFullPath, StringComparison.OrdinalIgnoreCase))
                    {
                        throw new IOException("Zip Slip detected! Entry tried to extract outside the destination directory: " + entry.FullName);
                    }

                    if (string.IsNullOrEmpty(entry.Name))
                    {
                        Directory.CreateDirectory(destinationPath);
                    }
                    else
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(destinationPath));
                        entry.ExtractToFile(destinationPath, true);
                    }
                }
            }
        }

        private static void EmptyExtractLocation()
        {
            if (ExtractLocationIsAvailable())
                Remover.Delete(new DirectoryInfo(ExtractLocation));
        }

        private static bool ExtractLocationIsAvailable()
        {
            if (Directory.Exists(ExtractLocation))
                return true;

            Directory.CreateDirectory(ExtractLocation);
            return false;
        }
    }
}