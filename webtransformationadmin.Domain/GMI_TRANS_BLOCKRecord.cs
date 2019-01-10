using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oracle.DataAccess.Types;

namespace webtransformationadmin.Domain
{
    public class GMI_TRANS_BLOCKRecord
    {
        public OracleString HOSTNAME { get; set; }
        public OracleString GMI_INITIATING_PROCESS { get; set; }
        public OracleString BLOCKING { get; set; }
        public OracleString USER_UPDATED { get; set; }
        public OracleDate DATE_UPDATED { get; set; }
        

    }
}
