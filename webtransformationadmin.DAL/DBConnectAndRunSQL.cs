// 
//                  $Id: DBConnectAndRunSQL.cs 26235 2013-06-21 11:38:39Z neil.middleton $
//     $LastChangedDate: 2013-06-21 12:38:39 +0100 (Fri, 21 Jun 2013) $
// $LastChangedRevision: 26235 $
//       $LastChangedBy: neil.middleton $
//             $HeadURL: https://sami.cdt.int.thomsonreuters.com/svn/gms_gmi/trunk/infrastructure/tools/webtransformationadmin/webtransformationadmin.DAL/DBConnectAndRunSQL.cs $

using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oracle.DataAccess.Client; //Add ref to C:\app\Administrator\product\11.2.0\client_1\odp.net\bin\2.x
using webtransformationadmin.Domain;

namespace webtransformationadmin.DAL
{
    public class DBConnectAndRunSQL
    {
        public DBConnectLog DBLog = new DBConnectLog();
        public DBConnectLog GetLog()
        {
            return DBLog;
        }

        public OracleConnection OpenConnection( string connectionString )
        {
            // takes a query and a connection string and returns a reader full of the data.
            OracleConnection connection = new OracleConnection();
            connection.ConnectionString = connectionString;
            
            // Save state to properties
            DBLog.OracConnectionOpen = false;
            DBLog.connection = connection;
            DBLog.LogInfo += "OPENING: Opening Connection";

            try { connection.Open(); }
            catch (SystemException e) { 
                DBLog.LogError += "Failed to connect to Database. exception: " + e;
                DBLog.Successful = false;
                DBLog.OracConnectionOpen = false;
            }

            DBLog.LogInfo += "OPENING: Oracle Connection State: " + connection.State;
            if (connection.State.ToString() != "Open")
            {
                DBLog.Successful = false;
                DBLog.OracConnectionOpen = false;
                throw new Exception("Error: The Oracle connection has been broken");
            }
            // Save the state to properties for external use.
            DBLog.OracConnectionOpen = true;
            DBLog.connection = connection;

            return connection;
        }

        public void CloseConnection( OracleConnection connection )
        {
            // takes a query and a connection string and returns a reader full of the data.
            DBLog.LogInfo += "CLOSING: Closing Connection";
            if (connection.State.ToString() != "Open") return;
            connection.Close();
            DBLog.LogInfo += "CLOSING: Oracle Connection State: " + connection.State;
        }

        public OracleDataReader GetResultsAsReader(string sqlquery, OracleConnection connection)
        {
             // Prepare and run the SQL command
            OracleCommand command = connection.CreateCommand();
            command.CommandText = sqlquery;
            DBLog.LogInfo += "Running SQL:" + sqlquery;
            // Create a reader for the results of the SQL Command
            OracleDataReader reader = command.ExecuteReader();
            return reader;           
        }
        public DataTable GetResultsAsDataTable(OracleDataReader reader)
        {
            // Create a datatable for the results in the reader
            DBLog.LogInfo += "Creating  a datatable for the results in the reader";
            DataTable dT = new DataTable();
            dT.Load(reader);
            return dT;
        }
        public List<string> GetResultsAsList(OracleDataReader reader)
        {
            // Create a  List<> for the results in the reader
            DBLog.LogInfo += "Creating a List<> for the results in the reader";
            List<string> resultList = new List<string>();
            while (reader.Read()){
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    resultList.Add(reader[i].ToString());

                }
            }

            return resultList;
        }

        public void SQLExecute(OracleCommand sqlExec)
        {
            // Prepare and run the SQL command
            
            DBLog.LogInfo += "SQLEXEC: " + sqlExec;
            sqlExec.ExecuteNonQuery();
        }
        
    }
}
