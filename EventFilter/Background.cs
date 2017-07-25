using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace EventFilter
{
    class Background
    {
        //protected int _keywordFilter = 0;

        //public static string keyLocation = "\\keywords.txt";
        //public static string eventLocation = "\\eventlog.txt";
        public string operators;
        public string operatorResult;

        //Keyword keywords = new Keyword();

        public Background()
        {

        }

        public string FindEventElement(string Id)
        {
            string[] eventArray = Array.ConstructEventArray(Keyword.EventLocation);
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
                    int a = 0;
                    while(!eventArray[i+a].Contains("Event["))
                    {
                        //data.Add(eventArray[i + a].ToString());
                        a--;
                    }

                    while(!eventArray[i+a+1].Contains("Event["))
                    {
                        data.Add(eventArray[i + a].ToString());
                        a++;
                    }

                    //int dataNr = 0;
                    //for (a = -13; a < 15; a++)
                    //{
                    //    if (a < 12)
                    //    {
                    //        if (dataNr < 6)
                    //        {
                    //            //if((i+a)<eventArray.Length)
                    //            //{
                    //                data.Add(eventArray[i + a].ToString());
                    //                dataNr++;
                    //            //}
                    //        }

                    //        if (dataNr >= 6)
                    //        {
                    //            if ((i+a) < eventArray.Length)
                    //            {
                    //                if (!eventArray[i + a].Contains(StopAtPart))
                    //                {
                    //                    data.Add(eventArray[i + a].ToString());
                    //                    dataNr++;
                    //                }
                    //                else
                    //                {
                    //                    stop = true;
                    //                    break;
                    //                }
                    //            }
                    //        }
                    //    }
                    //}
                }
            }

            return (Array.ConvertListToString(data, "\n"));
        }

        public string FindEventElement(string[] eventArray, string Id, int scanStart = -13, int scanLength = 15, string StopAtPart = "Event")
        {
            eventArray = Array.ConstructEventArray(Keyword.EventLocation);

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
                    for (int a = scanStart; a < scanLength; a++)
                    {
                        if (a < 12)
                        {
                            if (dataNr < 6)
                            {
                                if((i+a) < eventArray.Length)
                                {
                                    if (!eventArray[i + a].Contains(StopAtPart))
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

                            if (dataNr >= 6)
                            {
                                if((i + a) < eventArray.Length)
                                {
                                    if (!eventArray[i + a].Contains(StopAtPart))
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
            }

            return (Array.ConvertListToString(data, "\n"));
        }

        public int GetCount(string key)
        {
            string path = Keyword.EventLocation;

            string[] events = Array.ConstructEventArray(path);
            int countable = 0;

            for(int i = 0; i < events.Length; i++)
            {
                if(events[i].Contains(key))
                {
                    countable++;
                }
            }

            return countable;
        }

        public string GetDescription(string Id)
        {
            string[] eventArray = Array.ConstructEventArray(Keyword.EventLocation);
            List<string> data = new List<string>();

            for (int i =0; i < eventArray.Length; i++)
            {
                if(i.ToString() == Id)
                {
                    int a = 0;

                    if ((i+a) < eventArray.Length)
                    {
                        while (!eventArray[i + a].Contains("Description:"))
                        {
                            //data.Add(eventArray[i + a].ToString());
                            a--;
                        }

                        while ((i+a+1) < eventArray.Length && !eventArray[i + a + 1].Contains("Event["))
                        {
                            //while((i+a+1) < eventArray.Length)
                            //{
                                data.Add(eventArray[i + a + 1].ToString());
                                a++;
                            //}
                        }
                        
                        data.Remove(eventArray[(i+a)].ToString());
                    }
                }
            }

            return Array.ConcatArrayToString(data.ToArray());
        }


        /// <summary>
        /// Check if file exists
        /// </summary>
        /// <param name="file">File path to check</param>
        /// <returns></returns>
        public static bool CheckFileExistence(string file)
        {
            if (File.Exists(file))
            {
                return true;
            }

            return false;
        }
        /// <summary>
        /// Shortened version of GetCurrentDirectory
        /// </summary>
        /// <returns></returns>
        public static string GetLocation()
        {
            return Directory.GetCurrentDirectory();
        }
    }
}