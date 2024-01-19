<%@ Page Title="" Language="C#" MasterPageFile="~/SwitchOperationSite.Master" AutoEventWireup="true" CodeBehind="CardOrderRequest.aspx.cs" Inherits="AGS.SwitchOperations.CardOrderRequest" %>

<%@ Register Src="~/UserControls/UserButtons.ascx" TagPrefix="AGS" TagName="usrButtons" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phPageHeader" runat="server">
    <%--<link href="Styles/buttons.bootstrap.min.css" rel="stylesheet" />--%>

    <%--<script src="Scripts/jquery.dataTables_Right.min.js"></script>--%>
    <%--<script src="Scripts/dataTables.responsive_right.min.js"></script>--%>
    <%-- <style>
        .x_panel {
            position: relative;
            width: 100%;
            margin-bottom: 10px;
            padding: 10px 17px;
            display: inline-block;
            background: #fff;
            border: 1px solid #E6E9ED;
            -webkit-column-break-inside: avoid;
            -moz-column-break-inside: avoid;
            column-break-inside: avoid;
            opacity: 1;
            transition: all .2s ease;
        }

        .x_title {
            border-bottom: 2px solid #E6E9ED;
            padding: 1px 5px 6px;
            margin-bottom: 10px;
        }

            .x_title .filter {
                width: 40%;
                float: right;
            }

            .x_title h2 {
                margin: 5px 0 6px;
                float: left;
                display: block;
                text-overflow: ellipsis;
                overflow: hidden;
                white-space: nowrap;
            }

                .x_title h2 small {
                    margin-left: 10px;
                }

            .x_title span {
                color: #BDBDBD;
            }

        .bottom {
            border-bottom: 1px solid #E6E9ED;
        }

        .btn-outline:hover, .btn-outline:focus, .btn-outline:active, .btn-outline.active, .open > .dropdown-toggle.btn-outline {
            color: #fff;
            background-color: #E6E9ED;
        }
    </style>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phPageBody" runat="server">
    <asp:HiddenField ID="hdnTransactionDetails" runat="server" />
    <asp:HiddenField runat="server" ID="hdnSelectedCustomers" />
    <asp:HiddenField runat="server" ID="hdnFlag" />
    <asp:HiddenField ID="hdnAccessCaption" runat="server" />
    <div class="box-primary">
        <div class="box-header with-border">
            <i class="fa fa-list"></i>
            <!-- start sheetal card management change to -->
            <h2 class="box-title">Card Process </h2>
        </div>
        <!-- <div>-->
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
                            <input type="text" id="txtDate" aria-readonly="true" runat="server" class="datepicker form-control" style="margin-right: 0px;" />

                        </div>

                        <%--<div class="col-md-2" style="margin-left: 0px; padding-left: 0px">
                                <asp:Button ID="btnSet" runat="server" CssClass="btn btn-primary form-control" Style="margin-left: 0px; border-radius: 0px 10px 10px 0px;" OnClick="btnSet_Click" Text=">" />
                            </div>--%>
                        <%--    <div class="col-md-4">
                                <asp:DropDownList ID="ddlBatch" AutoPostBack="true" runat="server" CssClass="dropdown form-control" OnSelectedIndexChanged="ddlBatch_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>--%>
                        <%--</div>--%>
                    </div>
                    <div class="col-md-4">
                        <div class="form-group">
                            <label for="inputName" class="col-xs-4 control-label">Batch No:</label>
                            <div class="col-sm-7">
                                <asp:DropDownList runat="server" ID="ddlBatch" AutoPostBack="true" CssClass="dropdown form-control" OnSelectedIndexChanged="ddlBatch_SelectedIndexChanged"></asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="form-group">
                            <div class="col-sm-4"></div>
                            <div class="col-sm-7">
                                <%--<asp:Button runat="server" CssClass="btn btn-primary" ID="btnSearchCustomer" Text="Search" OnClick="btnSearch_Click" OnClientClick="return FunValidation()" />--%>
                                <asp:Button runat="server" CssClass="btn btn-primary" ID="btnSearchCustomer" Text="Search" />
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

        <%--  <div id="AuthDiv">
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
                        <div class="col-md-4">
                            <asp:Button ID="btnAccept" runat="server" Text="Accept" CssClass="btn-div btn-style btn btn-primary" OnClientClick="return funGetResult(1)" OnClick="AcceptRejectCardOpsRequests" />
                        </div>
                        <div class="col-md-4">
                            
                            <input type="button" value="Reject" onclick="funGetResult(2)" id="btnReject" class="btn-div btn-style btn btn-primary" />
                        </div>
                        <div class="col-md-4">
                            <asp:Button ID="Button1" runat="server" Text="Cancel" CssClass="btn-div btn-style btn btn-primary" OnClientClick="fnreset(null, true)" OnClick="Page_Load" />
                        </div>
                    </div>
                    <div class="col-md-4"></div>
                </div>
            </div>--%>
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
            $('#datatable-buttons').html($("#<%=hdnTransactionDetails.ClientID %>").val());

            <%--if ($('#<%=hdnTransactionDetails.ClientID %>').val() == '')
            {
                //$('#divResult').hide();
            }--%>

            $("#<%=hdnTransactionDetails.ClientID %>").val('');



            if ($("#datatable-buttons tbody tr").length > 0) {
                var handleDataTableButtons = function () {
                    if ($("#datatable-buttons").length) {
                        $("#datatable-buttons").DataTable({
                            "info": true,
                            //lengthMenu: [[10, 25, 50, -1], [10, 25, 50, "All"]],
                            //keys: true,
                            //deferRender: true,
                            //scrollCollapse: true,
                            //scroller: false,
                            //fixedHeader: false,
                            //Processing: true,
                            //order: [],
                            //columnDefs: [{
                            //    className: 'ResponsiveIcon',
                            //    orderable: false
                            //    , targets:8
                            //}],
                            //responsive: {
                            //    details: {
                            //        type: 'column'

                            //    }
                            //},
                            //responsive:true,
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
                $('#phPageBody_userBtns_btnProcess_U').show();
                //$('#SpnValidMsg').html('');
                //$('#ValidateMsgDiv').hide();
            }
            else {
                $('#DivResult').hide();
            }
            //If user has Process right
            if ($.inArray("P", $('#<%=hdnAccessCaption.ClientID%>').val().split(",")) > -1) {
                $('#SelectAllDiv').show()
            }
            else { $('#SelectAllDiv').hide() }


            if ($('#<%=hdnFlag.ClientID%>').val() == "1") {
                $('#<%=btnBack.ClientID%>').show();
                $('#phPageBody_userBtns_btnProcess_U').hide()
                $('#SelectAllDiv').hide()
            }
            else {
                $('#<%=btnBack.ClientID%>').hide();
                $('#SelectAllDiv').show()
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
    <%-- Click fuctions --%>
    <script>
        $('#phPageBody_userBtns_btnProcess_U').click(function () {
            if ($('#datatable-buttons tbody input[type=checkbox]:checked').length == 0) {
                $('#SpnValidMsg').html("Please select customers to process");
                $('#ValidateMsgDiv').show();
                return false;
            }
            else {
                $('#SpnValidMsg').html('');
                $('#ValidateMsgDiv').hide();
                var ArrayIds = [];
                $('#datatable-buttons tbody input[type=checkbox]:checked').each(function (i) {
                    ArrayIds[i] = $(this).attr("custid");
                });
                $("#<%=hdnSelectedCustomers.ClientID%>").val(ArrayIds.join(","))
                $('.shader').fadeIn();
            }
        })
    </script>
</asp:Content>
