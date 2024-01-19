<%@ Page Title="" Language="C#" MasterPageFile="~/SwitchOperationSite.Master" AutoEventWireup="true" CodeBehind="CheckCardOrder.aspx.cs" Inherits="AGS.SwitchOperations.CheckCardOrder" %>

<%@ Register Src="~/UserControls/UserButtons.ascx" TagPrefix="AGS" TagName="usrButtons" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phPageHeader" runat="server">
    <%--<link href="Styles/buttons.bootstrap.min.css" rel="stylesheet" />--%>
    <link href="Styles/DatePickerBootStrap.css" rel="stylesheet" />
    <%--   <link href="Styles/responsive.bootstrap.css" rel="stylesheet" />
    
    <script src="Scripts/dataTables.responsive.min.js"></script>
    <script src="Scripts/responsive.bootstrap.js"></script>--%>

    <%--  <script src="Scripts/dataTables.buttons.min.js"></script>
    <script src="Scripts/buttons.bootstrap.min.js"></script>--%>
    <script src="Scripts/bootstrap-DatePicker.js"></script>


    <%--<script src="Scripts/responsive.bootstrap.js"></script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phPageBody" runat="server">
    <asp:HiddenField ID="hdnTransactionDetails" runat="server" />
    <asp:HiddenField runat="server" ID="hdnSelectedCustomers" />
    <asp:HiddenField runat="server" ID="hdnFlag" />
    <asp:HiddenField ID="hdnAccessCaption" runat="server" />
    <div class="box-primary">
        <div class="box-header with-border">
            <i class="fa fa-list"></i>
            <!-- start sheetal card management change to check card order-->
            <h2 class="box-title">Check Card Order </h2>
        </div>
        <!-- <div >-->
        <div class="box-body">
            <div class="form-horizontal">
                <!--Display validation msg ------------------------------------------------------------------------->
                <div class="pad margin no-print" id="ValidateMsgDiv" style="display: none">
                    <div class="callout callout-info" style="margin-bottom: 0!important;">
                        <h4><i class="fa fa-info"></i>Information :</h4>
                        <span id="SpnValidMsg" class="text-center"></span>
                    </div>
                </div>
                <div class="row" id="SearchDiv" style="display: none">
                    <%--<div class="col-md-4">--%>
                    <%--<div class="form-group">
                                <label for="inputName" class="col-xs-4 control-label">Txn Date:</label>
                                <div class="col-sm-7">
                                    <input type="text" class="form-control datepicker" maxlength="15" runat="server" name="txtSearchTxnDate" id="txtSearchTxnDate" />
                                </div>
                            </div>--%>
                    <div class="col-md-4">
                        <div class="col-md-8" style="margin-right: 0px; padding-right: 0px">
                            <input type="text" id="txtDate" aria-readonly="true" runat="server" class="datepicker form-control" placeholder="Date" style="margin-right: 0px;" />

                        </div>
                    </div>

                    <div class="col-md-4">
                        <div class="form-group">
                            <div class="col-sm-4"></div>
                            <div class="col-sm-7">
                                <asp:Button runat="server" CssClass="btn btn-primary" ID="btnSearchCustomer" Text="Search" OnClick="btnSearch_Click" />
                            </div>
                        </div>
                    </div>



                </div>
            </div>
        </div>
    </div>
    <%--//divresult--%>
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

                                    <div class="pull-right" id="AuthDiv">
                                        <AGS:usrButtons runat="server" ID="userBtns" />
                                        <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="btn-div btn-style btn btn-primary" OnClick="Page_Load" Style="display: none" OnClientClick="fnreset(null, true);" />
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



    <%--*********************** //Scripts*******************--%>


    <script>
        $(document).ready(function () {
            $('.datepicker').datepicker({ format: "dd/mm/yyyy", autoclose: true,endDate: new Date() });
            $('#datatable-buttons').html($("#<%=hdnTransactionDetails.ClientID %>").val());

            <%--if ($('#<%=hdnTransactionDetails.ClientID %>').val() == '')
            {
                //$('#divResult').hide();
            }--%>

            $("#<%=hdnTransactionDetails.ClientID %>").val('');

            //If user has Accept right
            if ($.inArray("C", $('#<%=hdnAccessCaption.ClientID%>').val().split(",")) > -1) {
                $('#SelectAllDiv').show()
            }
            else { $('#SelectAllDiv').hide() }


            if ($("#datatable-buttons tbody tr").length > 0) {
                var handleDataTableButtons = function () {
                    if ($("#datatable-buttons").length) {
                        $("#datatable-buttons").DataTable({

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
                $('#phPageBody_userBtns_btnAccept_U').show();
                $('#SelectAllDiv').show();

            }
            else {
                $('#DivResult').hide();
            }



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

    </script>

    <script>
        $(document).ready(function () {
            if ($('#<%=hdnFlag.ClientID%>').val() == "1") {
                $('#<%=btnBack.ClientID%>').show();
                $('#phPageBody_userBtns_btnAccept_U').hide()
                $('#SelectAllDiv').hide();
            }
            else
                $('#<%=btnBack.ClientID%>').hide();
        })
    </script>
    <%-- Click fuctions --%>
    <script>
        $('#phPageBody_userBtns_btnAccept_U').click(function () {
            if ($('#datatable-buttons tbody input[type=checkbox]:checked').length == 0) {
                $('#SpnValidMsg').html("Please select card request to authorize");
                $('#ValidateMsgDiv').show();
                return false;
            }
            else {
                $('#SpnValidMsg').html('');
                $('#ValidateMsgDiv').hide();
                var ArrayIds = [];
                $('#datatable-buttons tbody input[type=checkbox]:checked').each(function (i) {
                    ArrayIds[i] = $(this).attr("id");
                });
                $("#<%=hdnSelectedCustomers.ClientID%>").val(ArrayIds.join(","))
                $('.shader').fadeIn();
            }
        })

        $('#<%=btnSearchCustomer.ClientID%>').click(function () {
            if ($('#<%=txtDate.ClientID%>').val() == "") {
                $('#SpnValidMsg').html("Please select Date");
                $('#ValidateMsgDiv').show();
                $('#<%=txtDate.ClientID%>').focus();
                return false;
            }
            else {
                $('#SpnValidMsg').html('');
                $('#ValidateMsgDiv').hide();
                return true;
            }
        });
    </script>
</asp:Content>
