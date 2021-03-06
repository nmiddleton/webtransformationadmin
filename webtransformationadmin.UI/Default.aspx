﻿<%@ Page Title="GMI Transformation Management" Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="webtransformationadmin.UI.Default1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
    <div>
        <table style="width:100%;">
            <tr>
                <td style="font-size: large; width: 613px;">
                    <table style="width:100%;">
                        <tr>
                            <td>
                    <strong>GMI Transformation Management</strong></td>
                            <td align="right">
                    <asp:Label ID="txtForename" runat="server" Text=""> </asp:Label>&nbsp;<asp:Label ID="txtSurname" runat="server" Text=""></asp:Label> &nbsp;[<asp:Label ID="txtUsername" runat="server" Text=""></asp:Label>]</td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="width: 600px"> &nbsp;</td>
            </tr>
        </table>
        Welcome. Use this page to manage hosts through GMI transformation. 
    <asp:ImageButton ID="ImgBtnIntroToggle" runat="server" 
        ImageUrl="~/Images/Help24x24p.png" onclick="ImgBtnIntroToggle_Click" 
            TabIndex="12" />
    <br />
    <br />
    <asp:Panel ID="pnlIntroText" runat="server" BorderStyle="Solid" 
        BorderWidth="1px" Visible="False" HorizontalAlign="Justify" 
            DefaultButton="ImgBtnIntroToggle" Width="800px">
        This page is used to block or free up host records transforming in GMI Database. 
        You can also initiate an immediate retransformation for a host.&nbsp;
        <table width="800">
            <tr>
                <td>
                    Hosts records transform in order of priority
                    <br />
                    1. TOM
                    <br />
                    2. CSV
                    <br />
                    3. SM (Service Manager)</td>
            </tr>
            <tr>
                <td>
                    Issues arise when a host has been entered by CSV method and then via Service 
                    Manager. The priority order means the Service Manager record (and subsequent 
                    updates) will be ignored while there is a CSV record in place. E.g.
                    <br />
                    <span style="font-family: 'Lucida Console'">TOM&nbsp;&nbsp;&nbsp;&nbsp; -&gt;
                    <br />
                    CSV&nbsp;&nbsp;&nbsp;&nbsp; -&gt; DTCP-GMIADSC01,DCM,GMIS. . .&nbsp; -&gt; transformed -&gt; GMI_REF (source=<span 
                        style="color: #009933"><strong>CSV</strong></span>)
                    <br />
                    SM&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; -&gt; DTCP-GMIADSC01,DCM,GMIS. . .&nbsp; -&gt; outranked </span>
                    <br />
                    <br />
                    <br />
                    Use this page to block the CSV record (or any and all records for a host!). By 
                    blocking the CSV record it is no longer considered at the time of 
                    transformation.
                    <br />
                    <span style="font-family: 'Lucida Console'">TOM&nbsp;&nbsp;&nbsp;&nbsp; -&gt;
                    <br />
                    CSV&nbsp;&nbsp;&nbsp;&nbsp; -&gt; DTCP-GMIADSC01,DCM,GMIS. . .&nbsp; -&gt; blocked
                    <br />
                    SM&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; -&gt; DTCP-GMIADSC01,DCM,GMIS. . .&nbsp; -&gt; transformed -&gt; GMI_REF (source=<span 
                        style="color: #009933"><strong>SM</strong></span>) </span>
                    <br />
                    <br />
                    You can also rerun that transformation here immediately. Within seconds the 
                    GMI_REF record will be updated accordingly.
                    <br />
                    <br />
                    Selections for blocking or retransformation appear in the “Action Cart” at the 
                    bottom of the page, where they collect until you decide to commit the action on 
                    them. Here there is also a link to the GMI_REF data for the host via agent 
                    viewer.
                    <br />
                    <br />
                    <span style="text-decoration: underline"><strong>Other facts: </strong></span>&nbsp;<br /> 
                    • Blocking occurs on a per host basis only, though you can block and transform 
                    multiple records at a time.
                    <br />
                    • Blocking all hosts records (CSV and TOM and SM) blocks all future host 
                    transformation so be careful.
                    <br />
                    • You need to be logged into SAFE to use this tool and the actions are recorded 
                    against SAFE user id</td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    For more information about GMI transformation, refer to our
                    <asp:HyperLink ID="HREFGMI_Home" runat="server">hub page</asp:HyperLink>
                    . (click the help icon again to minimise this panel)<br /></td>
            </tr>
        </table>
     </asp:Panel>

             Search for systems using these Filters: <br />
        <asp:Panel ID="pnlBasicSearch" runat="server" DefaultButton="btnSearch">
        <br />
        <table>
            <tr>
                <td style="width: 170px">Search on Hostname</td><td style="width: 114px">
                <asp:TextBox ID="txtSearchHost" runat="server" Width="170px" ></asp:TextBox> </td>
                <td>
                    <asp:Button ID="btnSearch" runat="server" onclick="btnSearch_Click" 
                        Text="Search" Font-Bold="True" Height="26px" Width="94px" />
                </td>
                <td>
                    <asp:ImageButton ID="imgButShowAdvFilters" runat="server" 
                        onclick="imgButShowAdvFilters_Click" ImageUrl="~/Images/FiltersShow.bmp" />
                </td>
                <td>
                    <asp:CheckBox ID="cckBoxAdvSearch" runat="server" AutoPostBack="True" 
                        Text="Show Advanced search " Visible="False" />
                </td>
            </tr>
        </table>
        </asp:Panel>
             <asp:Panel ID="pnlAdvSearch" runat="server" Visible="False">
                 <asp:Image ID="imgAdvSearchDivider0" runat="server" ImageUrl="~/Images/FiltersDivider.bmp" />
             <table>
            <tr>
                <td style="width: 170px">Status</td><td style="width: 170px">
                <asp:DropDownList ID="ddLGMISTATUS" runat="server" >
                </asp:DropDownList>
                </td>
                <td style="width: 170px">
                    Infrastructure Short Code</td>
                <td style="width: 180px">
                    <asp:TextBox ID="txtSearchINF" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 170px">
                    Environment</td>
                <td style="width: 170px">
                    <asp:DropDownList ID="ddLENVIRONMENT" runat="server" >
                    </asp:DropDownList>
                </td>
                <td style="width: 170px">
                    Capability Short code</td>
                <td style="width: 180px">
                    <asp:TextBox ID="txtSearchCAP" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 170px; height: 20px;">
                    Feed Name</td>
                <td style="width: 170px; height: 20px;">
                    <asp:DropDownList ID="ddlFEED" runat="server">
                    </asp:DropDownList>
                </td>
                <td style="width: 170px; height: 20px;">
                    Show Only Blocked Systems</td>
                <td style="width: 180px; height: 20px;">
                    <asp:CheckBox ID="cckBoxShowOnlyBlocked" runat="server" />
                </td>
            </tr>
            </table>
                 <asp:Image ID="imgAdvSearchDivider1" runat="server" ImageUrl="~/Images/FiltersDivider.bmp" />
             </asp:Panel>

        &nbsp;&nbsp;&nbsp; 
        <br />
      </div>
    <asp:Panel ID="pnlGridview" runat="server" Visible="False">
                <asp:GridView ID="GridView_GMITRANSResults" runat="server" 
            CellPadding="2" AllowSorting="True" BackColor="White" BorderColor="#CC9966" 
                    BorderStyle="None" BorderWidth="1px" Font-Names="Arial" 
                    Font-Size="14px" AllowPaging="True" 
    onpageindexchanging="GridView_GMITRANSResults_PageIndexChanging" 
        OnRowCommand="ItemsGridView_RowCommand" AutoGenerateColumns="False" 
        GridLines="None" CellSpacing="1" 
         >
                    <FooterStyle BackColor="#FFC745" ForeColor="Black" />
                    <HeaderStyle BackColor="#FFC745" Font-Bold="True" ForeColor="Black" 
                    Font-Names="Arial" Font-Size="15px" />
                    <PagerSettings PageButtonCount="3" Mode="NumericFirstLast" />
                    <PagerStyle BackColor="#FFFFCC" ForeColor="#330099" HorizontalAlign="Center" />
                    <RowStyle BackColor="White" ForeColor="#0063DC" Wrap="False" />
                    <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="#663399" />
                    <SortedAscendingCellStyle BackColor="#FEFCEB" />
                    <SortedAscendingHeaderStyle BackColor="#AF0101" />
                    <SortedDescendingCellStyle BackColor="#F6F0C0" />
                    <SortedDescendingHeaderStyle BackColor="#7E0000" />
                    <AlternatingRowStyle BackColor="#E2E4FF" Font-Names="Arial" />
                    <Columns>
                        <asp:BoundField DataField="FEED" ReadOnly="True" HeaderText="Origin" />
                        <asp:BoundField DataField="HOSTNAME" ReadOnly="True" HeaderText="Hostname" />
                        <asp:ButtonField ButtonType="Link" Text="Block" CommandName="BLOCK" HeaderText="Block" CausesValidation="False" />
                        <asp:ButtonField ButtonType="Link" Text="Unblock" CommandName="UNBLOCK" HeaderText="Unblock" CausesValidation="False" />
                        <asp:ButtonField ButtonType="Link" Text="Select/Deselect" CommandName="RETRANSFORM" HeaderText="Retransform" CausesValidation="False" />
                        <asp:ButtonField ButtonType="Link" Text="View Now" CommandName="GETGMI_REF" HeaderText="GMI_REF" CausesValidation="False" />
                        <asp:BoundField DataField="GMI_LOCATION" ReadOnly="True" HeaderText="Location" />
                        <asp:BoundField DataField="GMI_MGMT_REGION" HeaderText="Region" ReadOnly="True" />
                        <asp:BoundField DataField="GMI_ENVIRONMENT" ReadOnly="True" HeaderText="Environment" />
                        <asp:BoundField DataField="GMI_STATUS" ReadOnly="True" HeaderText="Status" />
                        <asp:BoundField DataField="GMI_INF" ReadOnly="True" HeaderText="INF" />
                        <asp:BoundField DataField="GMI_CAP" ReadOnly="True" HeaderText="CAP" />
                        <asp:BoundField DataField="GMI_LSG" ReadOnly="True" HeaderText="LSG" />
                        <asp:BoundField DataField="ORIGIN_DATE" ReadOnly="True" HeaderText="Origin Date" NullDisplayText="-" />
                        <asp:BoundField DataField="GMI_CLIENT_NAME" HeaderText="Client" ReadOnly="True" NullDisplayText="-"/>
                        <asp:BoundField DataField="GMI_DEVLOGMAP" HeaderText="DevLogMap" ReadOnly="True" NullDisplayText="-"/>
                        <asp:BoundField DataField="BLOCKING" ReadOnly="True"  HeaderText="Blocking" NullDisplayText="-" />
                        <asp:BoundField DataField="SC_LOGICAL_NAME" ReadOnly="True"  HeaderText="sc_logical_name" NullDisplayText="-" />
                        <asp:BoundField DataField="SRC_RECORD_ID" ReadOnly="True"  HeaderText="src_record_id" NullDisplayText="-" />
                    </Columns>
                    
                </asp:GridView>
                Show
                <asp:DropDownList ID="ddListPageRows" runat="server" Visible="False">
                </asp:DropDownList>
                &nbsp;entries (<asp:Label ID="lblDTRowCount" runat="server" Text="0"></asp:Label>
                &nbsp;found)
                <br />
    </asp:Panel>
        <asp:Panel ID="pnlGMIREF" runat="server" Visible="false" BorderColor="#003300" 
        BorderStyle="Solid" BorderWidth="1px">
        <table>
            <tr valign="middle">
                <td style="width: 320px; font-weight: bold;">Current GMI REF Data for
                    <asp:Label ID="lblGMI_REFHost" runat="server" Text="<hostname>"></asp:Label>
                </td>
                <td style="width: 114px"><asp:Button ID="btnReturnGMIREF" runat="server" 
                        Text="Hide" onclick="btnReturnGMIREF_Click" /></td>
                <td style="width: 114px"><asp:Button ID="btnRefreshGMIREF" runat="server" 
                        Text="Refresh" onclick="btnRefreshGMIREF_Click" Visible="false"/></td>

                <td>
                    <asp:Button ID="btnGMI_REF_DeleteRecord" runat="server" Text="Delete" 
                        Visible="false" Enabled="False" onclick="btnGMI_REF_DeleteRecord_Click1" />
                     <asp:Label ID="lblProcessID" runat="server" Text="" Visible="false"></asp:Label>
                </td>

                <td style="width: 500px; text-align: right;">
                    Test Event Severity&nbsp; <asp:DropDownList ID="ddlEventSeverity" runat="server">
         </asp:DropDownList> <asp:ImageButton ID="imgSendTestEventHelp" runat="server" 
                        ImageUrl="~/Images/Help24x24p.png" onclick="imgSendTestEventHelp_Click"  />
                </td>
            </tr>
        </table>
                    <asp:GridView ID="GridView_GMI_REF" runat="server" AllowSorting="True" 
                        AutoGenerateColumns="False" BackColor="White" BorderColor="#CC9966" 
                        BorderStyle="None" BorderWidth="1px" CellPadding="2" CellSpacing="1" 
                        Font-Names="Arial" Font-Size="14px" GridLines="None" OnRowCommand="GridView_GMI_REF_RowCommand" >
                        <FooterStyle BackColor="#FFC745" ForeColor="Black" />
                        <HeaderStyle BackColor="#33CC33" Font-Bold="False" Font-Names="Arial" 
                            Font-Size="15px" ForeColor="Black" Font-Italic="True" />
                        <PagerSettings Mode="NumericFirstLast" PageButtonCount="3" />
                        <PagerStyle BackColor="#FFFFCC" ForeColor="#330099" HorizontalAlign="Center" />
                        <RowStyle BackColor="White" ForeColor="#0063DC" Wrap="False" />
                        <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="#663399" />
                        <SortedAscendingCellStyle BackColor="#FEFCEB" />
                        <SortedAscendingHeaderStyle BackColor="#AF0101" />
                        <SortedDescendingCellStyle BackColor="#F6F0C0" />
                        <SortedDescendingHeaderStyle BackColor="#7E0000" />
                        <AlternatingRowStyle BackColor="#E2E4FF" Font-Names="Arial" />
                        <Columns>
                            <asp:BoundField DataField="GMI_INITIATING_PROCESS" HeaderText="Origin" ReadOnly="True" />
                            <asp:BoundField DataField="HOSTNAME" HeaderText="Hostname" ReadOnly="True" />
                            <asp:BoundField DataField="GMI_LOCATION" HeaderText="Location" ReadOnly="True" />
                            <asp:BoundField DataField="GMI_MGMT_REGION" HeaderText="Region" ReadOnly="True" />
                            <asp:BoundField DataField="GMI_ENVIRONMENT" HeaderText="Environment" ReadOnly="True" />
                            <asp:BoundField DataField="GMI_STATUS" HeaderText="Status" ReadOnly="True" />
                            <asp:BoundField DataField="INF" HeaderText="INF" ReadOnly="True" />
                            <asp:BoundField DataField="CAP" HeaderText="CAP" ReadOnly="True" />
                            <asp:BoundField DataField="LSG" HeaderText="LSG" ReadOnly="True" />
                            <asp:BoundField DataField="LAST_UPDATED" HeaderText="Origin Date" ReadOnly="True" />
                            <asp:BoundField DataField="GMI_CLIENT_NAME" HeaderText="Client Name" ReadOnly="True" NullDisplayText="-"/>
                            <asp:BoundField DataField="DEVLOGMAP" HeaderText="DevLogMap" ReadOnly="True" NullDisplayText="-"/>
                            <asp:BoundField DataField="GMI_SOURCE" HeaderText="Origin SRC" ReadOnly="True" />
                            <asp:BoundField DataField="SC_LOGICAL_NAME" HeaderText="sc_logical_name" ReadOnly="True" />
                            <asp:ButtonField ButtonType="Link"  Text="SendTestEvent" CommandName="SendEvent" HeaderText="Event Views" CausesValidation="False" />
                        
                            
                        </Columns>
                    </asp:GridView>
   <asp:Panel ID="pnlSendTestEvent" runat="server" Visible="true">

  
    <asp:Panel ID="pnlEventSent" runat="server" Visible="false">
             <br />
             <strong>Test Event SENT!<br /> </strong>The following event from this Device 
             should appear in your expected Event View(s) within a few seconds:<br />&nbsp;
             <asp:Table ID="tblEventSent" runat="server" CellPadding="6" CellSpacing="10" 
                 GridLines="Both">
                 <asp:TableRow>
                     <asp:TableCell><asp:Label ID="lblEventTimeStamp" runat="server" Text="" ></asp:Label></asp:TableCell>
                     <asp:TableCell><asp:Label ID="lblEventSeverity" runat="server" Text=""></asp:Label></asp:TableCell>
                     <asp:TableCell><asp:Label ID="lblEventHostname" runat="server" Text=""></asp:Label></asp:TableCell>
                     <asp:TableCell><asp:Label ID="lblEventSummary" runat="server" Text=""></asp:Label></asp:TableCell>
                     </asp:TableRow>
             </asp:Table>
     </asp:Panel>
     <asp:Panel ID="pnlSendTestEventHelp" runat="server" Visible="false">
        <br />
        A test event can be useful for ensuring your data is correct in GMI. It can 
        confirm that your Event View Filters are set up to include events from this device.
        <br /> To send a test event, click the SendTestEvent link on this GMI_REF record. 
         This will generate a new test event on behalf of your host. 
        <br /> The details will appear here then you can go and look in your Event Views for the matching details.


    
         <br />
         You can also change the severity of the event to suit and send as many events as 
         you need.<br /> 
         When you are finished testing events selecting a severity of CLEAR and sending 
         an event will cause any previous Test events to be cleared for the host.<br /> 
         For help with events not appearing in your Event View(s), contact
         <asp:HyperLink ID="eventViewMailTo" runat="server" 
             NavigateUrl="mailto:Thomson-Delivery-MANSYS@thomsonreuters.com" 
             Text="Delivery-Mansys"> </asp:HyperLink>
         .
         <br />
         Click the help icon again to close this help.</asp:Panel>  

    </asp:Panel> 
     <br />
  </asp:Panel>
            <br />
                <asp:Panel ID="panelCommit" runat="server" Visible="false">
                    <strong>
                    Action Cart Summary</strong><br />
                    <table style="width: 844px;" frame="box" border="1">
                        <tr>
                            <td style="width: 274px; height: 22px;">
                                &nbsp; Systems (feed to block)</td>
                            <td style="width: 187px; height: 22px;">
                                &nbsp; Systems (feed to unblock)</td>
                            <td rowspan="4" >
                                &nbsp;&nbsp;&nbsp;</td>
                            <td style="height: 22px; width: 274px;">
                                &nbsp; Systems to retransform</td>
                        </tr>
                        <tr>
                            <td>
                                <div style="overflow: scroll; height: 100px; width: 274px; vertical-align: top; ">
                                    <asp:BulletedList ID="bulCommitBlock" runat="server" BulletStyle="Disc" 
                                        DisplayMode="HyperLink" Target="_blank">
                                    </asp:BulletedList>
                                    


                                </div>
                            </td>
                            <td>
                                <div style="overflow: scroll; height: 100px; width: 274px; vertical-align: top; ">
                                     <asp:BulletedList ID="bulCommitUnBlock" runat="server" 
                                        BulletStyle="Circle" DisplayMode="HyperLink" Target="_blank">
                                    </asp:BulletedList>
                                </div>
                            </td>
                            <td style="width: 274px">
                                <div style="overflow: scroll; height: 100px; width: 274px; vertical-align: top; ">
                                    <asp:BulletedList ID="bulReTransform" runat="server" BulletStyle="Square" 
                                        DisplayMode="HyperLink" Target="_blank">
                                    </asp:BulletedList>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="2" >
                                &nbsp;</td>
                            <td style="width: 274px">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td align="center" colspan="2">
                                &nbsp; &nbsp;
                                <asp:Button ID="btnCommitChanges" runat="server" 
                                    onclick="btnCommitChanges_Click" Text="Commit Changes" />
                            </td>
                            <td style="width: 274px">
                                <asp:Button ID="btnReTransform" runat="server" Enabled="False" 
                                    onclick="btnReTransform_Click" Text="Retransform" 
                                    ToolTip="Rather than wait for an update from the feed, use this to run the retransformation now" />
                            </td>
                        </tr>
                    </table>
                    
                    <br />
                </asp:Panel>


            
                <br />
    <asp:Panel ID="pnlErrMsg" runat="server" Visible="False">
        <asp:TextBox ID="txtExceptionsBox" runat="server"></asp:TextBox>
        <asp:Label ID="lblCommitMsgs" runat="server" Font-Italic="True" 
            Font-Names="Arial" Font-Size="Medium" ForeColor="Blue"></asp:Label>
    </asp:Panel>


            
                <br />
                <asp:TextBox ID="txtSQLQuery" runat="server" Visible="False"></asp:TextBox>




    <br />
    <br />





    <br />


    <br />
    <br />
    <br />



</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="DebugContentPlaceHolder" runat="server">
</asp:Content>
