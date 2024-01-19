<%@ Page Title="" Language="C#" MasterPageFile="~/SwitchOperationSite.Master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true" CodeBehind="CardFeeConfigure.aspx.cs" Inherits="AGS.SwitchOperations.CardFeeConfigure" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phPageHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phPageBody" runat="server">
    <!--All Hiddenfields------------------------------------------------------------------------->
    <asp:HiddenField ID="hdnAccessCaption" runat="server" />
    <asp:HiddenField ID="hdnTransactionDetails" runat="server" />
    <asp:HiddenField ID="hdnFlag" runat="server" />


    <asp:Panel ID="pnlIssuerSearch" runat="server">
        <div style="font-size: 14px;">
            <div id="SearchBank">
                <div class="box-primary">
                    <div class="box-header with-border">
                        <i class="fa fa-credit-card"></i>

                        <h4 class="box-title">Card FeeConfiguration:</h4>
                        <div class="box-tools pull-right">
                            <button class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                        </div>
                        <!-- /.box-tools -->
                    </div>

                    <!--Display validation msg ------------------------------------------------------------------------->
                    <div class="pad margin no-print" id="errormsgDiv" style="display: none">
                        <div class="callout callout-info" style="margin-bottom: 0!important;">
                            <h4><i class="fa fa-info"></i>Information :</h4>
                            <span id="SpnErrorMsg" class="text-center"></span>
                        </div>
                    </div>



                    <div class="box-body">

                        <div class="box-body" id="SearchDiv">
                            <div class="form-horizontal">
                                <div class="row">
                                    <div class="col-md-4">
                                        <div class="form-group">
                                            <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>BankName:</label>
                                            <div class="col-sm-8">
                                                <asp:DropDownList ID="ddlSearchIssuerNo" runat="server" class="form-control" Style="width: 100%">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-md-4">
                                        <div class="form-group">
                                            <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>File Catagory:</label>
                                            <div class="col-sm-8">
                                                <asp:DropDownList ID="ddlFileCatagory" runat="server" class="form-control" Style="width: 100%">
                                                
                                                <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Issuance" Value="I" ></asp:ListItem>
                                <asp:ListItem Text="Reissue" Value="R" ></asp:ListItem>
                                <asp:ListItem Text="Annual" Value="A" ></asp:ListItem>
                                                    </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>


                                </div>

                                <div class="row">
                                    <div class="col-md-4">
                                        <div class="form-group">
                                            <div class="col-sm-4"></div>

                                            <div class="col-sm-3">
                                                <div class="col-md-8">
                                                    <asp:Button runat="server" CssClass="btn btn-primary pull-right" ID="btnSearch" Text="Search" OnClick="btnSearch_Click" OnClientClick="return FunSearchValidation();" />
                                                </div>
                                            </div>
                                            

                                            <div class="col-sm-4"></div>
                                        </div>
                                    </div>
                                </div>
                                <%--<div class="row" id="DivResultMsg">
                                    <div class="col-md-6">
                                        <h4>
                                            <label maxlength="20" runat="server" name="Name" id="LblResult" readonly="readonly" />
                                        </h4>
                                    </div>
                                </div>--%>


                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>

    <asp:Panel ID="pnlCardFeeMasterSave" runat="server">
           <%-- <div style="font-size: 14px;">
            <div id="SaveCardFeeDetails">
                <div class="box-primary">
                    <div class="box-header with-border">
                        <i class="fa fa-credit-card"></i>

                        <h4 class="box-title">Card FeeConfiguration:</h4>
                        <div class="box-tools pull-right">
                            <button class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                        </div>
                        <!-- /.box-tools -->
                    </div>--%>

                    <!--Display validation msg ------------------------------------------------------------------------->
                    <%--<div class="pad margin no-print" id="errormsgDivCardFeeMaster" style="display: none">
                        <div class="callout callout-info" style="margin-bottom: 0!important;">
                            <h4><i class="fa fa-info"></i>Information :</h4>
                            <span id="SpnErrorMsgCardFeeMaster" class="text-center"></span>
                        </div>
                    </div>--%>

        <div class="box-body">

            <div class="box-header">
                <h4 class="box-title">Card FeeConfiguration :</h4>
            </div>

            <div class="form-horizontal">

                <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label for="inputName" class="col-xs-4 control-label">IssuerNo:</label>
                        <div class="col-sm-8">
                            <input type="text" class="form-control" maxlength="10" runat="server" name="IssuerNo" id="txtIssuerNo" onkeypress="return FunChkIsNumber(event);" />
                        </div>
                    </div>
                </div>

                <div class="col-md-4">
                    <div class="form-group">
                        <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>SequenceNo:</label>
                        <div class="col-sm-8">
                            <input type="text" class="form-control" runat="server" name="SequenceNo" id="txtSequenceNo" maxlength="10" onkeypress="return FunChkIsNumber(event);" />
                        </div>
                    </div>
                </div>

                <div class="col-md-4">
                    <div class="form-group">
                        <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>BankStatus:</label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="ddlBankstatus" runat="server" class="form-control" Style="width: 100%">
                                <asp:ListItem Text="--Select--" Value="-1"></asp:ListItem>
                                <asp:ListItem Text="Active" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Inactive" Value="0"></asp:ListItem>
                            </asp:DropDownList>

                        </div>
                    </div>
                </div>

            </div>
                </div>
            </div>



        <div class="box-body">

            <div class="box-header">
                <h4 class="box-title">SFTP Details :</h4>
            </div>

            <div class="form-horizontal">

            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>Input Filepath:</label>
                        <div class="col-sm-8">
                            <input type="text" class="form-control" runat="server" name="SFTPIncoming" maxlength="500" id="txtSFTPIncoming" onkeypress="return GetSFTP_FolderPath(event);" />
                        </div>
                    </div>
                </div>

                <div class="col-md-4">
                    <div class="form-group">
                        <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>Output FilePath:</label>
                        <div class="col-sm-8">
                            <input type="text" class="form-control" runat="server" name="SFTPOutput" maxlength="500" id="txtSFTPOutput" onkeypress="return GetSFTP_FolderPath(event);" />
                        </div>
                    </div>
                </div>

                <div class="col-md-4">
                    <div class="form-group">
                        <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>Rejected FilePath:</label>
                        <div class="col-sm-8">
                            <input type="text" class="form-control" runat="server" name="SFTPRejected" maxlength="500" id="txtSFTPRejected" onkeypress="return GetSFTP_FolderPath(event);" />
                        </div>
                    </div>
                </div>
            </div>
                

            <%--return IsAlphaCapital(event);--%>
            <div class="row">
                    <div class="col-md-4">
                        <div class="form-group">
                            <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>UserName:</label>
                            <div class="col-sm-8">
                                <input type="text" class="form-control" maxlength="70" runat="server" name="UserName" id="txtUserName" onkeypress="return GetUser(event);" />
                            </div>
                        </div>
                    </div>

                    <div class="col-md-4">
                        <div class="form-group">
                            <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>Server Port:</label>
                            <div class="col-sm-8">
                                <input type="text" class="form-control" maxlength="10" runat="server" name="ServerPort" id="txtServerPort" onkeypress="return FunChkIsNumber(event);" />
                            </div>
                        </div>
                    </div>

                    <div class="col-md-4">
                        <div class="form-group">
                            <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>Server IP:</label>
                            <div class="col-sm-8">
                                <input type="text" class="form-control" maxlength="50" runat="server" name="ServerIP" id="txtServerIP" onkeypress="return GetIP(event);" />
                            </div>
                        </div>
                    </div>

                </div>


               <div class="row">
                   <div class="col-md-4">
                        <div class="form-group">
                            <label for="inputName" id="lblPsw" class="col-xs-4 control-label"><span style="color: red;">*</span>Password:</label>
                            <div class="col-sm-8">
                                <input type="text" class="form-control" maxlength="70" runat="server" name="Password" id="txtPassword" />
                            </div>
                        </div>
                    </div>

                    
                    </div>

                <div class="box-body">

            <div class="box-header">
                <h4 class="box-title">Card Fee Process Configuration :</h4>
            </div>

            <div class="form-horizontal">

            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>DateCriteria:</label>
                        <div class="col-sm-8">
                            <input type="text" class="form-control" runat="server" name="DateCriteria" id="txtDateCriteria" maxlength="3" onkeypress="return FunChkIsNumber(event);" />
                        </div>
                    </div>
                </div>

                <div class="col-md-4">
                    <div class="form-group">
                        <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>File Name Format:</label>
                        <div class="col-sm-8">
                            <input type="text" class="form-control" runat="server" name="FileNameFormat" maxlength="200" id="txtFileNameFormat" onkeypress="return FileNameFormat(event);" />
                        </div>
                    </div>
                </div>

                <div class="col-md-4">
                    <div class="form-group">
                        <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>Catagorywise Sequence:</label>
                        <div class="col-sm-8">
                            <input type="text" class="form-control" runat="server" name="Sequence" id="txtSequence" maxlength="5" onkeypress="return FunChkIsNumber(event);" />
                        </div>
                    </div>
                </div>
            </div>
                

            <%--return IsAlphaCapital(event);--%>
            <div class="row">
                    <div class="col-md-4">
                        <div class="form-group">
                            <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>Status:</label>
                            <div class="col-sm-8">
                                <input type="text" class="form-control" maxlength="2" runat="server" name="Status" id="txtStatus" onkeypress="return FunChkIsNumber(event);" />
                            </div>
                        </div>
                    </div>

                </div>
                </div>
                    </div>

                <div class="row">
                                    <div class="col-md-4">
                                        <div class="form-group">
                                            <div class="col-sm-4"></div>

                                            <div class="col-sm-3">
                                                <div class="col-md-8">
                                                    <asp:Button runat="server" CssClass="btn btn-primary pull-right" ID="btnsave" Text="Save" OnClick="btnSave_Click" OnClientClick="return FunSaveValidation();" />
                                                </div>
                                            </div>
                                            <div class="col-sm-3">
                        <div class="col-md-8">
                            <asp:Button runat="server" CssClass="btn btn-primary pull-right" ID="btnDelete" Text="Delete" OnClick="btnDelete_Click" OnClientClick="return confirm_user();" />
                        </div>
                    </div>


                                            <div class="col-sm-4"></div>
                                        </div>
                                    </div>
                                </div>


                <%--<div class="row">
            <div class="col-md-4">
                <div class="form-group">
                    <%--<div class="col-sm-4"></div>--%>

                    <%--<div class="col-sm-3">
                        <div class="col-md-8">
                            <asp:Button runat="server" CssClass="btn btn-primary pull-right" ID="btnsave" Text="Save" OnClick="btnSave_Click" OnClientClick="return FunSaveValidation();" />
                        </div>
                    </div>
                    
                    <div class="col-sm-3">
                        <div class="col-md-8">
                            <asp:Button runat="server" CssClass="btn btn-primary pull-right" ID="btnDelete" Text="Delete" OnClick="btnDelete_Click" OnClientClick="return confirm_user();" />
                        </div>
                    </div>


                </div>
            </div>
        </div>--%>

       </div>
    </div>

         <%--</div>
        </div>
     </div>--%>
    </asp:Panel>

    <div id="shader" class="shader">
        <div class="loadingContainer">
            <div id="divLoading3">
                <div id="loader-wrapper">
                    <div id="loader"></div>
                </div>
            </div>
        </div>
    </div>


    <div id="memberModal" class="modal fade bs-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel">
        <div class="modal-dialog modal-md">
            <div class="modal-content" style="border-radius: 6px">
                <div class="modal-header box-primary" style="border-top-right-radius: 6px; border-top-left-radius: 6px">
                    <button aria-label="Close" data-dismiss="modal" class="close" type="button"><span aria-hidden="true">×</span></button>
                    <h4 id="myLargeModalLabel" class="modal-title" style="font-weight: bold">Card FeeConfigure Data</h4>
                </div>
                <div class="modal-body" id="msgBody">
                    <asp:Label ID="LblMessage" runat="server"></asp:Label>
                    <%--<asp:Label ID="LblCardRPANMessage" runat="server"></asp:Label>--%>
                </div>
                <div class="modal-footer">
                    <button aria-label="Close" data-dismiss="modal" id="BtnModalClose" class="btn btn-primary" type="button"><span aria-hidden="true">OK</span></button>
                </div>
            </div>
        </div>
        <!-- /.modal-content -->
    </div>


    <script>
        //Validation on Search Button
        function FunSearchValidation() {
            var SearchIssuerNo = $('#phPageBody_ddlSearchIssuerNo').val()
            var FileCatagory = $('#phPageBody_ddlFileCatagory').val()
            if (SearchIssuerNo == "0") {
                $('#SpnErrorMsg').html('Please select bank');
                $('#errormsgDiv').show();
                return false;
            }

            if (FileCatagory == "0") {
                $('#SpnErrorMsg').html('Please select file catagory');
                $('#errormsgDiv').show();
                return false;
            }
           else {
                $('#errormsgDiv').hide();
                $('.shader').fadeIn();
            }
        }
    </script>

    <script>
        function FunSaveValidation() {

            var errrorTextPD = 'Please provide :  ';
            var errorFieldsPD = '';
           // var RCVREmailID = $('#phPageBody_txtRCVREmailID').val()
           // var eml = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;


            if ($("#phPageBody_txtIssuerNo").val() == "") {
                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>, Issuer No</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>Issuer No</b> ';
                }
                $('#<%=txtIssuerNo.ClientID%>').focus();
            }

            if ($("#phPageBody_txtSequenceNo").val() == "") {
                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>,Sequence No</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>Sequence No</b> ';
                }
                $('#<%=txtSequenceNo.ClientID%>').focus();
            }

            if ($("#phPageBody_ddlBankstatus").text() == "--Select--") {
            errortab = '1';

            if (errorFieldsPD != '') {
                errorFieldsPD = errorFieldsPD + '<b>,Bank Status</b> ';

            }
            else {
                errorFieldsPD = errorFieldsPD + '<b>Bank Status</b> ';

            }

            $('#<%=ddlBankstatus.ClientID%>').focus();
                 }

            if ($("#phPageBody_txtSFTPIncoming").val() == "") {
                errortab = '1';

                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>,Input File Path</b> ';

                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>Input File Path</b> ';

                }
                $('#<%=txtSFTPIncoming.ClientID%>').focus();
            }

            if ($("#phPageBody_txtSFTPOutput").val() == "") {
                errortab = '1';

                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>,Output File Path</b> ';

                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>Output File Path</b> ';

                }
                $('#<%=txtSFTPOutput.ClientID%>').focus();
            }

            if ($("#phPageBody_txtSFTPRejected").val() == "") {
                errortab = '1';

                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>,Rejected File Path</b> ';

                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>Rejected File Path</b> ';

                }
                $('#<%=txtSFTPRejected.ClientID%>').focus();
            }

            if ($("#phPageBody_txtUserName").val() == "") {
                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>,UserName</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>UserName</b> ';
                }
                $('#<%=txtUserName.ClientID%>').focus();
            }

            if ($("#phPageBody_txtServerPort").val() == "") {
                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>,Server Port</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>Server Port</b> ';
                }
                $('#<%=txtServerPort.ClientID%>').focus();
            }

            if ($("#phPageBody_txtServerIP").val() == "") {
                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>,Server IP</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>Server IP</b> ';
                }
                $('#<%=txtServerIP.ClientID%>').focus();
            }

            if ($("#phPageBody_txtPassword").val() == "") {
                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>,Password</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>Password</b> ';
                }
                $('#<%=txtPassword.ClientID%>').focus();
            }
            
            if ($("#phPageBody_txtDateCriteria").val() == "") {
                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>,Date Criteria</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>Date Criteria</b> ';
                }
                $('#<%=txtDateCriteria.ClientID%>').focus();
            }

            
            if ($("#phPageBody_txtFileNameFormat").val() == "") {
                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>,File Name Format</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>File Name Format</b> ';
                }
                $('#<%=txtFileNameFormat.ClientID%>').focus();
            }
            if ($("#phPageBody_txtSequence").val() == "") {
                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>,Sequence By File Catagory</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>Sequence By File Catagory</b> ';
                }
                $('#<%=txtSequence.ClientID%>').focus();
            }
            if ($("#phPageBody_txtStatus").val() == "") {
                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>,Status</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>Status</b> ';
                }
                $('#<%=txtStatus.ClientID%>').focus();
             }

            
            if (errorFieldsPD != '') {
                $('#SpnErrorMsg').html(errrorTextPD + errorFieldsPD);
                //window.scrollTo = function (x, y) { return true; };
                $('#errormsgDiv').show();
                $("html, body").animate({ scrollTop: 0 }, 600);
                return false;

                //return false;
            }
            else {
                errorFieldsPD = '';
                errrorTextSectionPD = '';
                errrorTextPD = '';
                $('#errormsgDiv').hide();

                // var ArrayIds = [];
                //$("[id*=SystemList] input:checked").each(function (i) {
                //    ArrayIds[i] = $(this).val();



                //alert(val.join(","))

                $('.shader').fadeIn();
            }
        }
</script>

     <script>


         function FunShowMsg() {
             //  setTimeout($('#myModal').modal('show'),2000);
             $('#memberModal').modal('show');
             //window.location.reload();

         }

         function Hidemodel() {
             //7-11
             $('#memberModal').modal('hide');
         }
    </script>
    <script>
        //to prevent model closing when click outside
        $('#memberModal').modal({
            backdrop: 'static',
            keyboard: false
        })
    </script>

     <script>
         function confirm_user() {
             if (confirm("Are you sure you want to delete the Record") == true)
                 return true;
             else
                 return false;
         }


    </script>


    <%--<script>
        $(document).ready(function () {
            $(function () {
                // $(function () {

                $('#phPageBody_btnSearch').click(function () {
                    //$('[id$="txtPassword"]').hide();
                    //$('#lblPsw').hide();
                    $('#lblPsw').css('display', 'none');
                    alert("asd")

                });

            });
        });

  </script>--%>
</asp:Content>
