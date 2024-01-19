<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="~/DFCC/Login.aspx.cs" Inherits="AGS.SwitchOperations.DFCC.DFCCLogin" %>

<!DOCTYPE html>
<html lang="en">

<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
    <!-- Meta, title, CSS, favicons, etc. -->
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">

    <title></title>

    <!-- Bootstrap -->
    <link href="../Styles/bootstrap.min.css" rel="stylesheet" />
    <!-- Font Awesome -->
    <%--<link href="Styles/font-awesome.min.css" rel="stylesheet">--%>
    <script src="../Scripts/jquery.min.js"></script>
    <!-- Custom Theme Style -->
    <link href="../Styles/custom.css" rel="stylesheet" />
    <%-- Loader --%>
    <link href="../Styles/Loader.css" rel="stylesheet" />
    <script src="../Scripts/Loader.js"></script>
    <script>
        function FunShowLoader() {
            if ($('#txtUsername').val() != "" && $('#txtPwd').val() != "") {
                $('.shader').fadeIn();
                return true
            }
        }
    </script>
</head>

<body class="login">
    
    <div>
        <div class="login_wrapper">
            <div class="animate form login_form" >
                <section class="login_content">
                            <asp:Image ID="Logo" runat="server" ImageUrl="~/DFCC/Images/DFCC_LOGO.png" Width="30px" />
                    <form runat="server">
                        <h1>Login</h1>
                        <div id="loginErrorMsg" class="alert alert-error hide" runat="server">Invalid username or password</div>
                        <div>
                            <!--start sheetal set autocomplete off will not show past values -->
                            <input type="text" class="form-control" placeholder="Username" required="" runat="server" id="txtUsername" onpaste="return false;" autocomplete="off" />
                        </div>
                        <div>
                            <input type="password" class="form-control" placeholder="Password" required="" runat="server" id="txtPwd" />
                        </div>
                        <div>
                            <%--<a class="btn btn-default submit" href="index.html">Log in</a>
                            <a class="btn btn-default submit" href="#">Reset</a>--%>
                            <div class="col-md-3"></div>
                            <div class="col-md-8">
                                <asp:Button runat="server" ID="btnLogin" OnClick="btnLogin_Click" Text="Log in" CssClass="btn btn-default submit" />
                            </div>
                        </div>

                        <div class="clearfix"></div>

                        <div class="separator">

                            <div class="clearfix"></div>
                            <br />

                            <div>
                                <h1><i class="fa fa-paw"></i></h1>
                                <p>©2019 All Rights Reserved.</p>
                            </div>
                        </div>
                    </form>
                </section>
            </div>

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
</body>

</html>