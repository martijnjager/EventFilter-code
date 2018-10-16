using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using EventFilter.Filesystem;

namespace EventFilter
{
    public static class Zip
    {
        public static List<string> Logs { get; } = new List<string> { "eventlog.txt", "EvtxSysDump.txt", "system-events.txt" , "eventlog.evtx"};
        public static string ExtractLocation { get; } = Bootstrap.CurrentLocation + "\\extract";

        public static void ExtractZip(string zipfile, ref string eventLocation)
        {
            Extract(zipfile);

            if (Directory.GetDirectories(ExtractLocation) != null)
            {
                eventLocation = Remover.ScanDirectories(ExtractLocation);
            }
        }

        private static void Extract(string zipfile)
        {
            EmptyExtractLocation();

            ZipFile.ExtractToDirectory(zipfile, ExtractLocation);
        }

        private static void EmptyExtractLocation()
        {
            if(ExtractLocationIsAvailable())
                Remover.Delete(new DirectoryInfo(ExtractLocation));
        }

        private static bool ExtractLocationIsAvailable()
        {
            if (!Directory.Exists(ExtractLocation))
            {
                Directory.CreateDirectory(ExtractLocation);

                return false;
            }

            return true;
        }
    }
}