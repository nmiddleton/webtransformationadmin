using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oracle.DataAccess.Types;

namespace webtransformationadmin.Domain
{
    public class GMI_TRANSRecord
    {
        public OracleString FEED { get; set; }    
        public OracleString HOSTNAME { get; set; }
        public OracleString GMI_ENVIRONMENT { get; set; }
        public OracleString GMI_STATUS { get; set; }
        public OracleString GMI_INF { get; set; }
        public OracleString GMI_CAP { get; set; }
        public OracleString GMI_LSG { get; set; }
        public OracleString GMI_LOCATION { get; set; }
        public OracleString GMI_MGMT_REGION { get; set; }
        public OracleString GMI_CLIENT_NAME { get; set; }
        public OracleString GMI_DEVLOGMAP { get; set; }
        public OracleString SRC_RECORD_ID { get; set; }
        public OracleString SC_LOGICAL_NAME { get; set; }
        //public OracleTimeStamp LAST_TRANSFORM { get; set; }
        public OracleDate ORIGIN_DATE { get; set; }

        // The status in the blocking table
        public bool IsBlocked{ get; set; }
        public OracleString BLOCKING { get; set; }
    }
}
