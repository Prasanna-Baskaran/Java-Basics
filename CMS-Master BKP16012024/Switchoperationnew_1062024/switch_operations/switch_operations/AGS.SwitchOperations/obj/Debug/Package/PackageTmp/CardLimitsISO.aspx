<%@ Page Title="" Language="C#" MasterPageFile="~/SwitchOperationSite.Master" AutoEventWireup="true" CodeBehind="CardLimitsISO.aspx.cs" Inherits="AGS.SwitchOperations.CardLimitsISO" %>
<%@ Register Src="~/UserControls/UserButtons.ascx" TagPrefix="AGS" TagName="usrButtons" %>
<asp:Content ID="Content2" ContentPlaceHolderID="phPageHeader" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phPageBody" runat="server">
    <asp:HiddenField ID="hdnCustomerID" runat="server" />
    <asp:HiddenField ID="hndCardNo" runat="server" />
    <asp:HiddenField ID="hdnResultStatus" runat="server" />
    <asp:HiddenField runat="server" ID="hdnFlag"/>
    <asp:HiddenField ID="hdnTransactionDetails" runat="server" />
    <asp:HiddenField ID="hdnLoginRoleID" runat="server" />
    <asp:HiddenField ID="hdnLimitStatusID" runat="server" />
    <asp:HiddenField ID="hdnAccessCaption" runat="server" />
     <asp:HiddenField ID="hdnRPANID" runat="server" />
    
    <%--<asp:Button runat="server" ID="hdnAcceptBtn" OnClick="btnAccept_Click" Style="display: none;" />
    <asp:Button runat="server" ID="hdnRejectBtn" OnClick="btnReject_Click" Style="display: none;" />--%>

    <asp:Panel ID="pnlCustomerDtl" runat="server">
        <div style="font-size: 14px;">
            <div id="SearchCustomer">
                <div class="box-primary">
                    <div class="box-header with-border">
                        <i class="fa fa-credit-card"></i>
                        <!-- start sheetal change card details to  Set card Limit-->
                        <h4 class="box-title">Set Card limit :</h4>
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
                        <%--*************************************Search Customer *************** --%>
                        <div class="box-body" id="SearchDiv">
                            <div class="form-horizontal">
                                <div class="row">
                                    <div class="col-md-4" style="display:none">
                                        <div class="form-group">
                                            <label for="inputName" class="col-xs-4 control-label">Application No:</label>
                                            <div class="col-sm-7">
                                                <asp:DropDownList runat="server" ID="ddlApplicationNo" CssClass="form-control"></asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="form-group">
                                            <label for="inputName" class="col-xs-4 control-label">CustomerID:</label>
                                            <div class="col-sm-7">
                                                <input type="text" class="form-control" maxlength="20" runat="server" name="CustomerID" id="txtSearchCustomerID" onkeypress="return FunChkIsNumber(event)" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="form-group">
                                            <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>Card No:</label>
                                            <div class="col-sm-8">
                                                <input type="text" class="form-control" maxlength="20" runat="server" name="CardNO" id="txtSearchCardNo" onkeypress="return FunChkIsNumber(event)" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="form-group">
                                            <div class="col-sm-4"></div>
                                            
                                            <div class="col-sm-4">
                                                <div class="col-md-7">
                                                    <asp:Button runat="server" CssClass="btn btn-primary pull-right" ID="btnSearchCustomer" Text="Search" OnClick="btnSearch_Click" OnClientClick="return FunValidation();" />
                                                </div>
                                            </div>
                                            <div class="col-sm-4"></div>
                                        </div>
                                    </div>

                                </div>
                            </div>
                        </div>

                        <div class="row"id="DivMsg" >
                            <div class="col-md-6">
                                <h4><label   maxlength="20" runat="server" name="Name" id="LblResult" readonly="readonly"/></h4>
                            </div>                                                     
                        </div>

                        <div class="box-body" id="InfoDiv">
                            <div id="CustomerDtlDiv">
                                <div class="box-header">
                                    <h2 class="box-title">Customer Details</h2>
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
                                </div>
                            </div>
                            <div id="CardDetailsDiv">
                                <div class="box-header">
                                    <h2 class="box-title">Card Details</h2>
                                </div>
                                <div class="form-horizontal">
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
                                                <label for="inputName" class="col-xs-4 control-label">Expiry Date:</label>
                                                <div class="col-sm-8">
                                                    <input type="text" class="form-control" maxlength="12" runat="server" name="Name" id="txtExpiryDate" readonly="readonly" />
                                                </div>
                                            </div>
                                        </div>
                                        <%-- 27/07 accountNo --%>
                                        <div class="col-md-4" >
                                            <div class="form-group">
                                                <label for="inputName" class="col-xs-4 control-label">Account No:</label>
                                                <div class="col-sm-8">
                                                    <input type="text" class="form-control" maxlength="20" runat="server" name="Name" id="txtAccountNo" readonly="readonly" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <label for="inputName" class="col-xs-4 control-label">Card Status:</label>
                                                <div class="col-sm-8">
                                                    <input type="text" class="form-control" runat="server" name="Name" id="txtCardStatus" readonly="readonly" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <label for="inputName" class="col-xs-4 control-label">Card Issued:</label>
                                                <div class="col-sm-8">
                                                    <input type="text" class="form-control" runat="server" maxlength="12" name="Name" id="txtCardIssued" readonly="readonly" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-4" style="display:none">
                                            <div class="form-group">
                                                <label for="inputName" class="col-xs-4 control-label">Card Status ID:</label>
                                                <div class="col-sm-8">
                                                    <input type="text" class="form-control" runat="server" maxlength="12" name="Name" id="txtCardStatusID" readonly="readonly" />
                                                </div>
                                            </div>
                                        </div>
                                        <%--<div class="col-md-4">
                                        <div class="form-group">
                                            <label for="inputName" class="col-xs-4 control-label">EmailId:</label>
                                            <div class="col-sm-7">
                                                <input type="text" class="form-control" runat="server" name="Name" id="Text6" readonly="readonly" />
                                            </div>
                                        </div>
                                    </div>--%>
                                    </div>
                                </div>
                            </div>
                            <div id="CardLimitDiv">
                                <div class="box-header">
                                    <h2 class="box-title">Card Limits</h2>
                                </div>
                                <div class="form-horizontal">
                                    <div class="row">
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <label for="inputName" class="col-xs-4 control-label">Number of Purchases:</label>
                                                <div class="col-sm-8">
                                                    <input type="text" class="form-control" maxlength="15" runat="server" name="Name" id="txtPurchaseNo" onkeypress="return FunChkIsNumber(event)" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <label for="inputName" class="col-xs-4 control-label">Daily Purchase Amount:</label>
                                                <div class="col-sm-8">
                                                    <input type="text" class="form-control" maxlength="15" runat="server" name="Name" id="txtDailyPurLmt" onkeypress="return FunChkIsNumber(event)" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <label for="inputName" class="col-xs-4 control-label">PT Purchase Amount:</label>
                                                <div class="col-sm-8">
                                                    <input type="text" class="form-control" maxlength="15" runat="server" name="Name" id="txtPtPurLmt" onkeypress="return FunChkIsNumber(event)" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <label for="inputName" class="col-xs-4 control-label">No of Withdrawals:</label>
                                                <div class="col-sm-8">
                                                    <input type="text" class="form-control" runat="server" maxlength="15" name="Name" id="txtWithdrawNo" onkeypress="return FunChkIsNumber(event)" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <label for="inputName" class="col-xs-4 control-label">Daily Withdrawal Amount:</label>
                                                <div class="col-sm-8">
                                                    <input type="text" class="form-control" runat="server" maxlength="15" name="Name" id="txtDailyWithdrawLmt" onkeypress="return FunChkIsNumber(event)" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <label for="inputName" class="col-xs-4 control-label">PT Withdrawal Amount:</label>
                                                <div class="col-sm-8">
                                                    <input type="text" class="form-control" runat="server" maxlength="15" name="Name" id="txtPtWithdrawLmt" onkeypress="return FunChkIsNumber(event)" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <label for="inputName" class="col-xs-4 control-label">No of Payments:</label>
                                                <div class="col-sm-8">
                                                    <input type="text" class="form-control" runat="server" maxlength="15" name="Name" id="txtPaymentNo" onkeypress="return FunChkIsNumber(event)" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <label for="inputName" class="col-xs-4 control-label">Daily Payment Amount:</label>
                                                <div class="col-sm-8">
                                                    <input type="text" class="form-control" runat="server" maxlength="15" name="Name" id="txtDailyPayLmt" onkeypress="return FunChkIsNumber(event)" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <label for="inputName" class="col-xs-4 control-label">PT Payment Amount:</label>
                                                <div class="col-sm-8">
                                                    <input type="text" class="form-control" runat="server" maxlength="15" name="Name" id="txtPtPayLmt" onkeypress="return FunChkIsNumber(event)" />
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
                                                    <input type="text" class="form-control" runat="server" maxlength="15" name="Name" id="txtDailyCNPLmt" onkeypress="return FunChkIsNumber(event)" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <label for="inputName" class="col-xs-4 control-label">PT CNP Amount:</label>
                                                <div class="col-sm-8">
                                                    <input type="text" class="form-control" runat="server" maxlength="15" name="Name" id="txtPtCNPLmt" onkeypress="return FunChkIsNumber(event)" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div id="RequestChngDiv">
                                <div class="row">
                                    <div class="form-group">
                                        <div class="col-md-4">
                                        <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>Remark:</label>
                                        <div class="col-sm-8">
                                            <%--<input type="text" class="form-control" runat="server" maxlength="20" name="Name" id="txtUpdateRemark" onkeypress="return onlyAlphabets(event,this);" />--%>
                                            <asp:TextBox runat="server" ID="txtUpdateRemark" CssClass="form-control"  TextMode="MultiLine"></asp:TextBox>

                                        </div>
                                            </div>
                                        <div class="col-md-4">

                                        </div>
                                    </div>
                                </div>
                                <div>
                                    <br />
                                </div>
                                <div class="row" id="SaveDiv">
                                    <div class="col-md-4"></div>
                                    <div class="">
                                            <%--<asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn-div btn-style btn btn-primary" OnClick="btnSave_Click"  />--%>
                                            <AGS:usrButtons runat="server" ID="userBtns" />
                                        </div>
                                        <%--<div>
                                            <input type="button"  id="Cancel"  onclick="FunClear()" class="btn-div btn-style btn btn-primary" value="Cancel"/>
                                        </div>--%>

                                    </div>
                                    
                                </div>

                                <div class="row" id="AuthDiv" style="display:none" >
                                    <div class="col-md-4"></div>
                                    <div class="col-md-4">                                        
                                        <div class="col-md-6">
                                            <asp:Button ID="Button1" runat="server" Text="Accept" CssClass="btn-div btn-style btn btn-primary" />
                                        </div>
                                        <div class="col-md-6">
                                            <asp:Button ID="Button2" runat="server" Text="Reject" CssClass="btn-div btn-style btn btn-primary"   />
                                        </div>

                                    </div>
                                    <div class="col-md-4"></div>
                                </div>
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

        
        <%--Response Msg Modal--%>
        <!-- Modal HTML -->
        <div id="memberModal" class="modal fade bs-example-modal-lg" tabindex="-1" role="dialog"  data-backdrop="static" aria-labelledby="myLargeModalLabel">
            <div class="modal-dialog modal-md">
                <div class="modal-content" style="border-radius: 6px">
                    <div class="modal-header box-primary" style="border-top-right-radius: 6px; border-top-left-radius: 6px">
                        <button aria-label="Close" data-dismiss="modal" class="close" type="button" onclick="CancelModal();"><span aria-hidden="true">×</span></button>
                        <h4 id="myLargeModalLabel" class="modal-title" style="font-weight:bold">Card Limit</h4>
                    </div>
                    <div class="modal-body" id="msgBody">
                        <label for="username" class="control-label" id="LblMsg" runat="server" style="font-weight: normal"></label>
                        <button aria-label="Close" data-dismiss="modal" id="BtnModalClose" class="btn btn-primary pull-right" type="button" onclick="CancelModal();"><span aria-hidden="true">OK</span></button>
                    </div>
                </div>
            </div>
            <!-- /.modal-content -->
        </div>

     <%--   <!-- /.modal-content for Reject  card limit -->

        <div id="RejectConfirmationModal" class="modal w3-quarter">
            <!-- Modal content -->
            <div class="modal-content">

                <div class="modal-header box-primary" style="border-top-right-radius: 6px; border-top-left-radius: 6px">
                    <button aria-label="Close" data-dismiss="modal" class="close" type="button"><span aria-hidden="true">×</span></button>
                    <h4 id="myLargeModaldelete" class="modal-title" style="font-weight: bold">Confirmation</h4>
                </div>
                <div class="modal_body">
                    <div>


                        <div class="w3-row" style="border-bottom: 1px solid #f7f3f5; padding: 5px;">
                            <div class="w3-half"><b>Do you want to reject card limit ?</b></div>
                            <div class="w3-half">

                                <input type='radio' name='IsConfirm' value='1' />Yes
                        &nbsp;&nbsp;<input type='radio' name='IsConfirm' value='2' aria-label="Close" data-dismiss="modal" />No

                            </div>

                        </div>

                        <div class="w3-row" style="border-bottom: 1px solid #f7f3f5; padding: 5px; display: none" id="remarkDiv">
                            <div class="w3-half"><b>Reason</b><span style="color: red;">*</span></div>
                            <div class="w3-half">

                                <asp:TextBox runat="server" ID="txtRejectReson" TextMode="MultiLine" MaxLength="50" onkeypress="return onlyAlphabets(event,this);"></asp:TextBox>
                            </div>

                        </div>
                        <div class="w3-row">
                            <div class="w3-half">

                                <button aria-label="Close" data-dismiss="modal" class="btn btn-primary pull-right" style="margin-right: 10px;" type="button"><span aria-hidden="true">CANCEL</span></button>

                            </div>
                            <div class="w3-half">
                                <asp:Button runat="server" CssClass="btn btn-primary" Text="Confirm" ID="Reject_Btn"  />
                            </div>
                        </div>
                    </div>
                </div>

            </div>
        </div>--%>
    </asp:Panel>

    <%--************************************* SCRIPTS *********************************--%>
    <script>

        //Cancel button click
        $('#phPageBody_userBtns_btnCancel_U').click(function () {

            fnreset(null, true);
            $(this).attr("onClick", "Page_Load")
            $("#<%=hdnFlag.ClientID %>").val('')

        });

        //Dynamic datatable bind
        $(document).ready(function () {

            //Start   Dynamic datatable bind
            $('#datatable-buttons').html($("#<%=hdnTransactionDetails.ClientID %>").val());
            $("#<%=hdnTransactionDetails.ClientID %>").val('');



            //For Data Table
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
            }
            //End   Dynamic datatable bind

            ////Hide LimitStatusID
            //$("#datatable-buttons tr :nth-child(6)").hide()

            /////for  temporary block Card and those user having Save rights
            if (($("#<%=txtCardStatusID.ClientID%>").val() == "1") && ($("#<%=hdnAccessCaption.ClientID%>").val() == "S")) {
                $('#RequestChngDiv').show();
            }
            else {
                $('#CardLimitDiv input[type="text"]').attr("readonly", true)
                $('#RequestChngDiv').hide();
            }


        });

        //Response Message
        function FunShowMsg() {

            $('#memberModal').modal('show');
            if ($('#phPageBody_hdnResultStatus').val() == 1) {
                //$('#phPageBody_ddlApplicationNo').val(0);
                fnreset(null, true);
                $("#<%=hdnFlag.ClientID %>").val() == "";
                $('textarea').empty();
                //$('#SearchDiv').show();
                //$('#InfoDiv').hide();
            }
        }


        if ($("#<%=hdnFlag.ClientID %>").val() == "2") {
            $('#SearchDiv').hide();
            $('#InfoDiv').show();
        }
        else {
            $('#SearchDiv').show();
            $('#InfoDiv').hide();
        }


        //$('#phPageBody_ddlApplicationNo').val('0')


        //Validation on Search Button
        function FunValidation() {

            var ApplicationNo = $('#phPageBody_ddlApplicationNo').val()
            var CustomerID = $('#phPageBody_txtSearchCustomerID').val()
            var CardNo = $('#phPageBody_txtSearchCardNo').val()



            if ((CustomerID == "") && (CardNo == "")) {
                $('#SpnErrorMsg').html('Please provide CustomerID / Card No');
                $('#errormsgDiv').show();
                return false;
            }
            else if (CardNo == "") {
                $('#SpnErrorMsg').html('Please provide Card No');
                $('#errormsgDiv').show();
                return false;
            }
            else {
                $('#errormsgDiv').hide();

                $("#<%=txtAddress.ClientID%>").val('');
                $("#<%=txtUpdateRemark.ClientID%>").val('');
                $('.shader').fadeIn();
                return true;
            }


    }

    //Validation Save        

    $("#phPageBody_userBtns_btnSave_U").click(function () {
        var errrorTextFD = 'Please provide ';

        var errorFieldsFD = '';

        if ($('#<%=txtUpdateRemark.ClientID%>').val().trim() == '') {

                errorFieldsFD = errorFieldsFD + (errorFieldsFD != '' ? '<b>,' : '<b>') + ' Remark</b> ';
            }
            if ($('#<%=txtPtCNPLmt.ClientID%>').val().trim() == '') {

                errorFieldsFD = errorFieldsFD + (errorFieldsFD != '' ? '<b>,' : '<b>') + ' PT CNP Amount</b> ';
            }
            if ($('#<%=txtDailyCNPLmt.ClientID%>').val().trim() == '') {

                errorFieldsFD = errorFieldsFD + (errorFieldsFD != '' ? '<b>,' : '<b>') + ' Daily CNP Amount</b> ';
            }

            if ($('#<%=txtPtPayLmt.ClientID%>').val().trim() == '') {

                errorFieldsFD = errorFieldsFD + (errorFieldsFD != '' ? '<b>,' : '<b>') + ' PT Payment Amount</b> ';
            }
            if ($('#<%=txtDailyPayLmt.ClientID%>').val().trim() == '') {

                errorFieldsFD = errorFieldsFD + (errorFieldsFD != '' ? '<b>,' : '<b>') + ' Daily Payment Amount</b> ';
            }

            if ($('#<%=txtPaymentNo.ClientID%>').val().trim() == '') {

                errorFieldsFD = errorFieldsFD + (errorFieldsFD != '' ? '<b>,' : '<b>') + ' No of Payments </b> ';
            }

            if ($('#<%=txtPtWithdrawLmt.ClientID%>').val().trim() == '') {

                errorFieldsFD = errorFieldsFD + (errorFieldsFD != '' ? '<b>,' : '<b>') + ' PT Withdrawl Amount</b> ';
            }
            if ($('#<%=txtDailyWithdrawLmt.ClientID%>').val().trim() == '') {

                errorFieldsFD = errorFieldsFD + (errorFieldsFD != '' ? '<b>,' : '<b>') + ' Daily Withdrawl Amount</b> ';
            }

            if ($('#<%=txtWithdrawNo.ClientID%>').val().trim() == '') {

                errorFieldsFD = errorFieldsFD + (errorFieldsFD != '' ? '<b>,' : '<b>') + ' No of Withdrawls </b> ';
            }



            if ($('#<%=txtPurchaseNo.ClientID%>').val().trim() == '') {

                errorFieldsFD = errorFieldsFD + (errorFieldsFD != '' ? '<b>,' : '<b>') + ' PT Withdrawl Amount</b> ';
            }
            if ($('#<%=txtDailyPurLmt.ClientID%>').val().trim() == '') {

                errorFieldsFD = errorFieldsFD + (errorFieldsFD != '' ? '<b>,' : '<b>') + ' Daily Withdrawl Amount</b> ';
            }

            if ($('#<%=txtDailyPurLmt.ClientID%>').val().trim() == '') {

                errorFieldsFD = errorFieldsFD + (errorFieldsFD != '' ? '<b>,' : '<b>') + ' No of Withdrawls </b> ';
            }

            if ($('#<%=txtPurchaseNo.ClientID%>').val().trim() == '') {

                errorFieldsFD = errorFieldsFD + (errorFieldsFD != '' ? '<b>,' : '<b>') + ' No of Purchases</b> ';
            }
            if ($('#<%=txtDailyPurLmt.ClientID%>').val().trim() == '') {

                errorFieldsFD = errorFieldsFD + (errorFieldsFD != '' ? '<b>,' : '<b>') + ' Daily Purchase Amount</b> ';
            }

            if ($('#<%=txtDailyPurLmt.ClientID%>').val().trim() == '') {

                errorFieldsFD = errorFieldsFD + (errorFieldsFD != '' ? '<b>,' : '<b>') + ' PT Purchase Amount</b> ';
            }



            if (errorFieldsFD != '') {
                $('#SpnErrorMsg').html(errrorTextFD + errorFieldsFD);
                $('#errormsgDiv').show();

                errorFieldsFD = '';
                errrorTextSectionFD = '';
                errrorTextFD = '';
                return false;
            }
            else {
                $('.shader').fadeIn();

            }
        });



        //Cancel Modal
        function CancelModal() {
            if ($('#phPageBody_hdnResultStatus').val() == 1) {
                FunClear();

            }
        }

        function FunClear() {
            fnreset(null, true);
            $('#SearchDiv').show();
            $('#InfoDiv').hide();

        }

        //Accept Limit Click
        function FunAcceptLimit(obj) {
            var tds = $(obj).parent().parent().find('td');
            var CustomerID = $(tds[1]).text();
            var Limit = $(tds[4]).text();
            var LimitStatusId = $(tds[5]).text();
            $('#phPageBody_hdnCustomerID').val(CustomerID);
            document.getElementById("phPageBody_hdnAcceptBtn").click();

        }

        //Reject Limit Click
        function FunRejectLimit(obj) {
            var tds = $(obj).parent().parent().find('td');
            var CustomerID = $(tds[1]).text();
            var Limit = $(tds[4]).text();
            var LimitStatusId = $(tds[5]).text();
            $('#phPageBody_hdnCustomerID').val(CustomerID);
            $('#phPageBody_txtRejectReson').val('');
            $('#remarkDiv').css("display", "none");
            $('#phPageBody_Reject_Btn').css("display", "none")
            $('input[name=IsConfirm]').attr('checked', false);
            //   $('#RejectConfirmationModal').modal('show');


            //document.getElementById("phPageBody_hdnRejectBtn").click();
        }

        //Confirm rejection
        $("[name$='IsConfirm']").click(function () {
            if ($("input:radio[name='IsConfirm']:checked").val() == "1") {

                $('#remarkDiv').css("display", "")
                $('#phPageBody_Reject_Btn').css("display", "")
                $('[id$="txtRejectReson"]').attr('required', true)
            }
        });


        ////Give Limit Pop-Up
        //function funGiveLimit(obj) {
        //    var tds = $(obj).parent().parent().find('td');
        //    var CustomerID = $(tds[1]).text();
        //    var Limit = $(tds[4]).text();
        //    var LimitStatusId = $(tds[5]).text();
        //    $('#phPageBody_hdnCustomerID').val(CustomerID);
        //    $('#phPageBody_txtLimit').val(Limit);

        //    $('#phPageBody_ddlAppNoModal').val(CustomerID);
        //    $('[id$="ddlAppNoModal"]').attr('disabled', 'disabled');

        //    $('#GiveLimitModal').modal('show');


        //}

    </script>
</asp:Content>
