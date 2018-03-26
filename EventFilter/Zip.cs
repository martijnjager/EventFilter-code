using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace EventFilter
{
    internal static class Zip
    {
        public static List<dynamic> Logs { get; } = new List<dynamic> { "eventlog.txt", "EvtxSysDump.txt", "system-events.txt" , "eventlog.evtx"};
        public static string Location { get; } = Bootstrap.CurrentLocation + "\\extract";

        public static void ExtractZip(string zipfile, ref string eventLocation)
        {
            if(!File.Exists(Location))
            {
                Directory.CreateDirectory(Location);
            }

            // Empty extraction folders
            Filesystem.Delete(new DirectoryInfo(Location));


            ZipFile.ExtractToDirectory(zipfile, Location);

            if(Directory.GetDirectories(Location) != null)
            {
                eventLocation = Filesystem.ScanDirectories(Location);
            }
        }
    }
}