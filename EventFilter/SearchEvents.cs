using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace EventFilter
{
    class SearchEvents
    {
        Background background = new Background();

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