<%@ Page Title="" Language="C#" MasterPageFile="~/SwitchOperationSite.Master" AutoEventWireup="true" CodeBehind="PinReissuance.aspx.cs" Inherits="AGS.SwitchOperations.PinReissuance" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phPageHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phPageBody" runat="server">
    <asp:HiddenField ID="hdnAccessCaption" runat="server" />
    <asp:HiddenField ID="HdnPrepaidFileFlag" runat="server" />
    <asp:HiddenField ID="hdnTransactionDetails" runat="server" />
<asp:Panel ID="pnladmincard" runat="server" DefaultButton="btnGenerate">
        <div style="font-size: 14px;">
            <div id="admincard">
                <div class="box-primary">
                    <div class="box-header with-border">
                        <i class="fa fa-credit-card"></i>

                        <h4 class="box-title">Pin Reissuance:</h4>
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

                        <div class="box-body" id="cardgenerateDiv">
                            <div class="form-horizontal">
                                <div class="row">
                             <div class="col-md-4">
                                        <div class="form-group">
                                            <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>Card No:</label>
                                            <div class="col-sm-8">
                                                <input type="text" class="form-control" maxlength="16" runat="server" name="CardNo" id="txtCardNo" onkeypress="return FunChkIsNumber(event) ;" />

                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-md-4">
                                        <div class="form-group">


                                            <div class="col-sm-8">
                                                <div class="col-md-10">
                                                    <asp:Button runat="server" CssClass="btn btn-primary pull-right" ID="btnGenerate" Text="GENERATE REQUEST" OnClick="btnGenerate_Click" OnClientClick="return FunSaveValidation();" />
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
    </asp:Panel>
    <asp:Panel ID="pnlGridData" runat="server">
        <div class="x_content">

        <table id="datatable-buttons" class="table table-striped table-bordered" style="width: 100%;">
        </table>
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

    <%-- Response Msg --%>
    <div id="memberModal" class="modal fade bs-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel">
        <div class="modal-dialog modal-md">
            <!--<a data-controls-modal="your_div_id" data-backdrop="static" data-keyboard="false" 7/11 href="#">-->
            <div class="modal-content" style="border-radius: 6px">
                <div class="modal-header box-primary" style="border-top-right-radius: 6px; border-top-left-radius: 6px">
                    <button aria-label="Close" data-dismiss="modal" class="close" type="button"><span aria-hidden="true">×</span></button>
                    <h4 id="myLargeModalLabel" class="modal-title" style="font-weight: bold">Pin Reissuance</h4>
                </div>
                <div class="modal-body" id="msgBody">
                    <%--<asp:Label ID="LblMessage" runat="server"></asp:Label>--%>
                    <label runat="server" name="Name" id="LblMessage" readonly="readonly"  />
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
            $('#datatable-buttons').html($("#<%=hdnTransactionDetails.ClientID %>").val());
            $("#<%=hdnTransactionDetails.ClientID %>").val('');


            //For Data Table
            if ($("#datatable-buttons tbody tr").length > 0) {


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
        });
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
         //$('[id$="ddlBankName"]').select(function () {
         //    Hidemodel();
         //})


        function FunSaveValidation() {

            var errrorTextPD = 'Please provide :  ';
            var errorFieldsPD = '';

            if ($("#phPageBody_txtCardNo").val() == "" || $("#phPageBody_txtCardNo").val() == 0 ) {
                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>, Card No</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>Card No</b> ';
                }
                $('#<%=txtCardNo.ClientID%>').focus();
            
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
                $('.shader').fadeIn();
               
            }
         }
</script>


</asp:Content>

