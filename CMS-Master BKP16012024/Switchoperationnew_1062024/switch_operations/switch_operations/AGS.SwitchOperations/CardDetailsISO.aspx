<%@ Page Title="" Language="C#" MasterPageFile="~/SwitchOperationSite.Master" AutoEventWireup="true" CodeBehind="CardDetailsISO.aspx.cs" Inherits="AGS.SwitchOperations.CardDetailsISO" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phPageHeader" runat="server">
    <script>
        <%-- Datatable fun----%>

        $(document).ready(function () {

            $('#datatable-buttons').html($("#<%=hdnTransactionDetails.ClientID %>").val());
            $("#<%=hdnTransactionDetails.ClientID %>").val('');
            if ($("#datatable-buttons tbody tr").length > 0) {
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
                $('#DivResult').show();
            }
            else {
                $('#DivResult').hide();
            }
        });


        //Validation on Search Button
        function FunValidation() {
            var AccNo = $('#phPageBody_txtSearchAccountNo').val()
            var CardNo = $('#phPageBody_txtSearchCardNo').val()
            // var Name = $('#phPageBody_txtSearchName').val()
            var NIC = $('#phPageBody_txtNICNo').val()
            var NameOnCard = $('#phPageBody_txtSearchNameOnCard').val()

            if (AccNo == "" && CardNo == ""// && Name == "" 
                && NIC == "" && NameOnCard == "") {
                $('#SpnErrorMsg').html('Please provide at least one value');
                $('#errormsgDiv').show();
                return false;
            }
            //if (CardNo == "" && Name == "") {
            //    $('#SpnErrorMsg').html('Please provide Card No / Customer Name');
            //    $('#errormsgDiv').show();
            //    return false;
            //}
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
            <!-- start sheetal card details chnage to card details view-->
            <h2 class="box-title">Card Details View </h2>
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
                    <asp:panel ID="pnltxtAccount" runat="server" class="col-md-6">
                        <div class="form-group">
                            <label for="inputName" class="col-xs-3 control-label">AccountNo:</label>
                            <div class="col-sm-9" style="margin-bottom: 15px;">
                                <input type="text" class="form-control" maxlength="20" runat="server" name="CustomerID" id="txtSearchAccountNo"
                                    onkeypress="return FunChkIsNumber(event)" />
                            </div>
                        </div>
                    </asp:panel>
                   <%-- <div class="col-md-6">
                        <div class="form-group">
                             <label for="inputName" class="col-xs-3 control-label">Customer Name:</label>
                            <div class="col-sm-9">
                                <input type="text" class="form-control" maxlength="25" runat="server" name="Name" id="txtSearchName" onkeypress="return onlyAlphabets(event,this);" />
                            </div>
                        </div>
                    </div>  --%>
                    <div class="col-md-6">
                        <div class="form-group">
                            <label for="inputName" class="col-xs-3 control-label">Card No:</label>
                            <div class="col-sm-9">
                                <input type="text" class="form-control" maxlength="20" runat="server" name="CardNO" id="txtSearchCardNo" onkeypress="return FunChkIsNumber(event)" />
                            </div>
                        </div>
                    </div>
</div>
                <div class="row">
                   

                    <div class="col-md-6">
                        <div class="form-group">
                            <label for="inputName" class="col-xs-3 control-label">NIC No:</label>
                            <div class="col-sm-9">
                                <input type="text" class="form-control" maxlength="20" runat="server" name="NICNo" id="txtNICNo" />
                            </div>
                        </div>
                    </div>
                   
                       <div class="col-md-6">
                            <div class="form-group">
                                <label for="inputName" class="col-xs-3 control-label">CIF No:</label>
                                <div class="col-sm-9">
                                    <input type="text" class="form-control" maxlength="20" runat="server" name="cif" id="txtCIF" />
                                </div>
                            </div>
                        </div>

                     <asp:Panel ID="pnlnameoncard" runat="server" class="col-md-6">
                        <div class="form-group">
                            <label for="NameOnCard" class="col-xs-3 control-label">Name On Card:</label>
                            <div class="col-sm-9">
                                <input type="text" class="form-control" maxlength="20" runat="server" name="NameOnCard" id="txtSearchNameOnCard" />
                            </div>
                        </div>
                                
                        

                    </asp:Panel> </div>
                <div class="row">
                    <div class="col-md-12 text-center">
                        &nbsp;
                        </div>
                </div>
                <div class="row">
                    <div class="col-md-12 text-center">
                        <div class="form-group">
                            <div class="col-md-12">
                                <asp:Button runat="server" CssClass="btn btn-primary" ID="btnSearch" Text="Search" OnClick="btnSearch_Click" OnClientClick="return FunValidation();" />
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
</asp:Content>
