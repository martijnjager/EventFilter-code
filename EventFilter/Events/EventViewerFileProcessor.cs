using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EventFilter.Events
{
    /// <summary>
    /// Custom .evtx file processor that reads and parses binary event log files
    /// without relying on the built-in EventLogReader API.
    /// Uses JSON configuration for structure definition.
    /// </summary>
    public class EventViewerFileProcessor
    {
        private readonly string _filePath;
        private int _successfulReads;
        private int _failedReads;
        private readonly EvtxStructureConfig _structureConfig;
        
        private const int EvtxHeaderSize = 4096;
        private const int ChunkHeaderSize = 512;
        private const int ChunkSize = 65536;
        private const string EvtxMagic = "ElfFile\0";
        private const string ChunkMagic = "\0ElfChnk";  // Note: null byte is BEFORE the string
        private const uint EventSignature = 0x002A2A00;  // "**" signature in little-endian format

        public int SuccessfulReads => _successfulReads;
        public int FailedReads => _failedReads;
        public bool HasProcessedEvents => _successfulReads > 0;

        public EventViewerFileProcessor(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentNullException(nameof(filePath));

            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Event log file not found: {filePath}");

            _filePath = filePath;
            _structureConfig = EvtxStructureConfigLoader.Load();
        }

        /// <summary>
        /// Processes event records using a callback function for each successfully parsed event.
        /// </summary>
        public bool ProcessEvents(Action<EvtxEventRecord> processCallback)
        {
            if (processCallback == null)
                throw new ArgumentNullException(nameof(processCallback));

            _successfulReads = 0;
            _failedReads = 0;

            try
            {
                using (var fs = new FileStream(_filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var reader = new BinaryReader(fs))
                {
                    if (!ValidateHeader(reader, EvtxMagic))
                    {
                        Messages.ProblemOccured("Invalid .evtx file header.");
                        return false;
                    }

                    fs.Position = EvtxHeaderSize;
                    
                    while (fs.Position < fs.Length)
                    {
                        long chunkStart = fs.Position;
                        
                        // Check if we have enough space for a chunk header
                        if (chunkStart + ChunkHeaderSize > fs.Length)
                            break;
                        
                        if (!ValidateHeader(reader, ChunkMagic))
                        {
                            // Try default chunk size if validation fails
                            if (chunkStart + ChunkSize <= fs.Length)
                            {
                                fs.Position = chunkStart + ChunkSize;
                                continue;
                            }
                            break;
                        }

                        // Read actual chunk size from header (or use default)
                        long currentPos = fs.Position;
                        int actualChunkSize = ReadChunkSize(reader, chunkStart);
                        fs.Position = currentPos; // Restore position after reading chunk size
                        
                        long chunkEnd = chunkStart + actualChunkSize;
                        if (chunkEnd > fs.Length)
                            chunkEnd = fs.Length;

                        fs.Position = chunkStart + ChunkHeaderSize;
                        ProcessChunkEvents(reader, processCallback, chunkEnd);
                        
                        // Move to next chunk
                        fs.Position = chunkStart + actualChunkSize;
                    }
                }

                if (_successfulReads == 0)
                {
                    Messages.ProblemOccured("No valid events could be read from the file.");
                    return false;
                }

                if (_failedReads > 0)
                {
                    Messages.ProblemOccured($"Read {_successfulReads} events, but {_failedReads} were corrupted.");
                }

                return true;
            }
            catch (UnauthorizedAccessException)
            {
                Messages.ProblemOccured("Permission denied accessing the event log file.");
                return false;
            }
            catch (Exception ex)
            {
                Messages.ProblemOccured($"Error processing event log: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Reads the chunk size from the chunk header. Falls back to default ChunkSize if unable to read.
        /// The chunk size is not directly stored but can be calculated from last record offset + header size.
        /// </summary>
        private int ReadChunkSize(BinaryReader reader, long chunkStart)
        {
            try
            {
                // Position after magic (8 bytes)
                reader.BaseStream.Position = chunkStart + 8;
                
                // Skip first/last record numbers (16 bytes)
                reader.ReadBytes(16);
                
                // Skip first record offset (8 bytes)
                reader.ReadBytes(8);
                
                // Read last record offset (8 bytes) - this tells us where the last event ends
                long lastRecordOffset = reader.ReadInt64();
                
                // Read header size (4 bytes)
                int headerSize = reader.ReadInt32();
                
                // If last record offset seems reasonable, use it to calculate chunk size
                // Otherwise fall back to standard chunk size
                if (lastRecordOffset > 0 && lastRecordOffset < 1048576) // Sanity check
                {
                    // The chunk extends from start to the last record offset
                    // But we should align to the standard chunk size if possible
                    return ChunkSize; // For now, use standard size
                }
                
                return ChunkSize;
            }
            catch
            {
                return ChunkSize;
            }
        }

        private bool ValidateHeader(BinaryReader reader, string expectedMagic)
        {
            try
            {
                byte[] magic = reader.ReadBytes(8);
                return Encoding.ASCII.GetString(magic) == expectedMagic;
            }
            catch
            {
                return false;
            }
        }

        private void ProcessChunkEvents(BinaryReader reader, Action<EvtxEventRecord> callback, long chunkEnd)
        {
            long chunkSize = chunkEnd - (reader.BaseStream.Position - ChunkHeaderSize);
            
            while (reader.BaseStream.Position + 24 < chunkEnd)
            {
                long eventStart = reader.BaseStream.Position;
                
                try
                {
                    uint signature = reader.ReadUInt32();
                    if (signature != EventSignature)
                    {
                        // Not a valid event signature, stop processing this chunk
                        break;
                    }
                    
                    int recordSize = reader.ReadInt32();
                    
                    // Validate record size is reasonable
                    // Record must fit within the chunk (can't be larger than chunk size)
                    if (recordSize < 24 || recordSize > chunkSize)
                    {
                        Messages.ProblemOccured($"Invalid record size {recordSize} (chunk size: {chunkSize}) at position {eventStart}");
                        break;
                    }
                    
                    // Check if record would extend beyond chunk boundary
                    if (eventStart + recordSize > chunkEnd)
                    {
                        // Record extends beyond chunk - this is corruption or wrong chunk size
                        Messages.ProblemOccured($"Event record at {eventStart} size {recordSize} extends beyond chunk end {chunkEnd}. Skipping rest of chunk.");
                        break;
                    }
                    
                    long recordId = reader.ReadInt64();
                    long timestamp = reader.ReadInt64();

                    int binXmlSize = recordSize - 24 - 4;
                    if (binXmlSize <= 0)
                    {
                        // No XML data
                        _failedReads++;
                        continue;
                    }
                    
                    byte[] binXml = reader.ReadBytes(binXmlSize);
                    reader.ReadInt32(); // Skip trailing size
                    
                    var eventRecord = ParseEventData(binXml, recordId, timestamp);
                    if (eventRecord != null)
                    {
                        callback(eventRecord);
                        _successfulReads++;
                    }
                    else
                    {
                        _failedReads++;
                    }
                }
                catch (Exception ex)
                {
                    _failedReads++;
                    Messages.ProblemOccured($"Error reading event at position {eventStart}: {ex.Message}");
                    
                    // Try to skip ahead and find next event
                    reader.BaseStream.Position = eventStart + 512;
                    if (reader.BaseStream.Position >= chunkEnd)
                        break;
                }
            }
        }

        private EvtxEventRecord ParseEventData(byte[] binXml, long recordId, long timestamp)
        {
            try
            {
                var strings = ExtractUtf16Strings(binXml);
                var fields = ExtractFieldValues(strings);
                
                return new EvtxEventRecord
                {
                    RecordId = recordId,
                    TimeCreated = DateTime.FromFileTimeUtc(timestamp),
                    EventId = GetFieldValue(fields, "EventId", "EventID") ?? "0",
                    Level = MapLevel(GetFieldValue(fields, "Level")),
                    Provider = GetFieldValue(fields, "Provider", "Name") ?? FindProviderByPrefix(strings),
                    Channel = GetFieldValue(fields, "Channel") ?? "Application",
                    Computer = GetFieldValue(fields, "Computer") ?? "Unknown",
                    UserSid = GetFieldValue(fields, "UserSid", "UserID") ?? "N/A",
                    RawXml = BuildDescription(strings)
                };
            }
            catch
            {
                return null;
            }
        }

        private List<string> ExtractUtf16Strings(byte[] data)
        {
            var strings = new List<string>();
            int i = 0;
            
            while (i + 1 < data.Length)
            {
                if (IsValidUtf16Char(data, i))
                {
                    int start = i;
                    
                    while (i + 1 < data.Length && IsValidUtf16Char(data, i))
                    {
                        i += 2;
                    }
                    
                    int length = i - start;
                    if (length >= 2)
                    {
                        string str = Encoding.Unicode.GetString(data, start, length);
                        if (!string.IsNullOrEmpty(str))
                        {
                            strings.Add(str);
                        }
                    }
                }
                else
                {
                    i++;
                }
            }
            
            return strings;
        }

        private bool IsValidUtf16Char(byte[] data, int i)
        {
            if (i + 1 >= data.Length) return false;
            
            byte low = data[i];
            byte high = data[i + 1];
            
            if (high == 0 && low >= 0x20 && low <= 0x7E)
                return true;
            
            if (high == 0 && low >= 0x80)
                return true;
            
            if (high == 0x01)
                return true;
            
            return false;
        }

        private Dictionary<string, string> ExtractFieldValues(List<string> strings)
        {
            var fields = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            
            // Find all element positions (excluding containers and data sections)
            var elementPositions = new List<int>();
            for (int i = 0; i < strings.Count; i++)
            {
                string s = strings[i].Trim();
                if (IsRealElement(s))
                {
                    elementPositions.Add(i);
                }
            }
            
            // Process each element
            for (int e = 0; e < elementPositions.Count; e++)
            {
                int elementIndex = elementPositions[e];
                string elementName = strings[elementIndex].Trim();
                
                // Find the end boundary (next element or end of list)
                int nextElementIndex = (e + 1 < elementPositions.Count) 
                    ? elementPositions[e + 1] 
                    : strings.Count;
                
                // Collect items between this element and the next
                var items = new List<string>();
                for (int i = elementIndex + 1; i < nextElementIndex; i++)
                {
                    string item = strings[i].Trim();
                    // Skip empty, single-char, ignored, containers, and data sections
                    if (!string.IsNullOrWhiteSpace(item) && 
                        item.Length > 1 && 
                        !IsIgnoredString(item) &&
                        !IsContainerElement(item) &&
                        !IsDataSection(item))
                    {
                        items.Add(item);
                    }
                }
                
                // Apply even/odd logic
                if (items.Count == 0)
                {
                    // Empty element (like <Security/>), skip
                    continue;
                }
                else if (items.Count == 1)
                {
                    // Single item = element value
                    fields[elementName] = items[0];
                }
                else if (items.Count % 2 == 0)
                {
                    // Even count = all attribute/value pairs
                    for (int i = 0; i < items.Count; i += 2)
                    {
                        fields[$"{elementName}.{items[i]}"] = items[i + 1];
                    }
                }
                else
                {
                    // Odd count > 1 = attribute/value pairs + element value at end
                    for (int i = 0; i < items.Count - 1; i += 2)
                    {
                        fields[$"{elementName}.{items[i]}"] = items[i + 1];
                    }
                    // Last item is the element value
                    fields[elementName] = items[items.Count - 1];
                }
            }
            
            return fields;
        }

        /// <summary>
        /// Checks if string is a real element (not a container or data section).
        /// </summary>
        private bool IsRealElement(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return false;
            
            if (_structureConfig?.ElementNames != null)
            {
                foreach (var elem in _structureConfig.ElementNames)
                {
                    if (value.Equals(elem, StringComparison.OrdinalIgnoreCase))
                        return true;
                }
            }
            
            return false;
        }

        /// <summary>
        /// Checks if string is a container element (ignored as boundary).
        /// </summary>
        private bool IsContainerElement(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return false;
            
            if (_structureConfig?.ContainerElements != null)
            {
                foreach (var elem in _structureConfig.ContainerElements)
                {
                    if (value.Equals(elem, StringComparison.OrdinalIgnoreCase))
                        return true;
                }
            }
            
            return false;
        }

        /// <summary>
        /// Checks if string is a data section (EventData, UserData).
        /// </summary>
        private bool IsDataSection(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return false;
            
            if (_structureConfig?.DataSections != null)
            {
                foreach (var section in _structureConfig.DataSections)
                {
                    if (value.Equals(section, StringComparison.OrdinalIgnoreCase))
                        return true;
                }
            }
            
            return false;
        }

        private string GetFieldValue(Dictionary<string, string> fields, params string[] keys)
        {
            foreach (var key in keys)
            {
                // First check if there's a configured mapping for this key
                if (_structureConfig?.FieldMappings != null && _structureConfig.FieldMappings.ContainsKey(key))
                {
                    foreach (var mapping in _structureConfig.FieldMappings[key])
                    {
                        if (fields.TryGetValue(mapping, out string value) && 
                            !string.IsNullOrWhiteSpace(value) && 
                            !IsIgnoredString(value))
                        {
                            return value;
                        }
                    }
                }
                
                // Then try direct key lookup
                if (fields.TryGetValue(key, out string directValue) && 
                    !string.IsNullOrWhiteSpace(directValue) && 
                    !IsIgnoredString(directValue))
                {
                    return directValue;
                }
            }
            
            return null;
        }

        private bool IsIgnoredString(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return true;
            
            if (_structureConfig?.IgnoredStrings != null)
            {
                foreach (var ignored in _structureConfig.IgnoredStrings)
                {
                    if (value.Contains(ignored))
                        return true;
                }
            }
            
            return false;
        }

        private bool IsKnownElement(string value)
        {
            return IsRealElement(value) || IsContainerElement(value) || IsDataSection(value);
        }

        private string FindProviderByPrefix(List<string> strings)
        {
            var prefixes = _structureConfig?.ProviderPrefixes ?? new List<string> { "Microsoft-Windows-" };
            
            foreach (var s in strings)
            {
                foreach (var prefix in prefixes)
                {
                    if (s.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                        return s.Trim();
                }
            }
            return "Unknown";
        }

        private string MapLevel(string level)
        {
            if (string.IsNullOrEmpty(level)) return "Information";
            
            string numericLevel = ExtractNumber(level);
            
            if (_structureConfig?.LevelMapping != null && 
                _structureConfig.LevelMapping.TryGetValue(numericLevel, out string mappedLevel))
            {
                return mappedLevel;
            }
            
            switch (numericLevel)
            {
                case "0": return "LogAlways";
                case "1": return "Critical";
                case "2": return "Error";
                case "3": return "Warning";
                case "4": return "Information";
                case "5": return "Verbose";
                default: return "Information";
            }
        }

        private string ExtractNumber(string value)
        {
            if (string.IsNullOrEmpty(value)) return null;
            
            var sb = new StringBuilder();
            foreach (char c in value)
            {
                if (char.IsDigit(c)) sb.Append(c);
            }
            return sb.Length > 0 ? sb.ToString() : null;
        }

        private string BuildDescription(List<string> strings)
        {
            var dataValues = new List<string>();
            bool inDataSection = false;
            
            for (int i = 0; i < strings.Count; i++)
            {
                string current = strings[i].Trim();
                
                // Enter data section
                if (IsDataSection(current))
                {
                    inDataSection = true;
                    continue;
                }
                
                // Exit if we hit a real element or container after entering data section
                if (inDataSection && (IsRealElement(current) || IsContainerElement(current)))
                {
                    break;
                }
                
                // Collect values in data section
                if (inDataSection)
                {
                    // Skip "Data" and "Binary" element names, collect their values
                    if (!current.Equals("Data", StringComparison.OrdinalIgnoreCase) &&
                        !current.Equals("Binary", StringComparison.OrdinalIgnoreCase) &&
                        !string.IsNullOrWhiteSpace(current) &&
                        current.Length > 1 &&
                        !IsIgnoredString(current))
                    {
                        dataValues.Add(current);
                    }
                }
            }
            
            return dataValues.Count > 0 
                ? "Event Data: " + string.Join(", ", dataValues)
                : string.Join(" ", strings);
        }
    }

    public class EvtxEventRecord
    {
        public long RecordId { get; set; }
        public DateTime TimeCreated { get; set; }
        public string EventId { get; set; }
        public string Level { get; set; }
        public string Provider { get; set; }
        public string Channel { get; set; }
        public string Computer { get; set; }
        public string UserSid { get; set; }
        public string RawXml { get; set; }

        public string GetDescription() => RawXml ?? "No description available";
    }
}
