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
    public class GMI_TRANS_BLOCKRecordHandler
    {
        StatusMsg Status = new StatusMsg();
        public void PLSQLExec(string Hostname, string Feedname, string Operation, string Username)
        {
            // Open the DatabaseHandler Object
            DBConnectAndRunSQL DB = new DBConnectAndRunSQL();
            // get SQL query config
            string OraConnectionDSource = WebConfigurationManager.AppSettings["GMIDB_Conn"];
            string OraUserPass = "User Id=" + WebConfigurationManager.AppSettings["GMIDB_user"] + ";Password=" + WebConfigurationManager.AppSettings["GMIDB_pass"] + ";";
            string OraConnectionString = OraUserPass + OraConnectionDSource;
            string OraStoredProc = WebConfigurationManager.AppSettings["PLSQL_GMI_TRANS"];
  
            // Set up command object
            OracleCommand OraCmd = new OracleCommand();
            OraCmd.CommandText = OraStoredProc;
            OraCmd.CommandType = CommandType.StoredProcedure;
            OraCmd.Parameters.Add("p_hostname", OracleDbType.Varchar2).Value = Hostname;
            OraCmd.Parameters.Add("p_initiating_process", OracleDbType.Varchar2).Value = Feedname;
            OraCmd.Parameters.Add("p_blocking",OracleDbType.Varchar2).Value =  Operation;
            OraCmd.Parameters.Add("p_user", OracleDbType.Varchar2).Value = Username;

            // Log
            Status.Print(false, true, "AUDIT:  ", Username + " " + OraCmd.CommandText + " " + Operation + " " + Hostname + " " + Feedname);
            // Open the connection for the command and give it the connection object
            OraCmd.Connection = DB.OpenConnection(OraConnectionString);
            OraCmd.ExecuteNonQuery();
            OraCmd.Dispose();

        }

        public void SQLInsert(string sqlInsert)
        {
            // Open the DatabaseHandler Object
            DBConnectAndRunSQL DB = new DBConnectAndRunSQL();
            // get SQL query config
            string OraConnectionDSource = WebConfigurationManager.AppSettings["GMIDB_Conn"];
            string OraUserPass = "User Id=" + WebConfigurationManager.AppSettings["GMIDB_user"] + ";Password=" + WebConfigurationManager.AppSettings["GMIDB_pass"] + ";";
            string OraConnectionString = OraUserPass + OraConnectionDSource;

            // Set up command object
            OracleCommand cmdInsert = new OracleCommand();
            cmdInsert.CommandText = sqlInsert;
            // Open the connection for the command and give it the connection object
            cmdInsert.Connection = DB.OpenConnection(OraConnectionString);
            cmdInsert.ExecuteNonQuery();
            cmdInsert.Dispose();

        }
        public void SQLArchive(string sqlArchive)
        {
            //  ARCHIVES A RECORD FROM THE BLOCKING TABLE TO THE ARHCIVE TABLE

            // Open the DatabaseHandler Object
            DBConnectAndRunSQL DB = new DBConnectAndRunSQL();
            // get SQL query config
            string OraConnectionDSource = WebConfigurationManager.AppSettings["GMIDB_Conn"];
            string OraUserPass = "User Id=" + WebConfigurationManager.AppSettings["GMIDB_user"] + ";Password=" + WebConfigurationManager.AppSettings["GMIDB_pass"] + ";";
            string OraConnectionString = OraUserPass + OraConnectionDSource;

            // Set up command object
            OracleCommand cmdInsert = new OracleCommand();
            cmdInsert.CommandText = sqlArchive;
            // Open the connection for the command and give it the connection object
            cmdInsert.Connection = DB.OpenConnection(OraConnectionString);
            cmdInsert.ExecuteNonQuery();
            cmdInsert.Dispose();

        }        
        
        public List<GMI_TRANS_BLOCKRecord> RecsAsList(string sqlSELECTQuery)
        {
            List<GMI_TRANS_BLOCKRecord> DataSet = new List<GMI_TRANS_BLOCKRecord>();
            // Open the DatabaseHandler Object
            DBConnectAndRunSQL DB = new DBConnectAndRunSQL();
            // get SQL query config
            string OraConnectionDSource = WebConfigurationManager.AppSettings["GMIDB_Conn"];
            string OraUserPass = "User Id=" + WebConfigurationManager.AppSettings["GMIDB_user"] + ";Password=" + WebConfigurationManager.AppSettings["GMIDB_pass"] + ";";
            string OraConnectionString = OraUserPass + OraConnectionDSource;

            OracleDataReader reader = DB.GetResultsAsReader(sqlSELECTQuery, DB.OpenConnection(OraConnectionString));
            if (reader.HasRows)
                Status.Print(false,true,"DEBUG:  ", "Reader has Lines: " +reader.HasRows);
            else
                // how to handle when no rows are returned?
                Status.Print(false, true, "DEBUG:  ", "Returns No Results: " + !reader.HasRows);

            //read the SQL results into the GMIREFRecord Object and add the object to a list.
            while (reader.Read())
            {
                //TODO work out if null are being returned fix them 

                GMI_TRANS_BLOCKRecord record = new GMI_TRANS_BLOCKRecord();

                record.HOSTNAME = reader.GetOracleString(reader.GetOrdinal("HOSTNAME"));
                record.GMI_INITIATING_PROCESS = reader.GetOracleString(reader.GetOrdinal("FEED"));
                record.BLOCKING = reader.GetOracleString(reader.GetOrdinal("BLOCKING"));
                record.USER_UPDATED = reader.GetOracleString(reader.GetOrdinal("USER_UPDATED"));
                record.DATE_UPDATED = reader.GetOracleDate(reader.GetOrdinal("DATE_UPDATED"));
                DataSet.Add(record);

            }
            return DataSet;
        }
    }
}
