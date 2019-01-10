using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace webtransformationadmin.DAL
{
    public class StatusMsg
    {
        public void Print(bool bLogToScreen, bool bLogToFile, string StatusMsgType, string StatusMsg)
        {
            string TimeStamp = DateTime.Today.ToLongDateString() + "   " + DateTime.Now.ToLongTimeString();
            string sMsg = TimeStamp + " " + StatusMsgType + " " + StatusMsg;

            if (bLogToFile)
            {
                // Write to a Log file
                CreateLogFiles LogFile = new CreateLogFiles();
                LogFile.WriteLog(sMsg);
            }
        }
    }
}
