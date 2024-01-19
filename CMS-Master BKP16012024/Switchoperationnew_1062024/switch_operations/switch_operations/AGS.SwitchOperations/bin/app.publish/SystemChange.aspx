<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SystemChange.aspx.cs" Inherits="AGS.SwitchOperations.SystemChange" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="Styles/bootstrap.min.css" rel="stylesheet" />
    <!-- Font Awesome -->
    <%--<link href="Styles/font-awesome.min.css" rel="stylesheet">--%>

    <!-- Custom Theme Style -->
    <link rel="stylesheet" href="Styles/AdminLTE.min.css" />
    <link href="Styles/custom.css" rel="stylesheet"/>

     <script src="Scripts/jquery.min.js"></script>
    <!-- Bootstrap -->
    <script src="Scripts/bootstrap.min.js"></script>
</head>
<body class="nav-md">
    <div class="container body">
        <div class="main_container">
            <!-- page content -->
            <div class="row">
                <div class="col-md-12">
                    <form runat="server">

                        <div class="modal-dialog">
                            <!-- Modal content -->
                            <div class="modal-content" style="border-radius: 4px;width: 601px;">

                                <div class="modal-header">
                                    
                                    <%--<h4 id="myLargeEdit" class="modal-title" style="font-weight: bold">Change System</h4>--%>
                                </div>

                                <!--Display validation msg ------------------------------------------------------------------------->
                                <div class="pad margin no-print" id="errormsgDiv" style="display: none">
                                    <div class="callout callout-info" style="margin-bottom: 0!important;">
                                        <h4><i class="fa fa-info"></i>Information :</h4>
                                        <span id="SpnErrorMsg" class="text-center"></span>
                                    </div>
                                </div>

                                <div class="modal-body">

                                    <div class="row">
                                        <div class="col-md-5">
                                        <label for="inputName" class="control-label pull-right"><span style="color: red;"></span><b>System:</b></label>
                                            </div>
                                        <div class="col-sm-6">
                                            <asp:DropDownList ID="DdlSystem" runat="server" class="form-control" Style="width: 100%"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="row"><br /></div>
                                    <div class="row"><br /></div>
                                    <div class="row">
                                        <div class="col-md-5"></div>
                                        <div class="col-md-6">
                                            <asp:Button runat="server" ID="btnChange" Text="Proceed" CssClass="btn  btn-primary submit" OnClick="btnProceed_Click"/>
                                        </div>
                                       
                                    </div>
                                </div>
                            </div>
                        </div>

                    </form>

                </div>
            </div>
        </div>

        <%--//******************* SCRIPTS **********************--%>
         <script type="text/javascript">
        $(document).ready(function () {
            $('#<%=btnChange.ClientID%>').click(function () {

                if($("#DdlSystem").val()=="0")
                {
                    $('#SpnErrorMsg').html("Provide System");
                    $('#errormsgDiv').show();
                    $("#DdlSystem").focus();
                    return false;
                    
                }
                else
                {
                    $('#SpnErrorMsg').html("");
                    $('#errormsgDiv').hide();
                    return true
                }
            })
        });

    </script>
    </div>

   
</body>
    
</html>
