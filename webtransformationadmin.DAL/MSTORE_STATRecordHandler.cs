using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using System.Web.Configuration;
using webtransformationadmin.Domain;
using webtransformationadmin.DAL;

namespace webtransformationadmin.DAL
{
    public class MSTORE_STATRecordHandler
    {
        public List<METSTORERecord> METSTORE_CSV_RecsAsList(string sqlQuery)
        {
            List<METSTORERecord> DataSet = new List<METSTORERecord>();
            // Open the DatabaseHandler Object
            DBConnectAndRunSQL DB = new DBConnectAndRunSQL();
            // get SQL query config
            string OraConnectionDSource = WebConfigurationManager.AppSettings["GMIDB_Conn"];
            string OraUserPass = "User Id=" + WebConfigurationManager.AppSettings["GMIDB_user"] + ";Password=" + WebConfigurationManager.AppSettings["GMIDB_pass"] + ";";
            string OraConnectionString = OraUserPass + OraConnectionDSource;


            OracleDataReader reader = DB.GetResultsAsReader(sqlQuery, DB.OpenConnection(OraConnectionString));
/*            if (GMI_TRANSreader.HasRows)
                Status.Print(false, true, "DEBUG:  ", "Has Lines: " + GMI_TRANSreader.HasRows);
            else
                // how to handle when no rows are returned?
                Status.Print(false, true, "DEBUG: ", "Returns No Results: " + !GMI_TRANSreader.HasRows);

*/            //read the SQL results into the GMIREFRecord Object and add the object to a list.
            while (reader.Read())
            {
                //TODO work out if null are being returned fix them 

                METSTORERecord record = new METSTORERecord();
                record.METRIC_ID = reader.GetOracleDecimal(reader.GetOrdinal("METRIC_ID")).ToInt32();
                record.DATE_TIME = reader.GetOracleTimeStamp(reader.GetOrdinal("DATE_TIME")).ToString();
                record.DATE_TIME = record.DATE_TIME.Remove(record.DATE_TIME.Length - 10);
                record.VALUE = reader.GetOracleDecimal(reader.GetOrdinal("VALUE")).ToInt32();
                
                DataSet.Add(record);

            }
            return DataSet;
        }
    }
}
