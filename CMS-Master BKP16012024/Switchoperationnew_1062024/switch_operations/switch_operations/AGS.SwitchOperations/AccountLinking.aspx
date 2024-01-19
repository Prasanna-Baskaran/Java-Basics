<%@ Page Title="" Language="C#" MasterPageFile="~/SwitchOperationSite.Master" AutoEventWireup="true" CodeBehind="AccountLinking.aspx.cs" Inherits="AGS.SwitchOperations.AccountLinking" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phPageHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phPageBody" runat="server">
    <!--All Hiddenfields------------------------------------------------------------------------->

    <asp:HiddenField ID="hdnCardNo" runat="server" />
    <asp:HiddenField ID="hdnAccountNo" runat="server" />
    <asp:HiddenField ID="hdnAccountType" runat="server" />
    <asp:HiddenField ID="hdnAccountQuilifier" runat="server" />
    <asp:HiddenField ID="hdnLinkflgtbl" runat="server" />

    <asp:HiddenField ID="hdnAccessCaption" runat="server" />
    <asp:HiddenField ID="hdnTransactionDetails" runat="server" />
    <asp:HiddenField ID="hdnFlag" runat="server" />


    <asp:HiddenField ID="hdnLinkingflag" runat="server" />

    <asp:HiddenField ID="hdnId" runat="server" />

    <asp:HiddenField ID="hdnCheckAccQuntifr" runat="server" />

    <asp:Button runat="server" ID="hdnLinkBtn" Text="Search" OnClick="hdnLinkBtn_Click" Style="display: none;" />

    <%--<asp:Button runat="server" ID="hdnDelinkBtn" Text="Search" OnClick="hdnDelinkBtn_Click" Style="display: none;" />--%>


    <asp:Panel ID="pnlAccountlink" runat="server">
        <div style="font-size: 14px;">
            <div id="SearchBank">
                <div class="box-primary">
                    <div class="box-header with-border">
                        <i class="fa fa-credit-card"></i>

                        <h4 class="box-title">Account Linking:</h4>
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
                                            <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>CardNo:</label>
                                            <div class="col-sm-8">
                                                <input type="text" class="form-control" maxlength="50" runat="server" name="SearchCardNo" id="txtSearchCardNo" onkeypress="return FunChkIsNumber(event);" />
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-md-2">
                                        <div class="form-group">
                                            <asp:Button runat="server" CssClass="btn btn-primary pull-right" ID="btnSearch" Text="Search" OnClick="btnSearch_Click" OnClientClick="return FunSearchValidation();" />
                                        </div>
                                    </div>

                                    <div class="col-sm-3">
                                    </div>

                                </div>
                                <%--<div class="row">
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
                                </div>--%>
                            </div>
                        </div>
                        <div class="row" id="DivResultMsg">
                            <div class="col-md-6">

                                <h4>
                                    <label maxlength="20" runat="server" name="Name" id="LblResult" readonly="readonly" />
                                </h4>
                            </div>
                            <div class="col-md-5">
                                <input type="button" class="btn btn-primary pull-right" id="BtnAddNew" data-targrt="#AddEditModal" value="Add New" onclick="funAddNew(); " />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>

    <%--<div id="AddEditModal" class="modal fade" data-backdrop="static" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content -->
            <div class="modal-content" style="border-radius: 4px; width: 797px;">

                <div class="modal-header">
                    <button aria-label="Close" data-dismiss="modal" class="close" type="button" onclick="funCancelModal()"><span aria-hidden="true">×</span></button>
                    <h4 id="myLargeEdit" class="modal-title" style="font-weight: bold">Account Linking</h4>
                </div>

                
                
                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-6">
                            <label for="inputName" class="col-md-5 control-label" id="lblIssuerno"><span style="color: red;"></span>IssuerNo:</label>
                            <div class="col-md-7">
                                <input type="text" class="form-control" id="txtIssuerNo" runat="server" placeholder="Enter Issuer No" maxlength="50" onkeypress="return FunChkIsNumber(event);" />

                            </div>
                        </div>
                        <div class="col-md-6">
                            <label for="inputName" class="col-md-5 control-label"><span style="color: red;"></span>CustomerId:</label>
                            <div class="col-md-7">
                                <input class="form-control" type="text" id="txtCustomerId" runat="server" placeholder="Enter Custemer id" maxlength="200" onkeypress="return FunChkIsNumber(event);" />

                            </div>
                        </div>


                    </div>

                    <div class="row">
                        <br />
                    </div>

                    
                    

                    <div class="row">

                        <div class="col-md-6">
                            <label for="inputName" class="col-md-5 control-label"><span style="color: red;"></span>AccountNo:</label>
                            <div class="col-md-7">
                                <input class="form-control" type="text" id="txtAccuntNo" runat="server" placeholder="Enter CardPrefix" maxlength="12" onkeypress="return FunChkIsNumber(event);" />
                            </div>

                        </div>
                        <div class="col-md-6">
                            <label for="inputName" class="col-md-5 control-label"><span style="color: red;"></span>FormStatusId:</label>
                            <div class="col-md-7">
                                <input class="form-control" type="text" id="txtFormStatusId" runat="server" placeholder="Enter Bin Description" maxlength="100" onkeypress="return FunChkIsNumber(event);" />

                            </div>
                        </div>

                    </div>

                    

                    <div class="row">
                        <br />
                    </div>


                    <div class="row">

                        <div class="col-sm-3">
                        </div>
                        <%--7-1-18--%>
    <%--<div class="col-sm-6 text-center">
                            <asp:Button runat="server" CssClass="btn btn-primary" Text="SAVE" Style="width: 69px" ID="AddBtn" OnClick="add_Click" />
                            <button aria-label="Close" data-dismiss="modal" class="btn btn-primary" onclick="funCancelModal()" type="button"><span aria-hidden="true">CANCEL</span></button>
                            <input type="button" id="BtnReset" aria-label="Reset" class="btn btn-primary" value="RESET" onclick="FunClear();" />
                            
                        </div>

                        <div class="col-sm-3">
                        </div>
                    </div>
                </div>
            </div>

        </div>

    </div>--%>

    <div>
        <table id="datatable-buttons" class="table table-striped table-bordered" style="width: 100%">
        </table>
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


    <div id="memberModal" class="modal fade bs-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel">
        <div class="modal-dialog modal-md">
            <div class="modal-content" style="border-radius: 6px">
                <div class="modal-header box-primary" style="border-top-right-radius: 6px; border-top-left-radius: 6px">
                    <button aria-label="Close" data-dismiss="modal" class="close" type="button"><span aria-hidden="true">×</span></button>
                    <h4 id="myLargeModalLabel" class="modal-title" style="font-weight: bold">Account Linking Delinking Data</h4>
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

    <div id="AddEditModal" class="modal fade" data-backdrop="static" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content -->
            <div class="modal-content" style="border-radius: 4px; width: 797px;">

                <div class="modal-header">
                    <button aria-label="Close" data-dismiss="modal" class="close" type="button" onclick="funCancelModal()"><span aria-hidden="true">×</span></button>
                    <h4 id="myLargeEdit" class="modal-title" style="font-weight: bold">Add New Account</h4>
                </div>


                <%--7-1-18--%>
                <!--Display validation msg ------------------------------------------------------------------------->
                <div class="pad margin no-print" id="errormsgDivForModel" style="display: none">
                    <div class="callout callout-info" style="margin-bottom: 0!important;">
                        <h4><i class="fa fa-info"></i>Information :</h4>
                        <span id="SpnErrorMsgForModel" class="text-center"></span>
                    </div>
                </div>


                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-6">
                            <label for="inputName" class="col-md-5 control-label"><span style="color: red;"></span>AccountNo:</label>
                            <div class="col-md-7">
                                <input type="text" class="form-control" id="txtAccountNo" runat="server" placeholder="Enter AccountNo" maxlength="30" onkeypress="return FunChkIsNumber(event);" />

                            </div>
                        </div>
                        <div class="col-md-6">
                            <label for="inputName" class="col-md-5 control-label"><span style="color: red;"></span>AccountType:</label>
                            <div class="col-md-7">
                                <asp:DropDownList ID="ddlAccountType" runat="server" class="form-control" Style="width: 100%">
                                    <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Saving" Value="10"></asp:ListItem>
                                    <asp:ListItem Text="Current" Value="20"></asp:ListItem>
                                </asp:DropDownList>

                            </div>
                        </div>


                    </div>

                    <div class="row">
                        <br />
                    </div>

                    <div class="row">
                        <div class="col-md-6">
                            <label for="inputName" class="col-md-5 control-label"><span style="color: red;"></span>CurrencyCode:</label>
                            <div class="col-md-7">
                                <asp:DropDownList ID="ddlCurrencyCode" runat="server" class="form-control" Style="width: 100%">
                                    <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="524" Value="524"></asp:ListItem>
                                    <asp:ListItem Text="840" Value="840"></asp:ListItem>
                                </asp:DropDownList>
                            </div>

                        </div>

                        <div class="col-md-6">
                            <asp:CheckBox ID="CheckAccountQuantifier" runat="server" />
                            <div class="col-md-7">
                                <label for="inputName" class="col-md-5 control-label"><span style="color: red;"></span>AccountQualifier:</label>
                            </div>

                        </div>



                    </div>



                    <div class="row">
                        <br />
                    </div>


                    <div class="row">

                        <div class="col-sm-3">
                        </div>
                        <%--7-1-18--%>
                        <div class="col-sm-6 text-center">
                            <asp:Button runat="server" CssClass="btn btn-primary" Text="Add" Style="width: 69px" ID="AddBtn" OnClick="AddAccount_Click" />
                            <button aria-label="Close" data-dismiss="modal" class="btn btn-primary" onclick="funCancelModal()" type="button"><span aria-hidden="true">CANCEL</span></button>
                            <input type="button" id="BtnReset" aria-label="Reset" class="btn btn-primary" value="RESET" onclick="FunClear();" />

                        </div>

                        <div class="col-sm-3">
                        </div>
                    </div>
                </div>
            </div>

        </div>

    </div>
    <script>
        function FunClear() {
            //fnreset(null, true);
            //$("#cbImg1").prop('checked', true); //// To check

            //$("#CheckAccountQuantifier").attr('checked', false);

            document.getElementById('phPageBody_CheckAccountQuantifier').checked = false;
            document.getElementById('phPageBody_ddlCurrencyCode').selectedIndex = '--Select--'
            document.getElementById('phPageBody_ddlAccountType').selectedIndex = '--Select--'
            <%--$("#<%=CheckAccountQuantifier.ClientID %>").prop('checked', true);--%>

            $('#errormsgDivForModel').hide();

        }
    </script>
    <script>

        function funAddNew() {
            //return FunSearchValidation();
            var IssuerNo = $('#phPageBody_txtSearchCardNo').val()

            if (IssuerNo == "") {
                $('#SpnErrorMsg').html('Please provide numeric CardNo');
                $('#errormsgDiv').show();
                return false;
            }
            else {
                $('#errormsgDiv').hide();


                $('[id$="txtAccountNo"]').val('');
            <%--$('#<%=ddlAccountType.ClientID%> option:selected').text("--Select--");--%>
                $('#<%=ddlAccountType.ClientID%>').val('0');

                $('#AddEditModal').modal('show');

                //$('[id$="txtToken"]').attr("readonly", false);
                $('#<%=ddlCurrencyCode.ClientID%>').val('0');

                //5-1-17
                $('#BtnReset').show();
            }

        }
    </script>

    <script>
        function funCancelModal() {
            $('#errormsgDivForModel').hide();
            $('[id$="txtAccountNo"]').val('');
            //$('[id$="txtSearchCardNo"]').val('');
            $('#<%=ddlAccountType.ClientID%>').val('0');
            $('#<%=ddlCurrencyCode.ClientID%>').val('0');
            
            <%--$("#<%=ddlDirection.ClientID%>")[0].selectedIndex = 0;--%>

            $('#AddEditModal').modal('hide')

        }
    </script>
    <script>
        $("#<%=AddBtn.ClientID %>").click(function () {
            var errrorTextPD = 'Please provide :  ';
            var errorFieldsPD = '';

            if ($("#phPageBody_txtAccountNo").val() == "") {
                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>, AccountNo</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>AccountNo</b> ';
                }
                $('#<%=txtAccountNo.ClientID%>').focus();
            }



            if ($("#phPageBody_ddlAccountType").val() == "0") {
                errortab = '1';

                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>,AccountType</b> ';

                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>AccountType</b> ';

                }

                $('#<%=ddlAccountType.ClientID%>').focus();
            }

            if ($("#phPageBody_ddlCurrencyCode").val() == "0") {
                errortab = '1';

                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>,CurrencyCode</b> ';

                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>CurrencyCode</b> ';

                }
                $('#<%=ddlCurrencyCode.ClientID%>').focus();
            }





            if (errorFieldsPD != '') {
                $('#SpnErrorMsgForModel').html(errrorTextPD + errorFieldsPD);
                $('#errormsgDivForModel').show();
                return false;
            }
            else {
                errorFieldsPD = '';
                errrorTextSectionPD = '';
                errrorTextPD = '';
                $('#errormsgDivForModel').hide();



                $('.shader').fadeIn();
            }
        });

    </script>

    <%--<div id="memberModalTOSearch" class="modal fade bs-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel">
        <div class="modal-dialog modal-md">
            <div class="modal-content" style="border-radius: 6px">
                <div class="modal-header box-primary" style="border-top-right-radius: 6px; border-top-left-radius: 6px">
                    <button aria-label="Close" data-dismiss="modal" class="close" type="button"><span aria-hidden="true">×</span></button>
                    <h4 id="myLargeModalLabel1" class="modal-title" style="font-weight: bold">Account Linking Delinking Data</h4>
                </div>
                <div class="modal-body" id="msgBody1">
                    <asp:Label ID="Label1" runat="server"></asp:Label>
                </div>
                <div class="modal-footer">
                    <button aria-label="Close" data-dismiss="modal" id="btnOk" OnClick="btnSearch_Click" class="btn btn-primary" type="button"><span aria-hidden="true">OK</span></button>
                </div>
            </div>
        </div>
    </div>--%>


    <script>
        $(document).ready(function () {


            //$(function () {
            //    $('[data-toggle="tooltip"]').tooltip()
            //});

            $('#datatable-buttons').html($("#<%=hdnTransactionDetails.ClientID %>").val());
            //check user has Edit rights

            if ($("#datatable-buttons tbody tr").length > 0) {

                var handleDataTableButtons = function () {
                    if ($("#datatable-buttons").length) {
                        $("#datatable-buttons").DataTable({
                            dom: "Bfrtip",
                            buttons: [
                                {
                                    extend: "copy",
                                    className: "btn-sm"
                                },
                                {
                                    extend: "csv",
                                    className: "btn-sm"
                                },
                                {
                                    extend: "excel",
                                    className: "btn-sm"
                                },
                                {
                                    extend: "pdfHtml5",
                                    className: "btn-sm"
                                },
                                {
                                    extend: "print",
                                    className: "btn-sm"
                                },
                            ],
                            responsive: true
                        });
                    }
                };

                TableManageButtons = function () {
                    "use strict";
                    return {
                        init: function () {
                            handleDataTableButtons();
                        }
                    };
                }();
                TableManageButtons.init();
                $('#datatable-buttons tbody input[type=button][isedit=0][value=Edit]').hide()
            }

            //$("#phPageBody_SystemList").find("td").css("padding-right", "10px")
        });

        //$('[id$="btn_Link"]').click(function () {
        //    $('[id$="hdnLinkflg"]').val($(this).parents('tr:first').find('td:eq(0)').text());
        //    $('[id$="hdnCardNo"]').val($(this).parents('tr:first').find('td:eq(1)').text());
        //    $('[id$="hdnAccountNo"]').val($(this).parents('tr:first').find('td:eq(2)').text());
        //    $('[id$="hdnAccountType"]').val($(this).parents('tr:first').find('td:eq(3)').text());
        //    $('[id$="hdnAccountQuilifier"]').val($(this).parents('tr:first').find('td:eq(4)').text());
        //    $("[id$='hdnLinkBtn']").click();
        //});
    </script>

    <%--<script>

        function funedit(obj) {
            var sIssuerNo = $(obj).attr('issuerno');
            var sCustomerid = $(obj).attr('customer_id');
            var sAccno = $(obj).attr('accno');
            var sFormstatusid = $(obj).attr('formstatusid');
            
            $('[id$="txtIssuerNo"]').val(sIssuerNo);
            $('[id$="txtCustomerId"]').val(sCustomerid);
            $('[id$="txtAccuntNo"]').val(sAccno);
            $('[id$="txtFormStatusId"]').val(sFormstatusid);
           
            $('#AddEditModal').modal('show');
            $('[id$="txtIssuerNo"]').attr("readonly", true);
            $('[id$="txtCustomerId"]').attr("readonly", true);
            $('[id$="txtAccuntNo"]').attr("readonly", true);
            //$('#txtCardProgram').attr('disabled', true);
            $('#BtnReset').hide();
            
            
           
        }
    </script>--%>

    <%--<script>
        function funCancelModal() {
           // $('#errormsgDivForModel').hide();

            $('[id$="txtIssuerNo"]').val('');
            $('[id$="txtCustomerId"]').val('');
            $('[id$="txtAccuntNo"]').val('');
            $('[id$="txtFormStatusId"]').val('');
            $('#AddEditModal').modal('hide');
                    }
    </script>
    --%>
    <%--<script>
        function FunClear() {
            fnreset(null, true);
</script>--%>
    <script>
        function FunLinkDelinkAccount(obj) {


            var Linkingflag = $(obj).attr('Linkingflag');

            var Linkflagtbl = $(obj).attr('Linkingflg');
            var CardNo = $(obj).attr('CardNo');
            var AccountNo = $(obj).attr('AccountNo');
            var AccountType = $(obj).attr('AccountType');
            var AccountQuilifier = $(obj).attr('AccountQuilifier');


            $('[id$="hdnLinkingflag"]').val(Linkingflag);

            $('[id$="hdnLinkflgtbl"]').val(Linkflagtbl);
            $('[id$="hdnCardNo"]').val(CardNo);
            $('[id$="hdnAccountNo"]').val(AccountNo);

            $('[id$="hdnAccountType"]').val(AccountType == "Saving" ? "10" : AccountType == "Current" ? "20" : AccountType);
            $('[id$="hdnAccountQuilifier"]').val(AccountQuilifier == "Primary" ? "1" : AccountQuilifier == "Secondary" ? "2" : AccountQuilifier == "Tertiary" ? "3" : AccountQuilifier == "Quaternary" ? "4" : AccountQuilifier == "Quinary" ? "5" : AccountQuilifier);

            $('[id$="hdnAccountType"]').val(AccountType == "Saving" ? "10" : AccountType == "Current" ? "20" : AccountType);
            $('[id$="hdnAccountQuilifier"]').val(AccountQuilifier == "Primary" ? "1" : AccountQuilifier == "Secondary" ? "2" : AccountQuilifier == "Tertiary" ? "3" : AccountQuilifier = "Quaternary" ? "4" : AccountQuilifier = "Quinary" ? "5" : AccountQuilifier);



            if (Linkingflag == 01 || Linkingflag == 02) {
                $("[id$='hdnLinkBtn']").click();

            }

            //$(document).ready(function () {
            //    $("#btn_Link").click(function () {

            //    //$('[id$="btn_Link"]').click(function () {
            //        $('[id$="hdnLinkflg"]').val($(this).parents('tr:first').find('td:eq(0)').text());
            //        $('[id$="hdnCardNo"]').val($(this).parents('tr:first').find('td:eq(1)').text());
            //        $('[id$="hdnAccountNo"]').val($(this).parents('tr:first').find('td:eq(2)').text());
            //        $('[id$="hdnAccountType"]').val($(this).parents('tr:first').find('td:eq(3)').text());
            //        $('[id$="hdnAccountQuilifier"]').val($(this).parents('tr:first').find('td:eq(4)').text());
            //        $("[id$='hdnLinkBtn']").click();
            //    });
            //});


        }
    </script>


    <script>
        //Validation on Search Button
        function FunSearchValidation() {
            var IssuerNo = $('#phPageBody_txtSearchCardNo').val()

            if (IssuerNo == "") {
                $('#SpnErrorMsg').html('Please provide numeric CardNo');
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


</asp:Content>

