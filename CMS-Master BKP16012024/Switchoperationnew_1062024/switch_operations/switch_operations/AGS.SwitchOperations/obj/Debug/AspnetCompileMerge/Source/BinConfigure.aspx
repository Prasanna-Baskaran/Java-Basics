<%@ Page Title="" Language="C#" MasterPageFile="~/SwitchOperationSite.Master" AutoEventWireup="true" CodeBehind="BinConfigure.aspx.cs" Inherits="AGS.SwitchOperations.BinConfigure" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phPageHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phPageBody" runat="server">

    <!--All Hiddenfields------------------------------------------------------------------------->
    <asp:HiddenField ID="hdnAccessCaption" runat="server" />
    <asp:HiddenField ID="hdnTransactionDetails" runat="server" />
    <asp:HiddenField ID="hdnFlag" runat="server" />

    <asp:Panel ID="pnlBankConfigureSearch" runat="server">
        <div style="font-size: 14px;">
            <div id="SearchBank">
                <div class="box-primary">
                    <div class="box-header with-border">
                        <i class="fa fa-credit-card"></i>

                        <h4 class="box-title">BIN Configure:</h4>
                        <div class="box-tools pull-right">
                            <button class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                        </div>
                        <!-- /.box-tools -->
                    </div>

                    <!--Display validation msg ------------------------------------------------------------------------->
                    <div class="pad margin no-print" id="errormsgDiv" style="display: none">
                        <div class="callout callout-info" style="margin-bottom: 0!important;">
                            <h4><i class="fa fa-info"></i>Information :</h4>
                            <span id="SpnErrorMsg" class="text-center"></span>
                        </div>
                    </div>



                    <div class="box-body">

                        <div class="box-body" id="SearchDiv">
                            <div class="form-horizontal">
                                <div class="row">
                                    <div class="col-md-4">
                                        <div class="form-group">
                                            <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>IssuerNo:</label>
                                            <div class="col-sm-8">
                                                <input type="text" class="form-control" maxlength="50" runat="server" name="SearchIssuerNo" id="txtSearchIssuerNo" onkeypress="return FunChkIsNumber(event);" />

                                            </div>
                                        </div>
                                    </div>

                                    <%--<div class="col-md-4">
                                        <div class="form-group">
                                            <label for="inputName" class="col-xs-4 control-label"><span style="color: red;"></span>CardPrefix:</label>
                                            <div class="col-sm-8">
                                                <input type="text" class="form-control" runat="server" maxlength="12" name="SearchCardPrefix" id="txtSearchCardPrefix" onkeypress="return FunChkIsNumber(event);" />

                                            </div>
                                        </div>
                                    </div>--%>

                                    <%--<div class="col-md-4">
                                        <div class="form-group">
                                            <label for="inputName" class="col-xs-4 control-label"><span style="color: red;"></span>CardProgram:</label>
                                            <div class="col-sm-8">
                                                <input type="text" class="form-control" runat="server" name="SearchCardProgram" id="txtSearchCardProgram" onkeypress="return FunChkAlphaNumeric(event);" />

                                            </div>
                                        </div>
                                    </div>--%>
                                </div>

                                <div class="row">
                                    <div class="col-md-4">
                                        <div class="form-group">
                                            <div class="col-sm-4"></div>

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

                    <%-- <div id="divAddReset">
                            <div class="row">
                    --%>            <%--<div class="col-md-6"></div>--%>
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


                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
    
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
                    <h4 id="myLargeEdit" class="modal-title" style="font-weight: bold">BIN Details</h4>
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
                            <label for="inputName" class="col-md-5 control-label" id="lblIssuerno"><span style="color: red;"></span>IssuerNo:</label>
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
                            <label for="inputName" class="col-md-5 control-label"><span style="color: red;"></span>AccountType:</label>
                            <div class="col-md-7">
                                <asp:DropDownList ID="ddlAccountType" runat="server" class="form-control" Style="width: 100%">
                                    <%-- <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Active" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Inactive" Value="2"></asp:ListItem>--%>
                                </asp:DropDownList>
                            </div>

                        </div>
                        <div class="col-md-6">

                            <label for="inputName" class="col-md-5 control-label"><span style="color: red;"></span>CardType:</label>
                            <div class="col-md-7">
                                <asp:DropDownList ID="ddlCardType" runat="server" class="form-control" Style="width: 100%">
                                </asp:DropDownList>

                            </div>

                        </div>


                    </div>

                    <div class="row">
                        <br />
                    </div>


                    <div class="row">

                        <div class="col-md-6">
                            <label for="inputName" class="col-md-5 control-label"><span style="color: red;"></span>CardPrefix:</label>
                            <div class="col-md-7">
                                <input class="form-control" type="text" id="txtCardPrefix" runat="server" placeholder="Enter CardPrefix" maxlength="12" onkeypress="return FunChkIsNumber(event);" />
                            </div>

                        </div>
                        <div class="col-md-6">
                            <label for="inputName" class="col-md-5 control-label"><span style="color: red;"></span>BinDesc:</label>
                            <div class="col-md-7">
                                <input class="form-control" type="text" id="txtBinDesc" runat="server" placeholder="Enter Bin Description" maxlength="100" onkeypress="return onlyAlphabets(event);" />

                            </div>
                        </div>

                    </div>

                    <div class="row">
                        <br />
                    </div>

                    <div class="row">

                        <div class="col-md-6">
                            <label for="inputName" class="col-md-5 control-label"><span style="color: red;"></span>InstitutionID:</label>
                            <div class="col-md-7">
                                <input class="form-control" type="text" id="txtInstitutionID" runat="server" placeholder="Enter InstitutionID" maxlength="11" onkeypress="return FunChkIsNumber(event);" />
                            </div>

                        </div>
                        <div class="col-md-6">
                            <label for="inputName" class="col-md-5 control-label"><span style="color: red;"></span>Currency:</label>
                            <div class="col-md-7">
                                <input class="form-control" type="text" id="txtCurrency" runat="server" placeholder="Enter Currency" maxlength="5" onkeypress="return FunChkIsNumber(event);" />

                            </div>
                        </div>

                    </div>

                    <div class="row">
                        <br />
                    </div>
                    

                    <div class="row">

                        <div class="col-md-6">
                            <label for="inputName" class="col-md-5 control-label"><span style="color: red;"></span>Switch CardType:</label>
                            <div class="col-md-7">
                                <input class="form-control" type="text" id="txtSwitch_CardType" runat="server" placeholder="Enter Switch CardType" maxlength="25" onkeypress="return FunChkAlphaNumeric(event);" />
                            </div>

                        </div>
                        <div class="col-md-6">
                            <label for="inputName" class="col-md-5 control-label"><span style="color: red;"></span>IsMagstrip:</label>
                            <div class="col-md-7">
                                <%--<input class="form-control" type="text" id="Text2" runat="server" placeholder="Enter Currency" maxlength="50" onkeypress="return FunChkIsNumber(event);" />--%>
                                <asp:DropDownList ID="ddlIsMagstrip" runat="server" class="form-control" Style="width: 100%">
                                    <asp:ListItem Text="--Select--" Value="-1"></asp:ListItem>
                                    <asp:ListItem Text="EMV Cards" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Magstrip" Value="1"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>

                    </div>

                    <div class="row">
                        <br />
                    </div>

                    <div class="row">
                        <div class="col-md-6">
                            <label for="inputName" class="col-md-5 control-label" id="lblSysid"><span style="color: red;"></span>SystemID:</label>
                            <div class="col-md-7">
                                <input class="form-control" type="text" id="txtSystemID" runat="server" placeholder="Enter SystemId" maxlength="200"  />
                            </div>

                        </div>

                       <div class="col-md-6">
                            <label for="inputName" class="col-md-5 control-label"><span style="color: red;"></span>PREFormat:</label>
                            <div class="col-md-7">
                                <input class="form-control" type="text" id="txtPREFormat" runat="server" placeholder="Enter PREFormat"  />
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


    <script>
        //Validation on Search Button
        function FunSearchValidation() {
            var IssuerNo = $('#phPageBody_txtSearchIssuerNo').val()

            if (IssuerNo == "") {
                $('#SpnErrorMsg').html('Please provide numeric IssuerNo');
                $('#errormsgDiv').show();
                return false;
            }

            else {
                $('#errormsgDiv').hide();
                $('.shader').fadeIn();
            }
        }
</script>
        
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
            else {
                $('#BtnAddNew').hide();
            }--%>
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
            //fnreset(null, true);
            //$('#SearchDiv').show();
            //$('#InfoDiv').hide();
            $('#errormsgDivForModel').hide();
            $('[id$="txtIssuerNo"]').val('');
            $('[id$="txtCardProgram"]').val('');
            $('#<%=ddlAccountType.ClientID%> option:selected').text();
            $('#<%=ddlAccountType.ClientID%>').val('0');
            $('#<%=ddlCardType.ClientID%>').val('0');
           //$("#<%=ddlCardType.ClientID%>")[0].selectedIndex = 0;
            $('[id$="txtCardPrefix"]').val('');
            $('[id$="txtBinDesc"]').val('');
            $('[id$="txtPREFormat"]').val('');
            //$('#AddEditModal').modal('hide')
            $('[id$="txtInstitutionID"]').val('');
            $('[id$="txtCurrency"]').val('');
            $('[id$="txtSwitch_CardType"]').val('');
            $('[id$="txtSystemID"]').val('');
            //$('[id$="txtBankID"]').val('');
            $('#<%=ddlIsMagstrip.ClientID%>').val('-1');

        }
        </script>
    <script>

        function funAddNew() {

            $('[id$="txtIssuerNo"]').val('');
            $('[id$="txtCardProgram"]').val('');
            //$('[id$="txt_username"]').val('');
            <%--$('#<%=DDLUserRole.ClientID%> option:selected').text("--Select--");--%>
            $('#<%=ddlAccountType.ClientID%>').val('0');
            $('[id$="txtCardPrefix"]').val('');
            $('[id$="txtBinDesc"]').val('');
            $('[id$="txtPREFormat"]').val('');
            //newly added
            $('[id$="txtInstitutionID"]').val('');
            $('[id$="txtCurrency"]').val('');
            $('[id$="txtSwitch_CardType"]').val('');
            $('[id$="txtIssuerNo"]').show();
            $('[id$="lblIssuerno"]').show();
            $('[id$="txtSystemID"]').hide();
            $('[id$="lblSysid"]').hide();
            
            //$('[id$="txtBankID"]').hide();
            

            $('#AddEditModal').modal('show');
            $('[id$="txtCardProgram"]').attr("readonly", false);
            $('[id$="txtIssuerNo"]').attr("readonly", false);
            $('[id$="txtCardPrefix"]').attr("readonly", false);
            
            //newly added
            $('[id$="ddlIsMagstrip"]').attr("readonly", false);
           $('#<%=ddlIsMagstrip.ClientID%>').val('-1');
            
            $('#<%=ddlIsMagstrip.ClientID%> option:selected').text("--Select--");

            $('#<%=ddlCardType.ClientID%>').val('0');
            $('#<%=ddlCardType.ClientID%> option:selected').text("--Select--");
            $("#<%=ddlCardType.ClientID%>").attr("disabled", false)
            $('#BtnReset').show();
            $('#BtnDelete').hide();
            $('#<%=BtnDelete.ClientID%>').hide();
        }
    </script>

    <script>

        function funedit(obj) {

            <%--$('[id$="txtIssuerNo"]').val('');

            $('[id$="txtCardProgram"]').val('');
            $('#<%=ddlAccountType.ClientID%> option:selected').text();
            $('#<%=ddlAccountType.ClientID%>').val('0');
            $('#<%=ddlCardType.ClientID%>').val('0');
            $('#<%=ddlCardType.ClientID%> option:selected').text();
            //newly added
            $('#<%=ddlIsMagstrip.ClientID%>').val('-1');
            $('#<%=ddlIsMagstrip.ClientID%> option:selected').text();

            $('[id$="txtCardPrefix"]').val('');
            $('[id$="txtBinDesc"]').val('');
            $('[id$="txtPREFormat"]').val('');
            //newly added
            $('[id$="txtInstitutionID"]').val('');
            $('[id$="txtCurrency"]').val('');
            $('[id$="txtSwitch_CardType"]').val('');
            $('[id$="txtSystemID"]').val('');
            $('[id$="txtBankID"]').val('');--%>
            $('[id$="txtSystemID"]').show();
            $('[id$="lblSysid"]').show();
            //$('[id$="txtIssuerNo"]').show();
            //$('[id$="lblIssuerno"]').show();
            
            //$('[id$="txtBankID"]').show();
            //$("[id*=SystemList] input").removeAttr("checked")
            <%--if (($("#<%=ddlCardType.ClientID%>").val(sCardType) == "") && ($("#<%=ddlCardType.ClientID%>").val(sCardType) == null)) {
                $('#<%=ddlCardType.ClientID%>').val('0');
            }--%>
            var sIssuerNo = $(obj).attr('IssuerNo');
            var sCardProgram = $(obj).attr('CardProgram');
            var sCardPrefix = $(obj).attr('CardPrefix');

            var sBinDesc = $(obj).attr('BinDesc');
            var sPREFormat = $(obj).attr('PREFormat');

            var sCardType = $(obj).attr('CardType');
            var sAccountType = $(obj).attr('AccountType');
            //newly added
            var sInstitutionID = $(obj).attr('InstitutionID');
            var sCurrency = $(obj).attr('Currency');
            var sSwitch_CardType = $(obj).attr('Switch_CardType');
            var sSystemID = $(obj).attr('SystemID');
            //var sBankID = $(obj).attr('BankID');
            var sIsMagstrip = $(obj).attr('IsMagstrip');
            
            $('[id$="txtIssuerNo"]').val(sIssuerNo);
            $('[id$="txtCardProgram"]').val(sCardProgram);
            $('[id$="txtCardPrefix"]').val(sCardPrefix);
            //$("#phPageBody_DDLUserRole")[0].selectedIndex = suserrole;
            $('[id$="txtBinDesc"]').val(sBinDesc);
            $('[id$="txtPREFormat"]').val(sPREFormat);
            // $("#phPageBody_dropdown_status").val(suserstatus);
            $("#<%=ddlCardType.ClientID%>").val(sCardType);
            $("#<%=ddlAccountType.ClientID%>").val(sAccountType);

            //newly added
            $('[id$="txtInstitutionID"]').val(sInstitutionID);
            $('[id$="txtCurrency"]').val(sCurrency);
            $('[id$="txtSwitch_CardType"]').val(sSwitch_CardType);
            $('[id$="txtSystemID"]').val(sSystemID);
            //$('[id$="txtBankID"]').val(sBankID);
            $("#<%=ddlIsMagstrip.ClientID%>").val(sIsMagstrip);

            $('#AddEditModal').modal('show');
            //$('[id$="txt_username"]').attr('disabled', 'disabled');
            // $($(obj).attr('systemid').split(",")).each(function (i, item) {
            // $("table[id*=SystemList] input[value=" + $.trim(item) + "]").prop("checked", "checked");
            // });
            //$("#txtCardProgram").prop("readonly", true);
            $('[id$="txtCardProgram"]').attr("readonly", true);
            //$('[id$="txtIssuerNo"]').attr("readonly", true);
            $('[id$="txtCardPrefix"]').attr("readonly", true);
            $('[id$="txtSystemID"]').attr("readonly", true);
            $('[id$="txtIssuerNo"]').attr("readonly", true);
            //$('#txtCardProgram').attr('disabled', true);
            $('#BtnReset').hide();
            $('#BtnDelete').show();
            $('#<%=BtnDelete.ClientID%>').show();
        }
    </script>

    <script>
        function funCancelModal() {
            $('#errormsgDivForModel').hide();

            $('[id$="txtIssuerNo"]').val('');
            $('[id$="txtCardProgram"]').val('');
            $('#<%=ddlAccountType.ClientID%> option:selected').text();
            $('#<%=ddlAccountType.ClientID%>').val('0');
            $('#<%=ddlCardType.ClientID%>').val('0');
           //$("#<%=ddlCardType.ClientID%>")[0].selectedIndex = 0;
            $('[id$="txtCardPrefix"]').val('');
            $('[id$="txtBinDesc"]').val('');
            $('[id$="txtPREFormat"]').val('');
            $('#AddEditModal').modal('hide')
            $('[id$="txtInstitutionID"]').val('');
            $('[id$="txtCurrency"]').val('');
            $('[id$="txtSwitch_CardType"]').val('');
            $('[id$="txtSystemID"]').val('');
            //$('[id$="txtBankID"]').val('');
            $('#<%=ddlIsMagstrip.ClientID%>').val('-1');
            $('#<%=ddlIsMagstrip.ClientID%> option:selected').text();
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

            if ($("#phPageBody_ddlAccountType").val() == "0") {
                errortab = '1';

                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>,AccountType</b> ';

                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>AccountType</b> ';

                }

                $('#<%=ddlAccountType.ClientID%>').focus();
            }

            if ($("#phPageBody_ddlCardType").val() == "0") {
                errortab = '1';

                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>,CardType</b> ';

                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>CardType</b> ';

                }
                $('#<%=ddlCardType.ClientID%>').focus();
            }


            if ($("#phPageBody_txtCardPrefix").val() == "") {
                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>,CardPrefix</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>CardPrefix</b> ';
                }
                $('#<%=txtCardPrefix.ClientID%>').focus();
            }

            if ($("#phPageBody_txtBinDesc").val() == "") {
                errortab = '1';

                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>,Bin Description</b> ';

                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>Bin Description</b> ';

                }
                $('#<%=txtBinDesc.ClientID%>').focus();
            }

            <%--if ($("#phPageBody_txtSystemID").val() == "") {
                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>,SystemID</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>SystemID</b> ';
                }
                $('#<%=txtSystemID.ClientID%>').focus();
            }--%>

            <%--if ($("#phPageBody_txtPREFormat").val() == "") {
                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>,PREFormat</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>PREFormat</b> ';
                }
                $('#<%=txtPREFormat.ClientID%>').focus();
            }

            if ($("#phPageBody_txtInstitutionID").val() == "") {
                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>,InstitutionID</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>InstitutionID</b> ';
                }
                $('#<%=txtInstitutionID.ClientID%>').focus();
            }
            if ($("#phPageBody_txtCurrency").val() == "") {
                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>,Currency</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>Currency</b> ';
                }
                $('#<%=txtCurrency.ClientID%>').focus();
            }--%>
            if ($("#phPageBody_txtSwitch_CardType").val() == "") {
                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>,Switch CardType</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>Switch CardType</b> ';
                }
                $('#<%=txtSwitch_CardType.ClientID%>').focus();
            }
            if ($("#phPageBody_ddlIsMagstrip").val() == "-1") {
                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>,Magstrip</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>Magstrip</b> ';
                }
                $('#<%=ddlIsMagstrip.ClientID%>').focus();
            }

            if (errorFieldsPD != '') {
                $('#SpnErrorMsgForModel').html(errrorTextPD + errorFieldsPD);
                $('#errormsgDivForModel').show();
                return false;
            }
            //if ($('#phPageBody_txtIssuerNo').val() != $('#phPageBody_txtSearchIssuerNo').val())
            //{
            //    $('#SpnErrorMsgForModel').html('Issuer no does not match with search');
            //    $('#errormsgDivForModel').show();
            //    return false;
            //}
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

    


</asp:Content>
