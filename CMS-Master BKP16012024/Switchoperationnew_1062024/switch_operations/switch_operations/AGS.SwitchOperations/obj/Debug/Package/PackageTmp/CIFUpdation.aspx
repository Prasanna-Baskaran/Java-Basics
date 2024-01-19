<%@ Page Title="" Language="C#" MasterPageFile="~/SwitchOperationSite.Master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true" CodeBehind="CIFUpdation.aspx.cs" Inherits="AGS.SwitchOperations.CIFUpdation" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phPageHeader" runat="server">
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phPageBody" runat="server">
    <asp:HiddenField ID="hdnAccessCaption" runat="server" />
    <asp:HiddenField ID="hdnFlag" runat="server" />
    <asp:HiddenField ID="hdnPGP" runat="server" />

    <asp:Panel ID="pnlCIFBanksearch" runat="server">
        <%--<div style="font-size: 14px;">--%>
            <div id="SearchBank">
                <div class="box-primary">
                    <div class="box-header with-border">
                        <i class="fa fa-credit-card"></i>

                        <h4 class="box-title">CIF Data Updation:</h4>
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
                                            <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>IssuerNo:</label>
                                            <div class="col-sm-8">
                                                <input type="text" class="form-control" maxlength="50" runat="server" name="SearchIssuerNo" id="txtSearchIssuerNo" onkeypress="return FunChkIsNumber(event);" />

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
                                           <%-- <div class="col-sm-3">
                                                <div class="col-md-8">

                                            <asp:Button runat="server" CssClass="btn btn-primary pull-right" ID="Button1" Text="Reset" OnClick="btnReset_Click" />
                                            </div>
                                                </div>
                                                    <div class="col-sm-4"></div>--%>
                                        </div>
                                    </div>
                                </div>


                            </div>
                        </div>
                    </div>
                </div>
            </div>
        <%--</div>--%>
    </asp:Panel>

<asp:Panel ID="pnlCIFUpadationSave" runat="server">

    <div class="box-body" id="BankSaveDiv">

            <div class="box-header">
                <h4 class="box-title">Bank Details:</h4>
            </div>

            <div class="form-horizontal">

    <div class="row">
        <div class="col-md-4">
                        <div class="form-group">
                            <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>IssuerNo:</label>
                            <div class="col-sm-8">
                                <input type="text" class="form-control" maxlength="50" runat="server" name="IssuerNo" id="txtIssuerNo" onkeypress="return FunChkIsNumber(event);" />
                            </div>
                        </div>
                    </div>
    </div>
</div>
        </div>

    <div class="box-body" id="CIFSFTPSaveDiv">

            <div class="box-header">
                <h4 class="box-title">CIF SFTP Server Details:</h4>
            </div>

            <div class="form-horizontal">

        <div class="row">
                    <div class="col-md-4">
                        <div class="form-group">
                            <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>SFTP UserName:</label>
                            <div class="col-sm-8">
                                <input type="text" class="form-control" maxlength="70" runat="server" name="SFTPUser" id="txtSFTPUser" onkeypress="return GetUser(event);" />
                            </div>
                        </div>
                    </div>

                    <div class="col-md-4">
                        <div class="form-group">
                            <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>SFTP Password:</label>
                            <div class="col-sm-8">
                                <input type="text" class="form-control" maxlength="50" runat="server" name="SFTPPassword" id="txtSFTPPassword" />
                            </div>
                        </div>
                    </div>

                    <div class="col-md-4">
                        <div class="form-group">
                            <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>SFTP Server:</label>
                            <div class="col-sm-8">
                                <input type="text" class="form-control" maxlength="20" runat="server" name="SFTPServer" id="txtSFTPServer" onkeypress="return GetIP(event);" />
                            </div>
                        </div>
                    </div>

                </div>

                <div class="row">
                    <div class="col-md-4">
                        <div class="form-group">
                            <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>SFTP Port:</label>
                            <div class="col-sm-8">
                                <input type="text" class="form-control" maxlength="6" runat="server" name="SFTPPort" id="txtSFTPPort" onkeypress="return FunChkIsNumber(event);" />
                            </div>
                        </div>
                    </div>
                    

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

                    </div>
                    
                <div class="row">
        <div class="col-md-4">
                    <div class="form-group">
                        <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>Archive FilePath:</label>
                        <div class="col-sm-8">
                            <input type="text" class="form-control" runat="server" name="SFTPArchive" maxlength="500" id="txtSFTPArchive" onkeypress="return GetSFTP_FolderPath(event);" />
                        </div>
                    </div>
                </div>

                <div class="col-md-4">
                    <div class="form-group">
                        <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>Enable State:</label>
                        <div class="col-sm-8">
                           <asp:DropDownList ID="ddlEnable" runat="server" class="form-control" Style="width: 100%">
                                <asp:ListItem Text="--Select--" Value="-1"></asp:ListItem>
                                <asp:ListItem Text="True" Value="1"></asp:ListItem>
                                <asp:ListItem Text="False" Value="0"></asp:ListItem>
                            </asp:DropDownList>  
                        </div>
                    </div>
                </div>
    </div>

    </div>
            </div>


        <div class="box-body" id="CIFFileSwitchserverDiv">

            <div class="box-header">
                <h4 class="box-title">CIF File Server Details:</h4>
            </div>

            <div class="form-horizontal">

                <div class="row">
                    <div class="col-md-4">
                        <div class="form-group">
                            <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>File Path:</label>
                            <div class="col-sm-8">
                                <input type="text" class="form-control" maxlength="500" runat="server" name="FilePath" id="txtFilePath" onkeypress="return GetFileFolderPath(event);" />

                            </div>
                        </div>
                    </div>
                    
                   
           <div class="col-md-6">
                        <div class="form-group">
                            <label for="inputName" class="col-xs-3 control-label">File Header:</label>
                            <div class="col-sm-9">
                                <input type="text" class="form-control" runat="server" name="FileHeader" id="txtFileHeader" onkeypress="return GetHeaderCIF(event);"/>
                            </div>
                        </div>
                    </div>
                    </div>
                </div>
            </div>

                    
                


                <%--return IsAlphaCapital(event);--%>
                
        <div class="box-body" id="CardRepinsftpSavediv">

            <div class="box-header">
                <h4 class="box-title">CIF Repin SFTP Server Details:</h4>
            </div>

            <div class="form-horizontal">

                <div class="row">
                    <div class="col-md-4">
                    <div class="form-group">
                        <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>Repin Input FilePath:</label>
                        <div class="col-sm-8">
                            <input type="text" class="form-control" runat="server" name="SFTPRepinInput" maxlength="500" id="txtSFTPRepinInput" onkeypress="return GetSFTP_FolderPath(event);" />
                        </div>
                    </div>
                </div>

                <div class="col-md-4">
                    <div class="form-group">
                        <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>Repin Output FilePath:</label>
                        <div class="col-sm-8">
                            <input type="text" class="form-control" runat="server" name="SFTPRepinOutput" maxlength="500" id="txtSFTPRepinOutput" onkeypress="return GetSFTP_FolderPath(event);" />
                        </div>
                    </div>
                </div>

                    
                     <div class="col-md-4">
                    <div class="form-group">
                        <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>Repin Archive FilePath:</label>
                        <div class="col-sm-8">
                            <input type="text" class="form-control" runat="server" name="SFTPRepinArchive" maxlength="500" id="txtSFTPRepinArchive" onkeypress="return GetSFTP_FolderPath(event);" />
                        </div>
                    </div>
                </div>
</div>

            </div>
        </div>

        <div class="box-body" id="CardRepinSwitchServerSavediv">

            <div class="box-header">
                <h4 class="box-title">CIF Repin Server Details:</h4>
            </div>

            <div class="form-horizontal">

        <div class="row">
                    <div class="col-md-4">
                        <div class="form-group">
                            <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>Repin File Path:</label>
                            <div class="col-sm-8">
                                <input type="text" class="form-control" maxlength="500" runat="server" name="RePINFilePath" id="txtRePINFilePath" onkeypress="return GetFileFolderPath(event);" />

                            </div>
                        </div>
                    </div>
                    
                   
           <div class="col-md-4">
                        <div class="form-group">
                            <label for="inputName" class="col-xs-4 control-label">Repin File Header:</label>
                            <div class="col-sm-8">
                                <input type="text" class="form-control" runat="server" name="RePINFileHeader" id="txtRePINFileHeader" onkeypress="return GetHeaderCIF(event);"/>
                            </div>
                        </div>
                    </div>

            <div class="col-md-4">
                        <div class="form-group">
                            <label for="inputName" class="col-xs-4 control-label">Trace:</label>
                            <div class="col-sm-8">
                              <asp:DropDownList ID="ddlTrace" runat="server" class="form-control" Style="width: 100%">
                                <asp:ListItem Text="--Select--" Value="-1"></asp:ListItem>
                                <asp:ListItem Text="True" Value="1"></asp:ListItem>
                                <asp:ListItem Text="False" Value="0"></asp:ListItem>
                            </asp:DropDownList>  
                            </div>
                        </div>
                    </div>

                    </div>

                <div class="row">
                    <div class="col-md-4">
                        <div class="form-group">
                            <label for="inputName" class="col-xs-4 control-label">IsPGP:</label>
                            <div class="col-sm-8">
                              <asp:DropDownList ID="ddlPGP" runat="server" class="form-control" Style="width: 100%" OnSelectedIndexChanged="ddlPGP_SelectedIndexChanged" AutoPostBack="True">
                                <asp:ListItem Text="--Select--" Value="-1"></asp:ListItem>
                                <asp:ListItem Text="True" Value="1"></asp:ListItem>
                                <asp:ListItem Text="False" Value="0"></asp:ListItem>
                            </asp:DropDownList>  

                            </div>

                            
                        </div>
                    </div>

                </div>
                </div>
            </div>

        

        <div class="box-body" id="PGPDetailsSaveDiv">

            <div class="box-header">
                <h4 class="box-title">CIF File PGP Details:</h4>
            </div>

            <div class="form-horizontal">

        <div class="row">
                    <div class="col-md-4">
                    <div class="form-group">
                        <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>PGP PublicKey FilePath:</label>
                        <div class="col-sm-8">
                            <input type="text" class="form-control" runat="server" name="PGPPublicKeyFilePath" maxlength="500" id="txtPGPPublicKeyFilePath" onkeypress="return GetFileFolderPath(event);" />
                        </div>
                    </div>
                </div>

                <div class="col-md-4">
                    <div class="form-group">
                        <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>PGP Private KeyFilePath:</label>
                        <div class="col-sm-8">
                            <input type="text" class="form-control" runat="server" name="PGPPrivateKeyFilePath" maxlength="500" id="txtPGPPrivateKeyFilePath" onkeypress="return GetFileFolderPath(event);" />
                        </div>
                    </div>
                </div>

                    
                     <div class="col-md-4">
                    <div class="form-group">
                        <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>PGP Input FilePath:</label>
                        <div class="col-sm-8">
                            <input type="text" class="form-control" runat="server" name="PGPInputFilePath" maxlength="500" id="txtPGPInputFilePath" onkeypress="return GetFileFolderPath(event);" />
                        </div>
                    </div>
                </div>
</div>
                <div class="row">
                    <div class="col-md-4">
                        <div class="form-group">
                            <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>PGP Password:</label>
                            <div class="col-sm-8">
                                <input type="text" class="form-control" maxlength="50" runat="server" name="PGPPassword" id="txtPGPPassword" />
                            </div>
                        </div>
                    </div>
                </div>
                
                </div>
            </div>

        

        
        <div class="row">
            <div class="col-md-4">
                <div class="form-group">
                    <%--<div class="col-sm-4"></div>--%>

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


                </div>
            </div>
        </div>
</asp:Panel>


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
         $(document).ready(function () {


             <%--if ($("#<%=ddlPGP.ClientID %>").val() == "1") {
                 $('#PGPDetailsSaveDiv').show();
             }
             else if ($('#phPageBody_ddlPGP').val() == "0") {

                 $('#PGPDetailsSaveDiv').hide();
             }--%>

             if ($("#<%=hdnPGP.ClientID %>").val() == "1") {

                 $('#PGPDetailsSaveDiv').show();
             }
             else if ($("#<%=hdnPGP.ClientID %>").val() == "0" || $("#<%=hdnPGP.ClientID %>").val() == "") {

                $('#PGPDetailsSaveDiv').hide();
            }
             else if ($("#<%=hdnFlag.ClientID %>").val() == "1"){
                $('#PGPDetailsSaveDiv').hide();
            }
         });
</script>
    <script>
        //Validation on Search Button
        function FunSearchValidation() {
            var SearchIssuerNo = $('#phPageBody_txtSearchIssuerNo').val()
           
            if (SearchIssuerNo == "") {
                $('#SpnErrorMsg').html('Please provide numeric issuer no');
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

    

    <script>
         function FunSaveValidation() {

             var errrorTextPD = 'Please provide :  ';
             var errorFieldsPD = '';
             // var RCVREmailID = $('#phPageBody_txtRCVREmailID').val()
             // var eml = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;


             <%--if ($("#phPageBody_txtSearchIssuerNo").val() == "") {
                 errortab = '1';
                 if (errorFieldsPD != '') {
                     errorFieldsPD = errorFieldsPD + '<b>, Issuer No</b> ';
                 }
                 else {
                     errorFieldsPD = errorFieldsPD + '<b>Issuer No</b> ';
                 }


                 $('#<%=txtSearchIssuerNo.ClientID%>').focus();
             }--%>

             if ($("#phPageBody_txtSFTPUser").val() == "") {
                 errortab = '1';
                 if (errorFieldsPD != '') {
                     errorFieldsPD = errorFieldsPD + '<b>,SFTP UserName</b> ';
                 }
                 else {
                     errorFieldsPD = errorFieldsPD + '<b>SFTP UserName</b> ';
                 }
                 $('#<%=txtSFTPUser.ClientID%>').focus();
             }

             if ($("#phPageBody_txtSFTPPassword").val() == "") {
                 errortab = '1';
                 if (errorFieldsPD != '') {
                     errorFieldsPD = errorFieldsPD + '<b>,SFTP Password</b> ';
                 }
                 else {
                     errorFieldsPD = errorFieldsPD + '<b>SFTP Password</b> ';
                 }
                 $('#<%=txtSFTPPassword.ClientID%>').focus();
            }

             if ($("#phPageBody_txtSFTPServer").val() == "") {
                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>,SFTP Server</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>SFTP Server</b> ';
                }
                $('#<%=txtSFTPServer.ClientID%>').focus();
            }

             if ($("#phPageBody_txtSFTPPort").val() == "") {
                errortab = '1';

                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>,SFTP Port</b> ';

                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>SFTP Port</b> ';

                }

                $('#<%=txtSFTPPort.ClientID%>').focus();
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

            if ($("#phPageBody_txtSFTPArchive").val() == "") {
                errortab = '1';

                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>,Archive File Path</b> ';

                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>Archive File Path</b> ';

                }
                $('#<%=txtSFTPArchive.ClientID%>').focus();
            }

            

            if ($("#phPageBody_ddlEnable").val() == "-1") {
                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>,Enable State</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>Enable State</b> ';
                }
                $('#<%=ddlEnable.ClientID%>').focus();
            }

            if ($("#phPageBody_txtFilePath").val() == "") {
                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>,File Path</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>File Path</b> ';
                }
                $('#<%=txtFilePath.ClientID%>').focus();
            }

            if ($("#phPageBody_txtFileHeader").val() == "") {
                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>,File Header</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>File Header</b> ';
                }
                $('#<%=txtFileHeader.ClientID%>').focus();
            }


            if ($("#phPageBody_txtSFTPRepinInput").val() == "") {
                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>,Repin Input File Path</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>Repin Input File Path</b> ';
                }
                $('#<%=txtSFTPRepinInput.ClientID%>').focus();
            }
            if ($("#phPageBody_txtSFTPRepinOutput").val() == "") {
                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>,Repin Output File Path</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>Repin Output File Path</b> ';
                }
                $('#<%=txtSFTPRepinOutput.ClientID%>').focus();
            }
            if ($("#phPageBody_txtSFTPRepinArchive").val() == "") {
                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>,Repin Archive File Path</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>Repin Archive File Path</b> ';
                }
                $('#<%=txtSFTPRepinArchive.ClientID%>').focus();
            }

            if ($("#phPageBody_txtRePINFilePath").val() == "") {
                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>,Repin File Path</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>Repin File Path</b> ';
                }
                $('#<%=txtRePINFilePath.ClientID%>').focus();
            }
            
            if ($("#phPageBody_txtRePINFileHeader").val() == "") {
                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>,Repin File Header</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>Repin File Header</b> ';
                }
                $('#<%=txtRePINFileHeader.ClientID%>').focus();
             }

            

            if ($("#phPageBody_ddlTrace").val() == "-1") {
                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>,Trace Field</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>Trace Field</b> ';
                }
                $('#<%=ddlTrace.ClientID%>').focus();
            }
            if ($("#phPageBody_ddlPGP").val() == "-1") {
                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>,IsPGP Field</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>IsPGP Field</b> ';
                }
                $('#<%=ddlPGP.ClientID%>').focus();
             }

            if ($("#phPageBody_ddlPGP").val() == "1") {
                if ($("#phPageBody_txtPGPPassword").val() == "") {
                    errortab = '1';
                    if (errorFieldsPD != '') {
                        errorFieldsPD = errorFieldsPD + '<b>,PGP Password</b> ';
                    }
                    else {
                        errorFieldsPD = errorFieldsPD + '<b>PGP Password</b> ';
                    }
                    $('#<%=txtPGPPassword.ClientID%>').focus();
                }
                if ($("#phPageBody_txtPGPInputFilePath").val() == "") {
                    errortab = '1';
                    if (errorFieldsPD != '') {
                        errorFieldsPD = errorFieldsPD + '<b>,PGP Input File Path</b> ';
                    }
                    else {
                        errorFieldsPD = errorFieldsPD + '<b>PGP Input File Path</b> ';
                    }
                    $('#<%=txtPGPInputFilePath.ClientID%>').focus();
                }

                if ($("#phPageBody_txtPGPPublicKeyFilePath").val() == "") {
                    errortab = '1';
                    if (errorFieldsPD != '') {
                        errorFieldsPD = errorFieldsPD + '<b>,PGP PublicKey File Path</b> ';
                    }
                    else {
                        errorFieldsPD = errorFieldsPD + '<b>PGP PublicKey File Path</b> ';
                    }
                    $('#<%=txtPGPPublicKeyFilePath.ClientID%>').focus();
                }
                if ($("#phPageBody_txtPGPPrivateKeyFilePath").val() == "") {
                    errortab = '1';
                    if (errorFieldsPD != '') {
                        errorFieldsPD = errorFieldsPD + '<b>,PGP PrivateKey File Path</b> ';
                    }
                    else {
                        errorFieldsPD = errorFieldsPD + '<b>PGP PrivateKey File Path</b> ';
                    }
                    $('#<%=txtPGPPrivateKeyFilePath.ClientID%>').focus();
                }
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



</asp:Content>
