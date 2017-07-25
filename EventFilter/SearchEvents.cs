using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.ComponentModel;

namespace EventFilter
{
    class SearchEvents
    {
        Background background = new Background();
        Keyword _keyword = new Keyword();
        public string[] eventId = new string[0];
        public string[] eventDate = new string[0];
        public string[] eventDescr = new string[0];

        public SearchEvents()
        {

        }

        public string SearchEvent(string Id)
        {
            return background.FindEventElement(Id);
        }

        public string[] SearchEvent(object sender, DoWorkEventArgs e)
        {
            Form1 form = new Form1();

            List<string> log = new List<string>();

            BackgroundWorker worker = sender as BackgroundWorker;

            try
            {
                string keyword = Keyword.Keywords;
                var keywords = Keyword.ValidateKeywords(keyword);
                var watch = System.Diagnostics.Stopwatch.StartNew();
                string[] _eventArray = Array.ConstructEventArray(Keyword.EventLocation);
                int resultCount = 0;
                log.Add("Log: Parameters used: \t filepath: " + Keyword.EventLocation + "\n\t keywords to use: " + Array.ConvertArrayToString(keywords, ", "));
                log.Add("Log: Lines in eventArray: " + _eventArray.Length);
                int i = -1;
                var lastKeyword = keywords[0];
                log.Add("Log: First lastKeyword: " + lastKeyword);
                //int progress = 3;
                int keyProgress = 0;
                int logProgress = keyProgress;

                foreach (var key in keywords)
                {
                    int localCounter = 0;

                    keyProgress++;
                    log.Add("Log: Overwriting lastKeyword " + lastKeyword + " with " + key + "\n\n");
                    keyProgress++;
                    lastKeyword = key;

                    log.Add("Log: Following results have been found using keyword: " + key);

                    for (i = 0; i < _eventArray.Length; i++)
                    {
                        string[] eventEntry = new string[3];

                        if (_eventArray[i].Contains(key))
                        {
                            int a = 0;

                            while (!_eventArray[i + a].Contains("Event["))
                            {
                                if (_eventArray[i + a].Contains("Description"))
                                {
                                    eventEntry[1] = _eventArray[(i + a) + 1].ToString();
                                    form._events.Add(_eventArray[i]);

                                    // Add the first line of description into the list.
                                    form._eventId.Add((i + a + 1).ToString());

                                    localCounter++;
                                    resultCount++;
                                }

                                if (_eventArray[i + a].Contains("Date"))
                                {

                                    form._eventDate.Add(_eventArray[i + a]);

                                    // Id
                                    eventEntry[2] = i.ToString();

                                    eventEntry[0] = _eventArray[i + a].ToString();

                                    log.Add("Log: \t Line nr: " + (i + a) + ": " + eventEntry[0] + ": " + eventEntry[1]);
                                    logProgress++;

                                    break;
                                }
                                a--;
                            }

                            // Count back from position of keyword to get the date
                            //for (int a = -13; a < counter; a++)
                            //{
                            //    if (_eventArray[i + a].Contains("Date"))
                            //    {
                            //        _eventId.Add(i.ToString());

                            //        _eventDate.Add(_eventArray[i + a]);

                            //        // Id
                            //        eventEntry[2] = i.ToString();

                            //        eventEntry[0] = _eventArray[i + a].ToString();

                            //        log.Add("Log: \t Line nr: " + (i + a) + ": " + eventEntry[0] + ": " + eventEntry[1]);
                            //        logProgress++;

                            //        break;
                            //    }
                            //}

                            //for (int a = -12; a < counter; a++)
                            //{
                            //    if (_eventArray[i + a].Contains("Description"))
                            //    {
                            //        eventEntry[1] = _eventArray[(i + a) + 1].ToString();
                            //        _events.Add(_eventArray[i]);
                            //        localCounter++;
                            //        resultCount++;

                            //        break;
                            //    }

                            // Count back from position of keyword to get the first line of description
                            //if (_eventArray[i - 1].Contains("Description"))
                            //{
                            //    eventEntry[1] = _eventArray[i].ToString();
                            //    _events.Add(_eventArray[i]);
                            //    resultCount++;
                            //    localCounter++;

                            //    break;
                            //}
                            //else
                            //{
                            //}
                            //}

                            if (resultCount >= 10000)
                            {
                                Form1.MessageWrite("There are over 5000 events matching the keywords", "Over 5000 events", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                break;
                            }

                            log.Add("Event: " + eventEntry[0] + " + " + eventEntry[1] + " + " + eventEntry[2]);
                            logProgress++;
                        }
                    }

                    if (resultCount >= 10000)
                    {
                        break;
                    }

                    log.Add("Log: \nFound " + localCounter.ToString() + " with keyword " + key);
                    logProgress++;
                    log.Add("Log: ===========================================\n\n\n\n\n");
                    logProgress++;
                }

                log.Add("Log: \n\nEvents found: " + resultCount.ToString());
                logProgress++;
                log.Add("Counter: Events found: " + resultCount.ToString());
                logProgress++;

                if (resultCount == 0)
                {
                    MessageBox.Show("No event log has the provided keywords.", "No result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                watch.Stop();
                var elapsedTime = watch.Elapsed.TotalSeconds;

                log.Add("Time: Found results in: " + elapsedTime.ToString());

                e.Result = form._eventId;

                return log.ToArray();
            }
            catch (Exception exc)
            {
                log.Add("Log: Error: " + exc.Message);
                Form1.MessageWrite("A problem has occured.\nPlease notify the developer of this issue!", "App crashed", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return log.ToArray();
            }
        }

        public string[] FilterDuplicates(List<string> description, List<string> id, List<string> date)
        {
            var tags = new HashSet<string>();

            //List<string> localDescr = new List<string>();
            List<string> localId = new List<string>();
            List<string> localDate = new List<string>();
            List<string> des = new List<string>();

            for (int i = 0; i < date.Count; i++)
            {
                //des.Add(background.GetDescription(id[i]));

                if(tags.Add(description[i]))
                {
                    //localDescr.Add(description[i].ToString());
                    localId.Add(id[i].ToString());
                    localDate.Add(date[i].ToString());
                }
            }

            eventId = localId.ToArray();
            eventDate = localDate.ToArray();

            return tags.ToArray();
        }

        #region code
        //public string[] FilterDuplicates(string[] events, List<string> DescriptionId)
        //{
        //    //string[] DescriptionId = ((IEnumerable)descriptionId).Cast<object>().Select(x => x.ToString()).ToArray();
        //    List<string> DateList = new List<string>();
        //    List<string> DescriptionList = new List<string>();

        //    var tags = new HashSet<string>();

        //    ListView newData = new ListView();

        //    for (int i = 0; i < DescriptionId.Count; i++)
        //    {
        //        if (DescriptionId[i].Contains("Date") == false)
        //        {
        //            DescriptionList.Add(DescriptionId[i].ToString());
        //        }
        //    }

        //    string[] Descriptions = new string[DescriptionId.Count];

        //    Descriptions = Array.ConstructArray(DescriptionList);

        //    // Get complete description
        //    for (int i = 0; i < DescriptionList.Count; i++)
        //    {
        //        newData.Items.Add(background.FindEventElement(events, Descriptions[i].ToString(), 0, 8, "Event[").Replace("\n", ""));
        //    }

        //    // Copy description trimmed to array
        //    string[] des = new string[DescriptionList.Count];
        //    int a = 0;
        //    foreach (ListViewItem item in newData.Items)
        //    {
        //        des[a] = item.ToString().Replace("ListViewItem: ", "").Trim('{').Trim('}');
        //        ++a;
        //    }

        //    List<string> data = new List<string>();
        //    // Copy description back to list
        //    for (int i = 0; i <= des.Length; i++)
        //    {
        //        if (i == des.Length)
        //            break;

        //        data.Add(des[i].ToString());
        //    }

        //    List<string> desc = new List<string>();
        //    // Save description to desc list
        //    for (int i = 0; i < data.Count; i++)
        //    {
        //        if (tags.Add(des[i]))
        //        {
        //            desc.Add(Descriptions[i].ToString());
        //        }
        //    }

        //    for (int i = 0; i < tags.Count * 2; i++)
        //    {
        //        if (DescriptionId[i].Contains("Date"))
        //        {
        //            DateList.Add(DescriptionId[i]);
        //        }
        //    }

        //    eventId = new string[tags.Count];
        //    eventDate = new string[tags.Count];

        //    eventId = Array.ConstructArray(desc);

        //    eventDate = Array.ConstructArray(DateList);

        //    des = tags.ToArray();

        //    return des;
        //}

        #endregion
    }
}