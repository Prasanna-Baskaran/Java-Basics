<%@ Page Title="" Language="C#" MasterPageFile="~/SwitchOperationSite.Master" AutoEventWireup="true" CodeBehind="TermDenominationCountChecker.aspx.cs" Inherits="AGS.SwitchOperations.TermDenominationCountChecker" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phPageHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phPageBody" runat="server">

    <input type="hidden" name="ResultStatus" id="hdnResultStatus" runat="server" />
    <asp:HiddenField ID="hdnTransactionDetails" runat="server" />
    <asp:HiddenField ID="hdnSystems" runat="server" />
    <asp:HiddenField runat="server" ID="hdnAccessCaption" />
    <asp:HiddenField runat="server" ID="LoginUserSystems" />


    <div class="box-primary">
        <div class="box-header with-border">
            <i class="fa fa-list"></i>
            <h2 class="box-title">Term Denomination Count Update Checker: </h2>
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
                                        <div>
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

        <div class="box-body" id="DivUpdateDetails" style="display: none;">

            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label for="inputName" class="col-xs-4 control-label">Terminal Id:</label>
                        <div class="col-sm-8">
                            <label for="inputName" class="col-xs-4 control-label" id="lblTerminalId" />
                            <%--<input type="text" class="form-control" maxlength="20" runat="server" name="SearchMobile" id="txtSearchCardNo" onkeypress="return FunChkIsNumber(event)" />--%>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label for="inputName" class="col-xs-4 control-label" id="lblTermDenominationId" style="display: none" />
                        <%--<label for="inputName" class="col-xs-4 control-label">Terminal Status:</label>
                        <div class="col-sm-8">
                            <label for="inputName" class="col-xs-4 control-label" id="lblTerminalStatus" />
                        </div>--%>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label for="inputName" class="col-xs-4 control-label">Cassette 1 Deno:</label>
                        <div class="col-sm-8">
                            <label for="inputName" class="col-xs-4 control-label" id="lblCassette1Deno" />
                            <%--<input type="text" class="form-control" maxlength="20" runat="server" name="SearchMobile" id="txtSearchCardNo" onkeypress="return FunChkIsNumber(event)" />--%>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label for="inputName" class="col-xs-4 control-label">Cassette 1 Count:</label>
                        <div class="col-sm-8">
                            <input type="text" class="form-control" maxlength="15" runat="server" name="Cassette1Count" id="txtCassette1Count" onkeypress="return FunChkIsNumber(event)" />
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label for="inputName" class="col-xs-4 control-label">Cassette 2 Deno:</label>
                        <div class="col-sm-8">
                            <label for="inputName" class="col-xs-4 control-label" id="lblCassette2Deno" />
                            <%--<input type="text" class="form-control" maxlength="20" runat="server" name="SearchMobile" id="txtSearchCardNo" onkeypress="return FunChkIsNumber(event)" />--%>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label for="inputName" class="col-xs-4 control-label">Cassette 2 Count:</label>
                        <div class="col-sm-8">
                            <input type="text" class="form-control" maxlength="15" runat="server" name="Cassette2Count" id="txtCassette2Count" onkeypress="return FunChkIsNumber(event)" />
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label for="inputName" class="col-xs-4 control-label">Cassette 3 Deno:</label>
                        <div class="col-sm-8">
                            <label for="inputName" class="col-xs-4 control-label" id="lblCassette3Deno" />
                            <%--<input type="text" class="form-control" maxlength="20" runat="server" name="SearchMobile" id="txtSearchCardNo" onkeypress="return FunChkIsNumber(event)" />--%>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label for="inputName" class="col-xs-4 control-label">Cassette 3 Count:</label>
                        <div class="col-sm-8">
                            <input type="text" class="form-control" maxlength="10" runat="server" name="Cassette3Count" id="txtCassette3Count" onkeypress="return FunChkIsNumber(event)" />
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label for="inputName" class="col-xs-4 control-label">Cassette 4 Deno:</label>
                        <div class="col-sm-8">
                            <label for="inputName" class="col-xs-4 control-label" id="lblCassette4Deno" />
                            <%--<input type="text" class="form-control" maxlength="20" runat="server" name="SearchMobile" id="txtSearchCardNo" onkeypress="return FunChkIsNumber(event)" />--%>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label for="inputName" class="col-xs-4 control-label">Cassette 4 Count:</label>
                        <div class="col-sm-8">
                            <input type="text" class="form-control" maxlength="10" runat="server" name="Cassette4Count" id="txtCassette4Count" onkeypress="return FunChkIsNumber(event)" />
                        </div>
                    </div>
                </div>
            </div>


            <div class="row" id="divSendCommand" style="display: none;">
                <div class="col-md-4">
                    <div class="form-group">

                        <label for="inputName" class="col-xs-4 control-label">Send Command:</label>

                        <div class="col-sm-8">
                            <asp:DropDownList ID="ddlSendCommand" runat="server" class="col-xs-8 select2-dropdown">
                                <%--<asp:ListItem Text="--Select--" Value="0"></asp:ListItem>--%>
                                <asp:ListItem Text="Close" Value="Close"></asp:ListItem>
                                <asp:ListItem Text="Open" Value="Open"></asp:ListItem>
                            </asp:DropDownList>

                            <%--<input type="text" class="form-control" maxlength="20" runat="server" name="SearchMobile" id="txtSearchCardNo" onkeypress="return FunChkIsNumber(event)" />--%>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <input type="button" class="btn btn-primary" style="align-items: flex-start" value="SendCommand" onclick="funSendCommand($(this));">
                        <div class="col-sm-8">
                            <%--<input type="text" class="form-control" maxlength="10" runat="server" name="Cassette4Count" id="Text2" onkeypress="return FunChkIsNumber(event)" />--%>
                        </div>
                    </div>
                </div>
            </div>


            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <input type="button" id="btnApprove" class="btn btn-primary" value="Approve" onclick="funProcessRequest($(this));">
                        <input type="button" id="btnReject" class="btn btn-primary" value="Reject" onclick="funProcessRequest($(this));">
                        <input type="button" id="btnBackTOList" class="btn btn-primary" value="BackToList" onclick="funBackToList($(this));">
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


    <script>
        $(document).ready(function () {
            $(function () {
                $('[data-toggle="tooltip"]').tooltip()
            });

            //Add UserPrefix 
            $("#spnUserPrefix").html('<%=HttpContext.Current.Session["UserPrefix"]%>')

            $('#datatable-buttons').html($("#<%=hdnTransactionDetails.ClientID %>").val());
            $("#<%=hdnTransactionDetails.ClientID %>").val('');


            //For Data Table
            if ($("#datatable-buttons tbody tr").length > 0) {
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
                $('#datatable-buttons tbody input[type=button][isedit=0][value=Edit]').hide()
            }

            $("#phPageBody_SystemList").find("td").css("padding-right", "10px")



        });

    </script>


    <script>

        function funedit(obj) {
            document.getElementById("DivUpdateDetails").style.display = "block";
            document.getElementById("DivResult").style.display = "none";

            if ($(obj).attr('Status') == 'Pending') {
                document.getElementById("btnApprove").disabled = false;
                document.getElementById("btnReject").disabled = false;
            }
            else {
                document.getElementById("btnApprove").disabled = true;
                document.getElementById("btnReject").disabled = true;

            }
            //alert($(obj).attr('Cassete1Count'));

            document.getElementById("lblTerminalId").innerHTML = $(obj).attr('TerminalID');
            document.getElementById("lblTermDenominationId").innerHTML = $(obj).attr('Id');
            document.getElementById("lblCassette1Deno").innerHTML = $(obj).attr('Cassete1Deno');
            document.getElementById("lblCassette2Deno").innerHTML = $(obj).attr('Cassete2Deno');
            document.getElementById("lblCassette3Deno").innerHTML = $(obj).attr('Cassete3Deno');
            document.getElementById("lblCassette4Deno").innerHTML = $(obj).attr('Cassete4Deno');

            document.getElementById("phPageBody_txtCassette1Count").value = $(obj).attr('Cassete1Count');
            document.getElementById("phPageBody_txtCassette2Count").value = $(obj).attr('Cassete2Count');
            document.getElementById("phPageBody_txtCassette3Count").value = $(obj).attr('Cassete3Count');
            document.getElementById("phPageBody_txtCassette4Count").value = $(obj).attr('Cassete4Count');
        }

    </script>

    <script>
        function funBackToList(obj) {
            location.reload();
        }
    </script>

    <script>
        function funProcessRequest(obj) {
            $.ajax({
                url: '/TermDenominationCountChecker.aspx?Fn=TDCUSubmit',
                data: 'tid=' + document.getElementById("lblTerminalId").innerHTML + '&RequestType=' + $(obj).attr('value') + '&TermDenominationId=' + document.getElementById("lblTermDenominationId").innerHTML,
                context: document.body,
                cache: false,
                type: 'POST',
                success: function (response) {
                    document.getElementById("btnApprove").disabled = true;
                    document.getElementById("btnReject").disabled = true;
                    alert(response.split('|')[0]);
                    //AssignValues(response.split('|')[0])
                }
            });
        }

    </script>

</asp:Content>
