<%@ Page Title="" Language="C#" MasterPageFile="~/SwitchOperationSite.Master" AutoEventWireup="true" CodeBehind="TermDenominationCountMaker.aspx.cs" Inherits="AGS.SwitchOperations.TermDenominationCountMaker" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phPageHeader" runat="server">
    <link href="Styles/responsive.bootstrap.css" rel="stylesheet" />
    <script src="Scripts/dataTables.responsive.min.js"></script>
    <script src="Scripts/responsive.bootstrap.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phPageBody" runat="server">
    <input type="hidden" name="ResultStatus" id="hdnResultStatus" runat="server" />
    <asp:HiddenField ID="hdnTransactionDetails" runat="server" />
    <asp:HiddenField ID="hdnSystems" runat="server" />
    <asp:HiddenField runat="server" ID="hdnAccessCaption" />
    <asp:HiddenField runat="server" ID="hdnSwitchExtraFields" />


    <div class="box-primary">
        <div class="box-header with-border">
            <i class="fa fa-list"></i>
            <h2 class="box-title">Term Denomination Count Update Maker: </h2>
        </div>

        <!--Display validation msg ------------------------------------------------------------------------->
        <div class="pad margin no-print" id="errormsgDiv" style="display: none">
            <div class="callout callout-info" style="margin-bottom: 0!important;">
                <h4><i class="fa fa-info"></i>Information :</h4>
                <span id="SpnErrorMsg" class="text-center"></span>
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
            </div>
            <div class="row">
                <div class="col-md-2">
                </div>
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
                        <label for="inputName" class="col-xs-4 control-label">Terminal Status:</label>
                        <div class="col-sm-8">
                            <label for="inputName" class="col-xs-4 control-label" id="lblTerminalStatus" />
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-md-2">
                </div>
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
                <div class="col-md-2">
                </div>
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
                <div class="col-md-2">
                </div>
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
                <div class="col-md-2">
                </div>
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
                <div class="col-md-2">
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <input type="button" class="btn btn-primary" value="Submit" onclick="funSubmit($(this));">

                        <input type="button" class="btn btn-primary" value="BackToList" onclick="funBackToList($(this));">
                        <%--<div class="col-sm-8">
                             <input type="text" class="form-control" maxlength="10" runat="server" name="Cassette4Count" id="Text2" onkeypress="return FunChkIsNumber(event)" /> 
                        </div>--%>
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

            $.ajax({
                url: '/TermDenominationCountMaker.aspx?Fn=TDCU',
                data: 'tid=' + $(obj).attr('TerminalID'),
                context: document.body,
                cache: false,
                type: 'POST',
                success: function (response) {
                    //alert(response.split('|')[0]);
                    AssignValues(response.split('|')[0])
                }
            });
        }

        function AssignValues(_response) {

            document.getElementById("lblTerminalId").innerHTML = _response.split('~')[0];
            document.getElementById("lblTerminalStatus").innerHTML = _response.split('~')[1];

            if (_response.split('~')[2] != '') {
                document.getElementById("lblCassette1Deno").innerHTML = _response.split('~')[2];
                document.getElementById("phPageBody_txtCassette1Count").disabled = false;
                document.getElementById("phPageBody_txtCassette1Count").value = _response.split('~')[3];
            }
            else {
                document.getElementById("lblCassette1Deno").innerHTML = '';
                document.getElementById("phPageBody_txtCassette1Count").disabled = true;
            }

            if (_response.split('~')[4] != '') {
                document.getElementById("lblCassette2Deno").innerHTML = _response.split('~')[4];
                document.getElementById("phPageBody_txtCassette2Count").value = _response.split('~')[5];
            }
            else {
                document.getElementById("lblCassette2Deno").innerHTML = '';
                document.getElementById("phPageBody_txtCassette2Count").disabled = true;
            }

            if (_response.split('~')[6] != '') {
                document.getElementById("lblCassette3Deno").innerHTML = _response.split('~')[6];
                document.getElementById("phPageBody_txtCassette3Count").value = _response.split('~')[7];
            }
            else {
                document.getElementById("lblCassette3Deno").innerHTML = '';
                document.getElementById("phPageBody_txtCassette3Count").disabled = true;
            }

            if (_response.split('~')[8] != '') {
                document.getElementById("lblCassette4Deno").innerHTML = _response.split('~')[8];
                document.getElementById("phPageBody_txtCassette4Count").value = _response.split('~')[9];
            }
            else {
                document.getElementById("lblCassette4Deno").innerHTML = '';
                document.getElementById("phPageBody_txtCassette4Count").disabled = true;
            }

            $("#<%=hdnSwitchExtraFields.ClientID %>").val('');
            $("#<%=hdnSwitchExtraFields.ClientID %>").val(_response.split('~')[10] + '|' + _response.split('~')[11] + '|' + _response.split('~')[12]);

            if (document.getElementById("lblTerminalStatus").innerHTML == "In Service") {
                //document.getElementById("divSendCommand").style.display = "block";
            }
            else {
                //document.getElementById("divSendCommand").style.display = "none";
            }
        }
    </script>

    <script>
        function funSendCommand(obj) {
            alert('command sent');
        }
    </script>
    <script>
        function funBackToList(obj) {
            location.reload();
        }
    </script>

    <script>
        function funSubmit(obj) {
            if (document.getElementById("lblTerminalStatus").innerHTML == "In Service") {
                alert("Terminal is In-Service");
                return true;
            }
            else {
                var errrorTextPD = 'Please provide :  ';
                var errorFieldsPD = '';

                if (document.getElementById("lblCassette1Deno").innerHTML != '' && $("#phPageBody_txtCassette1Count").val() == "") {
                    errortab = '1';
                    if (errorFieldsPD != '') {
                        errorFieldsPD = errorFieldsPD + '<b>, Cassette1 Count</b> ';
                    }
                    else {
                        errorFieldsPD = errorFieldsPD + '<b>Cassette1 Count</b> ';
                    }
                    $('#<%=txtCassette1Count.ClientID%>').focus();
                }

                if (document.getElementById("lblCassette2Deno").innerHTML != '' && $("#phPageBody_txtCassette2Count").val() == "") {
                    errortab = '1';
                    if (errorFieldsPD != '') {
                        errorFieldsPD = errorFieldsPD + '<b>, Cassette2 Count</b> ';
                    }
                    else {
                        errorFieldsPD = errorFieldsPD + '<b>Cassette2 Count</b> ';
                    }
                    $('#<%=txtCassette2Count.ClientID%>').focus();
                }

                if (document.getElementById("lblCassette3Deno").innerHTML != '' && $("#phPageBody_txtCassette3Count").val() == "") {
                    errortab = '1';

                    if (errorFieldsPD != '') {
                        errorFieldsPD = errorFieldsPD + '<b>,Cassette3 Count</b> ';

                    }
                    else {
                        errorFieldsPD = errorFieldsPD + '<b>Cassette3 Count</b> ';

                    }

                    $('#<%=txtCassette3Count.ClientID%>').focus();
                }

                if (document.getElementById("lblCassette4Deno").innerHTML != '' && $("#phPageBody_txtCassette4Count").val() == "") {
                    errortab = '1';

                    if (errorFieldsPD != '') {
                        errorFieldsPD = errorFieldsPD + '<b>,Cassette4 Count</b> ';

                    }
                    else {
                        errorFieldsPD = errorFieldsPD + '<b>Cassette4 Count</b> ';

                    }
                    $('#<%=txtCassette4Count.ClientID%>').focus();
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

                    <%--var ArrayIds = [];
                    $("[id*=SystemList] input:checked").each(function (i) {
                        ArrayIds[i] = $(this).val();


                    }); 
                    $("#<%=hdnSystems.ClientID%>").val(ArrayIds.join(","))
                      $('.shader').fadeIn();--%>
                }

                $.ajax({
                    url: '/TermDenominationCountMaker.aspx?Fn=TDCUSubmit',
                    data: 'tid=' + document.getElementById("lblTerminalId").innerHTML + '&Cassete1Deno=' + document.getElementById("lblCassette1Deno").innerHTML + '&Cassete1Count=' + document.getElementById("phPageBody_txtCassette1Count").value + '&Cassete2Deno=' + document.getElementById("lblCassette2Deno").innerHTML + '&Cassete2Count=' + document.getElementById("phPageBody_txtCassette2Count").value + '&Cassete3Deno=' + document.getElementById("lblCassette3Deno").innerHTML + '&Cassete3Count=' + document.getElementById("phPageBody_txtCassette3Count").value + '&Cassete4Deno=' + document.getElementById("lblCassette4Deno").innerHTML + '&Cassete4Count=' + document.getElementById("phPageBody_txtCassette4Count").value + '&SwitchExtraFields=' + $("#<%=hdnSwitchExtraFields.ClientID %>").val(),
                    context: document.body,
                    cache: false,
                    type: 'POST',
                    success: function (response) {
                        //alert(response.split('|')[0]);
                        alert(response.split('|')[0])
                        funBackToList(obj);
                    }
                });
            }
        }
    </script>

</asp:Content>
