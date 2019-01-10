using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using System.Web.Configuration;
using webtransformationadmin.Domain;
using webtransformationadmin.DAL;

namespace webtransformationadmin.DAL
{
    public class GMI_TRANSRecordHandler
    {
        StatusMsg Status = new StatusMsg();

        public void PLSQLExecRetransformation(string Hostname, string Username)
        {
            // Open the DatabaseHandler Object
            DBConnectAndRunSQL DB = new DBConnectAndRunSQL();
            // get SQL query config
            string OraConnectionDSource = WebConfigurationManager.AppSettings["GMIDB_Conn"];
            string OraUserPass = "User Id=" + WebConfigurationManager.AppSettings["GMIDB_user"] + ";Password=" + WebConfigurationManager.AppSettings["GMIDB_pass"] + ";";
            string OraConnectionString = OraUserPass + OraConnectionDSource;
            string OraStoredProc = WebConfigurationManager.AppSettings["PLSQL_RETRANSFORM"];

            // Set up command object
            OracleCommand OraCmd = new OracleCommand();
            OraCmd.CommandText = OraStoredProc;
            OraCmd.CommandType = CommandType.StoredProcedure;

            //Add parameter
            OracleParameter p_hostname = new OracleParameter("p_hostname", OracleDbType.Varchar2);
            p_hostname.Direction = ParameterDirection.Input;
            p_hostname.Value = Hostname;
            OraCmd.Parameters.Add(p_hostname);
            
            CreateLogFiles Log = new CreateLogFiles();
            Status.Print(false, true, "AUDIT:  ", Username + " " + OraCmd.CommandText + " " + p_hostname.Value);
            // Open the connection for the command and give it the connection object
            OraCmd.Connection = DB.OpenConnection(OraConnectionString);
            OraCmd.ExecuteNonQuery();
            OraCmd.Connection.Close();
            OraCmd.Dispose();
            
 
        }
        public List<GMI_TRANSRecord> GMI_TRANS_RecsAsList(string sqlQuery)
        {
            List<GMI_TRANSRecord> DataSet = new List<GMI_TRANSRecord>();
            // Open the DatabaseHandler Object
            DBConnectAndRunSQL DB = new DBConnectAndRunSQL();
            // get SQL query config
            string OraConnectionDSource = WebConfigurationManager.AppSettings["GMIDB_Conn"];
            string OraUserPass = "User Id=" + WebConfigurationManager.AppSettings["GMIDB_user"] + ";Password=" + WebConfigurationManager.AppSettings["GMIDB_pass"] + ";";
            string OraConnectionString = OraUserPass + OraConnectionDSource;
            

            OracleDataReader GMI_TRANSreader = DB.GetResultsAsReader(sqlQuery, DB.OpenConnection(OraConnectionString));
            if (GMI_TRANSreader.HasRows)
                Status.Print(false,true,"DEBUG:  ","Has Lines: " + GMI_TRANSreader.HasRows);
            else
                // how to handle when no rows are returned?
                Status.Print(false, true, "DEBUG: ", "Returns No Results: " + !GMI_TRANSreader.HasRows);

            //read the SQL results into the GMIREFRecord Object and add the object to a list.
            while (GMI_TRANSreader.Read())
            {
                //TODO work out if null are being returned fix them 

                GMI_TRANSRecord record = new GMI_TRANSRecord();
                record.FEED = GMI_TRANSreader.GetOracleString(GMI_TRANSreader.GetOrdinal("FEED"));
                record.HOSTNAME = GMI_TRANSreader.GetOracleString(GMI_TRANSreader.GetOrdinal("HOSTNAME"));
                record.GMI_STATUS = GMI_TRANSreader.GetOracleString(GMI_TRANSreader.GetOrdinal("GMI_STATUS"));
                record.GMI_ENVIRONMENT = GMI_TRANSreader.GetOracleString(GMI_TRANSreader.GetOrdinal("GMI_ENVIRONMENT"));
                record.GMI_INF = GMI_TRANSreader.GetOracleString(GMI_TRANSreader.GetOrdinal("INF"));
                record.GMI_CAP = GMI_TRANSreader.GetOracleString(GMI_TRANSreader.GetOrdinal("CAP"));
                record.GMI_LSG = GMI_TRANSreader.GetOracleString(GMI_TRANSreader.GetOrdinal("LSG"));
                record.GMI_LOCATION = GMI_TRANSreader.GetOracleString(GMI_TRANSreader.GetOrdinal("GMI_LOCATION"));
                record.GMI_MGMT_REGION = GMI_TRANSreader.GetOracleString(GMI_TRANSreader.GetOrdinal("GMI_MGMT_REGION"));
                record.GMI_CLIENT_NAME = GMI_TRANSreader.GetOracleString(GMI_TRANSreader.GetOrdinal("GMI_CLIENT_NAME"));
                record.GMI_DEVLOGMAP = GMI_TRANSreader.GetOracleString(GMI_TRANSreader.GetOrdinal("GMI_DEV_LOG_MAP"));
                record.BLOCKING = GMI_TRANSreader.GetOracleString(GMI_TRANSreader.GetOrdinal("BLOCKING"));
                //record.LAST_TRANSFORM = GMI_TRANSreader.GetOracleTimeStamp(GMI_TRANSreader.GetOrdinal("LAST_TRANSFORM"));
                record.ORIGIN_DATE = GMI_TRANSreader.GetOracleDate(GMI_TRANSreader.GetOrdinal("ORIGIN_DATE"));
                record.SRC_RECORD_ID = GMI_TRANSreader.GetOracleDecimal(GMI_TRANSreader.GetOrdinal("SRC_RECORD_ID")).ToString();
                record.SC_LOGICAL_NAME = GMI_TRANSreader.GetOracleString(GMI_TRANSreader.GetOrdinal("SC_LOGICAL_NAME"));
                if (record.GMI_DEVLOGMAP.ToString() == "null") { record.GMI_DEVLOGMAP = "-"; };
                if (record.SC_LOGICAL_NAME.ToString() == "null") { record.SC_LOGICAL_NAME = "-"; };
                if (record.SRC_RECORD_ID.ToString() == "null") { record.SRC_RECORD_ID = "-"; };
                if (record.GMI_CLIENT_NAME.ToString() == "null") { record.GMI_CLIENT_NAME = "-"; };
                if (record.BLOCKING.ToString() == "null") { record.BLOCKING = "-"; };
                DataSet.Add(record);
                
            }
            return DataSet;
        }
    }
}
