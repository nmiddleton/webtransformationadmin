using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using webtransformationadmin.Domain;
using webtransformationadmin.DAL;

namespace webtransformationadmin.Test
{
    class TestDB
    {
        StatusMsg Status = new StatusMsg();
        public void TestDBConnection()
        {
            string sqlTEST = "select * from GMI_TRANS";
            string OraConnectionString = "(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=DTCP-GMIGDBC01)(PORT=1521))(CONNECT_DATA=(SID=GMIDPROD)))";
            DBConnectAndRunSQL handler = new DBConnectAndRunSQL();

            // As a list of strings
            List<string> listSQLResults = handler.GetResultsAsList(handler.GetResultsAsReader(sqlTEST, handler.OpenConnection(OraConnectionString)));

            // Get the properties of the connection for the log and to close it.
            DBConnectLog DBLog = handler.DBLog;

            if (!DBLog.Successful)
            {
                Status.Print(true, true, "DBConnection", DBLog.LogError);
            }
            else
            {
                // Close the connection
                handler.CloseConnection(DBLog.connection);
                Status.Print(false, true, "DBConnection", DBLog.LogInfo);
            }
        }
    }
}
