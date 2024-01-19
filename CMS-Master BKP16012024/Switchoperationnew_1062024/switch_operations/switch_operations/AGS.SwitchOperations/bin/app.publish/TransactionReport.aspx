<%@ Page Title="" Language="C#" MasterPageFile="~/SwitchOperationSite.Master" AutoEventWireup="true" CodeBehind="TransactionReport.aspx.cs" Inherits="AGS.SwitchOperations.TransactionReport" %>

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
    
    <script src="Scripts/moment.min.js"></script>
    <script src="Scripts/daterangepicker.min.js"></script>
    <link href="Styles/daterangepicker.css" rel="stylesheet" />
    <%--<script src="Scripts/bootstrap-datetimepicker.min.js"></script>
    <link href="Styles/bootstrap-datetimepicker.min.css" rel="stylesheet" />--%>
    <script>
        <%-- Datatable fun----%>

        $(document).ready(function () {
            //var currentTime = new Date();
            //currentTime.setDate(currentTime.getDate() -parseInt($('[id$="HdnDateRange"]').val()));
            // alert(currentTime);
            //var startDateFrom = $.datepicker.parseDate("mm/dd-mm-dd", minValue);
            //var startDateFrom = new Date((currentTime.getMonth()) + "/" + currentTime.getDate() + "/" + currentTime.getFullYear());
            //alert((currentTime.getMonth() + 1) + "/" + currentTime.getDate() + "/" + currentTime.getFullYear());
            //$('.datepicker').datepicker({ format: "dd/mm/yyyy", autoclose: true, endDate: new Date(), maxDate: new Date(), startDate: startDateFrom });

            //$('.datepicker').datetimepicker();

            $(function () {
                $('#<%=txtdates.ClientID%>').daterangepicker({
                    "autoApply": true,
                    "maxSpan": {
                        "days": 30
                    },
                    ranges: {
                        'Today': [moment(), moment()],
                        'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
                        'Last 7 Days': [moment().subtract(6, 'days'), moment()],
                        'Last 30 Days': [moment().subtract(29, 'days'), moment()],
                        'This Month': [moment().startOf('month'), moment().endOf('month')],
                        'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
                    },
                    "locale": {
                        "format": "DD/MM/YYYY"
                    },
                    "alwaysShowCalendars": true,
                    "opens": "center"

                }, function (start, end, label) {
                    console.log("A new date selection was made: " + start.format('YYYY-MM-DD') + ' to ' + end.format('YYYY-MM-DD'));
                });
            });

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
           // alert($("[id$='ISEPS']").val());
            //added by uddesh ATPCM-656 start
            if ($("[id$='ISEPS']").val() == "1") {
                $('#div_RRN').show(); 
            } else {
                $('#div_RRN').hide(); 
            }
          //added by uddesh ATPCM-656 end


        });


        //Validation on Search Button
        function FunValidation() {
            var FromDate = $('#<%=txtdates.ClientID%>').val().split('-')[0].trim();
            var ToDate = $('#<%=txtdates.ClientID%>').val().split('-')[1].trim();
            //var ToDate = $('#phPageBody_txtSearchToDate').val()
            //var CardNo = $('#phPageBody_txtSearchCardNo').val()
            <%--var NameOnCard = $('#<%= txtSearchNameOnCard.ClientID%>').val()--%>
            var CustomerID = $('#phPageBody_txtSearchCustomerID').val()
            var RRN = $('#phPageBody_txtRRN').val()


            var errrorTextDD = 'Please provide ';
            // var errrorTextSectionDD = ' in Documentation.';
            var errorFieldsDD = '';

            //added by uddesh ATPCM-656 start
            //if (CustomerID == "") {
            //    errorFieldsDD = errorFieldsDD + (errorFieldsDD != '' ? '<b>,' : '<b>') + ' CustomerID</b> ';
            //}
            //if (CardNo == "" && RRN =="") {
            //    errorFieldsDD = errorFieldsDD + (errorFieldsDD != '' ? '<b>,' : '<b>') + ' CardNo</b> ';
            //}
            //added by uddesh ATPCM-656 end
            //if (CardNo == "" && NameOnCard == "") {
            //    errorFieldsDD = errorFieldsDD + (errorFieldsDD != '' ? '<b>,' : '<b>') + ' CardNo</b> ';
            //}

            if ((((FromDate == "") && (ToDate != "")) || ((ToDate == "") && (FromDate != "")) || ((ToDate == "") && (FromDate == "")))) {
                errorFieldsDD = errorFieldsDD + (errorFieldsDD != '' ? '<b>,' : '<b>') + ' From Date and To Date</b> ';
            }

            if ((Date.parse(FromDate) - Date.parse(ToDate)) > 0) {
                errorFieldsDD = errorFieldsDD + (errorFieldsDD != '' ? '<b>,' : '<b>') + ' Valid From Date and To Date</b> ';
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

    <%--<script type="text/javascript">
        $(function () {
            $('#datetimepicker1').datetimepicker();
        });
    </script>
    <script type="text/javascript">
        $(function () {
            $('#datetimepicker1').datetimepicker({
                //language: 'pt-BR'
            });
        });
    </script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phPageBody" runat="server">
    <asp:HiddenField ID="HdnDateRange" runat="server" Value="1" />
    <asp:HiddenField ID="hdnTransactionDetails" runat="server" />
     <asp:HiddenField ID="ISEPS" runat="server" Value="" /> <%--added by uddesh ATPCM-656 start--%>
    <div class="box-primary">
        <div class="box-header with-border">
            <i class="fa fa-list"></i>
            <!-- start sheetal card details chnage to card details view-->
            <h2 class="box-title">Transaction Report </h2>
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

                                <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>Date:</label>
                                <div class="col-sm-8">
                                    <%--<input type="text" class="form-control datepicker"
                                        maxlength="20" runat="server" name="FromDate" id="txtSearchFromDate"
                                        onkeypress="return FunChkIsNumber(event)" />--%>
                                    <input type="text" name="daterange" runat="server" id="txtdates" class="form-control"/>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6 hidden">
                            <div class="form-group">
                                <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>To Date:</label>
                                <div class="col-sm-8">
                                    <input type="text" class="form-control datepicker" maxlength="20" runat="server" name="ToDate" id="txtSearchToDate" onkeypress="return FunChkIsNumber(event)" />
                                </div>
                            </div>
                        </div>
                      <%--  <div class="col-md-6">
                            <div class="form-group">
                                <label for="inputName" class="col-xs-4 control-label">Card No:</label>
                                <div class="col-sm-8">
                                    <input type="text" class="form-control" maxlength="20" runat="server" name="CardNo" id="txtSearchCardNo" onkeypress="return FunChkIsNumber(event)" />
                                </div>
                            </div>
                        </div>--%>

                    </div>
                    <asp:Panel ID="pnlnameoncard" runat="server" class="row">

                            <div class="col-md-6 hidden" id="div_RRN">
                            <div class="form-group">
                                <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>RRN:</label>
                                <div class="col-sm-8">
                                    <input type="text" class="form-control" maxlength="20" runat="server" name="RRN" id="txtRRN" onkeypress="return FunChkIsNumber(event)" />
                                </div>
                            </div>
                        </div>

                        <div class="col-md-6  hidden">
                            <div class="form-group">
                                <label for="inputName" class="col-xs-4 control-label">CustomerID:</label>
                                <div class="col-sm-8">
                                    <input type="text" class="form-control" maxlength="20" runat="server" name="CustomerID" id="txtSearchCustomerID" onkeypress="return FunChkIsNumber(event)" />
                                </div>
                            </div>
                            </div>
                        <div class="col-md-6 ">
                            <div class="form-group">
                                <%--<div class='input-group date' id='datetimepicker1'>
                                    <input type='text' class="form-control" />
                                    <span class="input-group-addon">
                                        <span class="glyphicon glyphicon-calendar"></span>
                                    </span>
                                </div>--%>
                                  <label for="NameOnCard" class="col-xs-4 control-label">Name On Card:</label>
                                <div class="col-sm-8">
                                    <input type="text" class="form-control" maxlength="20" runat="server" name="NameOnCard" id="txtSearchNameOnCard" onkeypress="return FunChkAlphaNumeric(event)" />
                                </div>
                             
                            </div>
                            </div>
                        </asp:Panel>
                     
                    <div class="row" style="display: none">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="inputName" class="col-xs-4 control-label">Product Type:</label>
                                <div class="col-sm-8">
                                    <%--<input type="text" class="form-control" maxlength="25" runat="server" name="Name" id="txtSearchProduct" onkeypress="return onlyAlphabets(event,this);" />--%>

                                    <asp:DropDownList runat="server" ID="ddlProductType" CssClass="form-control"></asp:DropDownList>

                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="inputName" class="col-xs-4 control-label">Status:</label>
                                <div class="col-sm-8">
                                    <asp:DropDownList runat="server" ID="ddlStatus" CssClass="form-control"></asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12 text-center">
                            <div class="form-group">
                                        <asp:Button runat="server" CssClass="btn btn-primary" ID="btnSearch" Text="Search" OnClick="btnSearch_Click" OnClientClick="return FunValidation();" />
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
