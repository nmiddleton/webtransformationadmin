using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oracle.DataAccess.Types;

namespace webtransformationadmin.Domain
{
    public class METSTORERecord
    {

        public Int32 METRIC_ID { get; set; }
        public String DATE_TIME { get; set; }
        public Int32 VALUE { get; set; }

    }
}
