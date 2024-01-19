<%@ Page Title="" Language="C#" MasterPageFile="~/SwitchOperationSite.Master" AutoEventWireup="true" CodeBehind="BINMaster.aspx.cs" Inherits="AGS.SwitchOperations.BINMaster" %>

<asp:Content ID="Content2" ContentPlaceHolderID="phPageHeader" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phPageBody" runat="server">
    <!------------HIDDEN FIELDS-------------------------------->
    <asp:HiddenField ID="hdnTransactionDetails" runat="server" />
    <asp:HiddenField ID="hdnResultStatus" runat="server" />
    <input type="hidden" name="ID" id="hdnID" runat="server" />
    <asp:HiddenField ID="hdnFormStatusID" runat="server" />
    <asp:Button runat="server" ID="hdnAcceptBtn" OnClick="btnAccept_Click" Style="display: none;" />


    <div class="box-primary">
        <div class="box-header with-border">
            <i class="fa fa-list"></i>
            <h2 class="box-title">Bin Master </h2>
        </div>

        <%--//divresult--%>
        <div class="box-body" id="DivResult">
            <%--<div class="row">
                <div class="col-md-6"></div>
                <div class="col-md-6">
                    <input type="button" class="btn btn-primary pull-right" id="BtnAddNew" value="Add New" onclick="funAddNew()" />
                </div>
            </div>--%>
            <div class="row">
                <div class="col-md-12">
                    <div class="box-primary">
                        <!-- /.box-header -->
                        <div class="box-body no-padding">
                            <div class="row" style="padding: 0px 30px; padding-top: 15px;">
                                <div class="col-md-12">
                                    <div class="x_panel">
                                        <div class="">
                                            <div class="clearfix"></div>
                                        </div>
                                        <div class="x_content">
                                            <table id="datatable-buttons" class="table table-striped table-bordered" style="width:100%">
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
    </div>

    <!-- Modal HTML -->
    <%-- Response Msg --%>
    <div id="memberModal" class="modal fade bs-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel">
        <div class="modal-dialog modal-md">
            <div class="modal-content" style="border-radius: 6px">
                <div class="modal-header box-primary" style="border-top-right-radius: 6px; border-top-left-radius: 6px">
                    <button aria-label="Close" data-dismiss="modal" class="close" type="button"><span aria-hidden="true">×</span></button>
                    <h4 id="myLargeModalLabel" class="modal-title" style="font-weight: bold">Bin Master</h4>
                </div>
                <div class="modal-body" id="msgBody">
                    <label for="username" class="control-label" id="LblMsg" runat="server" style="font-weight: normal"></label>
                </div>
                <div class="modal-footer">
                    <button aria-label="Close" data-dismiss="modal" id="BtnModalClose" class="btn btn-primary" type="button"><span aria-hidden="true">OK</span></button>
                </div>
            </div>
        </div>
        <!-- /.modal-content -->
    </div>

    <%-- Edit Modal --%>
    <div id="AddEditModal" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content -->
            <div class="modal-content" style="border-radius: 4px;">

                <div class="modal-header">
                    <button aria-label="Close" data-dismiss="modal" class="close" type="button" onclick="funCancelModal()"><span aria-hidden="true">×</span></button>
                    <h4 id="myLargeEdit" class="modal-title" style="font-weight: bold">BIN</h4>
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
                        <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>InstitutionID:</label>
                        <div class="col-sm-7">
                            <asp:DropDownList runat="server" ID="ddlInstitutionID" CssClass="form-control"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="row">
                        <br />
                    </div>
                    <%--<div class="row">
                        <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>Card Type:</label>
                        <div class="col-sm-7">
                            <asp:DropDownList runat="server" ID="ddlCardType" CssClass="form-control"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="row">
                        <br />
                    </div>--%>

                    <div class="row">
                        <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>BIN:</label>
                        <div class="col-sm-7">
                            <input type="text" class="form-control" id="txtBin" runat="server" maxlength="12" onkeypress="return FunChkIsNumber(event)" />
                        </div>
                    </div>
                    <div class="row">
                        <br />
                    </div>
                    <div class="row">
                        <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>BIN Description:</label>
                        <div class="col-sm-7">
                            <input type="text" class="form-control" id="txtBinDesc" runat="server" maxlength="20" onkeypress="return onlyAlphabets(event,this);" />
                        </div>
                    </div>
                    <div class="row">
                        <br />
                    </div>
                    <div class="row">
                        <div class="col-sm-6">
                            <button aria-label="Close" data-dismiss="modal" class="btn btn-primary pull-right" onclick="funCancelModal()" style="margin-right: 10px;" type="button"><span aria-hidden="true">CANCEL</span></button>
                        </div>
                        <div class="col-sm-5">
                            <asp:Button runat="server" CssClass="btn btn-primary" Text="SAVE" Style="width: 63px" ID="SaveBtn" OnClick="btnSave_Click" />
                        </div>
                    </div>
                </div>
            </div>

        </div>
    </div>

    <!-- /.modal-content for Reject   -->
    <div id="RejectConfirmationModal" class="modal fade in" data-keyboard="false" data-backdrop="static" role="dialog" >
        <div class="modal-dialog">
            <!-- Modal content -->
            <div class="modal-content" style="border-radius: 4px;">

                <div class="modal-header">
                    
                    <h4 id="myLargeRejectModal" class="modal-title" style="font-weight: bold">Confirmation</h4>
                </div>
                <div class="modal-body">
                    <div class="row">

                        <label for="inputName" class="col-xs-5 control-label"><span style="color: red;">*</span>Do you want to reject BIN ?:</label>
                        <div class="col-sm-6">
                            <input type='radio' name='IsConfirm' value='1' />Yes
                        &nbsp;&nbsp;<input type='radio' name='IsConfirm' value='2' aria-label="Close" data-dismiss="modal" />No
                        </div>
                    </div>
                    <div class="row">
                        <br />
                    </div>
                    <div class="row" id="remarkDiv" style="display: none">
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
                            <asp:Button runat="server" CssClass="btn btn-primary" Text="Confirm" ID="Reject_Btn" OnClick="btnReject_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>



    <%---************************** SCRIPT *************************----%>
    <script>

        $(document).ready(function () {
            $('#datatable-buttons').html($("#<%=hdnTransactionDetails.ClientID %>").val());
            $("#<%=hdnTransactionDetails.ClientID %>").val('');
            //$("#datatable-buttons tr :nth-child(6)").hide()  
            //$("#datatable-buttons tr :nth-child(7)").hide()

        });
    </script>
    <%-- Datatable fun----%>
    <script>
        $(document).ready(function () {
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
        });
    </script>
    <%----************************ Click Function ************************--%>

    <%-- ******************* Validation *********************** --%>
    <script>
        $("#<%=SaveBtn.ClientID %>").click(function () {
            var errrorTextPD = 'Please provide ';
            var errorFieldsPD = '';

            if ($("#phPageBody_txtBinDesc").val() == "") {
                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>, BIN Description</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>BIN Description</b> ';
                }
                $('#<%=txtBinDesc.ClientID%>').focus();
            }

            if ($("#phPageBody_txtBin").val() == "") {
                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>, BIN</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>BIN</b> ';
                }
                $('#<%=txtBin.ClientID%>').focus();
            }

            if ($("#phPageBody_ddlInstitutionID").val() == "0") {
                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>, InstitutionID</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>InstitutionID</b> ';
                }
                $('#<%=ddlInstitutionID.ClientID%>').focus();
            }



            if (errorFieldsPD != '') {
                $('#SpnErrorMsg').html(errrorTextPD + errorFieldsPD);
                $('#errormsgDiv').show();
                return false;
            }
            else {
                errorFieldsPD = '';
                errrorTextSectionPD = '';
                errrorTextPD = '';
                $('#errormsgDiv').hide();
            }
        });
    </script>

    <%--******************************** //Message Box *********************--%>
    <script>
        //response msg
        function FunShowMsg() {
            //  setTimeout($('#myModal').modal('show'),2000);
            $('#memberModal').modal('show');

            //on success
            if ($('#phPageBody_hdnResultStatus').val() == 1) {
                $('input[type="text"]').val('');

            }
                //on update fail
            else if ($('#phPageBody_hdnResultStatus').val() == 2) {
                $('#AddEditModal').css("display", "");
                $('#DivResult').css("display", "none");
            }
        }

        //Add New Click
        function funAddNew() {
            $('#errormsgDiv').hide();
            $('#phPageBody_txtBin').val('');

            $('#phPageBody_txtBinDesc').val('');

            $('#phPageBody_hdnID').val('');
            $("#phPageBody_ddlInstitutionID").val('0')

            $('#AddEditModal').modal('show')
        }

        //Edit click
        function funEditClick(obj) {
            var tds = $(obj).parent().parent().find('td');
            var ID = $(tds[0]).text();
            var Bin = $(tds[1]).text();
            var BinDesc = $(tds[2]).text();
            var INSTID = $(tds[6]).text();

            $('#phPageBody_txtBin').val(Bin);

            $('#phPageBody_txtBinDesc').val(BinDesc);

            $('#phPageBody_hdnID').val(ID);

            $("#phPageBody_ddlInstitutionID")[0].selectedIndex = INSTID;
            $('#errormsgDiv').hide();
            $('#AddEditModal').modal('show')
        }

        //Cancel edit modal
        function funCancelModal() {

            $('[id$="txtRejectReson"]').attr('required', false)
            $('#errormsgDiv').hide();
            $('#AddEditModal').modal('hide')
        }


        //-------------------------
        //Accept Bin link Click
        function FunAcceptBin(obj) {
            var tds = $(obj).parent().parent().find('td');
            var ID = $(tds[0]).text();
            var Bin = $(tds[1]).text();
            var BinDesc = $(tds[2]).text();
            //var FormStatus = $(tds[4]).text();

            $('#phPageBody_hdnFormStatusID').val('1');
            $('#phPageBody_hdnID').val(ID);
            document.getElementById("phPageBody_hdnAcceptBtn").click();
        }

        //Reject Bin link Click
        function FunRejectBin(obj) {
            var tds = $(obj).parent().parent().find('td');
            var ID = $(tds[0]).text();
            $('#phPageBody_hdnID').val(ID);
            $('#phPageBody_txtRejectReson').val('');
            $('#remarkDiv').css("display", "none");
            $('#phPageBody_Reject_Btn').css("display", "none")
            $('input[name=IsConfirm]').attr('checked', false);
            $('#RejectConfirmationModal').modal('show');
        }

        //Confirm rejection
        $("[name$='IsConfirm']").click(function () {
            if ($("input:radio[name='IsConfirm']:checked").val() == "1") {

                $('#remarkDiv').css("display", "")
                $('#phPageBody_Reject_Btn').css("display", "")
                $('[id$="txtRejectReson"]').attr('required', true)
            }
        });
    </script>


</asp:Content>
