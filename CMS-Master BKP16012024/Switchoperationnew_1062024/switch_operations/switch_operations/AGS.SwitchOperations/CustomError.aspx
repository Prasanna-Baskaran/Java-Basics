<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomError.aspx.cs" Inherits="AGS.SwitchOperations.CustomError" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <!-- Bootstrap -->
    <link href="Styles/bootstrap.min.css" rel="stylesheet" />
    <!-- Custom Theme Style -->
    <link href="Styles/custom.min.css" rel="stylesheet" />
</head>
<body class="login">
    <div>
        <div class="login_wrapper">
            <div class="animate form login_form">
                <section class="login_content">
                    <form>
                              <div id="loginErrorMsg" class="alert alert-error " runat="server">This page you are looking for does not exist</div>
                    </form>
                
                </section>
            </div>
        </div>


    </div>

</body>
</html>
