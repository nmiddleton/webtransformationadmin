// 
//                  $Id: DBConnectLog.cs 25981 2013-05-31 15:31:45Z neil.middleton $
//     $LastChangedDate: 2013-05-31 16:31:45 +0100 (Fri, 31 May 2013) $
// $LastChangedRevision: 25981 $
//       $LastChangedBy: neil.middleton $
//             $HeadURL: https://sami.cdt.int.thomsonreuters.com/svn/gms_gmi/trunk/infrastructure/tools/webtransformationadmin/webtransformationadmin.Domain/DBConnectLog.cs $

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oracle.DataAccess.Client;

namespace webtransformationadmin.Domain
{
    public class DBConnectLog
    {
        string _LogInfo = "";
        string _LogError = "";
        public DBConnectLog()
        {
            Successful = true;
        }
        public string LogInfo
        {
            get
            { 
                return _LogInfo; 
            }
            set
            {
                _LogInfo = value + Environment.NewLine;
            }
        }
        public string LogError
        {
            get
            {
                return _LogError;
            }
            set
            {
                _LogError = value + Environment.NewLine;
            }
        }
        public bool Successful { get; set; }
        public bool OracConnectionOpen { get; set; }
        public OracleConnection connection { get; set; }
    }
    
}
