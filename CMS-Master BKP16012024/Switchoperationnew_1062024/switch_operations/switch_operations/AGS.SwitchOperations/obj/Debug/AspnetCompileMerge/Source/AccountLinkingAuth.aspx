<%@ Page Title="" Language="C#" MasterPageFile="~/SwitchOperationSite.Master" AutoEventWireup="true" CodeBehind="AccountLinkingAuth.aspx.cs" Inherits="AGS.SwitchOperations.AccountLinkingAuth" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phPageHeader" runat="server">

   <style>
        #datatable-buttons_wrapper tr td:nth-child(n+11) {
            visibility: hidden;
        }

        .accordian {
            background-color: #eee;
            color: #444;
            cursor: pointer;
            padding: 18px;
            width: 100%;
            margin: 10px 0;
            text-align: left;
            border: none;
            outline: none;
            transition: 0.4s;
            position:relative;
        }

            .accordian:hover {
                background-color: #ccc;
            }

        .panel {
            padding: 0 18px;
            background-color: white;
            display: none;
            /*overflow: hidden;*/
        }

        div[class^="acco_"].panel table{
            color: #000;
            width: 100% !important;
        }
        
        .dataTables_wrapper {margin-bottom:30px;}

        .accordian span.glyphicon{
            right:35px;
            top:50%;
            position:absolute;
            transform:translate(0,-50%);
        }
    </style>
    <script>
        document.addEventListener('DOMContentLoaded', function (event) {

            $(".accordian").click(function () {
                var id = $(this).attr('id');
                $('.' + id).toggle();
                $('#' + id + ' > span.glyphicon-plus-sign').toggle();
                $('#' + id + ' > span.glyphicon-minus-sign').toggle();
            })

        })
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phPageBody" runat="server">
    <!--All Hiddenfields------------------------------------------------------------------------->

    <asp:HiddenField ID="hdnCardNo" runat="server" />

    <asp:HiddenField ID="hdnCifId" runat="server" />

    <asp:HiddenField ID="hdnAccountNo" runat="server" />
    <asp:HiddenField ID="hdnAccountType" runat="server" />
    <asp:HiddenField ID="hdnAccountQuilifier" runat="server" />
    <asp:HiddenField ID="hdnLinkflgtbl" runat="server" />
    <asp:HiddenField ID="hdnAllSelectedValues" runat="server" />
    <asp:HiddenField ID="hdnAccLinkDetails" runat="server" />
    <asp:HiddenField ID="hdnAccessCaption" runat="server" />
    <asp:HiddenField ID="hdnTransactionDetails" runat="server" />
    <asp:HiddenField ID="hdnFlag" runat="server" />


    <asp:HiddenField ID="hdnLinkingflag" runat="server" />

    <asp:HiddenField ID="hdnId" runat="server" />

    <asp:HiddenField ID="hdnCheckAccQuntifr" runat="server" />
    <asp:HiddenField ID="hdnISOData" runat="server" />

    <asp:Panel ID="pnlAccountlink" runat="server">

        <div style="font-size: 14px;">
            <div id="SearchBank">
                <div class="box-primary">
                    <div class="box-header with-border">
                        <i class="fa fa-credit-card"></i>

                        <h4 class="box-title">Account Linking Authorization:</h4>
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

                </div>
                <div class="row" id="DivResultMsg" style="display: none">
                    <div class="col-md-6">
                        <h4>
                            <label runat="server" name="Name" id="LblResult" readonly="readonly" />
                        </h4>
                    </div>
                </div>

                <div class="box-body" id="DivResult">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="box-primary">
                                <!-- /.box-header -->
                                <div class="box-body no-padding">
                                    <div class="row" style="padding: 0px 30px; padding-top: 15px;">
                                        <div class="col-md-12">
                                            <div class="x_panel">
                                                <div class="">
                                                    <div>
                                                        <div id="SelectAllDiv">
                                                            <input id="select_all" runat="server" type="checkbox" />
                                                            <label runat="server" name="Name_select_all" id="LBLselect_all" readonly="readonly">SelectAll</label>
                                                        </div>
                                                        <div class="clearfix"></div>
                                                    </div>
                                                    <div class="x_content">
                                                        <table id="datatable-buttons" class="table table-striped table-bordered" style="width: 100%">
                                                        </table>
                                                        <div id="Table_Request" runat="server"></div>
                                                    </div>
                                                    <asp:Button runat="server" ID="BtnSave" Text="Approve" OnClick="BtnSave_Click" class="btn-div btn-style btn btn-primary" />
                                                    <input type="button" value="Reject" onclick="funGetResult(2)" id="BtnReject" class="btn-div btn-style btn btn-primary" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>


        <div id="RejectConfirmationModal" class="modal fade" data-backdrop="static" role="dialog">
            <div class="modal-dialog">
                <!-- Modal content -->
                <div class="modal-content" style="border-radius: 4px;">

                    <div class="modal-header">
                        <button aria-label="Close" data-dismiss="modal" class="close" type="button" onclick="funCancelModal()"><span aria-hidden="true">×</span></button>
                        <h4 id="myLargeRejectModal" class="modal-title" style="font-weight: bold">Confirmation</h4>
                    </div>

                    <div class="modal-body">
                        <div class="row" id="remarkDiv">
                            <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>Reason:</label>
                            <div class="col-sm-7">
                                <asp:TextBox runat="server" ID="txtRejectReson" CssClass="form-control" TextMode="MultiLine" MaxLength="50" onkeypress="return onlyAlphabets(event,this);"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <br />
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <button aria-label="Close" data-dismiss="modal" class="btn btn-primary pull-right" style="margin-right: 10px;" type="button" onclick="funCancelModal()"><span aria-hidden="true">CANCEL</span></button>
                            </div>
                            <div class="col-md-6">
                                <asp:Button runat="server" CssClass="btn btn-primary" Text="Confirm" ID="Reject_Btn" OnClick="Reject_Btn_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>


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
                    <h4 id="myLargeModalLabel" class="modal-title" style="font-weight: bold">Account Linking Delinking Data</h4>
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
                                <input type="text" class="form-control" id="txtAccountNo" runat="server" placeholder="Enter AccountNo" maxlength="16" onkeypress="return FunChkIsNumber(event);" />

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
                                    <asp:ListItem Text="356" Value="356"></asp:ListItem>

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

            $('#AddEditModal').modal('hide')

        }
    </script>
    <script>

        $(document).ready(function () {

            if ($("#<%=hdnAccLinkDetails.ClientID %>").val().length == 0) {
                $('[id$="id_grd1"]').hide()
            }

            //Start Dynamic datatable bind
            $('#datatable-buttons').html($("#<%=hdnAccLinkDetails.ClientID %>").val());
            $("#<%=hdnAccLinkDetails.ClientID %>").val('');
            //if ($("#datatable-buttons tbody tr").length > 0) {
            if ($("div[class^='acco_'].panel table tbody tr").length > 0) {
                //For Data Table
                var handleDataTableButtons = function () {
                    if ($("div[class^='acco_'].panel table").length) {
                        $("div[class^='acco_'].panel table").DataTable({
                            dom: "Bfrtip",
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
                $('#DivResult').show();
            }
            else {
                $('#DivResult').hide();
            }


            // ************* Select All function *******
            $('[id$="select_all"]').click(function (event) {  //"select all" change 

                //select all checkboxes
                if ($(this).is(":checked")) {
                    $('div[class^="acco_"].panel table tbody input[type=checkbox]:not(:disabled)').prop("checked", 'true')
                }
                else {
                    $('div[class^="acco_"].panel table tbody input[type=checkbox]').prop("checked", false)
                }
            });

            $('div[class^="acco_"].panel table tbody input[type=checkbox]').change(function () {

                if ($('div[class^="acco_"].panel table tbody input[type=checkbox]').length == $('div[class^="acco_"].panel table tbody input[type=checkbox]:checked').length) {
                    $('[id$="select_all"]').prop('checked', true);
                }
                else {
                    $('[id$="select_all"]').prop('checked', false);
                }
            });


            $('[id$="BtnSave"]').click(function () {
                //debugger;
                SetISOData();
                var allids = "";
                $('div[class^="acco_"].panel table tbody').find('tr').each(function () {
                    if ($(this).find('input[type="checkbox"]:checked').length > 0) {
                        var id = $(this).find('td').next().html() //+ "|" + $(this).find('td:eq(2)').text() + "|" + $(this).find('td:eq(3)').text();
                        if (allids == "") {
                            allids = id;
                        } else {
                            allids = allids + "," + id;
                        }
                    }
                });

                if (allids == '') {
                    document.getElementById('phPageBody_LblMessage').innerHTML = 'Please check at least one checkbox to approve.'
                    FunShowMsg();
                    return false;
                }
                else {
                    $('[id$="hdnAllSelectedValues"]').val(allids);
                }

            });


            $('#phPageBody_Reject_Btn').click(function () {
                var errrorTextPD = 'Please provide ';
                var errorFieldsPD = '';
                //if (!($('input[name="IsConfirm"]').is(':checked'))) {
                //    errorFieldsPD = errorFieldsPD + (errorFieldsPD != '' ? '<b>,' : '<b>') + ' option(Yes/No) </b> ';
                //}
                SetISOData();
                if ($('#phPageBody_txtRejectReson').val() == "") {
                    errorFieldsPD = errorFieldsPD + (errorFieldsPD != '' ? '<b>,' : '<b>') + ' Reason</b> ';
                }


                if (errorFieldsPD != '') {
                    document.getElementById('phPageBody_LblMessage').innerHTML = errrorTextPD + errorFieldsPD;
                    FunShowMsg();


                    //$('#SpnRejectErrorMsg').html(errrorTextPD + errorFieldsPD);
                    //$('#RejecterrormsgDiv').show();
                    return false;
                }
                else {
                    errorFieldsPD = '';
                    errrorTextSectionPD = '';
                    errrorTextPD = '';
                    $('.shader').fadeIn();
                    //$('#errormsgDiv').hide();
                }

            });
        });


        // Function to Get Data Fro ISOCall
        function SetISOData() {
            var Record = '';
            $('div[class^="acco_"].panel table tbody').find('tr').each(function () {
                if ($(this).find('input[type="checkbox"]:checked').length > 0) {
                    var id = $(this).find('td:nth-child(2)').text();
                    var accNo = $(this).find('td:nth-child(3)').text();
                    var custId = $(this).find('td:nth-child(4)').text();
                    var cardNo = $(this).find('td:nth-child(5)').text();
                    var linkFlag = $(this).find('td:nth-child(7)').text();
                    var accType = $(this).find('td:nth-child(8)').text();
                    var accQua = $(this).find('td:nth-child(9)').text();
                    if (Record == '')
                        Record = id + '|' + cardNo + '|' + accNo + '|' + accType + '|' + accQua + '|' + linkFlag + '|' + custId;
                    else
                        Record = Record + "," + id + '|' + cardNo + '|' + accNo + '|' + accType + '|' + accQua + '|' + linkFlag + '|' + custId;
                }
            });
            $("#<%=hdnISOData.ClientID%>").val(Record);
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

        function funGetResult(FormstatusID) {


            if ($('div[class^="acco_"].panel table tbody input[type=checkbox]:checked').length == 0) {
                document.getElementById('phPageBody_LblMessage').innerHTML = "Please select Card Requests to accept/reject";
                FunShowMsg();
                return false;
            }
            else {
                document.getElementById('phPageBody_LblMessage').innerHTML = "";
                //$('#errormsgDiv').hide();
       <%--         $("#<%=hdnFormStatusID.ClientID%>").val(FormstatusID);
                $("#<%=hdnReqType.ClientID%>").val($($('#datatable-buttons tbody input[type=checkbox]:checked')[0]).attr('reqTypeId'));--%>

                var ArrayIds = [];
                $('div[class^="acco_"].panel table tbody input[type=checkbox]:checked').each(function (i) {
                    ArrayIds[i] = $(this).attr("ReqID");

                });
                //alert(val.join(","))

                $("#<%=hdnAllSelectedValues.ClientID%>").val(ArrayIds.join(","))

                //for Reject request
                if (FormstatusID == "2") {

                    // $('[id$="txtRejectReson"]').attr('required', true)
                    $('#RejectConfirmationModal').modal('show');

                }
                else {
                    $('.shader').fadeIn();
                }
                return true;
            }
        }

        function FunShowMsg() {
            //  setTimeout($('#myModal').modal('show'),2000);
            $('#memberModal').modal('show');
            //window.location.reload();

        }

        function FunShowinfo() {
            $('[id$="DivResultMsg"]').show();
        }

        function FunHideinfo() {
            $('[id$="DivResultMsg"]').hide();
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
