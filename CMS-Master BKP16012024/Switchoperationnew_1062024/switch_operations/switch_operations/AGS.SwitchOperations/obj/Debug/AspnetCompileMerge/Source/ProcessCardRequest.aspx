<%@ Page Title="" Language="C#" MasterPageFile="~/SwitchOperationSite.Master" AutoEventWireup="true" CodeBehind="ProcessCardRequest.aspx.cs" Inherits="AGS.SwitchOperations.ProcessCardRequest" %>

<%@ Register Src="~/UserControls/UserButtons.ascx" TagPrefix="AGS" TagName="usrButtons" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phPageHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phPageBody" runat="server">
    <asp:HiddenField ID="hdnTransactionDetails" runat="server" />
    <asp:HiddenField runat="server" ID="hdnSelectedCustomers" />
    <asp:HiddenField runat="server" ID="hdnFlag" />
    <asp:HiddenField ID="hdnAccessCaption" runat="server" />
    <asp:HiddenField runat="server" ID="hdnFormStatusID" />
    <asp:HiddenField runat="server" ID="hdnCustomerID" />
    <asp:HiddenField runat="server" ID="hdnBankCustId" />

    <div class="box-primary">
        <div class="box-header with-border">
            <i class="fa fa-list"></i>
            <h2 class="box-title">Process Card Request</h2>
        </div>
        <div class="box-body">
            <div class="form-horizontal">
                <!--Display validation msg ------------------------------------------------------------------------->
                <div class="pad margin no-print" id="ValidateMsgDiv" style="display: none">
                    <div class="callout callout-info" style="margin-bottom: 0!important;">
                        <h4><i class="fa fa-info"></i>Information :</h4>
                        <span id="SpnValidMsg" class="text-center"></span>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-4">
                        <div class="form-group">
                            <label for="inputName" class="col-xs-4 control-label">Request Type:</label>
                            <div class="col-sm-8">
                                <asp:DropDownList runat="server" ID="DdlRequestType" CssClass="form-control">
                                    <asp:ListItem Text="Issuance" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Edit" Value="2"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4" id="">
                        <div class="form-group">

                            <div class="col-sm-8">
                                <asp:Button runat="server" CssClass="btn btn-primary"
                                    ID="btnSearchCustomer" Text="View" OnClick="btnSearchCustomer_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
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

                                    <div class="row" id="AuthDiv">
                                        <div class="col-md-4"></div>
                                        <div class="col-md-4">
                                            <div class="col-md-4">
                                                <asp:Button ID="btnAccept" runat="server" Text="Accept" CssClass="btn-div btn-style btn btn-primary"
                                                    OnClick="btnAccept_Click" />
                                            </div>
                                            <div class="col-md-4">
                                                <%--<asp:Button ID="Button2" runat="server" Text="Reject" CssClass="btn-div btn-style btn btn-primary" OnClientClick="return funGetResult(2)" OnClick="AcceptRejectCardOpsRequests" />--%>

                                                <asp:Button ID="btnReject" runat="server" Text="Reject" CssClass="btn-div btn-style btn btn-primary" OnClick="btnReject_Click" />
                                            </div>
                                            <%--<div class="col-md-4">
                                                <asp:Button ID="Button2" runat="server" Text="Cancel" CssClass="btn-div btn-style btn btn-primary" OnClientClick="fnreset(null, true)" OnClick="Page_Load" />
                                            </div>--%>
                                        </div>
                                        <div class="col-md-4"></div>
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

    <div id="memberModal" class="modal fade bs-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel">
        <div class="modal-dialog modal-md">
            <div class="modal-content" style="border-radius: 6px">
                <div class="modal-header box-primary" style="border-top-right-radius: 6px; border-top-left-radius: 6px">
                    <button aria-label="Close" data-dismiss="modal" class="close" type="button" onclick="CancelModal();"><span aria-hidden="true">×</span></button>
                    <h4 id="myLargeModalLabel" class="modal-title" style="font-weight: bold">Card Processing </h4>
                </div>
                <div class="modal-body" id="msgBody">
                    <label for="username" class="control-label" id="LblMsg" runat="server" style="font-weight: normal"></label>
                    <br />
                    <label for="username" class="control-label" id="LblMsgFailuer" runat="server" style="font-weight: normal"></label>
                </div>
                <div class="modal-footer">
                    <button aria-label="Close" data-dismiss="modal" id="BtnModalClose" class="btn btn-primary" type="button" onclick="CancelModal();"><span aria-hidden="true">OK</span></button>
                </div>
            </div>
        </div>
        <!-- /.modal-content -->
    </div>

    <script>
        $(document).ready(function () {
            $('#datatable-buttons').html($("#<%=hdnTransactionDetails.ClientID %>").val());
            if ($("#datatable-buttons tbody tr").length > 0) {
                document.getElementById("DivResult").style.display = "block";
            }
            else {
                document.getElementById("DivResult").style.display = "none";
            }
            $("#<%=hdnTransactionDetails.ClientID %>").val('');

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

            $('[id$="btnAccept"]').click(function () {
                var allids = "";
                $('.shader').fadeIn();
                $('#datatable-buttons tbody').find('tr').each(function () {
                    if ($(this).find('input[type="checkbox"]:checked').length > 0) {
                        var id = $(this).find('td').next().html();
                        if (allids == "") {
                            allids = id;
                        } else {
                            allids = allids + "," + id;
                        }
                    }
                });

                if (allids == '') {
                    document.getElementById('phPageBody_LblMsg').innerHTML = 'Please check at least one checkbox to Accept Request.'
                    FunShowMsg();
                    $('.shader').fadeOut();
                    return false;
                }
                else {
                    $('[id$="hdnCustomerID"]').val(allids);
                }

            });

            $('[id$="btnReject"]').click(function () {
                var allids = "";
                $('.shader').fadeIn();
                $('#datatable-buttons tbody').find('tr').each(function () {
                    if ($(this).find('input[type="checkbox"]:checked').length > 0) {
                        var id = $(this).find('td').next().html();
                        if (allids == "") {
                            allids = id;
                        } else {
                            allids = allids + "," + id;
                        }
                    }
                });
                if (allids == '') {
                    document.getElementById('phPageBody_LblMsg').innerHTML = 'Please check at least one checkbox to Reject Request.'
                    FunShowMsg();
                    $('.shader').fadeOut();
                    return false;
                }
                else {
                    $('[id$="hdnCustomerID"]').val(allids);
                }

            });

        });

    </script>
    <script type="text/javascript">
        function funGetSelectedCustomers(FormstatusID) {


            if ($('#datatable-buttons tbody input[type=checkbox]:checked').length == 0) {
                $('#SpnValidMsg').html("Please select Card Requests to accept/reject");
                $('#ValidateMsgDiv').show();
                return false;
            }
            else {
                $('#SpnValidMsg').html('');
                $('#ValidateMsgDiv').hide();
                $("#<%=hdnFormStatusID.ClientID%>").val(FormstatusID);


                var ArrayIds = [];
                $('#datatable-buttons tbody input[type=checkbox]:checked').each(function (i) {
                    ArrayIds[i] = $(this).attr("ID");

                });
                //alert(val.join(","))

                $("#<%=hdnCustomerID.ClientID%>").val(ArrayIds.join(","))
                $("#<%=hdnBankCustId.ClientID%>").val(ArrayIds.join(","))

                //for Reject request
                if (FormstatusID == "2") {

                    $('[id$="txtRejectReson"]').attr('required', true)
                    $('#RejectConfirmationModal').modal('show');
                    $('input[name="IsConfirm"]').attr("checked", false)
                    $('#phPageBody_txtRejectReson').val('')
                    //$('input[name="IsConfirm"]').val(0)

                }
                else {
                    $('.shader').fadeIn();
                }

                return true;
            }
        }

    </script>

    <script>
        function FunShowMsg() {
            //  setTimeout($('#myModal').modal('show'),2000);
            $('#memberModal').modal('show');
            //if ($('#phPageBody_hdnResultStatus').val() == 1) {
            //    $('input[type="text"]').val('');
            //}
        }
    </script>
</asp:Content>
