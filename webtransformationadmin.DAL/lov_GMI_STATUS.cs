using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Configuration;
using Oracle.DataAccess;

namespace webtransformationadmin.DAL
{
    public class lov_GMI_STATUS
    {
        public List<string> getLOV ()
        {
            // returns the possible list of values for the GMI_STATUS (COMMSSIONED, INSTALLED, DECOMMISSIONED etc)
            string OraConnectionDSource = WebConfigurationManager.AppSettings["GMIDB_Conn"];
            string OraUserPass = "User Id=" + WebConfigurationManager.AppSettings["GMIDB_user"] + ";Password=" + WebConfigurationManager.AppSettings["GMIDB_pass"] + ";";
            string OraConnectionString = OraUserPass + OraConnectionDSource;
            string sql_lov = WebConfigurationManager.AppSettings["sqlGetGMI_STATUS_LOV"];

            DBConnectAndRunSQL handler = new DBConnectAndRunSQL();

            // Create a list
            List<string> lstLOV = new List<string>();
            lstLOV = handler.GetResultsAsList(handler.GetResultsAsReader(sql_lov, handler.OpenConnection(OraConnectionString)));
            return lstLOV;
        }
    }
}
