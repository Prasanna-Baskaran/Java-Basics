<%@ Page Title="" Language="C#" MasterPageFile="~/SwitchOperationSite.Master" AutoEventWireup="true" CodeBehind="CheckCardOperation.aspx.cs" Inherits="AGS.SwitchOperations.CheckCardOperation" %>

<%@ Register Src="~/UserControls/UserButtons.ascx" TagPrefix="AGS" TagName="usrButtons" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phPageHeader" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="phPageBody" runat="server">
    <asp:HiddenField ID="hdnTransactionDetails" runat="server" />
    <asp:HiddenField ID="hdnResultStatus" runat="server" />
    <asp:HiddenField ID="hdnReqType" runat="server" />
    <asp:HiddenField ID="hdnRequestIDs" runat="server" />
    <asp:HiddenField ID="hdnFormStatusID" runat="server" />
    <asp:HiddenField runat="server" ID="hdnAccessCaption" />
    <asp:Panel ID="pnlCustomerDtl" runat="server">
        <div class="box-primary">
            <div class="box-header with-border">
                <i class="fa"></i>
                <!-- start sheetal card operations changes to accept card request-->
                <h4 class="box-title">Accept Card Request :</h4>
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
                <div id="SearchDiv">
                    <div class="form-horizontal">
                        <div class="row">
                            <div class="col-md-3">
                                <div class="form-group">
                                    <label for="inputName" class="col-xs-4 control-label">CustomerID:</label>
                                    <div class="col-sm-8">
                                        <input type="text" class="form-control" maxlength="20" runat="server" name="CustomerID" id="txtSearchCustomerID"  />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="form-group">
                                    <label for="inputName" class="col-xs-4 control-label">Card No:</label>
                                    <div class="col-sm-8">
                                        <input type="text" class="form-control" maxlength="20" runat="server" name="CardNO" id="txtSearchCardNo" onkeypress="return FunChkIsNumber(event)" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="form-group">
                                    <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>Request Type:</label>
                                    <div class="col-sm-8">
                                        <asp:DropDownList runat="server" ID="ddlRequestType" CssClass="form-control"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-2">
                                <div class="form-group">
                                    <div class="col-sm-4"></div>

                                    <div class="col-sm-4">
                                        <div class="col-md-7">
                                            <asp:Button runat="server" CssClass="btn btn-primary pull-right" ID="btnSearchCustomer" Text="Search" OnClientClick="return FunValidation();" OnClick="btnSearch_Click" />
                                        </div>
                                    </div>
                                    <div class="col-sm-4"></div>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>

                <div class="row" id="DivResultMsg">
                    <div class="col-md-6">
                        <h4>
                            <label maxlength="20" runat="server" name="Name" id="LblResult" readonly="readonly" style="color: #ab9e9e;" />
                        </h4>
                    </div>
                </div>

                <div id="DivResult">


                    <div class="row">
                        <div class="col-md-12">
                            <div class="box-primary">
                                <!-- /.box-header -->
                                <div class="box-body no-padding">
                                    <div class="row" style="padding: 0px 30px; padding-top: 15px;">
                                        <div class="col-md-12">
                                            <div class="x_panel">
                                                <div>
                                                    <div id="SelectAllDiv">
                                                        <input id="select_all" type="checkbox" /><span>Select All</span>
                                                    </div>
                                                    <div class="clearfix"></div>
                                                </div>

                                                <div class="x_content">

                                                    <table id="datatable-buttons" class="table table-striped table-bordered" style="width: 100%;">
                                                    </table>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div id="AuthDiv">
                        <div class="row" style="display: none">
                            <div class="col-md-4">
                                <label for="inputName" class="col-xs-4 control-label">Remark:</label>
                                <div class="col-sm-7">
                                    <asp:TextBox runat="server" ID="txtRemark" CssClass="form-control" TextMode="MultiLine" onkeypress="return onlyAlphabets(event,this);"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4"></div>
                            <div class="col-md-4">
                                <div>
                                    <div class="col-md-4">
                                        <asp:Button ID="btnAccept" runat="server" Text="Accept" CssClass="btn-div btn-style btn btn-primary" Style="display: none" OnClientClick="return funGetResult(1)" OnClick="AcceptRejectCardOpsRequests" />
                                    </div>
                                    <div class="col-md-4">
                                        <%--<asp:Button ID="Button2" runat="server" Text="Reject" CssClass="btn-div btn-style btn btn-primary" OnClientClick="return funGetResult(2)" OnClick="AcceptRejectCardOpsRequests" />--%>
                                        <input type="button" value="Reject" onclick="funGetResult(2)" id="btnReject" class="btn-div btn-style btn btn-primary" style="display: none" />
                                    </div>
                                    <div class="col-md-4">
                                        <asp:Button ID="Button1" runat="server" Text="Cancel" CssClass="btn-div btn-style btn btn-primary" OnClientClick="fnreset(null, true)" OnClick="Page_Load" Style="display: none" />
                                    </div>
                                </div>
                                <div>
                                    <AGS:usrButtons runat="server" ID="userBtns" />
                                </div>
                            </div>
                            <div class="col-md-4"></div>
                        </div>
                    </div>
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


        <!-- /.modal-content for Reject  -->

        <div id="RejectConfirmationModal" class="modal fade" data-backdrop="static" role="dialog">
            <div class="modal-dialog">
                <!-- Modal content -->
                <div class="modal-content" style="border-radius: 4px;">

                    <div class="modal-header">
                        <button aria-label="Close" data-dismiss="modal" class="close" type="button" onclick="funCancelModal()"><span aria-hidden="true">×</span></button>
                        <h4 id="myLargeRejectModal" class="modal-title" style="font-weight: bold">Confirmation</h4>
                    </div>
                    <%--//start Diksha--%>
                    <!--Display validation msg ------------------------------------------------------------------------->
                    <div class="pad margin no-print" id="RejecterrormsgDiv" style="display: none">
                        <div class="callout callout-info" style="margin-bottom: 0!important;">
                            <h4><i class="fa fa-info"></i>Information :</h4>
                            <span id="SpnRejectErrorMsg" class="text-center"></span>
                        </div>
                    </div>
                    <div class="modal-body">

                        <div class="row">

                            <label for="inputName" class="col-xs-5 control-label"><span style="color: red;">*</span>Do you want to reject this request?:</label>
                            <div class="col-sm-6">
                                <input type='radio' name='IsConfirm' value='1' />Yes
                        &nbsp;&nbsp;<input type='radio' name='IsConfirm' value='2' aria-label="Close" data-dismiss="modal" onclick="funCancelModal()" />No
                            </div>
                        </div>
                        <div class="row">
                            <br />
                        </div>
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
                                <asp:Button runat="server" CssClass="btn btn-primary" Text="Confirm" ID="Reject_Btn" OnClick="AcceptRejectCardOpsRequests" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <%-- /Modal View card operation req  details --%>
        <div id="ViewCardOpsReqModal" class="modal fade" data-backdrop="static" role="dialog">
            <div class="modal-dialog">
                <!-- Modal content -->
                <div class="modal-content" style="border-radius: 4px; width: 1000px; margin-left: -155px;">

                    <div class="modal-header">
                        <button aria-label="Close" data-dismiss="modal" class="close" type="button" onclick="FunCancelReqViewModal()"><span aria-hidden="true">×</span></button>
                        <h4 id="myLargeEdit" class="modal-title" style="font-weight: bold">Card Operation</h4>
                    </div>

                    <!--Display validation msg ------------------------------------------------------------------------->
                    <div class="pad margin no-print" id="ModalerrormsgDiv" style="display: none">
                        <div class="callout callout-info" style="margin-bottom: 0!important;">
                            <h4><i class="fa fa-info"></i>Information :</h4>
                            <span id="ModalSpnErrorMsg" class="text-center"></span>
                        </div>
                    </div>

                    <div class="modal-body" style="height: 300px; overflow-y: auto">
                        <div class="box-body" id="InfoDiv">
                            <div id="CustomerDtlDiv">
                                <div class="box-header">
                                    <h2 class="box-title">Customer Details</h2>
                                </div>
                                <div>
                                    <asp:HiddenField runat="server" ID="hdnRqID" />
                                    <asp:HiddenField runat="server" ID="hdnModalRqType" />
                                </div>
                                <div class="form-horizontal">
                                    <div class="row">
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <label for="inputName" class="col-xs-4 control-label">CustomerID:</label>
                                                <div class="col-sm-8">
                                                    <input type="text" class="form-control" maxlength="20" runat="server" name="Name" id="txtCustomerID" readonly="readonly" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <label for="inputName" class="col-xs-4 control-label">Customer Name:</label>
                                                <div class="col-sm-8">
                                                    <input type="text" class="form-control" maxlength="25" runat="server" name="Name" id="txtCustomerName" readonly="readonly" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <label for="inputName" class="col-xs-4 control-label">Date Of Birth:</label>
                                                <div class="col-sm-8">
                                                    <input type="text" class="form-control" maxlength="12" runat="server" name="Name" id="txtDOB" readonly="readonly" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <label for="inputName" class="col-xs-4 control-label">Address:</label>
                                                <div class="col-sm-8">
                                                    <asp:TextBox runat="server" ID="txtAddress" CssClass="form-control" ReadOnly="true" TextMode="MultiLine"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <label for="inputName" class="col-xs-4 control-label">MobileNo:</label>
                                                <div class="col-sm-8">
                                                    <input type="text" class="form-control" runat="server" maxlength="12" name="Name" id="txtMobile" readonly="readonly" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <label for="inputName" class="col-xs-4 control-label">EmailId:</label>
                                                <div class="col-sm-8">
                                                    <input type="text" class="form-control" runat="server" name="Name" id="txtEmail" readonly="readonly" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <label for="inputName" class="col-xs-4 control-label">Card No:</label>
                                                <div class="col-sm-8">
                                                    <input type="text" class="form-control" maxlength="20" runat="server" name="Name" id="txtCardNo" readonly="readonly" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <label for="inputName" class="col-xs-4 control-label">Request Type:</label>
                                                <div class="col-sm-8">
                                                    <asp:DropDownList runat="server" ID="ddlOpsReqType" disabled="disabled" CssClass="form-control"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-4"></div>
                                    </div>

                                    <div id="CardLimitDiv" style="display: none">
                                        <div class="box-header">
                                            <h2 class="box-title">Card Limits</h2>
                                        </div>
                                        <div class="form-horizontal">
                                            <div class="row">
                                                <div class="col-md-4">
                                                    <div class="form-group">
                                                        <label for="inputName" class="col-xs-4 control-label">Number of Purchases:</label>
                                                        <div class="col-sm-8">
                                                            <input type="text" class="form-control" maxlength="15" runat="server" name="Name" id="txtPurchaseNo" onkeypress="return FunChkIsNumber(event)" readonly="readonly" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-4">
                                                    <div class="form-group">
                                                        <label for="inputName" class="col-xs-4 control-label">Daily Purchase Amount:</label>
                                                        <div class="col-sm-8">
                                                            <input type="text" class="form-control" maxlength="15" runat="server" name="Name" id="txtDailyPurLmt" onkeypress="return FunChkIsNumber(event)" readonly="readonly" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-4">
                                                    <div class="form-group">
                                                        <label for="inputName" class="col-xs-4 control-label">PT Purchase Amount:</label>
                                                        <div class="col-sm-8">
                                                            <input type="text" class="form-control" maxlength="15" runat="server" name="Name" id="txtPtPurLmt" onkeypress="return FunChkIsNumber(event)" readonly="readonly" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-4">
                                                    <div class="form-group">
                                                        <label for="inputName" class="col-xs-4 control-label">No of Withdrawals:</label>
                                                        <div class="col-sm-8">
                                                            <input type="text" class="form-control" runat="server" maxlength="15" name="Name" id="txtWithdrawNo" onkeypress="return FunChkIsNumber(event)" readonly="readonly" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-4">
                                                    <div class="form-group">
                                                        <label for="inputName" class="col-xs-4 control-label">Daily Withdrawal Amount:</label>
                                                        <div class="col-sm-8">
                                                            <input type="text" class="form-control" runat="server" maxlength="15" name="Name" id="txtDailyWithdrawLmt" onkeypress="return FunChkIsNumber(event)" readonly="readonly" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-4">
                                                    <div class="form-group">
                                                        <label for="inputName" class="col-xs-4 control-label">PT Withdrawal Amount:</label>
                                                        <div class="col-sm-8">
                                                            <input type="text" class="form-control" runat="server" maxlength="15" name="Name" id="txtPtWithdrawLmt" onkeypress="return FunChkIsNumber(event)" readonly="readonly" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-4">
                                                    <div class="form-group">
                                                        <label for="inputName" class="col-xs-4 control-label">No of Payments:</label>
                                                        <div class="col-sm-8">
                                                            <input type="text" class="form-control" runat="server" maxlength="15" name="Name" id="txtPaymentNo" onkeypress="return FunChkIsNumber(event)" readonly="readonly" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-4">
                                                    <div class="form-group">
                                                        <label for="inputName" class="col-xs-4 control-label">Daily Payment Amount:</label>
                                                        <div class="col-sm-8">
                                                            <input type="text" class="form-control" runat="server" maxlength="15" name="Name" id="txtDailyPayLmt" onkeypress="return FunChkIsNumber(event)" readonly="readonly" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-4">
                                                    <div class="form-group">
                                                        <label for="inputName" class="col-xs-4 control-label">PT Payment Amount:</label>
                                                        <div class="col-sm-8">
                                                            <input type="text" class="form-control" runat="server" maxlength="15" name="Name" id="txtPtPayLmt" onkeypress="return FunChkIsNumber(event)" readonly="readonly" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="row">
                                                <div class="col-md-4">
                                                </div>
                                                <div class="col-md-4">
                                                    <div class="form-group">
                                                        <label for="inputName" class="col-xs-4 control-label">Daily CNP Amount:</label>
                                                        <div class="col-sm-8">
                                                            <input type="text" class="form-control" runat="server" maxlength="15" name="Name" id="txtDailyCNPLmt" onkeypress="return FunChkIsNumber(event)" readonly="readonly" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-4">
                                                    <div class="form-group">
                                                        <label for="inputName" class="col-xs-4 control-label">PT CNP Amount:</label>
                                                        <div class="col-sm-8">
                                                            <input type="text" class="form-control" runat="server" maxlength="15" name="Name" id="txtPtCNPLmt" onkeypress="return FunChkIsNumber(event)" readonly="readonly" />
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

                    <div class="modal-footer">
                        <div id="ModalAuthDiv" style="display: none">
                            <div class="row">
                                <div class="col-md-4">
                                    <label for="inputName" class="col-xs-4 control-label">Remark:</label>
                                    <div class="col-sm-8">
                                        <%--<input type="text" class="form-control" runat="server" maxlength="20" name="Name" id="txtUpdateRemark" onkeypress="return onlyAlphabets(event,this);" />--%>
                                        <asp:TextBox runat="server" ID="txtReason" CssClass="form-control" TextMode="MultiLine" onkeypress="return onlyAlphabets(event,this);"></asp:TextBox>

                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-4">
                                </div>
                                <div class="col-md-4">
                                    <div class="col-md-4">
                                        <asp:Button ID="ModalAccept" runat="server" Text="Accept" CssClass="btn-div btn-style btn btn-primary" OnClientClick="return funModalAcceptReject(1)" OnClick="AcceptRejectCardOpsRequests" />
                                    </div>
                                    <div class="col-md-4">
                                        <asp:Button ID="ModalReject" runat="server" Text="Reject" CssClass="btn-div btn-style btn btn-primary" OnClientClick="return funModalAcceptReject(2)" OnClick="AcceptRejectCardOpsRequests" />
                                        <%--<input type="button" value="Reject" onclick="funModalAcceptReject(2)" class="btn-div btn-style btn btn-primary" />--%>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

    </asp:Panel>

    <script>
        //Dynamic datatable bind
        $(document).ready(function () {

            //Start   Dynamic datatable bind
            $('#datatable-buttons').html($("#<%=hdnTransactionDetails.ClientID %>").val());
            $("#<%=hdnTransactionDetails.ClientID %>").val('');

            //Hide checkbox when request is  accepted/Rejected

            $('input[type=checkbox][formstatus=1]').attr('disabled', true)

            $('input[type=checkbox][formstatus=2]').attr('disabled', true)
            //If record found 
            if ($("#<%=hdnResultStatus.ClientID%>").val() == "1") {

                $("#DivResult").show()

                //If user has Accept right
                if ($.inArray("C", $('#<%=hdnAccessCaption.ClientID%>').val().split(",")) > -1) {
                    $('#AuthDiv').show();
                    $('#SelectAllDiv').show()


                }
                else {
                    $('#SelectAllDiv').hide()
                    $('#ModalAuthDiv').hide()

                }


            }
            else {

                $("#DivResult").hide()
            }





            //For Data Table
            if ($("#datatable-buttons tbody tr").length > 0) {
                //var handleDataTableButtons = function () {
                //    if ($("#datatable-buttons").length) {
                //        $("#datatable-buttons").DataTable({
                //            dom: "Bfrtip",
                //            buttons: [
                //              {
                //                  extend: "copy",
                //                  className: "btn-sm"
                //              },
                //              {
                //                  extend: "csv",
                //                  className: "btn-sm"
                //              },
                //              {
                //                  extend: "excel",
                //                  className: "btn-sm"
                //              },
                //              {
                //                  extend: "pdfHtml5",
                //                  className: "btn-sm"
                //              },
                //              {
                //                  extend: "print",
                //                  className: "btn-sm"
                //              },
                //            ],
                //            //responsive: true
                //            scrollX: true
                //        });
                //    }
                //};

                //TableManageButtons = function () {
                //    "use strict";
                //    return {
                //        init: function () {
                //            handleDataTableButtons();
                //        }
                //    };
                //}();

                //TableManageButtons.init();
                var handleDataTableButtons = function () {
                    if ($("#datatable-buttons").length) {
                        $("#datatable-buttons").DataTable({
                            "info": true,

                            scrollX: true
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

            }
            //End   Dynamic datatable bind

            ////Hide FormStatusID
            //$("#datatable-buttons tr :nth-child(6)").hide()





            // ************* Select All function *******
            $("#select_all").click(function (event) {  //"select all" change 

                //select all checkboxes
                if ($(this).is(":checked")) {
                    $('#datatable-buttons tbody input[type=checkbox]:not(:disabled)').prop("checked", 'true')
                }
                else {
                    $('#datatable-buttons tbody input[type=checkbox]').prop("checked", false)
                }
            });

            $('#datatable-buttons tbody input[type=checkbox]').change(function () {

                if ($('#datatable-buttons tbody input[type=checkbox]').length == $('#datatable-buttons tbody input[type=checkbox]:checked').length) {
                    $("#select_all").prop('checked', true);
                }
                else {
                    $("#select_all").prop('checked', false);
                }
            })

        });
        $('#phPageBody_userBtns_btnCancel_U').click(function () {
            fnreset(null, true);
        });

        //Reject button click
        $('#phPageBody_userBtns_btnReject_U').click(function () {
            $('input[name="IsConfirm"]').attr("checked", false)
            $('#phPageBody_txtRejectReson').val('')
            $('input[name="IsConfirm"]').val(0)
        });

        //confirm button click in reject pop up
        $('#phPageBody_Reject_Btn').click(function () {
            var errrorTextPD = 'Please provide ';
            var errorFieldsPD = '';
            if (!($('input[name="IsConfirm"]').is(':checked'))) {
                errorFieldsPD = errorFieldsPD + (errorFieldsPD != '' ? '<b>,' : '<b>') + ' option(Yes/No) </b> ';
            }

            if ($('#phPageBody_txtRejectReson').val() == "") {
                errorFieldsPD = errorFieldsPD + (errorFieldsPD != '' ? '<b>,' : '<b>') + ' Reason</b> ';
            }


            if (errorFieldsPD != '') {
                $('#SpnRejectErrorMsg').html(errrorTextPD + errorFieldsPD);
                $('#RejecterrormsgDiv').show();
                return false;
            }
            else {
                errorFieldsPD = '';
                errrorTextSectionPD = '';
                errrorTextPD = '';
                $('.shader').fadeIn();
                $('#errormsgDiv').hide();
            }

        });

        //Validation on Search Button
        function FunValidation() {


            var CustomerID = $('#phPageBody_txtSearchCustomerID').val()
            var CardNo = $('#phPageBody_txtSearchCardNo').val()
            var RequestType = $("#<%=ddlRequestType.ClientID%> ").val();



            if (RequestType == "0") {
                $('#SpnErrorMsg').html('Please provide Card Request Type');
                $('#errormsgDiv').show();
                return false;
            }
            else {
                $('#errormsgDiv').hide();
                //       $('input[type="text"]').val('');
                //     $('#phPageBody_txtAddress').val('');
                $('.shader').fadeIn();
                return true;
            }
        }

        //Fun for checkbox validation 
        function funGetResult(FormstatusID) {


            if ($('#datatable-buttons tbody input[type=checkbox]:checked').length == 0) {
                $('#SpnErrorMsg').html("Please select Card Requests to accept/reject");
                $('#errormsgDiv').show();
                return false;
            }
            else {
                $('#SpnErrorMsg').html('');
                $('#errormsgDiv').hide();
                $("#<%=hdnFormStatusID.ClientID%>").val(FormstatusID);
                $("#<%=hdnReqType.ClientID%>").val($($('#datatable-buttons tbody input[type=checkbox]:checked')[0]).attr('reqTypeId'));

                var ArrayIds = [];
                $('#datatable-buttons tbody input[type=checkbox]:checked').each(function (i) {
                    ArrayIds[i] = $(this).attr("ReqID");

                });
                //alert(val.join(","))

                $("#<%=hdnRequestIDs.ClientID%>").val(ArrayIds.join(","))

                //for Reject request
                if (FormstatusID == "2") {

                    // $('[id$="txtRejectReson"]').attr('required', true)
                    $('#RejectConfirmationModal').modal('show');

                }
                else
                {
                    $('.shader').fadeIn();
                }
                return true;
            }
        }

        //Confirm rejection
        $("[name$='IsConfirm']").click(function () {
            if ($("input:radio[name='IsConfirm']:checked").val() == "1") {

                $('#remarkDiv').css("display", "")
                $('#phPageBody_Reject_Btn').css("display", "")
                $('[id$="txtRejectReson"]').attr('required', true)
            }
        });

        //Cancel Reject Confirmation modal
        function funCancelModal() {

            //$('[id$="txtRejectReson"]').attr('required', false)
            $('input[name="IsConfirm"]').attr("checked", false)
            $('#phPageBody_txtRejectReson').val('')
            $('input[name="IsConfirm"]').val(0)

        }
        //fun Get Card req by ID
        function FunShowDetails(obj) {
            var reqid = $(obj).attr('reqid');
            var reqtypeid = $(obj).attr('reqtypeid');
            var SystemID = '<%=HttpContext.Current.Session["SystemID"]%>'


            $.ajax({
                type: "POST",
                url: "CheckCardOperation.aspx/FunViewDetails",
                data: '{ID:' + reqid + ',ReqTypeID:' + reqtypeid + ',SystemID:' + SystemID + ' }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
                },
                success: function (result) {
                    var resFromDB = result.d;
                    if (resFromDB == '') {
                        $("[id$=divAlert]").show();
                        $("[id$=lblAlertMessageHeader]").text('Alert !');
                        $("[id$=lblAlertMessageText]").text('User details not found.');
                        $("[id$=divAlert]").removeClass().addClass('alert alert-info alert-dismissable');
                    }
                    var tbl = JSON.parse(result.d)
                    if (tbl.length > 0) {
                        $("[id$='txtCustomerID']").val(tbl[0].CustomerID);
                        $("[id$='txtCardNo']").val(tbl[0].CardNo);
                        $("[id$='txtDOB']").val(tbl[0].DOB);
                        $("[id$='txtAddress']").val(tbl[0].Address);
                        $("[id$='txtEmail']").val(tbl[0].Email);
                        $("[id$='txtMobile']").val(tbl[0].MobileNo);
                        $("[id$='ddlOpsReqType']").append($('<option>').val(tbl[0].RequestTypeID));
                        $("[id$='ddlOpsReqType']")[0].selectedIndex = tbl[0].RequestTypeID;
                        $("[id$='txtCustomerName']").val(tbl[0].CustomerName);
                        $("[id$='hdnRqID']").val(tbl[0].ID);
                        //for set card limit req
                        if (tbl[0].RequestTypeID == "1") {
                            $("[id$='txtPurchaseNo']").val(tbl[0].PurchaseNo);
                            $("[id$='txtDailyPurLmt']").val(tbl[0].PurchaseDailyLimit);
                            $("[id$='txtPtPurLmt']").val(tbl[0].PurchasePTLimit);
                            $("[id$='txtWithdrawNo']").val(tbl[0].WithDrawNO);
                            $("[id$='txtDailyWithdrawLmt']").val(tbl[0].WithDrawDailyLimit);
                            $("[id$='txtPtWithdrawLmt']").val(tbl[0].WithDrawPTLimit);
                            $("[id$='txtPaymentNo']").val(tbl[0].PaymentNO);
                            $("[id$='txtDailyPayLmt']").val(tbl[0].PaymentDailyLimit);
                            $("[id$='txtPtPayLmt']").val(tbl[0].PaymentPTLimit);
                            $("[id$='txtDailyCNPLmt']").val(tbl[0].CNPDailyLimit);
                            $("[id$='txtPtCNPLmt']").val(tbl[0].CNPPTLimit)
                            $("#CardLimitDiv").css("display", "")
                        }
                        else {
                            $("#CardLimitDiv").css("display", "none")
                        }

                        if ((tbl[0].FormStatusID == "1") || (tbl[0].FormStatusID == "2")) {

                            $("#ModalAuthDiv").css("display", "none")
                        }
                        else {
                            //If user has Accept right
                            if ($.inArray("C", $('#<%=hdnAccessCaption.ClientID%>').val().split(",")) > -1) {
                                $("#ModalAuthDiv").css("display", "")
                            }
                        }


                        $("#ViewCardOpsReqModal").modal('show');
                    }
                }
            });
        }

        function funModalAcceptReject(formStaus) {
            $("#<%=hdnFormStatusID.ClientID%>").val(formStaus);
            $("#<%=hdnModalRqType.ClientID%>").val($("#<%=ddlOpsReqType.ClientID%>").val());

            //for Reject request
            if (formStaus == "2") {

                if ($("#<%=txtReason.ClientID%>").val() == "") {

                    $("#ModalSpnErrorMsg").html("Provide reason of reject");
                    $("#ModalerrormsgDiv").show();
                    return false;
                }
                else {
                    return true;
                    $("#ModalerrormsgDiv").hide();
                }
            }
            else {
                return true;
            }
        }

        function FunCancelReqViewModal() {
            fnreset("ViewCardOpsReqModal", true);
        }

    </script>

    <script>
     <%--//User control  buttons click--%>
        $(document).ready(function () {

            usrbtn.btnAcceptClick = function () {

                //FunReject();
                if (!funGetResult(1)) {
                    return false;
                }
                //return false;

            }


            usrbtn.btnRejectClick = function () {

                funGetResult(2);
                return false;

            }


        });

        //$('#phPageBody_userBtns_btnAccept_U').click(function () {
        //    funGetResult(1);
        //})
    </script>
</asp:Content>
