<%@ Page Title="" Language="C#" MasterPageFile="~/SwitchOperationSite.Master" AutoEventWireup="true" CodeBehind="CustomerUpdate.aspx.cs" Inherits="AGS.SwitchOperations.CustomerUpdate" %>

<%@ Register Src="~/UserControls/UserButtons.ascx" TagPrefix="AGS" TagName="usrButtons" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phPageHeader" runat="server">
    <script>
        $(document).ready(function () {

            if ($("#<%=hdnFlag.ClientID %>").val() == "2") {
                $('#SearchDiv').hide();
                $('#InfoDiv').show();
            }
            else {
                $('#SearchDiv').show();
                $('#InfoDiv').hide();
            }

            //for  temporary block Card and those user having Save rights , Show ReqType dropdown and save button
            <%--if ((($("#<%=txtCardStatusID.ClientID%>").val() == "0") || ($("#<%=txtCardStatusID.ClientID%>").val() == "1")) && ($("#<%=hdnAccessCaption.ClientID%>").val() == "S")) {
                $('#RequestChngDiv').show();
            }
            else {
                $('#RequestChngDiv').hide();
            }--%>

            $('#datatable-buttons').html($("#<%=hdnCardDetails.ClientID %>").val());

            $('#<%= hdnCardDetails.ClientID%>').val('');

            if ($("#datatable-buttons tbody tr").length > 0) {

                //hide view button for blocked card
                $("#datatable-buttons tbody tr td input[CardStatus='Lost']").remove();
                $("#datatable-buttons tbody tr td input[CardStatus='stolen']").remove();

                //For Data Table
                var handleDataTableButtons = function () {
                    if ($("#datatable-buttons").length) {
                        $("#datatable-buttons").DataTable({
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
                $('#DivCardResult').show();
                $('#SearchDiv').show();
            }
            else {
                $('#DivCardResult').hide();
            }

            $("#phPageBody_userBtns_btnSave_U").click(function () {
                var errrorTextFD = 'Please provide ';

                var errorFieldsFD = '';

               <%-- var msg = $('#<%=ddlRequestType.ClientID%>').val();--%>

               <%-- if ($('#<%=txtMobileno.ClientID%>').val().trim() == '') {

                    errorFieldsFD = errorFieldsFD + (errorFieldsFD != '' ? '<b>,' : '<b>') + ' Remark</b> ';
                    $('#<%=txtMobile.ClientID%>').focus();
                }--%>

                //if (msg == '0') {
                //    errorFieldsFD = errorFieldsFD + (errorFieldsFD != '' ? '<b>,' : '<b>') + ' Request Type</b> ';
                //}
                //else if (msg == "2") {

                //    msg = "Are you sure you want to temporary block this card ?";
                //}
                //else if (msg == "3" || msg == "4") {

                //    msg = "Are you sure you want to block this card ?";
                //}
                //else {
                //    msg = null;
                //}


                if (errorFieldsFD != '') {
                    $('#SpnErrorMsg').html(errrorTextFD + errorFieldsFD);
                    $('#errormsgDiv').show();

                    errorFieldsFD = '';
                    errrorTextSectionFD = '';
                    errrorTextFD = '';
                    return false;
                }
                else {
                    if (msg != null)
                        if (!confirm(msg))
                            return false;
                    $('.shader').fadeIn();

                }
            });

        });

        //Response Message
        function FunShowMsg() {

            $('#memberModal').modal('show');
            if ($('#phPageBody_hdnResultStatus').val() == 1) {
                fnreset(null, true);
                $("#<%=hdnFlag.ClientID %>").val() == "";

            }
        }

        //Validation on Search Button
        function FunValidation() {

            //var AppNo = $('#phPageBody_ddlApplicationNo').val()
            //var CustomerID = $('#phPageBody_txtSearchCustomerID').val()
            var CardNo = $('#phPageBody_txtSearchCardNo').val()
            
            if (CardNo == "") {
                $('#SpnErrorMsg').html('Please provide Card No');
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
            $("#<%=txtMobileno.ClientID%>").val('');
            $("#<%=txtAddress.ClientID%>").val('');
        }

        function funViewClick(obj) {

            var card = $(obj).attr('cardno');


            $("#<%= hdnCardNo.ClientID%>").val(card);

            $('.shader').fadeIn();
            document.getElementById("phPageBody_hdnViewBtn").click();
        }
        
        function funCancelClick(obj) {

            var card = $(obj).attr('cardno');


            $("#<%= hdnCardNo.ClientID%>").val(card);

            $('.shader').fadeIn();
            document.getElementById("phPageBody_hdnViewBtn").click();
        }
        function funcancel() {
            $('#<%=hdnFlag.ClientID%>').val('');
            $('#<%=LblResult.ClientID%>').text('');
            $('#<%=btnSearchCustomer.ClientID%>').click();
        }
    </script>


</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="phPageBody" runat="server">
    <asp:HiddenField ID="hdnPinResetCount" runat="server" />
    <asp:HiddenField ID="hdnCustomerID" runat="server" />
    <asp:HiddenField ID="hdnCardNo" runat="server" />
    <asp:HiddenField ID="hdnResultStatus" runat="server" />
    <asp:HiddenField runat="server" ID="hdnFlag" />
    <asp:HiddenField ID="hdnCardDetails" runat="server" />
    <asp:HiddenField ID="hdnAccessCaption" runat="server" />
    <asp:HiddenField ID="hdnRPANID" runat="server" />
    
    <asp:Button runat="server" ID="hdnViewBtn" Text="Search" OnClick="btnView_Click" Style="display: none;" />

    <asp:Panel ID="pnlCustomerDtl" runat="server">
        <div style="font-size: 14px;">
            <div id="SearchCustomer">
                <div class=" box-primary ">
                    <div class="box-header with-border">
                        <i class="fa fa-credit-card"></i>
                        <!--start sheetal card details change to card operation -->
                        <h4 class="box-title">Customer Data Update :</h4>
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
                                    <div class="col-md-4" style="display: none">
                                        <div class="form-group">
                                            <label for="inputName" class="col-xs-4 control-label">Application No:</label>
                                            <div class="col-sm-7">
                                                <asp:DropDownList runat="server" ID="ddlApplicationNo" CssClass="form-control"></asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <%--<div class="col-md-6">
                                        <div class="form-group">
                                            <label for="inputName" class="col-xs-4 control-label">CustomerID:</label>
                                            <div class="col-sm-7">
                                                <input type="text" class="form-control" maxlength="20" runat="server" name="CustomerID" id="txtSearchCustomerID"  />
                                            </div>
                                        </div>
                                    </div>--%>
                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label for="inputName" class="col-xs-4 control-label"><%--<span style="color: red;">*</span>--%>CIF ID:</label>
                                                <div class="col-sm-7">
                                                    <input type="text" class="form-control" maxlength="20" runat="server" name="CIFID" id="txtCifid" onkeypress="return FunChkIsNumber(event)" />
                                                </div>
                                            </div>
                                        </div>
                                       <%-- <div class="col-md-6">
                                            <div class="form-group">
                                                <label for="inputName" class="col-xs-4 control-label">CIF ID:</label>
                                                <div class="col-sm-7">
                                                    <input type="text" class="form-control" maxlength="25" runat="server" name="CIFID" id="txtSearchCIF" />
                                                </div>
                                            </div>
                                        </div>--%>
                                    </div>
                                    
                                    <div class="row">
                                        <div class="col-md-12 text-center">
                                            <div class="form-group">
                                                <div class="col-md-12">
                                                    <asp:Button runat="server" CssClass="btn btn-primary" ID="btnSearchCustomer" Text="Search" OnClick="btnSearch_Click" OnClientClick="return FunValidation();" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                            </div>
                        </div>

                    </div>
                    <div class="row" id="DivResultMsg">
                        <div class="col-md-6">
                            <h4>
                                <label maxlength="20" runat="server" name="Name" id="LblResult" readonly="readonly" />
                            </h4>
                        </div>
                    </div>

                    <div class="box-body" id="DivCardResult">
                        <div class="row">
                            <div class="col-md-12">
                                <div class="box-primary">
                                    <div class="box-body no-padding">
                                        <div class="row" style="padding: 0px 30px; padding-top: 15px;">
                                            <div class="col-md-12">
                                                <div class="x_panel">
                                                    <div class="">
                                                        <div class="clearfix"></div>
                                                    </div>
                                                    <div class="x_content">
                                                        <table id="datatable-buttons" class="table table-striped table-bordered" style="width: 100%">
                                                        </table>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
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
                                            <label for="inputName" class="col-xs-4 control-label">Customer ID:</label>
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
                                            <label for="inputName" class="col-xs-4 control-label">Mobile No:</label>
                                            <div class="col-sm-8">
                                                <input type="text" class="form-control" runat="server" maxlength="12" name="Name" id="txtMobile" readonly="readonly" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="form-group">
                                            <label for="inputName" class="col-xs-4 control-label">Email Id:</label>
                                            <div class="col-sm-8">
                                                <input type="text" class="form-control" runat="server" name="Name" id="txtEmail" readonly="readonly" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-4">
                                        <div class="form-group">
                                            <label for="NameOnCard" class="col-xs-4 control-label">Name On Card:</label>
                                            <div class="col-sm-8">
                                                <asp:TextBox runat="server" ID="txtNameOnCard" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                
                                 <div class="col-md-4">
                                        <div class="form-group">
                                            <label for="inputName" class="col-xs-4 control-label">Mother's maiden Name:</label>
                                            <div class="col-sm-8">
                                                <input type="text" class="form-control" runat="server" maxlength="12" name="Name" id="txtMotherMaidenName" readonly="readonly" />
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
                                    <div class="col-md-4">
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
                                   <!-- <div class="col-md-4" style="display: none">
                                        <div class="form-group">
                                            <label for="inputName" class="col-xs-4 control-label">Card Status ID:</label>
                                            <div class="col-sm-8">
                                                <input type="text" class="form-control" runat="server" maxlength="12" name="Name" id="txtCardStatusID" readonly="readonly" />
                                            </div>
                                        </div>
                                    </div> -->
                                    

                                </div>
                            </div>
                        </div>


                        <div id="RequestChngDiv">
                            <div class="row">

                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label for="inputName" class="col-xs-4 control-label"><span style="color: red;"></span>Mother's Maiden Name:</label>
                                        <div class="col-sm-8">
                                            <%--<asp:DropDownList runat="server" ID="ddlRequestType" CssClass="form-control"></asp:DropDownList>--%>
                                            <asp:TextBox runat="server" ID="txtMothername" CssClass="form-control" onkeypress="return onlyAlphabets(event,this);"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <label for="inputName" class="col-xs-4 control-label"><span style="color: red;"></span>Mobile No:</label>
                                    <div class="col-sm-8">
                                        <%--<input type="text" class="form-control" runat="server" maxlength="20" TextMode="MultiLine" name="Name" id="txtUpdateRemark" onkeypress="return onlyAlphabets(event,this);" />--%>
                                        <asp:TextBox runat="server" ID="txtMobileno" CssClass="form-control"></asp:TextBox>

                                    </div>
                                </div>
                                <div class="col-md-4">
                                </div>
                            </div>
                            <div>
                                <br />
                            </div>
                            <div class="row" id="SaveDiv">
                                <div class="col-md-4"></div>
                                <div class="col-md-4">
                                    <div class="col-md-4"></div>
                                    <div class="col-md-7">
                                        <%--<asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn-div btn-style btn btn-primary" OnClick="btnSave_Click" />--%>
                                        <AGS:usrButtons runat="server" ID="userBtns" />
                                    </div>
                                </div>
                                <div class="col-md-4"></div>
                            </div>

                            <div class="row" id="AuthDiv" style="display: none">
                                <div class="col-md-4"></div>
                                <div class="col-md-4">
                                    <div class="col-md-6">
                                        <asp:Button ID="Button1" runat="server" Text="Accept" CssClass="btn-div btn-style btn btn-primary" />
                                    </div>
                                    <div class="col-md-6">
                                        <asp:Button ID="Button2" runat="server" Text="Reject" CssClass="btn-div btn-style btn btn-primary" />
                                    </div>

                                </div>
                                <div class="col-md-4"></div>
                            </div>
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
        <div id="memberModal" class="modal fade bs-example-modal-lg" tabindex="-1" role="dialog" data-backdrop="static" aria-labelledby="myLargeModalLabel">
            <div class="modal-dialog modal-md">
                <div class="modal-content" style="border-radius: 6px">
                    <div class="modal-header box-primary" style="border-top-right-radius: 6px; border-top-left-radius: 6px">
                        <button aria-label="Close" data-dismiss="modal" class="close" type="button" onclick="CancelModal()"><span aria-hidden="true">×</span></button>
                        <h4 id="myLargeModalLabel" class="modal-title" style="font-weight: bold">Card Operation</h4>
                    </div>
                    <div class="modal-body" id="msgBody">
                        <label for="username" class="control-label" id="LblMsg" runat="server" style="font-weight: normal"></label>
                        <button aria-label="Close" data-dismiss="modal" id="BtnModalClose" class="btn btn-primary pull-right" onclick="CancelModal()" type="button"><span aria-hidden="true">OK</span></button>
                    </div>
                </div>
            </div>
            <!-- /.modal-content -->
        </div>
        </label>
    </asp:Panel>
</asp:Content>
