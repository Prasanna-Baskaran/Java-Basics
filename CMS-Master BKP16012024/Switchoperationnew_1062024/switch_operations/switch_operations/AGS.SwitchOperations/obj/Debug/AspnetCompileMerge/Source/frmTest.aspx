<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmTest.aspx.cs" Inherits="AGS.SwitchOperations.frmTest" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <h1>Test Page</h1>
        Name : 
        <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
        <asp:Button ID="btnClick" runat="server" Text="Click" OnClick="btnClick_Click" />
    </div>
    </form>
</body>
</html>
