<%@ Page Title="" Language="C#" MasterPageFile="~/SwitchOperationSite.Master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true" CodeBehind="BankConfigure.aspx.cs" Inherits="AGS.SwitchOperations.BankConfigure" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phPageHeader" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="phPageBody" runat="server">

    <!--All Hiddenfields------------------------------------------------------------------------->
    <asp:HiddenField ID="hdnAccessCaption" runat="server" />
    <asp:HiddenField ID="hdnFlag" runat="server" />

    <asp:Panel ID="pnlBankConfigureSearch" runat="server">
        <div style="font-size: 14px;">
            <div id="SearchBank">
                <div class="box-primary">
                    <div class="box-header with-border">
                        <i class="fa fa-credit-card"></i>

                        <h4 class="box-title">Bank Configure:</h4>
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

                                    <%--<div class="col-md-4">
                                        <div class="form-group">
                                            <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>CardPrefix:</label>
                                            <div class="col-sm-8">
                                                <input type="text" class="form-control" runat="server" maxlength="12" name="SearchCardPrefix" id="txtSearchCardPrefix" onkeypress="return FunChkIsNumber(event);" />

                                            </div>
                                        </div>
                                    </div>--%>

                                    <%--<div class="col-md-4">
                                        <div class="form-group">
                                            <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>CardProgram:</label>
                                            <div class="col-sm-8">
                                                <input type="text" class="form-control" runat="server" name="SearchCardProgram" id="txtSearchCardProgram" onkeypress="return FunChkAlphaNumeric(event);" />

                                            </div>
                                        </div>
                                    </div>--%>
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
                                            <div class="col-sm-3">
                                                <div class="col-md-8">

                                            <asp:Button runat="server" CssClass="btn btn-primary pull-right" ID="Button1" Text="Reset" OnClick="btnReset_Click" />
                                            </div>
                                                </div>
                                                    <div class="col-sm-4"></div>
                                        </div>
                                    </div>
                                </div>


                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>

    <%--tblbank param--%>

    <asp:Panel ID="pnlBankConfigureSave" runat="server">

        <div class="box-body" id="BankSavediv">

            <div class="box-header">
                <h4 class="box-title">Bank Data:</h4>
            </div>

            <div class="form-horizontal">

                <div class="row">
                    <div class="col-md-4">
                        <div class="form-group">
                            <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>BankName:</label>
                            <div class="col-sm-8">
                                <input type="text" class="form-control" maxlength="500" runat="server" name="BankName" id="txtBankName" onkeypress="return onlyAlphabets(event);" />

                            </div>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="form-group">
                            <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>IssuerNo:</label>
                            <div class="col-sm-8">
                                <input type="text" class="form-control" maxlength="50" runat="server" name="IssuerNo" id="txtIssuerNo" onkeypress="return FunChkIsNumber(event);" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="form-group">
                            <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>SystemID:</label>
                            <div class="col-sm-8">
                                <asp:DropDownList OnSelectedIndexChanged="SystemId_SelectedIndexChanged" ID="ddlSystemId" runat="server" class="form-control">
                                </asp:DropDownList>

                                <%--<input type="text" class="form-control" runat="server" name="SystemId" id="txtSystemID" onkeypress="return FunChkIsNumber(event);" />--%>
                            </div>
                        </div>
                    </div>
                </div>



                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label for="inputName" class="col-xs-3 control-label">SourceNodes:</label>
                            <div class="col-sm-9">
                                <input type="text" class="form-control" runat="server" name="SourceNodes" id="txtSourceNodes" onkeypress="return GetSourceSink(event);" />
                            </div>
                        </div>
                    </div>

                    <div class="col-md-6">
                        <div class="form-group">
                            <label for="inputName" class="col-xs-3 control-label">SinkNodes:</label>
                            <div class="col-sm-9">
                                <input type="text" class="form-control" runat="server" name="SinkNodes" id="txtSinkNodes" onkeypress="return GetSourceSink(event);" />
                            </div>
                        </div>
                    </div>
                </div>


                <%--return IsAlphaCapital(event);--%>
                <div class="row">

                    <div class="col-md-4">
                        <div class="form-group">
                            <label for="inputName" class="col-xs-4 control-label">UserPrefix:</label>
                            <div class="col-sm-8">
                                <input type="text" class="form-control" maxlength="10" runat="server" name="UserPrefix" id="txtUserPrefix" onkeypress="return onlyAlphabets(event);" />
                            </div>
                        </div>
                    </div>

                    <div class="col-md-4">
                        <div class="form-group">
                            <label for="inputName" class="col-xs-4 control-label">CustomerIdentity:</label>
                            <div class="col-sm-8">
                                <input type="text" class="form-control" maxlength="20" runat="server" name="CustIdentity" id="txtCustIdentity" onkeypress="return onlyAlphabets(event);" />

                            </div>
                        </div>
                    </div>


                    <div class="col-md-4">
                        <div class="form-group">
                            <label for="inputName" class="col-xs-4 control-label">CustomerIDLength:</label>
                            <div class="col-sm-8">
                                <input type="text" class="form-control" maxlength="20" runat="server" name="CustomerIDLen" id="txtCustomerIDLen" onkeypress="return FunChkIsNumber(event);" />
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </div>
        <%-- <div class="col-md-4">
                                <div class="form-group">
                                    <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>SystemID:</label>
                                    <div class="col-sm-8">
                                      <input type="text" class="form-control" maxlength="20" runat="server" name="SystemID" id="Text3" required="required" />                                      
                                    </div>
                                </div>
                            </div>--%>

        <%--tblCardautomation params--%>
        <div class="box-body" id="CardAutomationSavediv">

            <div class="box-header">
                <h4 class="box-title">Card Automation Data:</h4>
            </div>

            <div class="form-horizontal">

                <div class="row">
                    <div class="col-md-4">
                        <div class="form-group">
                            <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>BankFolder:</label>
                            <div class="col-sm-8">
                                <input type="text" class="form-control" runat="server" name="BankFolder" maxlength="40" id="txtBankFolder" onkeypress="return GetFolder(event);" />
                            </div>
                        </div>
                    </div>

                    <div class="col-md-4">
                        <div class="form-group">
                            <label for="inputName" class="col-xs-4 control-label">SwitchInstitutionID:</label>
                            <div class="col-sm-8">
                                <input type="text" class="form-control" maxlength="11" runat="server" name="SwitchInstitutionID" id="txtSwitchInstitutionID" onkeypress="return FunChkIsNumber(event);" />
                            </div>
                        </div>
                    </div>

                    <%--<div class="col-md-4">
                        <div class="form-group">
                            <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>Bank:</label>
                            <div class="col-sm-8">
                                <input type="text" class="form-control" runat="server" maxlength="1000" name="Bank" id="txtBank" onkeypress="return onlyAlphabets(event);" />
                            </div>
                        </div>
                    </div>--%>

                


                
                
                    <div class="col-md-4">
                        <div class="form-group">
                            <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>Receiver EmailID:</label>
                            <div class="col-sm-8">
                                <input type="text" class="form-control" maxlength="2000" runat="server" name="RCVREmailID" id="txtRCVREmailID" />
                            </div>
                        </div>
                    </div>
                </div>

            </div>
        </div>

        <div class="box-body" id="Winscpdiv">

            <div class="box-header">
                <h4 class="box-title">WIN SCP Server:</h4>
            </div>

            <div class="form-horizontal">

        <div class="row">
                    <div class="col-md-4">
                        <div class="form-group">
                            <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>WIN SCP User:</label>
                            <div class="col-sm-8">
                                <input type="text" class="form-control" maxlength="200" runat="server" name="WinSCP_User" id="txtWinSCP_User" onkeypress="return GetUser(event);" />
                            </div>
                        </div>
                    </div>

                    <div class="col-md-4">
                        <div class="form-group">
                            <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>WIN SCP Password:</label>
                            <div class="col-sm-8">
                                <input type="text" class="form-control" maxlength="200" runat="server" name="WinSCP_PWD" id="txtWinSCP_PWD" />
                            </div>
                        </div>
                    </div>

                    <div class="col-md-4">
                        <div class="form-group">
                            <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>WIN SCP IP:</label>
                            <div class="col-sm-8">
                                <input type="text" class="form-control" maxlength="100" runat="server" name="WinSCP_IP" id="txtWinSCP_IP" onkeypress="return GetIP(event);" />
                            </div>
                        </div>
                    </div>

                </div>

                <div class="row">
                    <div class="col-md-4">
                        <div class="form-group">
                            <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>WIN SCP Port:</label>
                            <div class="col-sm-8">
                                <input type="text" class="form-control" maxlength="20" runat="server" name="WinSCP_Port" id="txtWinSCP_Port" onkeypress="return FunChkIsNumber(event);" />
                            </div>
                        </div>
                    </div>
                    </div>
                </div>
            </div>

        <div class="box-body" id="AGSSftpSaveDiv">

            <div class="box-header">
                <h4 class="box-title">AGS SFTP Server:</h4>
            </div>

            <div class="form-horizontal">

        <div class="row">
                    <div class="col-md-4">
                        <div class="form-group">
                            <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>AGS SFTP User:</label>
                            <div class="col-sm-8">
                                <input type="text" class="form-control" maxlength="200" runat="server" name="AGS_SFTPUser" id="txtAGS_SFTP_User" onkeypress="return GetUser(event);" />
                            </div>
                        </div>
                    </div>

                    <div class="col-md-4">
                        <div class="form-group">
                            <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>AGS SFTP Password:</label>
                            <div class="col-sm-8">
                                <input type="text" class="form-control" maxlength="200" runat="server" name="AGS_SFTP_Pwd" id="txtAGS_SFTP_Pwd" />
                            </div>
                        </div>
                    </div>

                    <div class="col-md-4">
                        <div class="form-group">
                            <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>AGS SFTP Server:</label>
                            <div class="col-sm-8">
                                <input type="text" class="form-control" maxlength="100" runat="server" name="AGS_SFTPServer" id="txtAGS_SFTPServer" onkeypress="return GetIP(event);" />
                            </div>
                        </div>
                    </div>

                </div>

                <div class="row">
                    <div class="col-md-4">
                        <div class="form-group">
                            <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>AGS SFTP Port:</label>
                            <div class="col-sm-8">
                                <input type="text" class="form-control" maxlength="20" runat="server" name="AGS_SFTP_Port" id="txtAGS_SFTP_Port" onkeypress="return FunChkIsNumber(event);" />
                            </div>
                        </div>
                    </div>
                    </div>
                </div>
            </div>


        <div class="box-body" id="BankSftpSaveDiv">

            <div class="box-header">
                <h4 class="box-title">Bank Input SFTP Server:</h4>
            </div>

            <div class="form-horizontal">

        <div class="row">
                    <div class="col-md-4">
                        <div class="form-group">
                            <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>Bank SFTP User:</label>
                            <div class="col-sm-8">
                                <input type="text" class="form-control" maxlength="200" runat="server" name="B_SFTP_User" id="txtB_SFTP_User" onkeypress="return GetUser(event);" />
                            </div>
                        </div>
                    </div>

                    <div class="col-md-4">
                        <div class="form-group">
                            <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>Bank SFTP Password:</label>
                            <div class="col-sm-8">
                                <input type="text" class="form-control" maxlength="200" runat="server" name="B_SFTP_Pwd" id="txtB_SFTP_Pwd" />
                            </div>
                        </div>
                    </div>

                    <div class="col-md-4">
                        <div class="form-group">
                            <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>Bank SFTP Server:</label>
                            <div class="col-sm-8">
                                <input type="text" class="form-control" maxlength="100" runat="server" name="B_SFTPServer" id="txtB_SFTPServer" onkeypress="return GetIP(event);" />
                            </div>
                        </div>
                    </div>

                </div>

                <div class="row">
                    <div class="col-md-4">
                        <div class="form-group">
                            <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>Bank SFTP Port:</label>
                            <div class="col-sm-8">
                                <input type="text" class="form-control" maxlength="20" runat="server" name="B_SFTP_Port" id="txtB_SFTP_Port" onkeypress="return FunChkIsNumber(event);" />
                            </div>
                        </div>
                    </div>
                    </div>
                </div>
            </div>

        <div class="box-body" id="PREoutputsftpSave">

            <div class="box-header">
                <h4 class="box-title">PRE Output SFTP Server:</h4>
            </div>

            <div class="form-horizontal">

        <div class="row">
                    <div class="col-md-4">
                        <div class="form-group">
                            <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>PRE SFTP User:</label>
                            <div class="col-sm-8">
                                <input type="text" class="form-control" maxlength="200" runat="server" name="C_SFTP_User" id="txtC_SFTP_User" onkeypress="return GetUser(event);" />
                            </div>
                        </div>
                    </div>

                    <div class="col-md-4">
                        <div class="form-group">
                            <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>PRE SFTP Password:</label>
                            <div class="col-sm-8">
                                <input type="text" class="form-control" maxlength="200" runat="server" name="C_SFTP_Pwd" id="txtC_SFTP_Pwd" />
                            </div>
                        </div>
                    </div>

                    <div class="col-md-4">
                        <div class="form-group">
                            <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>PRE SFTP Server:</label>
                            <div class="col-sm-8">
                                <input type="text" class="form-control" maxlength="100" runat="server" name="C_SFTPServer" id="txtC_SFTPServer" onkeypress="return GetIP(event);" />
                            </div>
                        </div>
                    </div>

                </div>

                <div class="row">
                    <div class="col-md-4">
                        <div class="form-group">
                            <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>PRE SFTP Port:</label>
                            <div class="col-sm-8">
                                <input type="text" class="form-control" maxlength="20" runat="server" name="C_SFTP_Port" id="txtC_SFTP_Port" onkeypress="return FunChkIsNumber(event);" />
                            </div>
                        </div>
                    </div>
                    </div>
                </div>
            </div>

        <div class="box-body" id="CardAutoFilePathSavediv">

            <div class="box-header">
                <h4 class="box-title">CardAutomation FilePath Data:</h4>
            </div>

            <div class="form-horizontal">

                <div class="row">
                    <div class="col-md-4">
                        <div class="form-group">
                            <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>PGP KeyName:</label>
                            <div class="col-sm-8">
                                <input type="text" class="form-control" maxlength="200" runat="server" name="PGP_KeyName" id="txtPGP_KeyName" onkeypress="return onlyAlphabets(event);" />
                            </div>
                        </div>
                    </div>


                    <div class="col-md-4">
                        <div class="form-group">
                            <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>PGP Password:</label>
                            <div class="col-sm-8">
                                <input type="text" class="form-control" maxlength="200" runat="server" name="PGP_PWD" id="txtPGP_PWD" />
                            </div>
                        </div>
                    </div>




                    <div class="col-md-4">
                        <div class="form-group">
                            <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>AGSPGP KeyName:</label>
                            <div class="col-sm-8">
                                <input type="text" class="form-control" maxlength="200" runat="server" name="AGS_PGP_KeyName" id="txtAGS_PGP_KeyName" onkeypress="return onlyAlphabets(event);" />
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-4">
                        <div class="form-group">
                            <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>AGSPGP Password:</label>
                            <div class="col-sm-8">
                                <input type="text" class="form-control" maxlength="200" runat="server" name="AGS_PGP_PWD" id="txtAGS_PGP_PWD" />
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
                            <asp:Button runat="server"  CssClass="btn btn-primary pull-right" ID="btnsave" Text="Save" OnClick="btnSave_Click" OnClientClick="return FunSaveValidation();" />
                        </div>
                    </div>
                    <%--toTop('errormsgDiv');--%>
                    <div class="col-sm-3">
                        <div class="col-md-8">
                            <asp:Button runat="server" CssClass="btn btn-primary pull-right" ID="btnDelete" Text="Delete" OnClick="btnDelete_Click" OnClientClick="return confirm_user();" />
                        </div>
                    </div>


                </div>
            </div>
        </div>



        <%--</div>
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

    <%-- Response Msg --%>
    <div id="memberModal" class="modal fade bs-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel">
        <div class="modal-dialog modal-md">
            <!--<a data-controls-modal="your_div_id" data-backdrop="static" data-keyboard="false" 7/11 href="#">-->
            <div class="modal-content" style="border-radius: 6px">
                <div class="modal-header box-primary" style="border-top-right-radius: 6px; border-top-left-radius: 6px">
                    <button aria-label="Close" data-dismiss="modal" class="close" type="button"><span aria-hidden="true">×</span></button>
                    <h4 id="myLargeModalLabel" class="modal-title" style="font-weight: bold">Bank Configuration</h4>
                </div>
                <div class="modal-body" id="msgBody">
                    <asp:Label ID="LblMessage" runat="server"></asp:Label>
                </div>
                <div class="modal-footer">
                    <button aria-label="Close" data-dismiss="modal" id="BtnModalClose" class="btn btn-primary" type="button"><span aria-hidden="true">OK</span></button>
                </div>
            </div>
        </div>
        <!-- /.modal-content -->
    </div>


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
    <%--<script>
        $('#BtnModalClose').on('click', "btn-primary"), function () {
            location.reload();
        }
    </script>--%>
    <%--<script>
        $('#BtnModalClose').on('click', function (evt) {
           // window.location.reload();

            //setTimeout("location.reload(true);", 50);
        });
    </script>--%>

    <%--<script>
        function Reload()
        {
            location.reload();
        }
    </script>--%>

    <%--<script>
        jQuery('#memberModal').modal('show', {backdrop: 'static', keyboard: false});
    </script>--%>
    <%--<script>
        function myFunction() {
            location.reload();
        }
    </script>--%>











    <%--<script>
        function myFunction() {
                var elems = [];
                elems = elems.concat(document.getElementsByTagName("input"));
                elems = elems.concat(document.getElementsByTagName("text"));
                //and so on

                for (var i = 0, c = elems.length; i < c; i++) {
                    elems[i].value = "";
                }
            }
    </script>--%>

    <%--<script>

        function FunClearAllTextValue() {
            document.getElementById('txtIssuerNo').value = "";

            } 
        </script>--%>
    <%--<script>
    function FunClearAllTextValue() {

    var elements = [] ;
    elements = document.getElementsByClassName("form-control");

    for(var i=0; i<elements.length ; i++){
        elements[i].value = "";

    }

    } 
    </script>--%>

    <%--<script type="text/javascript">
        function CheckData(text) {
            $(text).val('');
        };
        onclick="return CheckData(this);"
    </script>--%>


    <script>
        function Clearall() {
            //7-11-17
            var elements = [];
            elements = document.getElementsByClassName("form-control");

            for (var i = 0; i < elements.length; i++) {
                elements[i].value = "";
            }

        }
    </script>

    <%--<script>
        function myFunction() {
    var objInput = document.getElementsByTagName("input");
    for (var iCount = 3; iCount < objInput.length; i++) {
        if (objInput[iCount].type == "text")
            objInput[iCount].value = "";
    }
    return false;
        }
        </script>--%>
    <%--<script>
        function myFunction() {
            var textBoxSet = $("input, [type='text']"); // assuming you want to cover 
            var index = 3;
            textBoxSet.each(function (index, element) {
                element.val('');
            });
        }
        </script>--%>
    <script>
        function toTop() {
            //document.getElementById(id).scrollTop=100
                    }
    </script>
   <%-- <script>
        $('#phPageBody_btnsave').click(function () {
            $("html, body").animate({ scrollTop: 0 }, 600);
            return false;
        });
    </script>--%>
    <%--<script>
window.scrollTo = function () { };
  
validationSummary.onpropertychange = function () {
     if (this.style.display != 'none') {
          validationSummary.scrollIntoView();
     }
}
    </script>     --%>
    <script>
        //Validation on Search Button
        function FunSearchValidation() {

            var IssuerNo = $('#phPageBody_txtSearchIssuerNo').val()
            var CardPrefix = $('#phPageBody_txtSearchCardPrefix').val()
            var CardProgram = $('#phPageBody_txtSearchCardProgram').val()

            if (IssuerNo == "") {
                $('#SpnErrorMsg').html('Please provide numeric IssuerNo');
                $('#errormsgDiv').show();
                return false;
            }
            //if (CardPrefix == "") {
            //    $('#SpnErrorMsg').html('Please provide cardprefix for bank');
            //    $('#errormsgDiv').show();
            //    return false;
            //}
            //if (CardProgram == "") {
            //    $('#SpnErrorMsg').html('Please provide cardprogram for bank');
            //    $('#errormsgDiv').show();
            //    return false;
            //}

            else {
                $('#errormsgDiv').hide();
                $('.shader').fadeIn();

            }


        }


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
            var RCVREmailID = $('#phPageBody_txtRCVREmailID').val()
            var eml = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;

           
            if ($("#phPageBody_txtBankName").val() == "") {
                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>, Bank Name</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>Bank Name</b> ';
                }
                $('#<%=txtBankName.ClientID%>').focus();
            }

            if ($("#phPageBody_txtIssuerNo").val() == "") {
                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>,Issuer No</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>Issuer No</b> ';
                }
                $('#<%=txtIssuerNo.ClientID%>').focus();
            }

        <%--if ($("phPageBody_txtBank").val() == "") {
            errortab = '1';

            if (errorFieldsPD != '') {
                errorFieldsPD = errorFieldsPD + '<b>,Bank</b> ';

            }
            else {
                errorFieldsPD = errorFieldsPD + '<b>Bank</b> ';

            }

            $('#<%=txtBank.ClientID%>').focus();
                 }--%>

            if ($("#phPageBody_txtBankFolder").val() == "") {
                errortab = '1';

                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>,BankFolder</b> ';

                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>BankFolder</b> ';

                }
                $('#<%=txtBankFolder.ClientID%>').focus();
            }


            <%--if ($("#phPageBody_txtRCVREmailID").val() == "") {
                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>,Receiver EmailId</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>Receiver EmailId</b> ';
                }
                $('#<%=txtRCVREmailID.ClientID%>').focus();
            }--%>

            if ($("#phPageBody_ddlSystemId").val() == "0") {
                errortab = '1';

                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>,SystemId</b> ';

                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>SystemId</b> ';

                }
                $('#<%=ddlSystemId.ClientID%>').focus();
            }
            if ($("#phPageBody_txtRCVREmailID").val() == "") {
                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>,Receiver Email</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>Receiver Email</b> ';
                }
                $('#<%=txtAGS_PGP_PWD.ClientID%>').focus();
             }
            if ($("#phPageBody_txtWinSCP_User").val() == "") {
                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>,WinSCP User</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>WinSCP User</b> ';
                }
                $('#<%=txtWinSCP_User.ClientID%>').focus();
            }

            if ($("#phPageBody_txtWinSCP_PWD").val() == "") {
                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>,WinSCP_Password</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>WinSCP_Password</b> ';
                }
                $('#<%=txtWinSCP_PWD.ClientID%>').focus();
            }

            if ($("#phPageBody_txtWinSCP_IP").val() == "") {
                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>,WinSCP IP</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>WinSCP IP</b> ';
                }
                $('#<%=txtWinSCP_IP.ClientID%>').focus();
            }
            if ($("#phPageBody_txtWinSCP_Port").val() == "") {
                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>,WinSCP Port</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>WinSCP Port</b> ';
                }
                $('#<%=txtWinSCP_Port.ClientID%>').focus();
            }
            //AGSSFTP
            if ($("#phPageBody_txtAGS_SFTP_User").val() == "") {
                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>,AGS SFTP User</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>AGS SFTP User</b> ';
                }
                $('#<%=txtAGS_SFTP_User.ClientID%>').focus();
            }
            if ($("#phPageBody_txtAGS_SFTP_Pwd").val() == "") {
                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>,AGS SFTP Password</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>AGS SFTP Password</b> ';
                }
                $('#<%=txtAGS_SFTP_Pwd.ClientID%>').focus();
            }

            if ($("#phPageBody_txtAGS_SFTPServer").val() == "") {
                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>,AGS SFTP Server</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>AGS SFTP Server</b> ';
                }
                $('#<%=txtAGS_SFTPServer.ClientID%>').focus();
            }

            if ($("#phPageBody_txtAGS_SFTP_Port").val() == "") {
                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>,AGS SFTP Port</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>AGS SFTP Port</b> ';
                }
                $('#<%=txtAGS_SFTP_Port.ClientID%>').focus();
            }
            
            
            //Bank SFTP
            if ($("#phPageBody_txtB_SFTP_User").val() == "") {
                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>,Bank SFTP User</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>Bank SFTP User</b> ';
                }
                $('#<%=txtB_SFTP_User.ClientID%>').focus();
            }
            if ($("#phPageBody_txtB_SFTP_Pwd").val() == "") {
                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>,Bank SFTP Password</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>Bank SFTP Password</b> ';
                }
                $('#<%=txtB_SFTP_Pwd.ClientID%>').focus();
            }
            
            if ($("#phPageBody_txtB_SFTPServer").val() == "") {
                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>,Bank SFTP Server</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>Bank SFTP Server</b> ';
                }
                $('#<%=txtB_SFTPServer.ClientID%>').focus();
            }
            if ($("#phPageBody_txtB_SFTP_Port").val() == "") {
                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>,Bank SFTP Port</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>Bank SFTP Port</b> ';
                }
                $('#<%=txtB_SFTP_Port.ClientID%>').focus();
            }
            

            //PRE Server
            if ($("#phPageBody_txtC_SFTP_User").val() == "") {
                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>,PRE SFTP User</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>PRE SFTP User</b> ';
                }
                $('#<%=txtC_SFTP_User.ClientID%>').focus();
            }
            if ($("#phPageBody_txtC_SFTP_Pwd").val() == "") {
                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>,PRE SFTP Password</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>PRE SFTP Password</b> ';
                }
                $('#<%=txtC_SFTP_Pwd.ClientID%>').focus();
            }
            

            if ($("#phPageBody_txtC_SFTPServer").val() == "") {
                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>,PRE SFTP Server</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>PRE SFTP Server</b> ';
                }
                $('#<%=txtC_SFTPServer.ClientID%>').focus();
            }
            if ($("#phPageBody_txtC_SFTP_Port").val() == "") {
                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>,PRE SFTP Port</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>PRE SFTP Port</b> ';
                }
                $('#<%=txtC_SFTP_Port.ClientID%>').focus();
            }
            
            //PGP
            if ($("#phPageBody_txtPGP_KeyName").val() == "") {
                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>,PGP KeyName</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>PGP KeyName</b> ';
                }
                $('#<%=txtPGP_KeyName.ClientID%>').focus();
            }
            if ($("#phPageBody_txtPGP_PWD").val() == "") {
                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>,PGP Password</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>PGP Password</b> ';
                }
                $('#<%=txtPGP_PWD.ClientID%>').focus();
            }

            if ($("#phPageBody_txtAGS_PGP_KeyName").val() == "") {
                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>, AGS PGP KeyName</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>AGS PGP KeyName</b> ';
                }
                $('#<%=txtAGS_PGP_KeyName.ClientID%>').focus();
            }



            if ($("#phPageBody_txtAGS_PGP_PWD").val() == "") {
                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>,AGS PGP Password</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>PRE SFTP Password</b> ';
                }
                $('#<%=txtAGS_PGP_PWD.ClientID%>').focus();
            }

           


            else if (eml.test($.trim($('#phPageBody_txtRCVREmailID').val())) == false ) 

            {
                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>,Proper Receiver Email</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>Proper Receiver Email</b> ';
                }
                $('#<%=txtAGS_PGP_PWD.ClientID%>').focus();
            }
            
            //                            $('#SpnErrorMsg').html('Please provide correct emailid');
            //                            $('#errormsgDiv').show();
            //                            return false;


            //if ($('#phPageBody_txtIssuerNo').val() != $('#phPageBody_txtSearchIssuerNo').val()) {
            //    $('#SpnErrorMsgForModel').html('Issuer no does not match with search');
            //    $('#errormsgDivForModel').show();
            //    return false;
            //}

            if (errorFieldsPD != '') {

                $('#SpnErrorMsg').html(errrorTextPD + errorFieldsPD);
                //window.scrollTo = function (x, y) { return true; };
                $('#errormsgDiv').show();
                $("html, body").animate({ scrollTop: 0 }, 600);
                return false;

               // return false;
                
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

    


            <%--//var BankName = $('#phPageBody_txtBankName').val()
            //var IssuerNoSave = $('#phPageBody_txtIssuerNo').val()
            //var Bank = $('#phPageBody_txtBank').val()
            //var BankFolder = $('#phPageBody_txtBankFolder').val()
            ////var CardPrefix = $('#phPageBody_txtCardPrefix').val()
            ////var CardProgram = $('#phPageBody_txtCardProgram').val()
            ////var IssuerNo = $('#phPageBody_txtSearchIssuerNo').val()
            //var RCVREmailID = $('#phPageBody_txtRCVREmailID').val()
            //var eml = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;
            ////var eml = /^([A-Za-z0-9_\-\.])+\@(?!gmail.com)(?!yahoo.com)+\.([A-Za-z]{2,4})$/;
            ////var email = /^([\w-\.]+@(?!gmail.com)(?!yahoo.com)([\w-]+\.)+[\w-]{2,4})?$/;
            ////var email = /^([\w.-\]+@(?!gmail.com)(?!yahoo.com)([\w-]+\.)+[\w-]{2,4})?$/;
            ////var em = /^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/;
            //var WinSCP_User = $('#phPageBody_txtWinSCP_User').val()
            //var WinSCP_PWD = $('#phPageBody_txtWinSCP_PWD').val()
            //var WinSCP_IP = $('#phPageBody_txtWinSCP_IP').val()
            //var WinSCP_Port = $('#phPageBody_txtWinSCP_Port').val()

            //var AGS_SFTPServer = $('#phPageBody_txtAGS_SFTPServer').val()
            //var AGS_SFTP_Port = $('#phPageBody_txtAGS_SFTP_Port').val()
            //var AGS_SFTP_Pwd = $('#phPageBody_txtAGS_SFTP_Pwd').val()
            //var AGS_SFTP_User = $('#phPageBody_txtAGS_SFTP_User').val()

            //var B_SFTPServer = $('#phPageBody_txtB_SFTPServer').val()
            //var B_SFTP_Port = $('#phPageBody_txtB_SFTP_Port').val()
            //var B_SFTP_Pwd = $('#phPageBody_txtB_SFTP_Pwd').val()
            //var B_SFTP_User = $('#phPageBody_txtB_SFTP_User').val()

            //var C_SFTPServer = $('#phPageBody_txtC_SFTPServer').val()
            //var C_SFTP_Port = $('#phPageBody_txtC_SFTP_Port').val()
            //var C_SFTP_Pwd = $('#phPageBody_txtC_SFTP_Pwd').val()
            //var C_SFTP_User = $('#phPageBody_txtC_SFTP_User').val()

            //var PGP_KeyName = $('#phPageBody_txtPGP_KeyName').val()
            //var PGP_PWD = $('#phPageBody_txtPGP_PWD').val()
            //var PGP_KeyName = $('#phPageBody_txtAGS_PGP_KeyName').val()
            //var AGS_PGP_PWD = $('#phPageBody_txtAGS_PGP_PWD').val()




            //if (BankName == "") {
            //    $('#SpnErrorMsg').html('Please provide bankName');
            //    $('#errormsgDiv').show();
            //    return false;
            //}
            
            //    if (IssuerNoSave == "") {
            //        $('#SpnErrorMsg').html('Please provide issuerNo');
            //        $('#errormsgDiv').show();
            //        return false;
            //    }
                
                    
                    
            //            if (Bank == "") {
            //                $('#SpnErrorMsg').html('Please provide bank');
            //                $('#errormsgDiv').show();
            //                return false;
            //            }

                        
            //                if (BankFolder == "") {
            //                    $('#SpnErrorMsg').html('Please provide folder name for bank');
            //                    $('#errormsgDiv').show();
            //                    return false;
            //                }

                            
                            
            //                    if ($('#phPageBody_ddlSystemId').val() == "0") {
            //                        $('#SpnErrorMsg').html('Please select Systemid ');
            //                        $('#errormsgDiv').show();
            //                        return false;
            //                    }
                                
            //                      //if (eml.test($.trim(RCVREmailID.val())) == false)
                                
            //                        if (eml.test($.trim($('#phPageBody_txtRCVREmailID').val())) == false && RCVREmailID != "") {
            //                            $('#SpnErrorMsg').html('Please provide correct emailid');
            //                            $('#errormsgDiv').show();
            //                            return false;

            //                    }


            //                        if (WinSCP_User == "") {
            //                            $('#SpnErrorMsg').html('Please provide WinSCP user');
            //                            $('#errormsgDiv').show();
            //                            return false;
            //                        }
            //                        if (WinSCP_PWD == "") {
            //                            $('#SpnErrorMsg').html('Please provide WinSCP password');
            //                            $('#errormsgDiv').show();
            //                            return false;
            //                        } if (WinSCP_IP == "") {
            //                            $('#SpnErrorMsg').html('Please provide WinSCP ip');
            //                            $('#errormsgDiv').show();
            //                            return false;
            //                        } if (WinSCP_Port == "") {
            //                            $('#SpnErrorMsg').html('Please provide WinSCP port');
            //                            $('#errormsgDiv').show();
            //                            return false;
            //                        } if (AGS_SFTPServer == "") {
            //                            $('#SpnErrorMsg').html('Please provide AGS SFTP server');
            //                            $('#errormsgDiv').show();
            //                            return false;
            //                        } if (AGS_SFTP_Port == "") {
            //                            $('#SpnErrorMsg').html('Please provide AGS SFTP port');
            //                            $('#errormsgDiv').show();
            //                            return false;
            //                        } if (AGS_SFTP_Pwd == "") {
            //                            $('#SpnErrorMsg').html('Please provide AGS SFTP password');
            //                            $('#errormsgDiv').show();
            //                            return false;
            //                        } if (AGS_SFTP_User == "") {
            //                            $('#SpnErrorMsg').html('Please provide AGS SFTP user');
            //                            $('#errormsgDiv').show();
            //                            return false;
            //                        } if (B_SFTPServer == "") {
            //                            $('#SpnErrorMsg').html('Please provide Bank SFTP server');
            //                            $('#errormsgDiv').show();
            //                            return false;
            //                        } if (B_SFTP_Port == "") {
            //                            $('#SpnErrorMsg').html('Please provide Bank SFTP port');
            //                            $('#errormsgDiv').show();
            //                            return false;
            //                        } if (B_SFTP_Pwd == "") {
            //                            $('#SpnErrorMsg').html('Please provide Bank SFTP password');
            //                            $('#errormsgDiv').show();
            //                            return false;
            //                        } if (B_SFTP_User == "") {
            //                            $('#SpnErrorMsg').html('Please provide Bank SFTP user');
            //                            $('#errormsgDiv').show();
            //                            return false;
            //                        } if (C_SFTPServer == "") {
            //                            $('#SpnErrorMsg').html('Please provide PRE SFTP server');
            //                            $('#errormsgDiv').show();
            //                            return false;
            //                        } if (C_SFTP_Port == "") {
            //                            $('#SpnErrorMsg').html('Please provide PRE SFTP port');
            //                            $('#errormsgDiv').show();
            //                            return false;
            //                        } if (C_SFTP_Pwd == "") {
            //                            $('#SpnErrorMsg').html('Please provide PRE SFTP password');
            //                            $('#errormsgDiv').show();
            //                            return false;
            //                        } if (C_SFTP_User == "") {
            //                            $('#SpnErrorMsg').html('Please provide PRE SFTP user');
            //                            $('#errormsgDiv').show();
            //                            return false;
            //                        } if (PGP_KeyName == "") {
            //                            $('#SpnErrorMsg').html('Please provide PGP Keyname');
            //                            $('#errormsgDiv').show();
            //                            return false;
            //                        } if (PGP_PWD == "") {
            //                            $('#SpnErrorMsg').html('Please provide PGP password');
            //                            $('#errormsgDiv').show();
            //                            return false;
            //                        } if (AGS_PGP_KeyName == "") {
            //                            $('#SpnErrorMsg').html('Please provide AGS PGP keyname');
            //                            $('#errormsgDiv').show();
            //                            return false;
            //                        } if (AGS_PGP_PWD == "") {
            //                            $('#SpnErrorMsg').html('Please provide AGS PGP password');
            //                            $('#errormsgDiv').show();
            //                            return false;
            //                        }
                                    
            //                     else {
            //                            $('#errormsgDiv').hide();

            //                        }


       // }--%>


    
</asp:Content>
