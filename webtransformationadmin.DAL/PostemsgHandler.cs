using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using webtransformationadmin.Domain;

namespace webtransformationadmin.DAL
{
    public class PostemsgHandler 
    {
        StatusMsg Status = new StatusMsg();

        public void SendTestEvent ( TestEvent ev) 
        {
            // Build the postemsg command
            string postemsg_exe = WebConfigurationManager.AppSettings["postemsg_home"] + WebConfigurationManager.AppSettings["postemsg_exe"];
            string postemsg_arg =  " -f " +  WebConfigurationManager.AppSettings["postemsg_home"] +  WebConfigurationManager.AppSettings["postemsg_cfg"]
                                + " -m " + ev.MESSAGE
                                + " " + ev.Hostname
                                + " " + ev.SEVERITY
                                + " " + ev.EventType
                                + " " + ev.EventGroup
                                + " " + ev.EventKey
                                + " " + ev.EventClass
                                + " " + ev.EventSource;
            //Log
            Status.Print(false,true, "RunProcess: ", postemsg_exe + postemsg_arg);

            // Call the Shell class
            RunShellProcess RunCmd = new RunShellProcess();
            string _Output = null;
            string _Error = null;
            RunCmd.Execute(postemsg_exe, postemsg_arg, ref _Output, ref _Error);
            Status.Print(false,true, "RunProcess: ", "Output[" + _Output + "] Err[" + _Error + "]");
        }
    }
}
