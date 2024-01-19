<%@ Page Title="" Language="C#" MasterPageFile="~/SwitchOperationSite.Master" AutoEventWireup="true" CodeBehind="BankPREStandard.aspx.cs" Inherits="AGS.SwitchOperations.BankPREStandard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phPageHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phPageBody" runat="server">

    <!--All Hiddenfields------------------------------------------------------------------------->
    <asp:HiddenField ID="hdnTransactionDetails" runat="server" />
    <asp:HiddenField ID="hdnFlag" runat="server" />
    <asp:HiddenField ID="hdnAccessCaption" runat="server" />

    <!--Display validation msg ------------------------------------------------------------------------->
    <div class="pad margin no-print" id="errormsgDiv" style="display: none">
        <div class="callout callout-info" style="margin-bottom: 0!important;">
            <h4><i class="fa fa-info"></i>Information :</h4>
            <span id="SpnErrorMsg" class="text-center"></span>
        </div>
    </div>


    <asp:Panel ID="pnlPREStandardSearch" runat="server">
        <div style="font-size: 14px;">
            <div id="SearchPRE">
                <div class="box-primary">
                    <div class="box-header with-border">
                        <i class="fa fa-credit-card"></i>

                        <h4 class="box-title">Bank PREStandard Details:</h4>
                        <div class="box-tools pull-right">
                            <button class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                        </div>
                        <!-- /.box-tools -->
                    </div>



                    <div class="box-body">

                        <div class="box-body" id="SearchDiv">
                            <div class="form-horizontal">
                                <div class="row">

                                    <div class="col-md-4">
                                        <div class="form-group">
                                            <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>IssuerNo:</label>
                                            <div class="col-sm-8">
                                                <input type="text" class="form-control" maxlength="50" runat="server" name="SearchIssuerNo" id="txtSearchIssuerNo" onkeypress="return FunChkIsNumber(event)" />
                                            </div>
                                        </div>
                                    </div>
                                    <%--<div class="col-md-4">
                                        <div class="form-group">
                                            <label for="inputName" class="col-xs-4 control-label"><span style="color: red;"></span>CardProgram:</label>
                                            <div class="col-sm-8">
                                                <input type="text" class="form-control" maxlength="200" runat="server" name="SearchCardProgram" id="txtSearchCardProgram" onkeypress="return FunChkAlphaNumeric(event);" />

                                            </div>
                                        </div>
                                    </div>--%>
                                    <%--<div class="col-md-4">
                                        <div class="form-group">
                                            <label for="inputName" class="col-xs-4 control-label">Token:</label>
                                            <div class="col-sm-8">
                                                <input type="text" class="form-control" maxlength="200" runat="server" name="SearchToken" id="txtSearchToken" onkeypress="return onlyAlphabets(event);" />

                                            </div>
                                        </div>
                                    </div>--%>

                                </div>
                                <div class="row">
                                    <div class="col-md-4">
                                        <div class="form-group">
                                            <div class="col-sm-4"></div>

                                            <%--<div class="col-sm-4">
                                                <div class="col-md-7">
                                                    <asp:Button runat="server" CssClass="btn btn-primary pull-right" ID="btnSearch" Text="Search" OnClick="btnSearch_Click" OnClientClick="return FunSearchValidation();" />
                                                    
                                                </div>
                                            </div>--%>
                                            <div class="col-sm-3">
                                                <div class="col-md-8">
                                                    <asp:Button runat="server" CssClass="btn btn-primary pull-right" ID="btnSearch" Text="Search" OnClick="btnSearch_Click" OnClientClick="return FunSearchValidation();" />
                                                </div>
                                            </div>
                                            <div class="col-sm-3">
                                            <div class="col-md-8">
                                         <input type="button" class="btn btn-primary pull-right" id="BtnAddNew" data-targrt="#AddEditModal" value="Add New" onclick="funAddNew()"/>
                                            </div>
                                                </div>
                                            <div class="col-sm-4"></div>
                                        </div>
                                    </div>
                                </div>


                            </div>
                        </div>

                        <div class="row">
                                    <%--<div class="col-md-4">--%>
                                        <div class="form-group">
                                            <div class="col-sm-6"></div>
                                         <%--<div class="col-sm-4">--%>
                                            <div class="col-md-6">
                                    <asp:Button runat="server" CssClass="btn btn-primary pull-right" ID="Button1" Text="Reset" OnClick="btnReset_Click" />
                                            </div>
                                          </div>
                                        </div>


                        <div class="row" id="DivResultMsg">
                            <div class="col-md-6">
                                <h4>
                                    <label maxlength="20" runat="server" name="Name" id="LblResult" readonly="readonly" />
                                </h4>
                            </div>
                        </div>

                     <%--<div id="divAddReset">
                            <div class="row">
                     --%>           <%--<div class="col-md-6"></div>--%>
                                <%--<div class="col-md-6">
                                    <input type="button" class="btn btn-primary pull-right" id="BtnAddNew" data-targrt="#AddEditModal" value="Add New" onclick="funAddNew()" />
                                </div>--%>
                                <%--<div class="col-sm-4">--%>
                                <%--<div class="col-md-6"></div>--%>
                                <%--<div class="col-md-6">
                                    <asp:Button runat="server" CssClass="btn btn-primary pull-right" ID="Button1" Text="Reset" OnClick="btnReset_Click" />
                                </div>--%>
                                <%--</div>--%>
                            <%--</div>
                         </div>--%>
                            <%--<div class="row">
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
            </div>--%>
                        
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>

    <%--<div class="x_content">
        
    --%>
    <%--<div>
        <table id="datatable-buttons" class="table table-striped table-bordered" style="width: 100%">
        </table>
    </div>--%>
   <div class="box-primary">
    <div class="box-body" id="DivReslt">
    <div class="row">
                <div class="col-md-12">
                    <div class="box-primary">
                        <!-- /.box-header -->
                        <div class="box-body no-padding">
                            <div class="row" style="padding: 0px 30px; padding-top: 15px;">
                                <div class="col-md-12">
                                    <%--<div class="x_panel">
                                        <div>
                                            <div class="clearfix"></div>
                                        </div>--%>


                                       <%-- <div class="x_content">--%>
                                            <table id="datatable-buttons" class="table table-striped table-bordered" style="width: 100%">
                                            </table>
                                       <%-- </div>--%>
                                    <%--</div>--%>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        </div>
    <%--7-1-18--%>
    <div id="shader" class="shader">
        <div class="loadingContainer">
            <div id="divLoading3">
                <div id="loader-wrapper">
                    <div id="loader"></div>
                </div>
            </div>
        </div>
    </div>

    <div id="AddEditModal" class="modal fade" data-backdrop="static" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content -->
            <div class="modal-content" style="border-radius: 4px; width: 797px;">

                <div class="modal-header">
                    <button aria-label="Close" data-dismiss="modal" class="close" type="button" onclick="funCancelModal()"><span aria-hidden="true">×</span></button>
                    <h4 id="myLargeEdit" class="modal-title" style="font-weight: bold">PRE StandardDetails</h4>
                </div>

                
                <%--7-1-18--%>
                <!--Display validation msg ------------------------------------------------------------------------->
                <div class="pad margin no-print" id="errormsgDivForModel" style="display: none">
                    <div class="callout callout-info" style="margin-bottom: 0!important;">
                        <h4><i class="fa fa-info"></i>Information :</h4>
                        <span id="SpnErrorMsgForModel" class="text-center"></span>
                    </div>
                </div>


                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-6">
                            <label for="inputName" class="col-md-5 control-label"><span style="color: red;"></span>IssuerNo:</label>
                            <div class="col-md-7">
                                <input type="text" class="form-control" id="txtIssuerNo" runat="server" placeholder="Enter Issuer No" maxlength="50" onkeypress="return FunChkIsNumber(event);" />

                            </div>
                        </div>
                        <div class="col-md-6">
                            <label for="inputName" class="col-md-5 control-label"><span style="color: red;"></span>CardProgram:</label>
                            <div class="col-md-7">
                                <input class="form-control" type="text" id="txtCardProgram" runat="server" placeholder="Enter Card Program" maxlength="200" onkeypress="return FunChkAlphaNumeric(event);" />

                            </div>
                        </div>


                    </div>

                    <div class="row">
                        <br />
                    </div>

                    <div class="row">
                        <div class="col-md-6">
                            <label for="inputName" class="col-md-5 control-label"><span style="color: red;"></span>Padding:</label>
                            <div class="col-md-7">
                                <asp:DropDownList ID="ddlPadding" runat="server" class="form-control" Style="width: 100%">
                                     <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Right" Value="Right"></asp:ListItem>
                                    <asp:ListItem Text="Left" Value="Left"></asp:ListItem>
                                </asp:DropDownList>
                            </div>

                        </div>
                        <div class="col-md-6">

                            <label for="inputName" class="col-md-5 control-label"><span style="color: red;"></span>Direction:</label>
                            <div class="col-md-7">
                                <asp:DropDownList ID="ddlDirection" runat="server" class="form-control" Style="width: 100%">
                                    <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Right" Value="Right"></asp:ListItem>
                                    <asp:ListItem Text="LEFT" Value="LEFT"></asp:ListItem>
                                </asp:DropDownList>

                            </div>

                        </div>


                    </div>

                    <div class="row">
                        <br />
                    </div>


                    <div class="row">

                        <div class="col-md-6">
                            <label for="inputName" class="col-md-5 control-label"><span style="color: red;"></span>Token:</label>
                            <div class="col-md-7">
                                <input class="form-control" type="text" id="txtToken" runat="server" placeholder="Enter Token" maxlength="200" onkeypress="return onlyAlphabets(event);" />
                            </div>

                        </div>
                        <div class="col-md-6">
                            <label for="inputName" class="col-md-5 control-label"><span style="color: red;"></span>OutputPosition:</label>
                            <div class="col-md-7">
                                <input class="form-control" type="text" id="txtOutputPosition" runat="server" placeholder="Enter Outposition" maxlength="20" onkeypress="return FunChkIsNumber(event)" />

                            </div>
                        </div>

                    </div>

                    <div class="row">
                        <br />
                    </div>

                    <div class="row">


                        <div class="col-md-6">
                            <label for="inputName" class="col-md-5 control-label"><span style="color: red;"></span>FixLength:</label>
                            <div class="col-md-7">
                                <input class="form-control" type="text" id="txtFixLength" runat="server" placeholder="Enter Fixlength" maxlength="100" onkeypress="return FunChkIsNumber(event);" />
                            </div>

                        </div>

                        <div class="col-md-6">
                            <label for="inputName" class="col-md-5 control-label"><span style="color: red;"></span>StartPos:</label>
                            <div class="col-md-7">
                                <input class="form-control" type="text" id="txtStartPos" runat="server" placeholder="Enter Start Position" maxlength="20" onkeypress="return FunChkIsNumber(event);" />

                            </div>
                        </div>

                    </div>


                    <div class="row">
                        <br />
                    </div>
                    <div class="row">

                        <div class="col-md-6">
                            <label for="inputName" class="col-md-5 control-label"><span style="color: red;"></span>EndPos:</label>
                            <div class="col-md-7">
                                <input class="form-control" type="text" id="txtEndPos" runat="server" placeholder="Enter End Position" maxlength="20" onkeypress="return FunChkIsNumber(event);" />
                            </div>

                        </div>
                    </div>



                    <div class="row">
                        <br />
                    </div>


                    <div class="row">

                        <div class="col-sm-3">
                        </div>
                        <%--7-1-18--%>
                        <div class="col-sm-6 text-center">
                            <asp:Button runat="server" CssClass="btn btn-primary" Text="SAVE" Style="width: 69px" ID="AddBtn" OnClick="add_Click" />
                            <button aria-label="Close" data-dismiss="modal" class="btn btn-primary" onclick="funCancelModal()" type="button"><span aria-hidden="true">CANCEL</span></button>
                            <input type="button" id="BtnReset" aria-label="Reset" class="btn btn-primary" value="RESET" onclick="FunClear();" />
                            <asp:Button runat="server" CssClass="btn btn-primary" Text="DELETE" Style="width: 69px" ID="BtnDelete" OnClick="Delete_Click" OnClientClick="return confirm_user();" />
                        </div>

                        <div class="col-sm-3">
                        </div>
                    </div>
                </div>
            </div>

        </div>

    </div>


    
    <div id="memberModal" class="modal fade bs-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel">
        <div class="modal-dialog modal-md">
            <div class="modal-content" style="border-radius: 6px">
                <div class="modal-header box-primary" style="border-top-right-radius: 6px; border-top-left-radius: 6px">
                    <button aria-label="Close" data-dismiss="modal" class="close" type="button"><span aria-hidden="true">×</span></button>
                    <h4 id="myLargeModalLabel" class="modal-title" style="font-weight: bold">Bank PREStandard Data</h4>
                </div>
                <div class="modal-body" id="msgBody">
                    <asp:Label ID="LblMessage" runat="server"></asp:Label>
                    <%--<asp:Label ID="LblCardRPANMessage" runat="server"></asp:Label>--%>
                </div>
                <div class="modal-footer">
                    <button aria-label="Close" data-dismiss="modal" id="BtnModalClose" class="btn btn-primary" type="button"><span aria-hidden="true">OK</span></button>
                </div>
            </div>
        </div>
        <!-- /.modal-content -->
    </div>

    <%--<script>
    $("#BtnModalClose").click(function () {
        location.reload();
        
    });
</script>--%>

    <%--<script>
        $(document).ready(function () {

            if ($("#<%=hdnFlag.ClientID %>").val() == "2") {
                $('#SearchDiv').hide();
                //$('#SearchDiv').hide();
                //$('#InfoDiv').show();
            }
            else {
                $('#SearchDiv').show();
                //$('#InfoDiv').hide();
            }
        });
    </script>--%>

    <script>
        $(document).ready(function () {
            $(function () {
                $('[data-toggle="tooltip"]').tooltip()
            });

            $('#datatable-buttons').html($("#<%=hdnTransactionDetails.ClientID %>").val());
            //check user has Edit rights
            if ($.inArray("E", ($('#<%=hdnAccessCaption.ClientID%>').val().split(","))) > -1) {
                $('#datatable-buttons input[type="button"][value="Edit"]').show();
            }
            else {
                $('#datatable-buttons input[type="button"][value="Edit"]').hide();
            }
            <%--if ($.inArray("D", ($('#<%=hdnAccessCaption.ClientID%>').val().split(","))) > -1) {
                $('#datatable-buttons input[type="button"][value="Delete"]').show();
            }
            else {
                $('#datatable-buttons input[type="button"][value="Delete"]').hide();
            }--%>
            //check user has Add rights

            if ($("#<%=hdnFlag.ClientID %>").val() == "3" && ($.inArray("A", $('#<%=hdnAccessCaption.ClientID%>').val().split(",")) > -1)) {
                $('#BtnAddNew').hide();


            }
            else if ($.inArray("A", $('#<%=hdnAccessCaption.ClientID%>').val().split(",")) > -1) {
                $('#BtnAddNew').show();
            }


            <%--if ($.inArray("A", $('#<%=hdnAccessCaption.ClientID%>').val().split(",")) > -1) {
                $('#BtnAddNew').show();
            }
            <%--if ($.inArray("D", $('#<%=hdnAccessCaption.ClientID%>').val().split(",")) > -1) {
                $('#BtnAddNew').show();
            }--%>
            //else {
                //$('#BtnAddNew').hide();
            //}--%>
        //$("#<%=hdnTransactionDetails.ClientID %>").val('');
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
    <%--7-1-18--%>
    <script>
        function FunClear() {
            fnreset(null, true);
            //$('#SearchDiv').show();
            //$('#InfoDiv').hide();
            $('#errormsgDivForModel').hide();
            <%--$('[id$="txtIssuerNo"]').val('');
            $('[id$="txtCardProgram"]').val('');
            $('[id$="txtToken"]').val('');
            $('#<%=ddlPadding.ClientID%> option:selected').text();
            $('#<%=ddlPadding.ClientID%>').val('0');
            $('[id$="txtOutputPosition"]').val('');
            $('[id$="txtFixLength"]').val('');
            $("#<%=ddlDirection.ClientID%>")[0].selectedIndex = 0;
                $('[id$="txtStartPos"]').val('');
                $('[id$="txtEndPos"]').val('');
--%>

        }
        </script>
    <script>

        function funAddNew() {

            $('[id$="txtIssuerNo"]').val('');
            $('[id$="txtCardProgram"]').val('');
            //$('[id$="txt_username"]').val('');
            <%--$('#<%=DDLUserRole.ClientID%> option:selected').text("--Select--");--%>
            $('#<%=ddlPadding.ClientID%>').val('0');
            $('[id$="txtToken"]').val('');
            $('[id$="txtOutputPosition"]').val('');
            $('[id$="txtFixLength"]').val('');
            $('[id$="txtStartPos"]').val('');
            $('[id$="txtEndPos"]').val('');
            $('#AddEditModal').modal('show');
            $('[id$="txtCardProgram"]').attr("readonly", false);
            $('[id$="txtIssuerNo"]').attr("readonly", false);
            $('[id$="txtToken"]').attr("readonly", false);
            $('#<%=ddlDirection.ClientID%>').val('0');
            $("#<%=ddlDirection.ClientID%>").attr("disabled", false)
            //5-1-17
            $('#BtnReset').show();
            $('#BtnDelete').hide();
            $('#<%=BtnDelete.ClientID%>').hide();
        }
    </script>

    <script>

        function funedit(obj) {

            $('[id$="txtIssuerNo"]').val('');
            $('[id$="txtCardProgram"]').val('');
            $('#<%=ddlPadding.ClientID%> option:selected').text();
            $('#<%=ddlPadding.ClientID%>').val('0');
            $('#<%=ddlDirection.ClientID%>').val('0');
            $('[id$="txtToken"]').val('');
            $('[id$="txtOutputPosition"]').val('');
            $('[id$="txtFixLength"]').val('');
            $('[id$="txtStartPos"]').val('');
            $('[id$="txtEndPos"]').val('');
            //$("[id*=SystemList] input").removeAttr("checked")

            var sIssuerNo = $(obj).attr('IssuerNo');
            var sCardProgram = $(obj).attr('CardProgram');
            var sPadding = $(obj).attr('Padding');
            
            var sDirection = $(obj).attr('Direction');
            var sToken = $(obj).attr('Token');
            
            var sOutputPosition = $(obj).attr('OutputPosition');
            var sFixLength = $(obj).attr('FixLength');
            var sStartPos = $(obj).attr('StartPos');
            var sEndPos = $(obj).attr('EndPos');
            

            $('[id$="txtIssuerNo"]').val(sIssuerNo);
            $('[id$="txtCardProgram"]').val(sCardProgram);
            $('[id$="txtToken"]').val(sToken);
            //$("#phPageBody_DDLUserRole")[0].selectedIndex = suserrole;
            $('[id$="txtOutputPosition"]').val(sOutputPosition);
            $('[id$="txtFixLength"]').val(sFixLength);
            // $("#phPageBody_dropdown_status").val(suserstatus);
            $('[id$="txtStartPos"]').val(sStartPos);
            $('[id$="txtEndPos"]').val(sEndPos);
            $("#<%=ddlPadding.ClientID%>").val(sPadding);
            $("#<%=ddlDirection.ClientID%>").val(sDirection);
            $('#AddEditModal').modal('show');
            //$('[id$="txt_username"]').attr('disabled', 'disabled');
            // $($(obj).attr('systemid').split(",")).each(function (i, item) {
            // $("table[id*=SystemList] input[value=" + $.trim(item) + "]").prop("checked", "checked");
            // });
            //$("#txtCardProgram").prop("readonly", true);
            $('[id$="txtCardProgram"]').attr("readonly", true);
            $('[id$="txtIssuerNo"]').attr("readonly", true);
            $('[id$="txtToken"]').attr("readonly", true);
            //$('#txtCardProgram').attr('disabled', true);
            //5-1-17
            $('#BtnReset').hide();
            $('#BtnDelete').show();
            $('#<%=BtnDelete.ClientID%>').show();
        }
    </script>

    <script>

        function fundelete(obj) {
            //5-1-17
            $('[id$="txtIssuerNo"]').val('');
            $('[id$="txtCardProgram"]').val('');
            $('#<%=ddlPadding.ClientID%> option:selected').text();
            $('#<%=ddlPadding.ClientID%>').val('0');
            $('#<%=ddlDirection.ClientID%>').val('0');
            $('[id$="txtToken"]').val('');
            $('[id$="txtOutputPosition"]').val('');
            $('[id$="txtFixLength"]').val('');
            $('[id$="txtStartPos"]').val('');
            $('[id$="txtEndPos"]').val('');
            //$("[id*=SystemList] input").removeAttr("checked")

            var sIssuerNo = $(obj).attr('IssuerNo');
            var sCardProgram = $(obj).attr('CardProgram');
            var sPadding = $(obj).attr('Padding');

            var sDirection = $(obj).attr('Direction');
            var sToken = $(obj).attr('Token');

            var sOutputPosition = $(obj).attr('OutputPosition');
            var sFixLength = $(obj).attr('FixLength');
            var sStartPos = $(obj).attr('StartPos');
            var sEndPos = $(obj).attr('EndPos');


            $('[id$="txtIssuerNo"]').val(sIssuerNo);
            $('[id$="txtCardProgram"]').val(sCardProgram);
            $('[id$="txtToken"]').val(sToken);
            //$("#phPageBody_DDLUserRole")[0].selectedIndex = suserrole;
            $('[id$="txtOutputPosition"]').val(sOutputPosition);
            $('[id$="txtFixLength"]').val(sFixLength);
            // $("#phPageBody_dropdown_status").val(suserstatus);
            $('[id$="txtStartPos"]').val(sStartPos);
            $('[id$="txtEndPos"]').val(sEndPos);
            $("#<%=ddlPadding.ClientID%>").val(sPadding);
            $("#<%=ddlDirection.ClientID%>").val(sDirection);
            $('#AddEditModal').modal('show');
            //$('[id$="txt_username"]').attr('disabled', 'disabled');
            // $($(obj).attr('systemid').split(",")).each(function (i, item) {
            // $("table[id*=SystemList] input[value=" + $.trim(item) + "]").prop("checked", "checked");
            // });
            //$("#txtCardProgram").prop("readonly", true);
            $('[id$="txtCardProgram"]').attr("readonly", true);
            $('[id$="txtIssuerNo"]').attr("readonly", true);
            $('[id$="txtToken"]').attr("readonly", true);
            //$('#txtCardProgram').attr('disabled', true);
            //5-1-17
            $('#BtnReset').hide();
            $('#BtnDelete').show();
            //$('#AddBtn').hide();
            $('#<%= AddBtn.ClientID %>').hide();

        }
    </script>

    <script>
        function funCancelModal() {
            $('#errormsgDivForModel').hide();
            $('[id$="txtIssuerNo"]').val('');
            $('[id$="txtCardProgram"]').val('');
            $('[id$="txtToken"]').val('');
            $('#<%=ddlPadding.ClientID%> option:selected').text();
                $('#<%=ddlPadding.ClientID%>').val('0');
                $('[id$="txtOutputPosition"]').val('');
                $('[id$="txtFixLength"]').val('');
                $("#<%=ddlDirection.ClientID%>")[0].selectedIndex = 0;
                $('[id$="txtStartPos"]').val('');
                $('[id$="txtEndPos"]').val('');

                $('#AddEditModal').modal('hide')

        }
    </script>

    <script>
        function confirm_user() {
            if (confirm("Are you sure you want to delete the Record") == true)
                return true;
            else
                return false;
        }
</script>

    <script>
            function FunShowMsg() {
                //  setTimeout($('#myModal').modal('show'),2000);
                $('#memberModal').modal('show');
            }
    </script>

    <script>
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

    <%--<script>
        function Reload(e)
        {
            window.location.reload();
            return true;
        }
    </script>
    --%>

    <script>
        //function to clear textboxes
        function FunClearAllTextValue() {

            var elements = [];
            elements = document.getElementsByClassName("form-control");

            for (var i = 3; i < elements.length; i++) {
                elements[i].value = "";
            }

        }
    </script>

    <script>
        //Validation on Search Button
        function FunSearchValidation() {
            var IssuerNo = $('#phPageBody_txtSearchIssuerNo').val()
            var CardProgram = $('#phPageBody_txtSearchCardProgram').val()
            var Token = $('#phPageBody_txtSearchToken').val()

            if (IssuerNo == "") {
                $('#SpnErrorMsg').html('Please provide numeric IssuerNo');
                $('#errormsgDiv').show();
                return false;
            }
            //if (CardProgram == "") {
            //    $('#SpnErrorMsg').html('Please provide card program for bank');
            //    $('#errormsgDiv').show();
            //    return false;
            //}
            //if (Token == "") {
            //    $('#SpnErrorMsg').html('Please provide token for bankPREstandard');
            //    $('#errormsgDiv').show();
            //    return false;
            //}

            else {
                $('#errormsgDiv').hide();
                $('.shader').fadeIn();
            }


        }


    </script>

    <%--7-1-18--%>
    <script>
        $("#<%=AddBtn.ClientID %>").click(function () {
            var errrorTextPD = 'Please provide :  ';
            var errorFieldsPD = '';

            if ($("#phPageBody_txtIssuerNo").val() == "") {
                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>, Issuer No</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>Issuer No</b> ';
                }
                $('#<%=txtIssuerNo.ClientID%>').focus();
            }

            if ($("#phPageBody_txtCardProgram").val() == "") {
                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>,Card Program</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>Card Program</b> ';
                }
                $('#<%=txtCardProgram.ClientID%>').focus();
        }

            if ($("#phPageBody_ddlPadding").val() == "0") {
            errortab = '1';

            if (errorFieldsPD != '') {
                errorFieldsPD = errorFieldsPD + '<b>,Padding</b> ';

            }
            else {
                errorFieldsPD = errorFieldsPD + '<b>Padding</b> ';

            }

            $('#<%=ddlPadding.ClientID%>').focus();
                 }

            if ($("#phPageBody_ddlDirection").val() == "0") {
                     errortab = '1';

                     if (errorFieldsPD != '') {
                         errorFieldsPD = errorFieldsPD + '<b>,Direction</b> ';

                     }
                     else {
                         errorFieldsPD = errorFieldsPD + '<b>Direction</b> ';

                     }
                     $('#<%=ddlDirection.ClientID%>').focus();
                 }


            if ($("#phPageBody_txtToken").val() == "") {
                     errortab = '1';
                     if (errorFieldsPD != '') {
                         errorFieldsPD = errorFieldsPD + '<b>,Token</b> ';
                     }
                     else {
                         errorFieldsPD = errorFieldsPD + '<b>Token</b> ';
                     }
                     $('#<%=txtToken.ClientID%>').focus();
                 }

            if ($("#phPageBody_txtOutputPosition").val() == "") {
                     errortab = '1';

                     if (errorFieldsPD != '') {
                         errorFieldsPD = errorFieldsPD + '<b>,Output Position</b> ';

                     }
                     else {
                         errorFieldsPD = errorFieldsPD + '<b>Output Position</b> ';

                     }
                     $('#<%=txtOutputPosition.ClientID%>').focus();
                 }

            if ($("#phPageBody_txtFixLength").val() == "") {
                     errortab = '1';
                     if (errorFieldsPD != '') {
                         errorFieldsPD = errorFieldsPD + '<b>,Fix Length</b> ';
                     }
                     else {
                         errorFieldsPD = errorFieldsPD + '<b>Fix Length</b> ';
                     }
                     $('#<%=txtFixLength.ClientID%>').focus();
                 }

            <%--if ($("#phPageBody_txtStartPos").val() == "") {
                     errortab = '1';
                     if (errorFieldsPD != '') {
                         errorFieldsPD = errorFieldsPD + '<b>,Start Position</b> ';
                     }
                     else {
                         errorFieldsPD = errorFieldsPD + '<b>txtStart Position</b> ';
                     }
                     $('#<%=txtStartPos.ClientID%>').focus();
            }

            if ($("#phPageBody_txtEndPos").val() == "") {
                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>,End Position</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>End Position</b> ';
                }
                $('#<%=txtEndPos.ClientID%>').focus();
            }--%>
            //if ($('#phPageBody_txtIssuerNo').val() != $('#phPageBody_txtSearchIssuerNo').val()) {
            //    $('#SpnErrorMsgForModel').html('Issuer no does not match with search');
            //    $('#errormsgDivForModel').show();
            //    return false;
            //}

                 if (errorFieldsPD != '') {
                     $('#SpnErrorMsgForModel').html(errrorTextPD + errorFieldsPD);
                     $('#errormsgDivForModel').show();
                     return false;
                 }
                 else {
                     errorFieldsPD = '';
                     errrorTextSectionPD = '';
                     errrorTextPD = '';
                     $('#errormsgDivForModel').hide();

                     // var ArrayIds = [];
                     //$("[id*=SystemList] input:checked").each(function (i) {
                     //    ArrayIds[i] = $(this).val();



                     //alert(val.join(","))

                     $('.shader').fadeIn();
                 }
        });

    </script>

    <script>
        function FunSaveValidation() {

            //function to validate save button
            var IssuerNo = $('#phPageBody_txtIssuerNo').val()
            var CardProgram = $('#phPageBody_txtCardProgram').val()
            var Token = $('#phPageBody_txtToken').val()
            var IssuerNoSearch = $('#phPageBody_txtSearchIssuerNo').val()

            if (IssuerNo == "") {
                $('#SpnErrorMsg').html('Please provide issuerNo');
                $('#errormsgDiv').show();
                return false;
            }

            if (IssuerNoSearch != IssuerNo) {
                $('#SpnErrorMsg').html('IssuerNo does not match');
                $('#errormsgDiv').show();
                return false;
            }

            if (CardProgram == "") {
                $('#SpnErrorMsg').html('Please provide card program for bank');
                $('#errormsgDiv').show();
                return false;
            }
            if (Token == "") {
                $('#SpnErrorMsg').html('Please provide token for bankPREstandard');
                $('#errormsgDiv').show();
                return false;
            }
            if ($('#phPageBody_ddlPadding').val() == "0") {
                $('#SpnErrorMsg').html('please select padding');
                $('#errormsgDiv').show();
                return false;
            }
            if ($('#phPageBody_ddlDirection').val() == "0") {
                $('#SpnErrorMsg').html('please select direction');
                $('#errormsgDiv').show();
                return false;
            }

            else {
                $('#errormsgDiv').hide();

            }

        }

    </script>
    
</asp:Content>
