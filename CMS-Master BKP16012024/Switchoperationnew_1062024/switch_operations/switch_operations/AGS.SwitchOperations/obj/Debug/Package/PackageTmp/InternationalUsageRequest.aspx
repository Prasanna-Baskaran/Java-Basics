<%@ Page Title="" Language="C#" MasterPageFile="~/SwitchOperationSite.Master" AutoEventWireup="true" CodeBehind="InternationalUsageRequest.aspx.cs" Inherits="AGS.SwitchOperations.InternationalUsageRequest" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phPageHeader" runat="server">
    
   <script type="text/javascript" >
       function funClearMsg()
    {
        $('#LblResult').text('');
     
    }
    </script>
    <style type="text/css">
        .auto-style1 {
            width: 100%;
            border-style: solid;
            border-width: 1px;
        }
        .auto-style2 {
            width: 130px;
        }
        .auto-style5 {
            height: 22px;
        }
        .auto-style6 {
            width: 248px;
        }
        .auto-style7 {
            height: 55px;
            width: 296px;
        }
        .auto-style8 {
            width: 130px;
            height: 37px;
        }
        .auto-style9 {
            height: 37px;
        }
        .auto-style10 {
            height: 47px;
        }
        .auto-style12 {
            height: 37px;
            width: 20px;
        }
        .auto-style13 {
            height: 5px;
            width: 130px;
        }
        .auto-style14 {
            height: 22px;
            width: 130px;
        }
        </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phPageBody" runat="server">
    <table cellpadding="2" cellspacing="3" class="auto-style1" style="border-style: none">
        <tr>
            <td class="auto-style10" colspan="6" style="border-style: solid; border-width: thin; font-family: Arial, Helvetica, sans-serif; font-size: large; background-color: #0099CC; color: #FFFFFF;">International Usage Request</td>
        </tr>
        <tr>
            <td class="auto-style13"></td>
        </tr>
        <tr>
            <td class="auto-style8" style="border: thin dotted #FFFFCC;"><label runat="server" id="Lblcard" readonly="readonly" >Card Number</label></td>
            <td class="auto-style12" style="border: thin dotted #FFFFCC; "><input class="auto-style6"  runat="server" name="txt" type="text" id="txtCardNum" maxlength="16" onkeypress="return FunChkIsNumber(event)" onblur="funClearMsg()" tabindex="1" /></td>
            <td class="auto-style9" style="border: thin dotted #FFFFCC;">
                &nbsp;</td>
            <td class="auto-style9" style="border: thin dotted #FFFFCC;">
                <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary " Text="Search" OnClick="btnSearch_Click" Height="33px" TabIndex="2" OnClientClick="return $('.shader').fadeIn();" />
            </td>
            <td class="auto-style9"></td>
            <td class="auto-style9"></td>
        </tr>
        <tr>
           <td class="auto-style14" style="border: thin dotted #FFFFCC;" ><label runat="server" id="LblIntUsage" readonly="readonly" >International Usage Block</label></td>
           <td class="auto-style5" style="border: thin dotted #FFFFCC;" colspan="3"><input id="chkIntUsage" runat="server"  type="checkbox" tabindex="3" /></td>
            <td class="auto-style5"></td>
            <td class="auto-style5"></td>
        </tr>
        <tr>
            <td class="auto-style2" rowspan="2" style="border: thin dotted #FFFFCC;"><label runat="server" id="LblRemark" readonly="readonly" >Remark</label></td>
            <td rowspan="2" style="border: thin dotted #FFFFCC;" colspan="3">  <textarea id="txtRemark"  runat="server" style="resize:none" class="auto-style7" name="S1" tabindex="4"></textarea> </td>
            <td class="auto-style5"></td>
            <td class="auto-style5"></td>
        </tr>
        <tr>
            <td class="auto-style5"></td>
            <td class="auto-style5"></td>
        </tr>
        <tr>
            <td class="auto-style2" style="border-style: none; border-width: thin" rowspan="2"></td>
            <td style="border-style: none; border-width: thin" colspan="3" rowspan="2">
                <asp:Button ID="BtnRequest" runat="server" Text="International Usage Change Request" OnClick="BtnRequest_Click" CssClass="btn btn-primary " Width="274px" TabIndex="6" OnClientClick="return $('.shader').fadeIn();"/>
            </td>
            <td class="auto-style5"></td>
            <td class="auto-style5"></td>
        </tr>
        <tr>
            <td class="auto-style5"></td>
            <td class="auto-style5"></td>
        </tr>
        </table>
     <div class="row" id="DivResultMsg">
            <div class="col-md-6">
                <h4>
                    <label runat="server" name="Name" id="LblResult" readonly="readonly"  />
                </h4>
            </div>
         </div>
       <div id="shader" class="shader">
        <div class="loadingContainer">
            <div id="divLoading3">
                <div id="loader-wrapper">
                    <div id="loader"></div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
