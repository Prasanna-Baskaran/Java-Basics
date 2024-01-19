<%@ Page Title="" Language="C#" MasterPageFile="~/SwitchOperationSite.Master" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="AGS.SwitchOperations.ChangePassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phPageHeader" runat="server">
    <script>

        //Validation on Save Button
        function FunValidation() {

            var newpass = $('#phPageBody_txtNewPassword').val()
            var connewpass = $('#phPageBody_txtConfirmNewPassword').val()

            if (newpass != connewpass) {
                $('#SpnErrorMsg').html('New Password & Confirm new password do not match');
                $('#errormsgDiv').show();
                return false;
            }
            else {
                $('#errormsgDiv').hide();
                return true;
            }
        }

        function FunShowMsg(msg) {

            $('#SpnErrorMsg').html(msg);
            $('#errormsgDiv').show();
        }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#<%=txtNewPassword.ClientID %>").keyup(function (e) {
                console.log('hi');
                //if ($('[id$="phPageBody_txtNewPassword"]').val() == '') { return true; }
                var strongRegex = new RegExp("^(?=.{7,})(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])(?=.*\\W).*$", "g");
                var mediumRegex = new RegExp("^(?=.{7,})(((?=.*[A-Z])(?=.*[a-z]))|((?=.*[A-Z])(?=.*[0-9]))|((?=.*[a-z]PASSW[0-9]))).*$", "g");
                var enoughRegex = new RegExp("(?=.{6,}).*", "g");
                if (false == enoughRegex.test($(this).val())) {
                    $(this).css({ 'color': 'red' });
                    $('#lblNewPassword').html('Strength : Password is too short!');
                    $('#lblNewPassword').removeClass().addClass('label label-danger');
                } else if (strongRegex.test($(this).val())) {

                    $('#lblNewPassword').className = 'ok';
                    $(this).css("color", "green");
                    $('#lblNewPassword').html('Strength : Password is Strong!');
                    $('#lblNewPassword').removeClass().addClass('label label-success');

                } else if (mediumRegex.test($(this).val())) {
                    $(this).css("color", "orange");
                    $('#lblNewPassword').html('Strength : Password is Medium!');
                    $('#lblNewPassword').removeClass().addClass('label label-warning');
                } else {
                    $(this).css("color", "red");
                    $('#lblNewPassword').html('Strength : Password is Weak!');
                    $('#lblNewPassword').removeClass().addClass('label label-danger');
                }
                return true;
            });
        });
    </script>
    <style>
        .login_content {
            margin: 0 auto;
            padding: 25px 0 0;
            position: relative;
            text-align: center;
            text-shadow: 0 1px 0 #fff;
            min-width: 280px;
        }

            .login_content a,
            .login_content .btn-default:hover {
                text-decoration: none;
            }

                .login_content a:hover {
                    text-decoration: underline;
                }

            .login_content h1 {
                font: normal 25px Helvetica, Arial, sans-serif;
                letter-spacing: -0.05em;
                line-height: 20px;
                margin: 10px 0 30px;
            }

                .login_content h1:before, .login_content h1:after {
                    content: "";
                    height: 1px;
                    position: absolute;
                    top: 10px;
                    width: 27%;
                }

                .login_content h1:after {
                    background: #7e7e7e;
                    background: linear-gradient(left, #7e7e7e 0%, white 100%);
                    right: 0;
                }

                .login_content h1:before {
                    background: #7e7e7e;
                    background: linear-gradient(right, #7e7e7e 0%, white 100%);
                    left: 0;
                }

                .login_content h1:before, .login_content h1:after {
                    content: "";
                    height: 1px;
                    position: absolute;
                    top: 10px;
                    width: 20%;
                }

                .login_content h1:after {
                    background: #7e7e7e;
                    background: linear-gradient(left, #7e7e7e 0%, white 100%);
                    right: 0;
                }

                .login_content h1:before {
                    background: #7e7e7e;
                    background: linear-gradient(right, #7e7e7e 0%, white 100%);
                    left: 0;
                }

        .login_content {
            margin: 20px 0;
            position: relative;
        }

            .login_content input[type="text"], .login_content input[type="email"], .login_content input[type="password"], .login_content select {
                border-radius: 3px;
                -ms-box-shadow: 0 1px 0 #fff, 0 -2px 5px rgba(0, 0, 0, 0.08) inset;
                -o-box-shadow: 0 1px 0 #fff, 0 -2px 5px rgba(0, 0, 0, 0.08) inset;
                box-shadow: 0 1px 0 #fff, 0 -2px 5px rgba(0, 0, 0, 0.08) inset;
                border: 1px solid #c8c8c8;
                color: #777;
                width: 100%;
            }

                .login_content input[type="password"]:not(#phPageBody_txtNewPassword), .foo > .input-group, .login_content select {
                    margin: 0 0 20px;
                }

                .login_content input[type="text"]:focus, .login_content input[type="email"]:focus, .login_content input[type="password"]:focus {
                    -ms-box-shadow: 0 0 2px #ed1c24 inset;
                    -o-box-shadow: 0 0 2px #ed1c24 inset;
                    box-shadow: 0 0 2px #A97AAD inset;
                    background-color: #fff;
                    border: 1px solid #A878AF;
                    outline: none;
                }

        #username {
            background-position: 10px 10px !important;
        }

        #password {
            background-position: 10px -53px !important;
        }

        .login_content div a {
            font-size: 12px;
            margin: 10px 15px 0 0;
        }

        .reset_pass {
            margin-top: 10px !important;
        }

        .login_content div .reset_pass {
            margin-top: 13px !important;
            margin-right: 39px;
            float: right;
        }

        .separator {
            border-top: 1px solid #D8D8D8;
            margin-top: 10px;
            padding-top: 10px;
        }

        .button {
            background: #f7f9fa;
            background: linear-gradient(top, #f7f9fa 0%, #f0f0f0 100%);
            filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#f7f9fa', endColorstr='#f0f0f0', GradientType=0);
            -ms-box-shadow: 0 1px 2px rgba(0, 0, 0, 0.1) inset;
            -o-box-shadow: 0 1px 2px rgba(0, 0, 0, 0.1) inset;
            box-shadow: 0 1px 2px rgba(0, 0, 0, 0.1) inset;
            border-radius: 0 0 5px 5px;
            border-top: 1px solid #CFD5D9;
            padding: 15px 0;
        }

            .button a {
                background: url(http://cssdeck.com/uploads/media/items/8/8bcLQqF.png) 0 -112px no-repeat;
                color: #7E7E7E;
                font-size: 17px;
                padding: 2px 0 2px 40px;
                text-decoration: none;
                transition: all 0.3s ease;
            }

                .button a:hover {
                    background-position: 0 -135px;
                    color: #00aeef;
                }

        .foo #lblNewPassword {
            position: absolute;
            top: 25%;
            left: calc(100% - -12px);
            z-index: 3;
        }
        div.foo {
            position:relative;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phPageBody" runat="server">

    <div>
        <div class="">
            <div class="box-header with-border">
                <i class="fa fa-list"></i>
                <h3 class="box-title">Change Password</h3>
                <div class="box-tools pull-right">
                </div>
            </div>
        </div>

        <!--Display validation msg ------------------------------------------------------------------------->
        <div class="pad margin no-print" id="errormsgDiv" style="display: none">
            <div class="callout callout-info" style="margin-bottom: 0!important;">
                <h4><i class="fa fa-info"></i>Information :</h4>
                <span id="SpnErrorMsg" class="text-center"></span>
            </div>
        </div>

        <div class="login_wrapper">
            <div class="animate form login_form">
                <section class="login_content">
                    <%--<asp:Image ID="Logo" runat="server" ImageUrl="~/Images/SDB_LOGO.png" />--%>
                    <%--<h1>Change Password</h1>--%>
                    <div>
                        <!--start sheetal set autocomplete off will not show past values -->
                        <%--<input type="text" class="form-control" placeholder="Username" required="" runat="server" id="txtUsername" onpaste="return false;" autocomplete="off" />--%>
                        <asp:DropDownList runat="server" ID="ddlUserID" CssClass="form-control" Enabled="false">
                        </asp:DropDownList>
                    </div>
                    <div>
                        <input type="password" class="form-control" placeholder="Current password" required="" runat="server" id="txtCurrentPassword" maxlength="30" />
                    </div>
                    <div class="foo">
                        <div class="input-group">
                            <input type="password" class="form-control" placeholder="New password" required="" runat="server" id="txtNewPassword" maxlength="30" />
                            <div id="PwdPolicy" class="input-group-addon text-danger"><i class="fa fa-info text-danger" data-toggle="tooltip" data-placement="bottom" title="Password Policy : Must be at least seven characters long, one upper case letter, one lower case letter, one number, one special character!"></i></div>
                        </div>
                        <label id="lblNewPassword"></label>
                    </div>
                    <div>
                        <input type="password" class="form-control" placeholder="Confirm new password" required="" runat="server" id="txtConfirmNewPassword" maxlength="30" />
                    </div>

                    <div>
                        <div class="col-md-12  justify-content-center">
                            <asp:Button runat="server" ID="btnSave" OnClick="btnSave_Click" Text="Save" CssClass="btn btn-default submit" OnClientClick="return FunValidation();" />
                        </div>
                    </div>

                    <div class="clearfix"></div>

                    <div class="separator">

                        <div class="clearfix"></div>
                        <br />

                    </div>
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
</asp:Content>
