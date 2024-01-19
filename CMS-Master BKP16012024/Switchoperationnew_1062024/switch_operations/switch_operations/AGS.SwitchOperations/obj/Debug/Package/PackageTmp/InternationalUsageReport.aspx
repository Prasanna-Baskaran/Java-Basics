<%@ Page Title="" Language="C#" MasterPageFile="~/SwitchOperationSite.Master" AutoEventWireup="true" CodeBehind="InternationalUsageReport.aspx.cs" Inherits="AGS.SwitchOperations.InternationalUsageReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phPageHeader" runat="server">
     <link href="Styles/buttons.bootstrap.min.css" rel="stylesheet" />
    <script src="Scripts/bootstrap-DatePicker.js"></script>
    <link href="Styles/DatePickerBootStrap.css" rel="stylesheet" />
    <script src="Scripts/dataTables.buttons.min.js"></script>
    <script src="Scripts/buttons.bootstrap.min.js"></script>
    <script src="Scripts/buttons.html5.min.js"></script>
    <script src="Scripts/buttons.print.min.js"></script>
    <script src="Scripts/jszip.min.js"></script>
    <script src="Scripts/pdfmake.min.js"></script>
    <script src="Scripts/vfs_fonts.js"></script>
    <script>
        <%-- Datatable fun----%>

        $(document).ready(function () {
            $('.datepicker').datepicker({ format: "dd/mm/yyyy", autoclose: true, endDate: new Date() });
            $('#datatable-buttons').html($("#<%=hdnTransactionDetails.ClientID %>").val());
            $("#<%=hdnTransactionDetails.ClientID %>").val('');
            if ($("#datatable-buttons tbody tr").length > 0) {
                //For Data Table
                var handleDataTableButtons = function () {
                    if ($("#datatable-buttons").length) {
                        $("#datatable-buttons").DataTable({
                            "info": true,
                            lengthMenu: [[10, 25, 50, -1], [10, 25, 50, "All"]],
                            keys: true,
                            deferRender: true,
                            scrollCollapse: true,
                            scrollX: true,
                            scroller: true,
                            fixedHeader: false,
                            order: [],
                            dom: "lfrBtip",
                            buttons: [
                         {
                             extend: "copy",
                             className: "btn-sm"
                         },
                         {
                             extend: "csv",
                             className: "btn-sm"
                         },
                         //{
                         //    extend: "excel",
                         //    className: "btn-sm"
                         //},
                         //{
                         //    extend: "pdfHtml5",
                         //    className: "btn-sm"
                         //},
                         {
                             extend: "print",
                             className: "btn-sm"
                         },
                            ],

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
                $('#DivResult').show();
            }
            else {
                $('#DivResult').hide();
            }
        });


        //Validation on Search Button
        function FunValidation() {
            var FromDate = $('#phPageBody_txtSearchFromDate').val()
            var ToDate = $('#phPageBody_txtSearchToDate').val()
            //var CardNo = $('#phPageBody_txtSearchCardNo').val()
            //var CustomerID = $('#phPageBody_txtSearchCustomerID').val()

            var errrorTextDD = 'Please provide ';
            // var errrorTextSectionDD = ' in Documentation.';
            var errorFieldsDD = '';

            //if (CustomerID == "") {
            //    errorFieldsDD = errorFieldsDD + (errorFieldsDD != '' ? '<b>,' : '<b>') + ' CustomerID</b> ';
            //}
            //if (CardNo == "") {
            //    errorFieldsDD = errorFieldsDD + (errorFieldsDD != '' ? '<b>,' : '<b>') + ' CardNo</b> ';
            //}

            if ((((FromDate == "") && (ToDate != "")) || ((ToDate == "") && (FromDate != "")) || ((ToDate == "") && (FromDate == "")) || (ToDate < FromDate))) {
                errorFieldsDD = errorFieldsDD + (errorFieldsDD != '' ? '<b>,' : '<b>') + ' valid From Date and To Date</b> ';
            }

            if (errorFieldsDD != '') {
                $('#SpnErrorMsg').html(errrorTextDD + errorFieldsDD);
                $('#errormsgDiv').show();
                errorFieldsDD = '';
                errrorTextDD = '';
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phPageBody" runat="server">
    <asp:HiddenField ID="hdnTransactionDetails" runat="server" />
    <div class="box-primary">
        <div class="box-header with-border">
            <i class="fa fa-list"></i>
            <h2 class="box-title">International Usage Report </h2>
        </div>
        <!--Display validation msg ------------------------------------------------------------------------->
        <div class="pad margin no-print" id="errormsgDiv" style="display: none">
            <div class="callout callout-info" style="margin-bottom: 0!important;">
                <h4><i class="fa fa-info"></i>Information :</h4>
                <span id="SpnErrorMsg" class="text-center"></span>
            </div>
        </div>
        <div class="box-body" id="SearchDiv">
            <div class="form-horizontal">
                <div class="row">
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">

                                <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>From Date:</label>
                                <div class="col-sm-8">
                                    <input type="text" class="form-control datepicker" maxlength="20" runat="server" name="FromDate" id="txtSearchFromDate" onkeypress="return FunChkIsNumber(event)" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>To Date:</label>
                                <div class="col-sm-8">
                                    <input type="text" class="form-control datepicker" maxlength="20" runat="server" name="ToDate" id="txtSearchToDate" onkeypress="return FunChkIsNumber(event)" />
                                </div>
                            </div>
                        </div>
                    </div>

                    
                    <div class="row">
                        <div class="col-md-6"></div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <div class="col-sm-4"></div>

                                <div class="col-sm-4"></div>
                                <div class="col-sm-4">
                                    <div class="col-md-7">
                                        <asp:Button runat="server" CssClass="btn btn-primary pull-right" ID="btnSearch" Text="Search" OnClick="btnSearch_Click" OnClientClick="return FunValidation();" />
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
                    <label runat="server" name="Name" id="LblResult" readonly="readonly" />
                </h4>
            </div>
        </div>

        <%--//divresult--%>
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

    </label>

</asp:Content>
