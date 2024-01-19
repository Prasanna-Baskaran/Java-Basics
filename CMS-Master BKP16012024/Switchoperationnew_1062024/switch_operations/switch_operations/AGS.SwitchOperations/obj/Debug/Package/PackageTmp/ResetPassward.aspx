<%@ Page Title="" Language="C#" MasterPageFile="~/SwitchOperationSite.Master" AutoEventWireup="true" CodeBehind="ResetPassward.aspx.cs" Inherits="AGS.SwitchOperations.ResetPassward" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phPageHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phPageBody" runat="server">
    <asp:HiddenField ID="hdnTransactionDetails" runat="server" />
    <asp:HiddenField ID="hdnUserID" runat="server" />
    <asp:Button runat="server" ID="BtnReset" style="display:none" OnClick="btnReset_Click" />
    <div class="row">
        <div class="col-md-12">
            <div class="box-primary">
                <!-- /.box-header -->
                <div class="box-body no-padding">
                    <div class="row" style="padding: 0px 30px; padding-top: 15px;">
                        <div class="col-md-12">
                            <div class="x_panel">
                                <div class="x_title">
                                    <h2>Reset Passward</h2>
                                    <div class="clearfix"></div>
                                </div>
                                <div class="x_content">
                                    <!--Display validation msg ------------------------------------------------------------------------->
                                    <div class="row">
                                        <div class="pad margin no-print" id="errormsgDiv" style="display: none">
                                            <div class="callout callout-info" style="margin-bottom: 0!important;">
                                                <h4><i class="fa fa-info"></i>Information :</h4>
                                                <span id="SpnErrorMsg" class="text-center"></span>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row" id="SearchDiv">
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label for="inputName" class="col-xs-5 control-label">User Name:</label>
                                                <div class="col-sm-7">
                                                    <input runat="server" type="text" class="form-control" id="txtSearchUserName" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-4"></div>
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <asp:Button runat="server" CssClass="btn btn-primary" ID="btnSearch" Text="Search" OnClientClick="return FunValidation(1);" OnClick="btnSearch_Click" />
                                            </div>
                                        </div>

                                    </div>

                                    <div class="row" id="DivResult">
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

        <!-- Modal HTML -->
        <div id="memberModal" class="modal fade bs-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel">
            <div class="modal-dialog modal-md">
                <div class="modal-content" style="border-radius: 6px">
                    <div class="modal-header box-primary" style="border-top-right-radius: 6px; border-top-left-radius: 6px">
                        <button aria-label="Close" data-dismiss="modal" class="close" type="button"><span aria-hidden="true">×</span></button>
                        <h4 id="myLargeModalLabel" class="modal-title" style="font-weight: bold">Customer Registration</h4>
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

    <%--//************************* SCRIPTS *********************************--%>
    <script>
        $(document).ready(function () {
            $('#datatable-buttons').html($("#<%=hdnTransactionDetails.ClientID %>").val());
            $("#<%=hdnTransactionDetails.ClientID %>").val('');

            //For Data Table
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

            $('#<%=hdnUserID.ClientID%>').val('');
        });
    </script>

    <script>
        function FunValidation(Para) {
            //Search button validation
            if (Para == "1") {
                var Username = $('#<%=txtSearchUserName.ClientID%>').val();

                if (Username == "") {
                    $('#SpnErrorMsg').html('Please provide Username');
                    $('#errormsgDiv').show();
                 //   $("#<%=txtSearchUserName.ClientID%>").focus();
                    return false;
                }
                else  {
                    $('#errormsgDiv').hide();

                    return true;
                }
            }


        }
        function FunReset(obj)
        {
            var UserID = $(obj).attr('userid');
            $('#<%=hdnUserID.ClientID%>').val(UserID);
             $('#<%= BtnReset.ClientID %>').click();

        }
        //Response Message
        function FunShowMsg() {         
            $('#memberModal').modal('show');
        }
    </script>
</asp:Content>
