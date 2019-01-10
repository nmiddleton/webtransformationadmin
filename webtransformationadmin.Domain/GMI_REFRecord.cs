using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oracle.DataAccess.Types;

namespace webtransformationadmin.Domain
{
    public class GMI_REFRecord
    {
        public OracleString GMI_INITIATING_PROCESS { get; set; }
        public OracleString HOSTNAME { get; set; }
        public OracleString GMI_SOURCE { get; set; }
        public OracleDate LAST_UPDATED { get; set; }
        public OracleString GMI_LOCATION { get; set; }
        public OracleString GMI_MGMT_REGION { get; set; }
        public OracleString GMI_ENVIRONMENT { get; set; }
        public OracleString GMI_STATUS { get; set; }
        public OracleString INF { get; set; }
        public OracleString CAP { get; set; }
        public OracleString LSG { get; set; }
        public OracleString GMI_CLIENT_NAME { get; set; }
        public OracleString DEVLOGMAP { get; set; }
        public OracleString SC_LOGICAL_NAME { get; set; }
    }
}