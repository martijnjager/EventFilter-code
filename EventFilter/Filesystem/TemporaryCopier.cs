using System;
using System.IO;

namespace EventFilter.Filesystem
{
    /// <summary>
    /// Handles copying files to a temporary location with a GUID-based filename.
    /// This is useful for isolating problematic event log files for safer processing.
    /// </summary>
    public static class TemporaryCopier
    {
        private const string TempSubdirectory = "EventFilter";

        /// <summary>
        /// Copies a file to a temporary location with a GUID-based filename.
        /// Creates a directory structure: %TEMP%\EventFilter\{GUID}.{extension}
        /// </summary>
        /// <param name="sourceFilePath">The full path to the file to copy</param>
        /// <returns>A FileInfo object pointing to the copied file in the temp location</returns>
        /// <exception cref="ArgumentNullException">Thrown when sourceFilePath is null or empty</exception>
        /// <exception cref="FileNotFoundException">Thrown when the source file does not exist</exception>
        public static FileInfo CopyToTemporaryLocation(string sourceFilePath)
        {
            if (string.IsNullOrWhiteSpace(sourceFilePath))
                throw new ArgumentNullException(nameof(sourceFilePath), "Source file path cannot be null or empty.");

            var sourceFile = new FileInfo(sourceFilePath);

            if (!sourceFile.Exists)
                throw new FileNotFoundException($"Source file not found: {sourceFilePath}");

            // Create the EventFilter subdirectory in %TEMP%
            string tempBasePath = CreateTemporaryDirectory();

            // Generate a new filename with GUID and preserve the original extension
            string newFileName = $"{Guid.NewGuid()}{sourceFile.Extension}";
            string destinationPath = Path.Combine(tempBasePath, newFileName);

            // Copy the file to the temporary location
            sourceFile.CopyTo(destinationPath, overwrite: true);

            return new FileInfo(destinationPath);
        }

        public static string CreateTemporaryDirectory()
        {
            string tempBasePath = BasePath();
            var tempDirectory = new DirectoryInfo(tempBasePath);
            if (!tempDirectory.Exists)
                tempDirectory.Create();

            return tempBasePath;
        }

        public static string BasePath()
        {
            return Path.Combine(Path.GetTempPath(), TempSubdirectory);
        }
    }
}
