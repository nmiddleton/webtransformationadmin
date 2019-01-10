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
    public class GMI_REFRecordHandler
    {
        StatusMsg Status = new StatusMsg();

        public List<GMI_REFRecord> GMI_REF_RecsAsList(string sqlQuery)
        {
            List<GMI_REFRecord> DataSet = new List<GMI_REFRecord>();
            // Open the DatabaseHandler Object
            DBConnectAndRunSQL DB = new DBConnectAndRunSQL();
            // get SQL query config
            string OraConnectionDSource = WebConfigurationManager.AppSettings["GMIDB_Conn"];
            string OraUserPass = "User Id=" + WebConfigurationManager.AppSettings["GMIDB_user"] + ";Password=" + WebConfigurationManager.AppSettings["GMIDB_pass"] + ";";
            string OraConnectionString = OraUserPass + OraConnectionDSource;
            

            OracleDataReader GMI_REFreader = DB.GetResultsAsReader(sqlQuery, DB.OpenConnection(OraConnectionString));
            if (GMI_REFreader.HasRows)
                Status.Print(false,true,"DEBUG:  ","Has Lines: " + GMI_REFreader.HasRows);
            else
                // how to handle when no rows are returned?
                Status.Print(false, true, "DEBUG: ", "Returns No Results: " + !GMI_REFreader.HasRows);

            //read the SQL results into the GMIREFRecord Object and add the object to a list.
            while (GMI_REFreader.Read())
            {
                //TODO work out if null are being returned fix them 

                GMI_REFRecord record = new GMI_REFRecord();
                record.GMI_INITIATING_PROCESS = GMI_REFreader.GetOracleString(GMI_REFreader.GetOrdinal("GMI_INITIATING_PROCESS"));
                record.HOSTNAME = GMI_REFreader.GetOracleString(GMI_REFreader.GetOrdinal("HOSTNAME"));
                record.GMI_SOURCE = GMI_REFreader.GetOracleString(GMI_REFreader.GetOrdinal("GMI_SOURCE"));
                record.LAST_UPDATED = GMI_REFreader.GetOracleDate(GMI_REFreader.GetOrdinal("LAST_UPDATED"));
                record.GMI_LOCATION = GMI_REFreader.GetOracleString(GMI_REFreader.GetOrdinal("GMI_LOCATION"));
                record.GMI_MGMT_REGION = GMI_REFreader.GetOracleString(GMI_REFreader.GetOrdinal("GMI_MGMT_REGION"));
                record.GMI_ENVIRONMENT = GMI_REFreader.GetOracleString(GMI_REFreader.GetOrdinal("GMI_ENVIRONMENT"));
                record.GMI_STATUS = GMI_REFreader.GetOracleString(GMI_REFreader.GetOrdinal("GMI_STATUS"));
                record.INF = GMI_REFreader.GetOracleString(GMI_REFreader.GetOrdinal("INF"));
                record.CAP = GMI_REFreader.GetOracleString(GMI_REFreader.GetOrdinal("CAP"));
                record.LSG = GMI_REFreader.GetOracleString(GMI_REFreader.GetOrdinal("LSG"));
                record.GMI_CLIENT_NAME = GMI_REFreader.GetOracleString(GMI_REFreader.GetOrdinal("GMI_CLIENT_NAME"));
                record.DEVLOGMAP = GMI_REFreader.GetOracleString(GMI_REFreader.GetOrdinal("GMI_DEV_LOG_MAP"));
                record.SC_LOGICAL_NAME = GMI_REFreader.GetOracleString(GMI_REFreader.GetOrdinal("SC_LOGICAL_NAME"));
                if (record.DEVLOGMAP.ToString() == "null") { record.DEVLOGMAP = "-"; };
                if (record.SC_LOGICAL_NAME.ToString() == "null") { record.SC_LOGICAL_NAME = "-"; };
                if (record.GMI_CLIENT_NAME.ToString() == "null") { record.GMI_CLIENT_NAME = "-"; };
                DataSet.Add(record);
                
            }
            return DataSet;
        }
        public void GMI_REFArchive(string Hostname, string Feedname, string Username)
        {
            // Open the DatabaseHandler Object
            Status.Print(false, true, "AUDIT:  ", "GMI_REFArchive " + Hostname + " " + Feedname + " " + Username);
            DBConnectAndRunSQL DB = new DBConnectAndRunSQL();
            // get SQL query config
            string OraConnectionDSource = WebConfigurationManager.AppSettings["GMIDB_Conn"];
            string OraUserPass = "User Id=" + WebConfigurationManager.AppSettings["GMIDB_user"] + ";Password=" + WebConfigurationManager.AppSettings["GMIDB_pass"] + ";";
            string OraConnectionString = OraUserPass + OraConnectionDSource;
            string sqlcmd = WebConfigurationManager.AppSettings["PLSQLARCHIVE_GMI_REF"];

            sqlcmd = sqlcmd.Replace("/*HOST_MODIFIER*/", Hostname);
            sqlcmd = sqlcmd.Replace("/*FEED_MODIFIER*/", Feedname);

            Status.Print(false, true, "Username:  ", "GMI_REFArchive " + sqlcmd);
            // Set up command object
            OracleCommand cmdDelete = new OracleCommand();
            cmdDelete.CommandText = sqlcmd;
            // Open the connection for the command and give it the connection object
            cmdDelete.Connection = DB.OpenConnection(OraConnectionString);
            cmdDelete.ExecuteNonQuery();
            cmdDelete.Dispose();
            
 
        }
        public void PRC_ARCHIVE_DEVICE(string Hostname, string Feedname, string Username)
        {
            // Open the DatabaseHandler Object
            DBConnectAndRunSQL DB = new DBConnectAndRunSQL();
            // get SQL query config
            string OraConnectionDSource = WebConfigurationManager.AppSettings["GMIDB_Conn"];
            string OraUserPass = "User Id=" + WebConfigurationManager.AppSettings["GMIDB_user"] + ";Password=" + WebConfigurationManager.AppSettings["GMIDB_pass"] + ";";
            string OraConnectionString = OraUserPass + OraConnectionDSource;
            string OraStoredProc = WebConfigurationManager.AppSettings["PLSQLARCHIVE_GMI_REF"];

            // Set up command object
            OracleCommand OraCmd = new OracleCommand();
            OraCmd.CommandText = OraStoredProc;
            OraCmd.CommandType = CommandType.StoredProcedure;

            //Add parameter
            OracleParameter process_id = new OracleParameter("process_id", OracleDbType.Decimal, ParameterDirection.Input);
            OracleParameter p_sc_logical_name = new OracleParameter("p_sc_logical_name", OracleDbType.Varchar2, ParameterDirection.Input);
            OracleParameter p_hostname = new OracleParameter("p_hostname", OracleDbType.Varchar2, ParameterDirection.Input);
            OracleParameter p_commit = new OracleParameter("p_commit", OracleDbType.Varchar2, ParameterDirection.Input);
            decimal FeedNumber = Convert.ToDecimal(WebConfigurationManager.AppSettings["Feedname"]);
            process_id.Value = FeedNumber;
            p_sc_logical_name.Value = "";
            p_hostname.Value = Hostname;
            p_commit.Value = 'Y';
            
            OraCmd.Parameters.Add(process_id);
            OraCmd.Parameters.Add(p_sc_logical_name);
            OraCmd.Parameters.Add(p_hostname);
            OraCmd.Parameters.Add(p_commit);

            CreateLogFiles Log = new CreateLogFiles();
            Status.Print(false, true, "AUDIT:  ", Username + " " + OraCmd.CommandText + " " + process_id.Value + " " + p_sc_logical_name.Value + " " + p_hostname.Value + " " + p_commit.Value);
            // Open the connection for the command and give it the connection object
            OraCmd.Connection = DB.OpenConnection(OraConnectionString);
            OraCmd.ExecuteNonQuery();
            OraCmd.Connection.Close();
            OraCmd.Dispose();


        }
    }
}
