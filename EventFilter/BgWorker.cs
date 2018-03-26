using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;


namespace EventFilter
{
    public class BgWorker : BackgroundWorker
    {
        public void reportProgress(ref int percentProgress, object userState)
        {
            ReportProgress(percentProgress, userState);
        }
    }
}
