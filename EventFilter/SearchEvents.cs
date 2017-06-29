using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace EventFilter
{
    class SearchEvents
    {
        ArrayHandler array = new ArrayHandler();
        Bug bug = new Bug();
        public List<string> events = new List<string>();

        public SearchEvents()
        {

        }

        public string GetKeywords(string path)
        {
            string line = "";

            if (File.Exists(path) == true)
            {
                StreamReader getKeywords = new StreamReader(path);
                line = getKeywords.ReadLine();
            }
            else
            {
                //bug.BugReportLog("No keywords found at path " + path);
            }

            return line;
        }

        public bool CheckFileExistence(string file)
        {
            if (File.Exists(Directory.GetCurrentDirectory() + file))
            {
                return true;
            }

            return false;
        }

        public string SearchEvent(string[] eventArray, string Id)
        {
            List<string> data = new List<string>();

            bool stop = false;

            for (int i = 0; i < eventArray.Length; i++)
            {
                if (stop == true)
                {
                    break;
                }

                if (i.ToString() == Id)
                {
                    int dataNr = 0;
                    for (int a = -13; a < 15; a++)
                    {
                        if (a < 12)
                        {
                            if (dataNr < 6)
                            {
                                data.Add(eventArray[i + a].ToString());
                                dataNr++;
                            }

                            if (dataNr >= 6)
                            {
                                if (!eventArray[i + a].Contains("Event"))
                                {
                                    data.Add(eventArray[i + a].ToString());
                                    dataNr++;
                                }
                                else
                                {
                                    stop = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            return (array.ConvertListToString(data, "\n"));


            #region commented code
            /*string line = "";
            //string[] findKeyword = array.ConvertStringToArray(keyword, "\n");
            
            for(int i = 0; i < findKeyword.Length; i++)
            {
                if (findKeyword[i].Contains(date))
                {
                    // Get line number based on date it has been found
                    int start = findKeyword[i].IndexOf(':');
                    int end = findKeyword[i].IndexOf(':', findKeyword[i].IndexOf(':'));

                    line = findKeyword[i].Substring(start, end);
                    line = new String(line.Where(Char.IsDigit).ToArray());
                    break;
                }
            }*/
            //bool stop = false;

            //if(date != "")
            //{
            /* for (int i = 0; i < eventArray.Length; i++)
             {

                 if(stop == true)
                 {
                     break;
                 }

                 // Rewrite to check for all this + the identified line
                 if(i.ToString() == line)
                 {
                     int dataNr = 0;
                     for (int a = -3; a < 25; a++)
                     {
                         if (a < 22)
                         {
                             if (dataNr < 6)
                             {
                                 data.Add(eventArray[i + a].ToString());
                                 dataNr++;
                             }

                             if (dataNr >= 6)
                             {
                                 if (!eventArray[i + a].Contains("Event"))
                                 {
                                     data.Add(eventArray[i + a].ToString());
                                     dataNr++;
                                 }
                                 else
                                 {
                                     stop = true;
                                     break;
                                 }
                             }
                         }
                     }
                 }
             }*/
            //}
            //else
            //{
            //return "Date is invalid!";
            //}
            #endregion
        }

        public int CountSearchEvent(string[] eventArray, string countable)
        {
            int counted = 0;

            for (int i = 0; i < eventArray.Length; i++)
            {
                if (eventArray[i].Contains(countable))
                {
                    counted++;
                }
            }

            return counted;
        }

        public string[] FindEventsBefore(string[] eventArray, string date)
        {
            string[] results = new string[eventArray.Length];

            for (int i = eventArray.Length; i > 0; i--)
            {
                if (eventArray[i].Contains(date))
                {
                    results[i] = eventArray[i].ToString();
                }
            }

            return results;
        }

        public string[] FindEventsAfter(string[] eventArray, string date)
        {
            string[] results = new string[eventArray.Length];

            for (int i = 0; i < eventArray.Length; i++)
            {
                if (eventArray[i].Contains(date))
                {
                    results[i] = eventArray[i].ToString();
                }
            }

            return results;
        }

        public string[] AnalyzeResults(string[] data, string[] keywords)
        {
            string[] checkKeywords = new string[keywords.Length];
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i].Contains("Events"))
                {
                    int keyCheck = 0;
                    foreach(string key in keywords)
                    {
                        if(data[i - 1].Contains(key) == false)
                        {
                            checkKeywords[keyCheck] = false.ToString();
                        }
                        keyCheck++;
                    }
                }
            }

            return data;
        }

        
        public string[] CreateStack(string[] events, int startPos, int stackLength)
        {
            string[] stack = new string[stackLength];

            for(int i = 0; i < stack.Length; i++)
            {
                stack[i] = events[startPos + i].ToString();
            }

            return stack;
        }

        public List<string> SearchAllEvents(string filePath, string keywords, string ResultCount, string Time)
        {
            List<string> logs = new List<string>();

            try
            {
                // Initialization
                keywords = keywords.Replace(" ", "");
                var keyword = keywords.Split(',');
                var watch = System.Diagnostics.Stopwatch.StartNew();

                var lines = File.ReadLines(filePath);
                string[] eventArray = new string[lines.Count()];

                int resultCount = 0;

                logs.Add("Parameters used: \t filepath: " + filePath + "\n\t keywords to use: " + array.ConvertArrayToString(keyword, ", "));

                logs.Add("Lines in eventArray: " + lines.Count());

                int i = -1;
                foreach (var line in lines)
                {
                    i++;
                    eventArray[i] = line;
                }

                var lastKeyword = keyword.First();

                logs.Add("First lastKeyword: " + lastKeyword);

                foreach (var key in keyword)
                {
                    int localCounter = 0;
                    logs.Add("Overwriting lastKeyword " + lastKeyword + " with " + key + "\n\n");
                    lastKeyword = key;

                    logs.Add("Following results have been found using keyword: " + key);

                    for (i = 0; i < eventArray.Length; i++)
                    {
                        string[] eventEntry = new string[2];

                        if (eventArray[i].Contains(key))
                        {
                            resultCount++;
                            localCounter++;

                            int counter = 0;

                            for (int a = -12; a < counter; a++)
                            {
                                string text = eventArray[i + a];
                                // Count back from position of keyword to get the first line of description
                                if (eventArray[i - 1].Contains("Description"))
                                {
                                    eventEntry[1] = eventArray[i].ToString();

                                    logs.Add("\t" + eventArray[i].ToString());

                                    break;
                                }
                                else
                                {
                                    text = eventArray[i + a];
                                    if (text.Contains("Description"))
                                    {
                                        eventEntry[1] = eventArray[(i + a) + 1].ToString();

                                        logs.Add("\t" + eventArray[(i + a) + 1].ToString());

                                        break;
                                    }
                                }
                            }

                            // Count back from position of keyword to get the date
                            for (int a = -13; a < counter; a++)
                            {
                                string text = eventArray[i + a];
                                if (text.Contains("Date"))
                                {
                                    eventEntry[0] = eventArray[i + a].ToString();
                                }
                            }

                            events.Add(eventEntry.ToString());
                        }
                    }

                    logs.Add("\nFound " + localCounter.ToString() + " with keyword " + key);
                    logs.Add("===========================================\n\n\n\n\n");
                }

                ResultCount = "\n\nEvents found: " + resultCount.ToString();
                logs.Add(ResultCount);

                if (resultCount == 0)
                {
                    MessageBox.Show("No event log has the provided keywords.", "No result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                watch.Stop();
                var elapsedTime = watch.Elapsed.TotalSeconds;

                Time = "Found results in: " + elapsedTime.ToString();

                return logs;
            }
            catch (Exception e)
            {
                logs.Add("Error: " + e.Message);
                MessageBox.Show("A problem has occured.\nPlease notify the developer of this issue!", "App crashed", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return logs;
            }
        }

        public string[] ValidateKeywords(string keywords)
        {
            string[] keys = keywords.Split(',');

            for(int i = 0; i < keys.Length; i++)
            {
                keys[i] = keys[i].Trim();
            }

            return keys;
        }
    }
}
