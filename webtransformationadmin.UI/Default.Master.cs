// 
//                  $Id: Default.Master.cs 26960 2013-11-07 14:39:20Z neil.middleton $
//     $LastChangedDate: 2013-11-07 14:39:20 +0000 (Thu, 07 Nov 2013) $
// $LastChangedRevision: 26960 $
//       $LastChangedBy: neil.middleton $
//             $HeadURL: https://sami.cdt.int.thomsonreuters.com/svn/gms_gmi/trunk/infrastructure/tools/webtransformationadmin/webtransformationadmin.UI/Default.Master.cs $


using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;

public partial class DefaultMasterPage : MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        TitleContentPlaceHolder.Page.Title = WebConfigurationManager.AppSettings["ThisAppName"];

        if (!this.IsPostBack)
        {
            //VersionLabel.Text = string.Format("#{0}", webpasswordtool.Common.Configuration.Version);
        }
    }
}
