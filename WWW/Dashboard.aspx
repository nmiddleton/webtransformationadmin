<%@ Page Title="GMI Transformation Dashboard" Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="webtransformationadmin.UI.Dashboard" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">


    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
        <asp:Timer ID="TimerRefresh" runat="server" Enabled="False" Interval="30000" 
        ontick="TimerRefresh_Tick">
        </asp:Timer>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="TimerRefresh" EventName="Tick" />
            </Triggers>
            <ContentTemplate>
                    <table style="width:100%;">
            <tr>
                <td style="font-size: large; width: 613px;">
                    <table style="width:100%;">
                        <tr>
                            <td style="width: 283px">
                    <strong>GMI Transformation Dashboard</strong></td>
                            <td>
                                <asp:CheckBox ID="cbxAutoRefresh"  Text="Auto Refresh" runat="server" 
                                    Checked="True" oncheckedchanged="cbxAutoRefresh_CheckedChanged" 
                                    AutoPostBack="True" />
                            &nbsp;
                                <asp:Label ID="lblLastRefresh" runat="server" Font-Italic="True" 
                                    Font-Size="Small"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            </table>
        <asp:Chart ID="Chart_CSVQueue" runat="server" Width="320px">
            <Series>
                <asp:Series Name="Series1" ChartType="StepLine">
                </asp:Series>
            </Series>
            <ChartAreas>
                <asp:ChartArea Name="ChartArea1">
                </asp:ChartArea>
            </ChartAreas>
        </asp:Chart>
        <asp:Chart ID="Chart_SMQueue" runat="server" Width="320px">
            <Series>
                <asp:Series ChartType="StepLine" Name="Series1">
                </asp:Series>
            </Series>
            <ChartAreas>
                <asp:ChartArea Name="ChartArea1">
                </asp:ChartArea>
            </ChartAreas>
        </asp:Chart>
        <asp:Chart ID="Chart_TOMQueue" runat="server" Width="320px">
            <Series>
                <asp:Series ChartType="StepLine" Name="Series1">
                </asp:Series>
            </Series>
            <ChartAreas>
                <asp:ChartArea Name="ChartArea1">
                </asp:ChartArea>
            </ChartAreas>
        </asp:Chart>
                <br />
                <asp:Chart ID="Chart_TTime" runat="server" Width="1000px">
            <Series>
                <asp:Series ChartType="StepLine" Name="Series1">
                </asp:Series>
            </Series>
            <ChartAreas>
                <asp:ChartArea Name="ChartArea1">
                <AxisY Title="Duration (secs)" ></AxisY>

                </asp:ChartArea>
            </ChartAreas>
        </asp:Chart>
    
        </ContentTemplate>
    </asp:UpdatePanel>
               </asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="DebugContentPlaceHolder" runat="server">
</asp:Content>
