<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="~/SDF/Login.aspx.cs" Inherits="AGS.SwitchOperations.SDF.SDFLogin" %>

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
    <!-- Animate.css -->
    <link href="../Styles/animate.css/animate.min.css" rel="stylesheet" />
    <!-- Custom Theme Style -->
    <link href="../Styles/custom.css" rel="stylesheet" />
    <%-- Loader --%>
    <link href="../Styles/Loader.css" rel="stylesheet" />
    <script src="../Scripts/Loader.js"></script>
    <script>
        function Funvalidlogin() {
            if ($('#<%=txtUsername.ClientID%>').val() == "") {
                $('#<%=txtUsername.ClientID%>').focus();
                return false
            }
            else if ($('#<%=txtPwd.ClientID%>').val() == "") {
                $('#<%=txtPwd.ClientID%>').focus();
                return false
            }
            else {
                $('.shader').fadeIn();
                return true
            }
        }
        function Funvalidreset() {
            if ($('#<%=txtresetusername.ClientID%>').val() == "") {
                $('#<%=txtresetusername.ClientID%>').focus();
                return false
            }
            else {
                $('.shader').fadeIn();
                return true
            }
        }
    </script>


    <script>
        function FunShowLoader() {
            if ($('#txtUsername').val() != "" && $('#txtPwd').val() != "") {
                $('.shader').fadeIn();
                return true
            }
        }
    </script>
<style>
.login_content h1:before, .login_content h1:after
{top:132px;}
</style>
</head>

<body class="login">

    <div>
        <form runat="server">
            <a class="hiddenanchor" id="signup"></a>
            <a class="hiddenanchor" id="signin"></a>
            <div class="login_wrapper">
                <div class="animate form login_form">
                    <asp:Panel ID="panel2" DefaultButton="btnLogin" runat="server">
                        <section class="login_content">
                            <asp:Image ID="Logo" runat="server" ImageUrl="~/SDF/Images/SDFLogo.png" Width="300px" />
                            <div class="login-form">
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
                                        <asp:Button runat="server" ID="btnLogin" OnClick="btnLogin_Click" Text="Log in" CssClass="btn btn-block btn-primary submit" />
                                </div>

                                <div class="clearfix"></div>

                                <div class="separator">

                                    <p class="change_link">
                                        Forgot Password?
                          <a href="#signup" class="to_reset_password">Reset Password </a>
                                    </p>
                                    <div class="clearfix"></div>
                                    <br />

                                    <div>
                                        <h1><i class="fa fa-paw"></i></h1>
                                        <p>©2019 All Rights Reserved.</p>
                                    </div>
                                </div>
                            </div>
                        </section>
                    </asp:Panel>
                </div>
                <div id="register" class="animate form registration_form">
                    <asp:Panel ID="panel1" DefaultButton="btnResetPassword" runat="server">
                        <section class="login_content">
                            <asp:Image ID="Image1" runat="server" ImageUrl="~/SDF/Images/SDFLogo.png" Width="300px" />
                            <div class="login-form">
                                <h1>Reset Password</h1>
                                <div id="ResetErrorMsg" class="alert alert-error hide" runat="server">Invalid username</div>
                                <div>
                                    <input type="text" id="txtresetusername" runat="server" class="form-control" placeholder="Username" autocomplete="off" />
                                </div>
                                <div>
                                    <asp:Button runat="server" ID="btnResetPassword" OnClick="btnResetPassword_Click" Text="Send Reset Link" OnClientClick="return Funvalidreset()" CssClass="btn btn-block btn-primary submit" />
                                </div>

                                <div class="clearfix"></div>

                                <div class="separator">
                                    <p class="change_link">
                                        Remember your password ?
                                <a href="#signin" class="to_register">Log in </a>
                                    </p>

                                    <div class="clearfix"></div>
                                    <br />

                                    <div>
                                        <%--<img src="/Images/AGS.jpg" width="20%" />--%>
                                        <p>©2019 All Rights Reserved.</p>
                                    </div>
                                </div>
                            </div>
                        </section>
                    </asp:Panel>
                </div>
            </div>
        </form>
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
