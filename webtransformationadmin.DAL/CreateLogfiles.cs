// 
//                  $Id: CreateLogfiles.cs 26257 2013-07-01 13:41:17Z neil.middleton $
//     $LastChangedDate: 2013-07-01 14:41:17 +0100 (Mon, 01 Jul 2013) $
// $LastChangedRevision: 26257 $
//       $LastChangedBy: neil.middleton $
//             $HeadURL: https://sami.cdt.int.thomsonreuters.com/svn/gms_gmi/trunk/infrastructure/tools/webtransformationadmin/webtransformationadmin.DAL/CreateLogfiles.cs $

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web.Configuration;

namespace webtransformationadmin.DAL
{
    public class CreateLogFiles
    {
        private string sActLog;
        private string sDirectory = WebConfigurationManager.AppSettings["iis_ActivityLogDir"];
        private string sAppName = WebConfigurationManager.AppSettings["ThisAppName"];

        public CreateLogFiles()
        {
            if (!Directory.Exists(sDirectory))
            {
                Directory.CreateDirectory(sDirectory);
            }
            //this variable used to create log filename format "
            //for example filename : ErrorLogYYYYMMDD
            string sYear    = DateTime.Now.Year.ToString();
            string sMonth    = DateTime.Now.Month.ToString().PadLeft(2,'0');
            string sDay = DateTime.Now.Day.ToString().PadLeft(2, '0');
            sActLog = sDirectory + "/Activity_" + sAppName + "_" + sYear + sMonth + sDay + ".log";
            sActLog.Replace("//", "/");
        }
        public void WriteLog(string sMsg)
        {
            StreamWriter sw = new StreamWriter(sActLog, true);
            sw.WriteLine(sMsg);
            sw.Flush();
            sw.Close();
        }

    }
}
