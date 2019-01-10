using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using Oracle.DataAccess;
using webtransformationadmin.Domain;
using webtransformationadmin.DAL;
using System.Security.Cryptography;

namespace webtransformationadmin.UI
{
    public partial class Default1 : System.Web.UI.Page
    {
        string strBLOCKED = WebConfigurationManager.AppSettings["FEED_BLOCKED_string"];
        string strAddAction = WebConfigurationManager.AppSettings["ACTION_BLOCK_string"];
        string strRemAction = WebConfigurationManager.AppSettings["ACTION_UNBLOCK_string"];
        string IMGURLFiltersShow = WebConfigurationManager.AppSettings["IMGURLFiltersShow"];
        string IMGURLFiltersHide = WebConfigurationManager.AppSettings["IMGURLFiltersHide"];
        string GTMAdmins = WebConfigurationManager.AppSettings["GTMAdmins"];
        int RetransSleepTime = Convert.ToInt32(WebConfigurationManager.AppSettings["retransformSleep"]);
        sqlGMITRANS sqlGMITRANS=new sqlGMITRANS();
        List<string> gridRowsShown = new List<string> {"20", "30", "100" };
        List<string> lstSeverity = new List<string> { "FATAL", "CRITICAL", "MAJOR", "MINOR", "WARNING", "CLEAR" };
        // Set up the object to print msgs
        StatusMsg Status = new StatusMsg();
        User LoginUser = new User();
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Initial Page load

                // SSO Authenticate
                SAFESSO();
                Status.Print(false, true, "Login:  ", LoginUser.Forename + " " + LoginUser.Surname + " " + "[" + LoginUser.ID + "] has logged on to page");

                //Admin users can additionally see the Delete GMI_REF Record button see web.config GTMAdmins
                if (GTMAdmins.Contains(LoginUser.ID)){ 
                    btnGMI_REF_DeleteRecord.Visible=true;
                    btnGMI_REF_DeleteRecord.Enabled=true;
                    lblProcessID.Visible = true;
                }

                // Set initial Filter image state
                imgButShowAdvFilters.ImageUrl = IMGURLFiltersShow;

                // CHECK DATABASE CONNECTION IS OK HERE


                // Populate drop down controls.
                // Maybe get this value from a cookie.
                ddListPageRows.DataSource = gridRowsShown;
                ddListPageRows.DataBind();
                ddListPageRows.SelectedItem.Value = gridRowsShown[0];
                ddListPageRows.SelectedIndex = 0;

                // Get gmi_status as list of values and add a blank unselected at the top
                lov_GMI_STATUS lovGMISTATUS = new lov_GMI_STATUS();
                ddLGMISTATUS.DataSource = lovGMISTATUS.getLOV();
                ddLGMISTATUS.DataBind();
                ddLGMISTATUS.Items.Insert(0, new ListItem(String.Empty, String.Empty));
                ddLGMISTATUS.SelectedIndex = 0;

                // Get gmi_environment as list of values and add a blank unselected at the top
                lov_GMI_ENVIRONMENT lovGMI_ENV = new lov_GMI_ENVIRONMENT();
                ddLENVIRONMENT.DataSource = lovGMI_ENV.getLOV();
                ddLENVIRONMENT.DataBind();
                ddLENVIRONMENT.Items.Insert(0, new ListItem(String.Empty, String.Empty));
                ddLENVIRONMENT.SelectedIndex = 0;

                //Get Feeds as list of values
                lov_FEED lov_FEEDS = new lov_FEED();
                ddlFEED.DataSource = lov_FEEDS.getLOV();
                ddlFEED.DataBind();
                ddlFEED.Items.Insert(0, new ListItem(String.Empty, String.Empty));
                ddlFEED.SelectedIndex = 0;

                ddlEventSeverity.Items.Insert(0, lstSeverity[0]);
                ddlEventSeverity.Items.Insert(1, lstSeverity[1]);
                ddlEventSeverity.Items.Insert(2, lstSeverity[2]);
                ddlEventSeverity.Items.Insert(3, lstSeverity[3]);
                ddlEventSeverity.Items.Insert(4, lstSeverity[4]);
                ddlEventSeverity.Items.Insert(5, lstSeverity[5]);
                ddlEventSeverity.SelectedIndex = 4;
            }
            else
            {

            }

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {

            //set the sql query as a property of the data table
            //dataTableProps DT = new dataTableProps();
            string sqlSelect = WebConfigurationManager.AppSettings["GMI_TRANS_select"];
            string sqlFrom = WebConfigurationManager.AppSettings["GMI_TRANS_from"];
            string sqlJoinsubSelect = WebConfigurationManager.AppSettings["GMI_TRANS_joinview"];
            string sqlWhere = WebConfigurationManager.AppSettings["GMI_TRANS_where"];
            //string sqlGroupBy = WebConfigurationManager.AppSettings["GMI_TRANS_groupby"];
            string sqlOrderBy = WebConfigurationManager.AppSettings["GMI_TRANS_orderby"];


            // Modify Query for Host search
            if (txtSearchHost.Text.Length > 0) { sqlJoinsubSelect = sqlJoinsubSelect.Replace("/*INNER_WHERE_MODIFIER*/", " AND HOSTNAME like '%" + ToOracleWildcard(txtSearchHost.Text.ToUpper()) + "%' /*INNER_WHERE_MODIFIER*/ "); }
            // Modify Query for Advanced Search options INF/CAP/GMI_STATUS/ENVIRON 
            if (txtSearchINF.Text.Length > 0) { sqlJoinsubSelect = sqlJoinsubSelect.Replace("/*INNER_WHERE_MODIFIER*/", "AND GMI_INFRASTRUCTURE like '%" + ToOracleWildcard(txtSearchINF.Text.ToUpper()) + "%' /*INNER_WHERE_MODIFIER*/"); }
            if (txtSearchCAP.Text.Length > 0) { sqlJoinsubSelect = sqlJoinsubSelect.Replace("/*INNER_WHERE_MODIFIER*/", "AND GMI_CAPABILITY like '%" + ToOracleWildcard(txtSearchCAP.Text.ToUpper()) + "%' /*INNER_WHERE_MODIFIER*/ "); }
            if (ddLGMISTATUS.SelectedIndex > 0) { sqlJoinsubSelect = sqlJoinsubSelect.Replace("/*INNER_WHERE_MODIFIER*/", "AND GMI_STATUS ='" + ddLGMISTATUS.SelectedItem.Value + "' /*INNER_WHERE_MODIFIER*/ "); }
            if (ddLENVIRONMENT.SelectedIndex > 0) { sqlJoinsubSelect = sqlJoinsubSelect.Replace("/*INNER_WHERE_MODIFIER*/", "AND GMI_ENVIRONMENT ='" + ddLENVIRONMENT.SelectedItem.Value + "' /*INNER_WHERE_MODIFIER*/ "); }
            if (ddlFEED.SelectedIndex > 0) { sqlJoinsubSelect = sqlJoinsubSelect.Replace("/*INNER_WHERE_MODIFIER*/", "AND GMI_INITIATING_PROCESS='" + ddlFEED.SelectedItem.Value + "' /*INNER_WHERE_MODIFIER*/ "); }
            // Show only blocked records
            if (cckBoxShowOnlyBlocked.Checked == true) { sqlWhere = sqlWhere.Replace("/*WHERE_MODIFIER*/", "AND BLOCKING ='" + strBLOCKED + "' /*WHERE_MODIFIER*/ "); }

            // Concatenate parts into oquery
            sqlGMITRANS.Text = sqlSelect + sqlFrom + sqlJoinsubSelect + /*sqlJoinBlock + */ sqlWhere + sqlOrderBy;

            // store the SQL query on the page for now
            txtSQLQuery.Text = sqlGMITRANS.Text;

            //Update the grid view using the Datatable properties and rebind the datatable
            updateDataBoundTable(GridView_GMITRANSResults, sqlGMITRANS.Text);
            
            // Reveal the Gridview panel
            pnlGridview.Visible = true;

        }
        public string ToOracleWildcard(string Wildcarded)
        {
            // Convert to Oracle wildcards 
            string Cleaned = Wildcarded.Replace("*", "%");
            Cleaned = Cleaned.Replace("?", "_");
            //Remove leading or trailing wildcards
            Cleaned = Cleaned.TrimStart('%').TrimEnd('%');
            return Cleaned;
        }
        public string HostnameFromCartItem(string bulletstring)
        {
            // Get the Hostname and the feedname from the text of the bulleted list
            // Format HOSTNAME (FEED)
            // eg DTCP-GMIBFE01A (CSV)
            return bulletstring.Substring(0, bulletstring.IndexOf(" "));
        }
        public string FeednameFromCartItem(string bulletstring)
        {
            // Get the Hostname and the feedname from the text of the bulleted list
            // Format HOSTNAME (FEED)
            // eg DTCP-GMIBFE01A (CSV)
            return bulletstring.Substring(bulletstring.IndexOf("(") + 1, bulletstring.IndexOf(")") - bulletstring.IndexOf("(") - 1);
        }

        private void updateDataBoundTable(GridView GV, string sqlQuery)
        {
            // Debug
            Status.Print(false, true, "SQLQRY: ", "[" +txtUsername.Text + "]" + sqlQuery);
            List<GMI_TRANSRecord> dataset = new List<GMI_TRANSRecord>();
            GMI_TRANSRecordHandler handler = new GMI_TRANSRecordHandler();
            dataset = handler.GMI_TRANS_RecsAsList(sqlQuery);
            GV.DataSource = dataset;
            GV.PageSize = Convert.ToInt32(ddListPageRows.SelectedItem.Value);
            GV.DataBind();

            // Update the label for the number of records found
            lblDTRowCount.Text = dataset.Count.ToString();
            CommitControlsVisibility();
       }
        protected void GridView_GMITRANSResults_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView_GMITRANSResults.PageIndex = e.NewPageIndex;
            updateDataBoundTable(GridView_GMITRANSResults, txtSQLQuery.Text);
        }

        protected void ItemsGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            // Handle leaking into here from pagination changes
            string sCmdArg = e.CommandArgument.ToString(); 
            if (sCmdArg == "Last") { return; };
            if (sCmdArg == "First") { sCmdArg = "0"; };

            //Convert the row index stored in the CommandArgument property to an Integer.
            int index = Convert.ToInt32(sCmdArg);

            // Retrieve the row that contains the button clicked by the user from the Rows collection.
            GridViewRow row = GridView_GMITRANSResults.Rows[index];

            ListItem CommitSet = new ListItem();
            string Feedname = Server.HtmlDecode(row.Cells[0].Text) ;
            string Hostname = Server.HtmlDecode(row.Cells[1].Text) ;
            string Blockstate = Server.HtmlDecode(row.Cells[16].Text);
            //string HOSTINFO_URL = WebConfigurationManager.AppSettings["HOSTINFO_URLprefix"];
                        
            CommitSet.Text = Hostname + " (" + Feedname + ") ";
            CommitSet.Value = Hostname;
            
            //Clear away msgs
            lblCommitMsgs.Text = "";

            if (e.CommandName == "BLOCK" )
            {
                if ( Blockstate == strBLOCKED)
                {
                    // if we are not undoing from unblocking, message that its not appropriate for this list
                    if (!bulCommitUnBlock.Items.Contains(CommitSet))
                    {
                        // already blocked, don't add to list
                        lblCommitMsgs.Text = CommitSet.Text + " not added, it is already in this state";
                    }
                }
                else
                {
                    // Add to block list
                    if (!bulCommitBlock.Items.Contains(CommitSet))
                    {
                        bulCommitBlock.Items.Add(CommitSet);
                        lblCommitMsgs.Text = "Added";
                    }
                }
                // Remove from unblock list, can only be in one list
                if (bulCommitUnBlock.Items.Contains(CommitSet))
                {
                    bulCommitUnBlock.Items.Remove(CommitSet);
                    lblCommitMsgs.Text = "Removed";
                }
            }
            if (e.CommandName == "UNBLOCK")
            {
                if (Blockstate  != strBLOCKED)
                {
                    // if we are not undoing from blocking, message that its not appropriate for this list
                    if (!bulCommitBlock.Items.Contains(CommitSet))
                    {
                        // already unblocked, don't add to list
                        lblCommitMsgs.Text = CommitSet.Text + " not added, it is already in this state";
                    }
                }
                else
                {
                    // Add to block list
                    if (!bulCommitUnBlock.Items.Contains(CommitSet))
                    {
                        bulCommitUnBlock.Items.Add(CommitSet);
                        lblCommitMsgs.Text = "Added";
                    }
                }
                // Remove from block list, can only be in one list
                if (bulCommitBlock.Items.Contains(CommitSet))
                {
                    bulCommitBlock.Items.Remove(CommitSet);
                    lblCommitMsgs.Text = "Removed";
                }
            }
            if (e.CommandName == "RETRANSFORM")
            {

                // Add to retransform list
                if (!bulReTransform.Items.Contains(CommitSet))
                {
                    bulReTransform.Items.Add(CommitSet);
                    lblCommitMsgs.Text = "Added";
                }
                else
                {
                    bulReTransform.Items.Remove(CommitSet);
                    lblCommitMsgs.Text = "Removed";


                }
            }
            if (e.CommandName == "GETGMI_REF")
            {
                lblGMI_REFHost.Text = Hostname;
                lblProcessID.Text = Feedname;
                pnlGMIREF.Visible = true;
                btnRefreshGMIREF_Click(sender, e);

            }
            
            // Update the visibility of controls.
            CommitControlsVisibility();
        }

        private void updateGridviewGMI_REF(GridView GV, string sqlQuery)
        {
            // Debug
            Status.Print(false, true, "SQLQRY: ", "[" + txtUsername.Text + "]" + sqlQuery);
            List<GMI_TRANSRecord> dataset = new List<GMI_TRANSRecord>();
            GMI_TRANSRecordHandler handler = new GMI_TRANSRecordHandler();
            dataset = handler.GMI_TRANS_RecsAsList(sqlQuery);
            GV.DataSource = dataset;
            GV.DataBind();

            // Update the label for the number of records found
            lblDTRowCount.Text = dataset.Count.ToString();
            CommitControlsVisibility();

        }
        protected void CommitControlsVisibility()
        {
            // Show or hide stuff if data exists or there is a set of records to commit.
            bool GridviewDataExists;
            bool PanelDataExists;
            bool CommitSetExists;
            bool ReTransformSetExists;

            // Control visibility of the Grid View
            if (GridView_GMITRANSResults.Rows.Count > 0)
            {
                GridviewDataExists = true;
            }
            else
            {
                GridviewDataExists = false;
            }
            ddListPageRows.Visible = GridviewDataExists;
            GridView_GMITRANSResults.Visible = GridviewDataExists;

            // Control restransformation button
            if (bulReTransform.Items.Count > 0 )
            {
                ReTransformSetExists = true;
            }
            else
            {
                ReTransformSetExists = false;
            }
            btnReTransform.Enabled = ReTransformSetExists;

            // Control Panel visibility
            if (bulCommitBlock.Items.Count > 0 || bulCommitUnBlock.Items.Count > 0 || bulReTransform.Items.Count > 0 || lblCommitMsgs.Text.Length > 0)
            {
                PanelDataExists = true;
            }
            else
            {
                PanelDataExists = false;
            }
            panelCommit.Visible = PanelDataExists;
            

            // Control Commit button
            if (bulCommitBlock.Items.Count > 0 || bulCommitUnBlock.Items.Count > 0)
            {
                CommitSetExists = true;
            }
            else
            {
                CommitSetExists = false;
            }
            btnCommitChanges.Enabled = CommitSetExists;
        }


        protected void ImgBtnIntroToggle_Click(object sender, ImageClickEventArgs e)
        {
            // Use help button to toggle visbility of Intro Text
            pnlIntroText.Visible = !pnlIntroText.Visible;
            if (pnlIntroText.Visible)
            {

                ImgBtnIntroToggle.ImageUrl = "~/Images/Helphide24x24p.png";
            }
            else
            {
                ImgBtnIntroToggle.ImageUrl = "~/Images/Help24x24p.png";
            }
                
        }

       
        protected void SAFESSO()
        {
            string sso_key = WebConfigurationManager.AppSettings["sso_key"];
            string sso_url = WebConfigurationManager.AppSettings["sso_url"];

            // Try to get post data
            var SSODisposition = HttpContext.Current;
            string digest = SSODisposition.Request["digest"];

            if (Utility.IsNullOrEmpty(digest))
            {
                // Go to SAFE login
                Response.Redirect(sso_url);
            }
            // Get back from SAFE LOGIN, get POST afresh
            digest = SSODisposition.Request["digest"];
            LoginUser.ID = SSODisposition.Request["uid"];
            LoginUser.Forename = SSODisposition.Request["firstname"];
            LoginUser.Surname = SSODisposition.Request["lastname"];
            string Time = SSODisposition.Request["Time"];

            //Create a Hex hash from our own key to compare with the posted Digest
            MD5 md5 = new MD5CryptoServiceProvider();
            // built from usid time concatenated with key.
            string sPassword = LoginUser.ID + Time + sso_key;

            // Logging
            Status.Print(false, true, "digest: ", digest);

            // Create hash
            byte[] pwdHash = md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(sPassword));
            StringBuilder pwdHex = new StringBuilder();
            for (int i = 0; i < pwdHash.Length; i++)
            {
                pwdHex.Append(pwdHash[i].ToString("x2"));
            }

            //Compare to the POSTed digest
            Status.Print(false, true, "pwdHex: ", pwdHex.ToString());
            if (digest == pwdHex.ToString())
            {
                Status.Print(false, true, "SAFE:   ", LoginUser.ID +" Authenticated SUCCESS");
                txtUsername.Enabled = false;
            }
            else
            {
                Status.Print(false, true, "SAFE:   ", LoginUser.ID + " Authenticated Failed");
            }
            txtUsername.Text = LoginUser.ID;
            txtForename.Text = LoginUser.Forename;
            txtSurname.Text = LoginUser.Surname;
        }
        protected void btnCommitChanges_Click(object sender, EventArgs e)
        {
            string Username = txtUsername.Text;

            // Create one list
            // Commit the changes to the database.
            GMI_TRANS_BLOCKRecordHandler GMI_TRANS_BLOCK = new GMI_TRANS_BLOCKRecordHandler();

            foreach (ListItem item in bulCommitBlock.Items)
            {
                if (!bulReTransform.Items.Contains(item))
                {
                    bulReTransform.Items.Add(item);
                }
                // Get the Hostname and the feedname from the text of the bulleted list
                string Host = HostnameFromCartItem(item.Text);
                string Feed = FeednameFromCartItem(item.Text);
                GMI_TRANS_BLOCK.PLSQLExec(Host, Feed, strAddAction, txtUsername.Text);
            }

            foreach (ListItem item in bulCommitUnBlock.Items)
            {
                if (!bulReTransform.Items.Contains(item))
                {
                    bulReTransform.Items.Add(item);
                }
                // Get the Hostname and the feedname from the text of the bulleted list
                string Host = HostnameFromCartItem(item.Text);
                string Feed = FeednameFromCartItem(item.Text);
                GMI_TRANS_BLOCK.PLSQLExec(Host, Feed, strRemAction, txtUsername.Text);

            }

            // Empty the Commit set
            bulCommitBlock.Items.Clear();
            bulCommitUnBlock.Items.Clear();
            lblCommitMsgs.Text = "Changes committed to the database";
            // Refresh the gridview data
            btnSearch_Click(sender, e);
            // Recalculate the control visibility\enablement
            CommitControlsVisibility();
        }
        protected void btnReTransform_Click(object sender, EventArgs e)
        {
            foreach (ListItem item in bulReTransform.Items)
            {
                
                // Get the Hostname and the feedname from the text of the bulleted list
                string Host = HostnameFromCartItem(item.Text);
                string Feed = FeednameFromCartItem(item.Text);
                GMI_TRANSRecordHandler GMI_TRANS = new GMI_TRANSRecordHandler();
                try
                {
                    GMI_TRANS.PLSQLExecRetransformation(Host, txtUsername.Text);
                }
                catch (Oracle.DataAccess.Client.OracleException Oe)
                {
                    ExceptionMsg(Oe.ErrorCode.ToString());
                }
                finally { }

                System.Threading.Thread.Sleep(RetransSleepTime);
            }
            // Empty the Retransform set OR RELACE IT WITH LINK to AV?
            bulReTransform.Items.Clear();
            lblCommitMsgs.Text = "Transformation initiated. Refresh Search in 60 secs to observe new transform time";
            CommitControlsVisibility();
        }
        protected void ExceptionMsg(string sException)
        {
            pnlErrMsg.Visible = true;
            txtExceptionsBox.Text = sException;
            CreateLogFiles Log = new CreateLogFiles();
            Log.WriteLog(sException);
        }
        protected void imgButShowAdvFilters_Click(object sender, ImageClickEventArgs e)
        {
            // Control Adv Search options Panel visibilty
            cckBoxAdvSearch.Checked = !cckBoxAdvSearch.Checked;
            pnlAdvSearch.Visible = cckBoxAdvSearch.Checked;
            if (cckBoxAdvSearch.Checked == true)
            {
                imgButShowAdvFilters.ImageUrl = IMGURLFiltersHide;
            }
            else 
            { 
                imgButShowAdvFilters.ImageUrl = IMGURLFiltersShow;
            }
        }

        protected void btnReturnGMIREF_Click(object sender, EventArgs e)
        {
            pnlGMIREF.Visible = false;
            pnlEventSent.Visible = false;
            CommitControlsVisibility();
        }

        protected void btnRefreshGMIREF_Click(object sender, EventArgs e)
        {
            GridView_GMI_REF.DataSource = null;
            GridView_GMI_REF.DataBind();
           // get the GMI_REF data for the host and bind it to the GMI_REF grid view
            string sqlGMI_REF = WebConfigurationManager.AppSettings["GMI_REF_select"] + "'" + lblGMI_REFHost.Text + "'";

                List<GMI_REFRecord> dataset = new List<GMI_REFRecord>();
                GMI_REFRecordHandler handler = new GMI_REFRecordHandler();
                dataset = handler.GMI_REF_RecsAsList(sqlGMI_REF);
                GridView_GMI_REF.DataSource = dataset;
                GridView_GMI_REF.DataBind();

                // cleanup
                dataset.Clear();

                // Hide the current data
                panelCommit.Visible = false;
                
        }


        protected void GridView_GMI_REF_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            // Handle leaking into here from pagination changes
            string sCmdArg = e.CommandArgument.ToString();
            if (sCmdArg == "Last") { return; };
            if (sCmdArg == "First") { sCmdArg = "0"; };

            //Convert the row index stored in the CommandArgument property to an Integer.
            int index = Convert.ToInt32(sCmdArg);

            // Retrieve the row that contains the button clicked by the user from the Rows collection.
            GridViewRow row = GridView_GMI_REF.Rows[index];

            TestEvent ev = new TestEvent();
            // Populate key values (colheader,cell.Text) for event object
            string webServer = System.Environment.MachineName;
            ev.Feedname = Server.HtmlDecode(row.Cells[0].Text);
            ev.Hostname = " Node=\"" + Server.HtmlDecode(row.Cells[1].Text) + "\"";
            ev.SEVERITY = " Severity=\"" + ddlEventSeverity.SelectedValue +"\"";
            ev.SEVCOLOR = WebConfigurationManager.AppSettings[ddlEventSeverity.SelectedValue];
            
            ev.EventKey = " EventKey=\"" + WebConfigurationManager.AppSettings["ThisAppName"] + "\"";
            ev.EventGroup = " EventGroup=\"" + WebConfigurationManager.AppSettings["postemsg_EventGroup"] + "\"";
            ev.EventClass = " " + WebConfigurationManager.AppSettings["postemsg_EventClass"]; // GenericEvent
            ev.EventSource = " " + WebConfigurationManager.AppSettings["ThisAppName"]; //GTM
            ev.MESSAGE = "\"SAFE User " + txtUsername.Text + " initiated GTM Test Event from " + webServer + " on behalf of " + webServer + " (" + ev.Feedname + ")\"";
            
            if (e.CommandName == "SendEvent")
            {
                PostemsgHandler postemsg = new PostemsgHandler();
                postemsg.SendTestEvent(ev);
            }
            // fill out on-screen Event verification
            lblEventTimeStamp.Text = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString();
            lblEventHostname.Text = ev.Hostname;
            lblEventSummary.Text = ev.MESSAGE;
            lblEventSeverity.Text = ev.SEVERITY;
            pnlEventSent.Visible = true;
            tblEventSent.BackColor = System.Drawing.ColorTranslator.FromHtml(ev.SEVCOLOR);
            // Update the visibility of controls.
            CommitControlsVisibility();
        }


        protected void imgSendTestEventHelp_Click(object sender, ImageClickEventArgs e)
        {
            pnlSendTestEventHelp.Visible = !pnlSendTestEventHelp.Visible;
        }

        protected void btnGMI_REF_DeleteRecord_Click1(object sender, EventArgs e)
        {
            Status.Print(false, true, "AUDIT:   ", LoginUser.ID + " Deleting GMI_REF Record" + lblGMI_REFHost.Text +" "+ lblProcessID.Text +" "+ LoginUser.ID);
            // Get the Hostname from the panel lable text and pass this user name 
            GMI_REFRecordHandler GMI_REF = new GMI_REFRecordHandler();
            try
            {
                GMI_REF.PRC_ARCHIVE_DEVICE(lblGMI_REFHost.Text, lblProcessID.Text, LoginUser.ID);
            }
            catch (Oracle.DataAccess.Client.OracleException Oe)
            {
                ExceptionMsg(Oe.ErrorCode.ToString());
            }
            finally { }
            // refresh the gridview
            btnRefreshGMIREF_Click(sender, e);
        }

    }

}
